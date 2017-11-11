﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Timer = System.Timers.Timer;

namespace OCR_to_SQL
{
    public partial class Form1 : Form
    {
        Timer t = new Timer(1000);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ex1 = textBox1.Text;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = textBox2.Text;
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;         
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            string[] files = Directory.GetFiles(textBox1.Text);
            //textBox1.Text = files[0];

            for (int i = 0; i < files.Length; i++)
            {
                startInfo.Arguments = String.Format("{0} {1}//output{2} hocr", files[i], ex1, i + 1);
                
                try
                {
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                }
                catch
                {
                    MessageBox.Show("Error running Tesseract-OCR");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = ("Select the folder in which scanned documents are available:");
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd2 = new OpenFileDialog();
            if (fbd2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fbd2.FileName;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string tesseractPath = "C:\\Program Files (x86)\\Tesseract-OCR\\tesseract.exe";
            if (File.Exists(tesseractPath))
            {
                textBox2.Text = tesseractPath;
            }
            else
            {
                t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
                t.Start();
            }

        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            MessageBox.Show("Tesseract-OCR installation could not be found!\nPlease manually enter the path to tesseract.exe!");
        }
    }
}