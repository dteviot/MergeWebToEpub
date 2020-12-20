using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public partial class AddChapterForm : Form
    {
        public AddChapterForm()
        {
            InitializeComponent();
        }

        public void PopulateControls(Epub epub, EpubItem itemToInsertBefore, int chapterNum)
        {
            this.epub = epub;
            this.itemToInsertBefore = itemToInsertBefore;
            int idVal = epub.Opf.GetPageItems().GetMaxPrefix()+ 1;
            string idNum = idVal.ToString("D4");
            textBoxId.Text = "xhtml" + idNum;
            textBoxTitle.Text = $"Chapter {chapterNum}";
            textBoxPath.Text = $"OEBPS/Text/{idNum}_Chapter{chapterNum}.xhtml";
            changeEpubAction = InsertChapter;
        }

        public void PopulateControlsForUpdate(Epub epub, EpubItem toUpdate)
        {
            this.toUpdate = toUpdate;
            var entryDetails = epub.ToC.FindTocEntry(toUpdate.AbsolutePath);
            tocEntry = entryDetails.entries[entryDetails.index];
            textBoxId.Enabled = false;
            textBoxId.Text = toUpdate.Id;
            textBoxTitle.Text = tocEntry.Title;
            textBoxSource.Text = tocEntry.ContentSrc;
            textBoxSource.Enabled = false;
            textBoxPath.Text = toUpdate.AbsolutePath;
            textBoxPath.Enabled = false;
            textBox1.Text = toUpdate.RawBytes.ToXhtml().ToString();
            changeEpubAction = UpdateChapter;
            this.Text = "Update Chapter";
            button2.Text = "Update";
        }

        public void InsertChapter()
        {
            var newChapter = new EpubItem()
            {
                Id = textBoxId.Text,
                AbsolutePath = textBoxPath.Text,
                MediaType = Epub.XhtmlMedia,
                RawBytes = Encoding.UTF8.GetBytes(Epub.EmptyXhtmlDoc),
                Source = textBoxSource.Text 
            };
            var newTocEntry = new TocEntry()
            {
                Title = textBoxTitle.Text,
                Item = newChapter
            };
            epub.InsertChapter(newChapter, newTocEntry, itemToInsertBefore);
        }

        public void UpdateChapter()
        {
            tocEntry.Title = textBoxTitle.Text;
            var doc = XDocument.Parse(textBox1.Text);
            toUpdate.RawBytes = doc.ToSBytes();
        }

        public Epub epub { get; set; }
        public EpubItem itemToInsertBefore { get; set; }

        private EpubItem toUpdate { get; set; }
        private TocEntry tocEntry { get; set; }
        private Action   changeEpubAction { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            changeEpubAction();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
