using System;
using ATMSimulator;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMSimulator
{
    partial class Menu : Form
    {
        public Account ActiveAccount { get; set; }
        private bool useRaceCondition = false; 

        private void RaceButton_Click(object sender, EventArgs e)
        {
            useRaceCondition = false;
            OpenForm1();
            
        }

        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(ActiveAccount);
            form1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(ActiveAccount);
            form1.ShowDialog();
        }

        private void OpenForm1()
        {
            if (ActiveAccount != null)
            {
                Form1 form1 = new Form1(ActiveAccount);
                form1.ShowDialog();
            }
            else
            {
                MessageBox.Show("No active account. Please log in first.", "Error");
            }
        }
    }
}
