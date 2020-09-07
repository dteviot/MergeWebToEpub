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
            InitListView();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(CreateCombiner);
        }

        private void appendToEndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(AddEpub);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(SaveToFile);
        }

        private void deleteCheckedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(DeleteCheckedItems);
        }

        private void CreateCombiner()
        {
            var epubs = BrowseForEpub(false);
            if (epubs.Count == 1)
            {
                combiner = new EpubCombiner(epubs[0]);
                PopulateListView();
            }
        }

        private void AddEpub()
        {
            var epubs = BrowseForEpub(true);
            foreach (var epub in epubs)
            {
                combiner.Add(epub);
            }
            PopulateListView();
        }

        private List<Epub> BrowseForEpub(bool multiselect)
        {
            var epubs = new List<Epub>();
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = lastEpubFileName;
                ofd.Filter = "Epub files (*.epub)|*.epub|All files (*.*)|*.*";
                ofd.Multiselect = multiselect;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var f in ofd.FileNames)
                    {
                        var epub = new Epub();
                        epub.ReadFile(f);
                        epubs.Add(epub);
                    }
                }
            }
            return epubs;
        }

        private void SaveToFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = lastEpubFileName + "(2)";
                saveFileDialog.Filter = "Epub files (*.epub)|*.epub|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    combiner.InitialEpub.WriteFile(saveFileDialog.FileName);
                }
            }
        }

        private void DeleteCheckedItems()
        {
            foreach(ListViewItem item in listViewEpubItems.Items)
            {
                if (item.Checked)
                {
                    combiner.InitialEpub.DeleteItem(item.Tag as EpubItem);
                }
            }
            PopulateListView();
        }

        private void RunTrappingExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                MessageBox.Show(e.ToString());
            }
        }

        private void InitListView()
        {
            var cols = listViewEpubItems.Columns;
            cols.Add("ID", 100);
            cols.Add("Zip Name", 100);
            cols.Add("Title", 100);
            listViewEpubItems.CheckBoxes = true;
            listViewEpubItems.View = View.Details;
            listViewEpubItems.GridLines = true;
        }

        private void PopulateListView()
        {
            listViewEpubItems.BeginUpdate();
            listViewEpubItems.Items.Clear();
            var opf = combiner.InitialEpub.Opf;
            var titlesMap = combiner.InitialEpub.ToC.BuildScrToTitleMap();
            foreach (var s in opf.Spine)
            {
                var epubItem = opf.IdIndex[s];
                var zipName = epubItem.AbsolutePath;
                string title = null;
                if (!titlesMap.TryGetValue(zipName, out title))
                {
                    title = "<none>";
                }
                var listItem = new ListViewItem(new string[] { epubItem.Id, zipName, title })
                {
                    Tag = epubItem
                };
                listViewEpubItems.Items.Add(listItem);
            }
            listViewEpubItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewEpubItems.EndUpdate();
        }

        private static string lastEpubFileName;
        private static EpubCombiner combiner;

    }
}
