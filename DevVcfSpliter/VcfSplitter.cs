using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DevVcfSpliter
{
    public partial class VcfSplitter : Form
    {
        private string _fileLocation=string.Empty;
        private string _folderLocation  = string.Empty;
        private readonly string[] _stringSeparators = new string[] { "END:VCARD" };
        private int _counter;
        public VcfSplitter()
        {
            InitializeComponent();
            openFileDialog.Filter = @"VCF file (*.vcf)|*.vcf";
            openFileDialog.Title = @"Select a vcf file which contains multiple vcf";
            openFileDialog.CheckFileExists = true;
            errorLabel.Text= string.Empty;
            errorLabel.ForeColor = Color.Black;
            
        }

        private void sourceButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                _fileLocation = openFileDialog.FileName;
                sourceTextBox.Text = _fileLocation;
            }
        }

        private void destinationButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                _folderLocation = folderBrowserDialog.SelectedPath;
                destinationTextBox.Text = _folderLocation;
            }
        }
        
        private void splitButton_Click(object sender, EventArgs e)
        {
            Splite();
            

        }

        private void Splite()
        {
            _counter = 0;
            errorLabel.Text = string.Empty;
            if (!string.IsNullOrEmpty(_fileLocation) && _fileLocation.ToLower().Contains(".vcf"))
            {
                if (!string.IsNullOrEmpty(_folderLocation))
                {
                    try
                    {
                        string text = File.ReadAllText(_fileLocation);

                        string[] vcfs = text.Split(_stringSeparators, StringSplitOptions.None);
                        int totalSplites = vcfs.Length;
                        progressBar.Maximum = totalSplites;
                        foreach (string s in vcfs)
                        {
                            CreateFile(s);
                            _counter++;
                            if (_counter <= totalSplites)
                            {
                                progressBar.Value = _counter;
                            }
                            
                        }
                        MessageBox.Show(_counter + @" contacts  split successfully");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
                else
                {
                    errorLabel.ForeColor = Color.Red;
                    errorLabel.Text = @"You should select destination folder";
                }
            }
            else
            {
                errorLabel.ForeColor = Color.Red;
                errorLabel.Text = @"You should select vcf file";
            }
           
        }
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CreateFile(string s)
        {
            s = string.Concat(s, _stringSeparators[0]);
            using (FileStream fs = File.Create(_folderLocation+"\\Contact"+_counter+".vcf", 1024, FileOptions.WriteThrough))
            {
                // Add some text to file
                Byte[] bytes = new UTF8Encoding(true).GetBytes(s);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        

    }
}
