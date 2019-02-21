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
        private bool edited = false; //Has the document been edited yet?
        private string fileName;
        private string windowTitle = "TextEditor - "; //The title for the main frame
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
            if (savedAs) saveFile();
            //edited = false;
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

        private void saveAndExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (edited)
            {
                if (!savedAs) saveFileAs();
                if (savedAs) saveFile();
            }
            Form1.ActiveForm.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (edited)
            {
                var result = getUserChoice();
                if(result == DialogResult.Yes)
                {
                    if (!savedAs) saveFileAs();
                    if (savedAs) saveFile();
                }
                if (result == DialogResult.Cancel) return;
            }
            var filePath = string.Empty;
            var fileContent = string.Empty;
            openFileDialog1.Filter = "txt file (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                var fileStream = openFileDialog1.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                    richTextBox1.Text = fileContent.ToString();
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    fileName = filePath;
                    Form1.ActiveForm.Text = windowTitle + fileName;
                    savedAs = true;
                    edited = false;
                    label2.Text = "False";
                }
            }
        }

        private DialogResult getUserChoice()
        {
            DialogResult result = MessageBox.Show("Do you wan't to save them?", "You have unsaved changes", MessageBoxButtons.YesNoCancel);
            return result;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (edited)
            {
                var result = getUserChoice();
                if (result == DialogResult.Yes)
                {
                    if (!savedAs) saveFileAs();
                    if (savedAs) saveFile();
                }
                if (result == DialogResult.Cancel) return;
            }
            richTextBox1.Text = string.Empty;
            fileName = "dok1.txt";
            savedAs = false;
            edited = false;
            label2.Text = "False";
            Form1.ActiveForm.Text = windowTitle + fileName;
        }
    }
}
