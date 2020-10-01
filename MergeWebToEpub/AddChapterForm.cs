﻿using System;
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
    public partial class AddChapterForm : Form
    {
        public AddChapterForm()
        {
            InitializeComponent();
        }

        public void PopulateControls(EpubCombiner combiner, EpubItem selectedItem)
        {
            this.combiner = combiner;
            this.selectedItem = selectedItem;
            int idVal = combiner.GetMaxPrefix(combiner.InitialEpub.Opf.GetPageItems()) + 1;
            string idNum = idVal.ToString("D4");
            textBoxId.Text = "xhtml" + idNum;
            int chapterNum = combiner.ExtractProbableChapterNumber(selectedItem) + 1;
            textBoxTitle.Text = $"Chapter {chapterNum}";
            textBoxPath.Text = $"OEBPS/Text/{idNum}_Chapter{chapterNum}.xhtml";
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
            combiner.InitialEpub.InsertChapter(newChapter, newTocEntry, selectedItem);
            combiner.RefreshInternalIndexs();
        }

        public EpubCombiner combiner { get; set; }
        public EpubItem selectedItem { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            InsertChapter();
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
