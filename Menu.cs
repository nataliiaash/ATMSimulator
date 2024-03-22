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
        
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(ActiveAccount);
            // Set the size of the form
            form1.Size = new Size(600, 700);
            form1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(ActiveAccount);
            // Set the size of the form
            form1.Size = new Size(600, 700);
            form1.ShowDialog();
        }


    }
}
