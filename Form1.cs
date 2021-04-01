using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VegetableFarm3
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        public int day = 0;
        public int earnings = 100;

        public Form1()
        {
            InitializeComponent();
            foreach (CheckBox cb in tableLayoutPanel1.Controls)
                field[cb] = new Cell();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);

            Earn(cb);

            if (cb.Checked) Plant(cb);
            else Harvest(cb);
        }

        private void Earn(CheckBox cb) {
            switch (field[cb].state)
            {
                case CellState.Empty:
                    earnings -= 2;
                    break;
                case CellState.Planted:
                    earnings += 2;
                    break;
                case CellState.Green:
                    earnings -= 1;
                    break;
                case CellState.Immature:
                    earnings += 3;
                    break;
                case CellState.Mature:
                    earnings -= 5;
                    break;
                case CellState.Overgrow:
                    earnings -= 1;
                    break;
            }
        }

        private void Harvest(CheckBox cb)
        {
            field[cb].Harvest();
            UpdateBox(cb);
        }

        private void Plant(CheckBox cb)
        {
            field[cb].Plant();
            UpdateBox(cb);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (CheckBox cb in tableLayoutPanel1.Controls)
                NextStep(cb);
            day++;
            labDay.Text = "Day: " + day;
            LabEarnings.Text = "Earnings: " + earnings;
        }

        private void NextStep(CheckBox cb)
        {
            field[cb].NextStep();
            UpdateBox(cb);
        }

        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Planted:
                    c = Color.Black;
                    break;
                case CellState.Green:
                    c = Color.Green;
                    break;
                case CellState.Immature:
                    c = Color.Yellow;
                    break;
                case CellState.Mature:
                    c = Color.Red;
                    break;
                case CellState.Overgrow:
                    c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (sender as TrackBar);

            timer1.Interval = tb.Value;
        }
    }

    enum CellState
    {
        Empty,
        Planted,
        Green,
        Immature,
        Mature,
        Overgrow
    }



    class Cell
    {
        public CellState state = CellState.Empty;
        private int progress = 0;

        const int prPlanted = 20;
        const int prGreen = 100;
        const int prImmature = 120;
        const int prMature = 140;

        public void NextStep()
        {
            if ((state != CellState.Overgrow) && (state != CellState.Empty))
            {
                progress++;
                if ((progress == prPlanted) || (progress == prGreen) || (progress == prMature) || (progress == prImmature))
                {
                    state++;
                }
            }
        }

        internal void Plant()
        {
            state = CellState.Planted;
        }

        internal void Harvest()
        { 
            state = CellState.Empty;
            progress = 0;
        }
    }
}
