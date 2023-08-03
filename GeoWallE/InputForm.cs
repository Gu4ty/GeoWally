using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoWallE
{
    public partial class InputForm : Form
    {
        public double input1 { get; private set; }
        public double input2 { get; private set; }
        public double input3 { get; private set; }
        Random r;

        private void btnRandom_Click(object sender, EventArgs e)
        {
            nudInput1.Value = (decimal)(r.Next(50, 700) + r.NextDouble());
            nudInput2.Value = (decimal)(r.Next(50, 700) + r.NextDouble());
            nudInput3.Value = (decimal)(r.Next(50, 700) + r.NextDouble());

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            input1 = (double)nudInput1.Value;
            input2 = (double)nudInput2.Value;
            input3 = (double)nudInput3.Value;
            this.Close();
        }

        public InputForm(string title, params string[] inputLabels)
        {
            InitializeComponent();
            lblMsg.Text = title;
            r = new Random();
            switch (inputLabels.Length)
            {
                case 1:
                    {
                        lblInput1.Text = inputLabels[0];
                        lblInput2.Visible = false;
                        lblInput3.Visible = false;
                        nudInput2.Visible = false;
                        nudInput3.Visible = false;
                        break;
                    }
                case 2:
                    {
                        lblInput1.Text = inputLabels[0];
                        lblInput2.Text = inputLabels[1];
                        lblInput3.Visible = false;
                        nudInput3.Visible = false;
                        break;
                    }
                case 3:
                    {
                        lblInput1.Text = inputLabels[0];
                        lblInput2.Text = inputLabels[1];
                        lblInput3.Text = inputLabels[2];
                        break;
                    }

                default:
                    break;
            }


        }
    }
}
