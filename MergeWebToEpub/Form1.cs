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
        /// <summary>
        /// Track information for each row in List View
        /// </summary>
        public class ListRow
        {
            public EpubItem Item { get; set; }
            public string Title { get; set; }
            public bool MissingChapter { get; set; }

            public ListViewItem ToListViewItem()
            {
                return new ListViewItem(new string[] { Item.Id, Title ?? "<none>", Item.AbsolutePath });
            }

            public TocEntry ToTocEntry()
            {
                return new TocEntry() { Item = Item, Title = Title };
            }
        }

    public Form1()
        {
            InitializeComponent();
            InitListView();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunTrappingExceptions(LoadEpub);
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

        private void LoadEpub()
        {
            var epubs = BrowseForEpub(false);
            if (epubs.Count == 1)
            {
                epub = epubs[0];
                PopulateListView();
                PopulateThumbnails();
            }
        }

        private void AddEpub()
        {
            var epubs = BrowseForEpub(true);
            var combiner = new EpubCombiner(epub);
            foreach (var epub in epubs)
            {
                combiner.Add(epub);
            }
            PopulateListView();
            PopulateThumbnails();
        }

        private List<Epub> BrowseForEpub(bool multiselect)
        {
            var epubs = new List<Epub>();
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Epub files (*.epub)|*.epub|All files (*.*)|*.*";
                ofd.Multiselect = multiselect;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!multiselect)
                    {
                        this.Text = ofd.FileName;
                    }

                    epubs = ofd.FileNames.Select(LoadEpub).ToList();
                }
            }
            return epubs;
        }

        private void SaveToFile()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Epub files (*.epub)|*.epub|All files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    epub.WriteFile(saveFileDialog.FileName);
                }
            }
        }

        private Epub LoadEpub(string fileName)
        {
            var epub = new Epub();
            epub.ReadFile(fileName);
            var errors = epub.Validate();
            if (0 < errors.Count)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Epub file '{fileName}' has the following errors:");
                foreach(var error in errors)
                {
                    sb.AppendLine(error);
                }
                System.Diagnostics.Trace.WriteLine(sb.ToString());
                MessageBox.Show(sb.ToString());
            }
            return epub;
        }

        private void DeleteSelectedItems()
        {
            var indices = GetSelecctedIndices();
            indices.Reverse();
            epub.DeleteItems(indices.Select(index => rows[index].Item).ToList());
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
            listViewEpubItems.SelectedIndices.Clear();
            var srcToTitle = epub.ToC.BuildScrToTitleMap();
            int previousChapterNumber = Epub.NoChapterNum;
            rows.Clear();
            foreach (var item in epub.Opf.Spine)
            {
                string title = null;
                srcToTitle.TryGetValue(item.AbsolutePath, out title);
                int currentChapterNumber = title.ExtractProbableChapterNumber();
                rows.Add(new ListRow()
                {
                    Item = item,
                    Title = title,
                    MissingChapter = ((currentChapterNumber != Epub.NoChapterNum) 
                        && (previousChapterNumber != Epub.NoChapterNum) 
                        && (currentChapterNumber != (previousChapterNumber + 1)))
                });
                previousChapterNumber = currentChapterNumber;
            }
            listViewEpubItems.VirtualListSize = rows.Count;
            Refresh();
        }

        private void listViewEpubItems_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            var row = rows[e.ItemIndex];
            e.Item = row.ToListViewItem();
            HighlighItemChapterOutOfSequence(row, e.Item);
        }

        private void HighlighItemChapterOutOfSequence(ListRow row, ListViewItem viewItem)
        {
            if (row.MissingChapter)
            {
                viewItem.BackColor = Color.LightPink;
            }
        }

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
            pasteItemssToolStripMenuItem.Enabled = (0 < cutItems.Count) && (selectedCount == 1);
            renumberIDsToolStripMenuItem.Enabled = (selectedCount == 1);
        }

        private void insertAfterSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AddChapterForm();
            var index = listViewEpubItems.SelectedIndices[0];
            var row = rows[index];
            form.PopulateControls(epub, row.Item, row.Title.ExtractProbableChapterNumber() + 1);
            if (form.ShowDialog() == DialogResult.OK)
            {
                PopulateListView();
            }
        }

        private void cutItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutSelectedItems();
        }

        private void pasteItemssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteAtSelectedItem();
        }

        private void renumberIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenumberItemIdsStartingAtSelectedItem();
        }

        private void CutSelectedItems()
        {
            var indices = GetSelecctedIndices();
            cutItems = indices.Select(index => rows[index]).ToList();
            var deleteOrder = cutItems.Select(row => row.Item).Reverse().ToList();
            epub.DeleteItems(deleteOrder);
            PopulateListView();
        }

        private List<int> GetSelecctedIndices()
        {
            List<int> indices = new List<int>();
            foreach (int index in listViewEpubItems.SelectedIndices)
            {
                indices.Add(index);
            }
            indices.Sort();
            return indices;
        }

        private void PasteAtSelectedItem()
        {
            var insertAt = rows[listViewEpubItems.SelectedIndices[0]].Item;
            var chapters = cutItems.Select(row => row.Item).ToList();
            var tocEntries = cutItems.Select(row => row.ToTocEntry()).ToList();
            epub.InsertChapters(chapters, tocEntries, insertAt);
            cutItems.Clear();
            PopulateListView();
        }

        public void RenumberItemIdsStartingAtSelectedItem()
        {
            var index = listViewEpubItems.SelectedIndices[0];
            epub.RenumberItemIds(index);
            PopulateListView();
        }

        public void PopulateThumbnails()
        {
            imageListThumbs.Images.Clear();
            imageListThumbs.ImageSize = new Size(ThumbnailDimension, ThumbnailDimension);
            listViewThumbs.Clear();

            var imageItems = epub.Opf.GetImageItems();
            for (int i = 0; i < imageItems.Count; ++i)
            {
                var item = imageItems[i];
                var img = item.ExtractImage();
                imageListThumbs.Images.Add(img.MakeThumbnail(ThumbnailDimension));
                listViewThumbs.Items.Add(item.AbsolutePath, i);
            }
        }

        private Epub epub;
        private List<ListRow> rows = new List<ListRow>();
        private List<ListRow> cutItems = new List<ListRow>();

        private const int ThumbnailDimension = 80;

        private void changeWebpToJpegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            epub.ConvertWebpImagesToJpeg();
            PopulateThumbnails();
        }

        private void nextMissingChapterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScrollToMissingChapter();
        }

        private void ScrollToMissingChapter()
        {
            int topRow = listViewEpubItems.TopItem?.Index ?? 0;
            for(int i = topRow + 1; i < rows.Count; ++i)
            {
                if (rows[i].MissingChapter)
                {
                    listViewEpubItems.TopItem = listViewEpubItems.Items[i];
                    return;
                }
            }
        }
    }
}
