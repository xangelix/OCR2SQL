using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
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
            public string Zip { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox3.Text != "")
            {
                string ex1 = textBox1.Text;

                var dictionary = new Dictionary<string, Person>();

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
                        indexOfFirstPhrase += textBox2.Text.Length;
                        int indexOfSecondPhrase = outputs[i].IndexOf("khfjlkahfgkjeahfjhaekhfkjahjslf", indexOfFirstPhrase);
                        if (indexOfSecondPhrase >= 0)
                            resolvedoutputs[i] = outputs[i].Substring(indexOfFirstPhrase, indexOfSecondPhrase - indexOfFirstPhrase);
                        else
                            resolvedoutputs[i] = outputs[i].Substring(indexOfFirstPhrase);
                    }
                    string tempOut = DeleteLines(resolvedoutputs[i], 3, false);

                    int index = resolvedoutputs[i].IndexOf(tempOut, StringComparison.Ordinal);
                    string cleanPath = (index < 0)
                        ? resolvedoutputs[i]
                        : resolvedoutputs[i].Remove(index, tempOut.Length);

                    string[] resolvedOutputsName = resolvedoutputs[i].Split(new Char[] { '\n' });

                    int resolvingIndex = 0;
                    resolvedOutputsName = resolvedOutputsName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    string personName = "name not set";
                    string personStreet = "street not set";
                    string personCity = "person not set";
                    string personState = "state not set";
                    string personZip = "zip not set";


                    foreach (string s in resolvedOutputsName)
                    {

                        if (true)
                        {
                            switch (resolvingIndex)
                            {
                                case 0:
                                    //MessageBox.Show("Name: " + s);
                                    personName = s.Trim();
                                    break;
                                case 1:
                                    //MessageBox.Show("Street Address: " + s);
                                    personStreet = s.Trim();
                                    break;
                                case 2:
                                    //MessageBox.Show("City: " + s);
                                    personCity = s.Trim();
                                    break;
                            }

                        }
                        resolvingIndex++;

                    }

                    try
                    {
                        personZip = personCity.Substring(personCity.Length - 5);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(String.Format("Error: {0}", ex.Message));
                    }

                    int cityIndex = personCity.IndexOf(",");

                    if (cityIndex > 0)
                    {
                        personState = personCity.Substring(cityIndex + 2, 2);
                    }

                    if (cityIndex > 0)
                    {
                        personCity = personCity.Substring(0, cityIndex);
                    }

                    dictionary.Add("NewPerson" + i, new Person());
                    dictionary["NewPerson" + i].Name = personName;
                    dictionary["NewPerson" + i].Street = personStreet;
                    dictionary["NewPerson" + i].City = personCity;
                    dictionary["NewPerson" + i].State = personState;
                    dictionary["NewPerson" + i].Zip = personZip;

                    Form2 form2 = new Form2();

                    form2.Text = files[i];

                    form2.textBox1.Text = dictionary["NewPerson" + i].Name;
                    form2.textBox2.Text = dictionary["NewPerson" + i].Street;
                    form2.textBox3.Text = dictionary["NewPerson" + i].City;
                    form2.textBox4.Text = dictionary["NewPerson" + i].State;
                    form2.textBox5.Text = dictionary["NewPerson" + i].Zip.ToString();

                    form2.textBox6.Text = textBox3.Text;

                    form2.richTextBox1.Text = resolvedoutputs[i];

                    if (form2.richTextBox1.Text.Contains("UFF") || form2.richTextBox1.Text.Contains("INS"))
                    {
                        form2.radioButton8.Checked = true;
                    }
                    else if (form2.richTextBox1.Text.Contains("DEL"))
                    {
                        form2.radioButton1.Checked = true;
                    }
                    else if (form2.richTextBox1.Text.Contains("EPT") || form2.richTextBox1.Text.Contains("AI"))
                    {
                        form2.radioButton5.Checked = true;
                    }
                    else if (form2.richTextBox1.Text.Contains("STR"))
                    {
                        form2.radioButton4.Checked = true;
                    }
                    else if (form2.richTextBox1.Text.Contains("NUM"))
                    {
                        form2.radioButton2.Checked = true;
                    }
                    else if (form2.richTextBox1.Text.Contains("VAC"))
                    {
                        form2.radioButton3.Checked = true;
                    }
                    else if (form2.richTextBox1.Text.Contains("KNO") || form2.richTextBox1.Text.Contains("TT"))
                    {
                        form2.radioButton7.Checked = true;
                    }

                    form2.pictureBox1.Image = Image.FromFile(files[i]);

                    form2.ShowDialog();

                    string reason = "";

                    //Determining Reason
                    if (form2.radioButton1.Checked)
                    {
                        reason = form2.radioButton1.Text;
                    }
                    else if (form2.radioButton2.Checked)
                    {
                        reason = form2.radioButton2.Text;
                    }
                    else if (form2.radioButton3.Checked)
                    {
                        reason = form2.radioButton3.Text;
                    }
                    else if (form2.radioButton4.Checked)
                    {
                        reason = form2.radioButton4.Text;
                    }
                    else if (form2.radioButton5.Checked)
                    {
                        reason = form2.radioButton5.Text;
                    }
                    else if (form2.radioButton6.Checked)
                    {
                        reason = form2.textBox7.Text;
                    }
                    else if (form2.radioButton7.Checked)
                    {
                        reason = form2.radioButton7.Text;
                    }
                    else if (form2.radioButton8.Checked)
                    {
                        reason = form2.radioButton8.Text;
                    }

                    using (StreamWriter sw = File.AppendText(textBox1.Text + "\\" + form2.textBox6.Text + ".sql"))
                    {
                        sw.WriteLine(String.Format("INSERT INTO people (name, street, city, state, zip, reason) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", form2.textBox1.Text, form2.textBox2.Text, form2.textBox3.Text, form2.textBox4.Text, form2.textBox5.Text, reason));
                    }

                    if (i == files.Length - 1)
                    {
                        //Confirmation
                        MessageBox.Show("Saved to: " + textBox1.Text + "\\" + form2.textBox6.Text + ".sql");
                    }

                }

                Application.Exit();
            }
            else
            {
                MessageBox.Show("Please fill in all required forms!");
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
    }
}
