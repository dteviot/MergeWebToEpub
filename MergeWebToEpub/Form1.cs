using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeWebToEpub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(CreateCombiner);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(AddEpub);
        }

        private void buttonBrowseInitialFile_Click(object sender, EventArgs e)
        {
            BrowseForFile(textBoxInituialFile);
        }

        private void buttonBrowseAdd_Click(object sender, EventArgs e)
        {
            BrowseForFile(textBoxToAddFileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(SaveToFile);
        }

        private void CreateCombiner()
        {
            var epub = new Epub();
            epub.ReadFile(textBoxInituialFile.Text);
            combiner = new EpubCombiner(epub);
        }

        private void AddEpub()
        {
            var epub = new Epub();
            epub.ReadFile(textBoxToAddFileName.Text);
            combiner.Add(epub);
        }

        private void BrowseForFile(TextBox textBox)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = textBox.Text;
                openFileDialog.Filter = "Epub files (*.epub)|*.epub|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = openFileDialog.FileName;
                }
            }
        }

        private void SaveToFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = textBoxInituialFile.Text + "(2)";
                saveFileDialog.Filter = "Epub files (*.epub)|*.epub|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    combiner.InitialEpub.WriteFile(saveFileDialog.FileName);
                }
            }
        }


        private void RunTrappingExceptions(Action action)
        {
            try
            {
                action();
                MessageBox.Show("Done");
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                MessageBox.Show(e.ToString());
            }
        }

        private static EpubCombiner combiner;
    }
}
