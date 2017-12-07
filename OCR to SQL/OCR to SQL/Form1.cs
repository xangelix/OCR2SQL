using System;
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
using System.Xml;
using System.Text.RegularExpressions;
using Tesseract;

namespace OCR_to_SQL
{
    public partial class Form1 : Form
    {
        public static string DeleteLines(string stringToRemoveLinesFrom, int numberOfLinesToRemove, bool startFromBottom = false)
        {
            string toReturn = "";
            string[] allLines = stringToRemoveLinesFrom.Split(
                    separator: Environment.NewLine.ToCharArray(),
                    options: StringSplitOptions.RemoveEmptyEntries);
            if (startFromBottom)
                toReturn = String.Join(Environment.NewLine, allLines.Take(allLines.Length - numberOfLinesToRemove));
            else
                toReturn = String.Join(Environment.NewLine, allLines.Skip(numberOfLinesToRemove));
            return toReturn;
        }

        Timer t = new Timer(1000);

        public Form1()
        {
            InitializeComponent();
        }

        class Person
        {
            public string Name { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public int Zip { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ex1 = textBox1.Text;

            var dictionary = new Dictionary<string, Person>();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = textBox2.Text;
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;         
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            string[] files = Directory.GetFiles(textBox1.Text);
            List<string> outputs = new List<string>();
            List<string> resolvedoutputs = new List<string>();


            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    using (var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(files[i]))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();

                                outputs.Add(text);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error: {0}", ex.Message));
                }

                resolvedoutputs.Add(outputs[i]);

                int indexOfFirstPhrase = outputs[i].IndexOf("32920");
                if (indexOfFirstPhrase >= 0)
                {
                    indexOfFirstPhrase += "32920".Length;
                    int indexOfSecondPhrase = outputs[i].IndexOf("khfjlkahfgkjeahfjhaekhfkjahjslf", indexOfFirstPhrase);
                    if (indexOfSecondPhrase >= 0)
                        resolvedoutputs[i] = outputs[i].Substring(indexOfFirstPhrase, indexOfSecondPhrase - indexOfFirstPhrase);
                    else
                        resolvedoutputs[i] = outputs[i].Substring(indexOfFirstPhrase);
                }
                string tempOut = DeleteLines(resolvedoutputs[i], 3, false);
                //MessageBox.Show(resolvedoutputs[i]);

                //MessageBox.Show(tempOut);


                int index = resolvedoutputs[i].IndexOf(tempOut, StringComparison.Ordinal);
                string cleanPath = (index < 0)
                    ? resolvedoutputs[i]
                    : resolvedoutputs[i].Remove(index, tempOut.Length);

                MessageBox.Show(resolvedoutputs[i]);
                //string[] resolvedOutputsName = resolvedoutputs[i].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string[] resolvedOutputsName = resolvedoutputs[i].Split(new Char[] { '\n' });

                int resolvingIndex = 0;
                resolvedOutputsName = resolvedOutputsName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                string personName = "name not set";
                string personStreet = "name not set";
                string personCity = "name not set";
                //string personState = "name not set";
                //string personZip = "name not set";


                foreach (string s in resolvedOutputsName)
                {
                    
                    if (true)
                    {
                        switch (resolvingIndex)
                        {
                            case 0:
                                //MessageBox.Show("Name: " + s);
                                personName = s;
                                break;
                            case 1:
                                //MessageBox.Show("Street Address: " + s);
                                personStreet = s;
                                break;
                            case 2:
                                //MessageBox.Show("City: " + s);
                                personCity = s;
                                break;
                        }
                        
                    }
                    resolvingIndex++;

                }

                dictionary.Add("NewPerson" + i, new Person());
                dictionary["NewPerson" + i].Name = personName;
                dictionary["NewPerson" + i].Street = personStreet;
                dictionary["NewPerson" + i].City = personCity;
                //dictionary["NewPerson" + i].State = "Tim Jones";
                //dictionary["NewPerson" + i].Zip = "Tim Jones";
            }

            string outputsString = string.Join(", ", outputs.ToArray());
            MessageBox.Show(outputsString);



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

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd2 = new OpenFileDialog();
            if (fbd2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fbd2.FileName;
            }
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
