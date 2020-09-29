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

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(DeleteSelectedItems);
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
                    epubs = ofd.FileNames.Select(LoadEpub).ToList();
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

        private Epub LoadEpub(string fileName)
        {
            var epub = new Epub();
            epub.ReadFile(fileName);
            var errors = epub.ValidateXhtml();
            if (0 < errors.Count)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Epub file '{fileName}' has the following errors:");
                foreach(var error in errors)
                {
                    sb.AppendLine(error);
                }
                throw new Exception(sb.ToString());
            }
            return epub;
        }

        private void DeleteSelectedItems()
        {
            List<int> indices = new List<int>();
            foreach (int index in listViewEpubItems.SelectedIndices)
            {
                indices.Add(index);
            }
            indices.Sort();
            indices.Reverse();
            listViewEpubItems.SelectedIndices.Clear();
            var spine = combiner.InitialEpub.Opf.Spine;
            foreach (var index in indices)
            {
                combiner.InitialEpub.DeleteItem(spine[index]);
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
            cols.Add("ID", -2);
            cols.Add("Title", -2);
            cols.Add("Zip Name", -2);
            listViewEpubItems.FullRowSelect = true;
            listViewEpubItems.View = View.Details;
            listViewEpubItems.GridLines = true;
            listViewEpubItems.VirtualMode = true;
        }

        private void PopulateListView()
        {
            listViewEpubItems.VirtualListSize = combiner.InitialEpub.Opf.Spine.Count;
        }

        private void listViewEpubItems_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var epubItem = combiner.InitialEpub.Opf.Spine[e.ItemIndex];
            var zipName = epubItem.AbsolutePath;
            string title = null;
            if (!combiner.ScrToTitleMap.TryGetValue(zipName, out title))
            {
                title = "<none>";
            }
            e.Item = new ListViewItem(new string[] { epubItem.Id, title, zipName });
            if (e.ItemIndex % 10 == 0)
            {
                e.Item.BackColor = Color.LightGreen;
            }
            HighlighItemChapterOutOfSequence(e.ItemIndex, epubItem, e.Item);
        }

        private void HighlighItemChapterOutOfSequence(int itemIndex, EpubItem item, ListViewItem viewItem)
        {
            if (0 < itemIndex)
            {
                int previous = combiner.ExtractProbableChapterNumber(combiner.InitialEpub.Opf.Spine[itemIndex - 1]);
                int current = combiner.ExtractProbableChapterNumber(item);
                if (current != (previous + 1))
                {
                    viewItem.BackColor = Color.LightPink;
                }
            }
        }

        private static string lastEpubFileName;
        private static EpubCombiner combiner;

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedItems();
        }

        private void contextMenuStripSpine_Opening(object sender, CancelEventArgs e)
        {
            EnableMenuBasedOnSelection();
        }

        private void EnableMenuBasedOnSelection()
        {
            int selectedCount = listViewEpubItems.SelectedIndices.Count;
            deleteItemToolStripMenuItem.Enabled = (0 < selectedCount);
            insertAfterSelectedToolStripMenuItem.Enabled = (selectedCount == 1);
        }

        private void insertAfterSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implemented");
        }
    }
}
