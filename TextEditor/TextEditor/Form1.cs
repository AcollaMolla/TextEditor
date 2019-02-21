using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        private bool edited = false;
        private string fileName;
        private string windowTitle = "TextEditor - ";
        private bool savedAs = false; //Has the user specified a name for this document yet?

        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "True";
            edited = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(edited)
            {
                //////////The line below was inspired from this stackoverflow thread: https://stackoverflow.com/questions/3036829/how-do-i-create-a-message-box-with-yes-no-choices-and-a-dialogresult ///////////
                DialogResult result = MessageBox.Show("Do you wan't to save them?", "You have unsaved changes", MessageBoxButtons.YesNoCancel);
                if(result == DialogResult.Cancel) e.Cancel = true;
                if(result == DialogResult.Yes)
                {
                    e.Cancel = true;
                    if (!savedAs) saveFileAs();
                    if (savedAs) saveFile();
                    e.Cancel = false;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!savedAs) saveFileAs();
        }

        private void saveFileAs()
        {
            Stream stream;
            saveFileDialog1.Filter = "txt file (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog1.OpenFile()) != null)
                {
                    fileName = saveFileDialog1.FileName;
                    stream.Close();
                    savedAs = true;
                    saveFile();
                }
            }
        }
        private void saveFile()
        {
            File.WriteAllText(fileName, richTextBox1.Text);
            Console.WriteLine("save to: " + fileName);
            Form1.ActiveForm.Text = windowTitle + fileName;
            edited = false;
            label2.Text = "False";
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileAs();
        }
    }
}
