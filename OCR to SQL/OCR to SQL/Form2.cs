using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OCR_to_SQL
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
            {
                //Database Creation
                string[] databaseName = { "CREATE DATABASE " + textBox6.Text + ";"};
                System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Form1"];
                File.WriteAllLines(((Form1)f).textBox1.Text + "\\" + textBox6.Text + ".sql", databaseName);



                //Closing
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all required forms!");
            }
            
        }
    }
}
