﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    /// <summary>
    /// The functions used to combine Epubs
    /// </summary>
    public class EpubCombiner
    {
        public EpubCombiner(Epub initial)
        {
            InitialEpub = initial;
        }

        public void Add(Epub toAppend)
        {
            this.ToAppend = toAppend;
            Combine();
        }

        public void Combine()
        {
            /*
             * Calculate new names of XHTML files
            Copy HTML files, fixing up any hyperlinks and img tags.  Make note of images that are in use.
            Copy images (with new names) that are in use.  Ignore images that are no longer being used or are duplictes
            Add entries to manifest and table of contents.  Notes
            There may be two table of contents if it's an epub 3
            Don't bother copying stylesheet
             */
            PrepareForMerge();
            foreach (var item in ToAppend.Opf.GetImageItems())
            {
                CopyImageEpubItem(item);
            }
            foreach (var item in ToAppend.Opf.GetPageItems())
            {
                CopyPageEpubItem(item);
            }
            CopyTableOfContents();
            CopySpine();
        }

        private void PrepareForMerge()
        {
            NewAbsolutePaths.Clear();
            NewItemIds.Clear();
            ImageHashes.Clear();

            CalculateNewPathsAndIds();
            CalculateImageHashes();
        }

        public void CalculateImageHashes()
        {
            foreach (var p in InitialEpub.Opf.GetImageItems())
            {
                if (p.RawBytes.Length != 0)
                {
                    ImageHashes[p.RawBytes.ToHash()] = p.AbsolutePath;
                }
            }
        }

        public void CalculateNewPathsAndIds()
        {
            var pages = InitialEpub.Opf.GetPageItems();
            int maxPrefix = pages.GetMaxPrefix();
            foreach(var p in ToAppend.Opf.GetPageItems())
            {
                CalcNewPathAndID(p, maxPrefix + 1);
            }

            var images = InitialEpub.Opf.GetImageItems();
            maxPrefix = images.GetMaxPrefix();
            foreach (var i in ToAppend.Opf.GetImageItems())
            {
                CalcNewPathAndID(i, maxPrefix + 1);
            }
        }

        public void CalcNewPathAndID(EpubItem item, int offset)
        {
            string oldAbsolutePath = item.AbsolutePath;
            var oldFileName = oldAbsolutePath.getZipFileName();
            var oldprefix = EpubItem.PrefixAsInt(oldFileName);
            var fileName = oldFileName.StripPrefixFromFileName();
            var path = oldAbsolutePath.GetZipPath();
            if (!string.IsNullOrEmpty(path))
            {
                path += '/';
            }

            // note possible conflict as "cover" does not have a prefix
            // which might conflict if there was also a page with 0000_cover.xhtml
            // So, if page has a prefix, bump offest by one.
            var bump = EpubItem.ExtractPrefixFromFileName(oldFileName) == null ? 0 : 1;

            var newPrefix = (oldprefix + offset + bump).ToString("D4");
            var newAbsolutePath = $"{path}{newPrefix}_{fileName}";
            NewAbsolutePaths.Add(oldAbsolutePath, newAbsolutePath);

            string newId = item.Id.StripDigits() + newPrefix;
            NewItemIds.Add(item.Id, newId);
        }

        public void CopyPageEpubItem(EpubItem item)
        {
            CopyEpubItem(item, (i) => EpubUtils.UpdateXhtmlPage(i, NewAbsolutePaths));
        }

        public void CopyImageEpubItem(EpubItem item)
        {
            var hash = item.RawBytes.ToHash();

            // don't copy images that are already in epub
            if (hash != 0)
            {
                string existingImage = null;
                if (ImageHashes.TryGetValue(hash, out existingImage))
                {
                    NewAbsolutePaths[item.AbsolutePath] = existingImage;
                    return;
                }
            }

            CopyEpubItem(item, (i) => i.RawBytes);
        }

        public void CopyEpubItem(EpubItem item, Func<EpubItem, byte[]>docUpdater)
        {
            var newItem = new EpubItem()
            {
                Id = NewItemIds[item.Id],
                AbsolutePath = NewAbsolutePaths[item.AbsolutePath],
                MediaType = item.MediaType,
                RawBytes = docUpdater(item),
                Source = item.Source
            };
            InitialEpub.Opf.AppendItem(newItem);
        }

        public string FixupUrl(string uri, string itemPath)
        {
            return EpubUtils.FixupUrl(uri, itemPath, NewAbsolutePaths);
        }

        public void CopyTableOfContents()
        {
            var newTocEntries = CopyTocEntries(ToAppend.ToC.Entries);
            InitialEpub.ToC.Entries.AddRange(newTocEntries);
        }

        public List<TocEntry> CopyTocEntries(List<TocEntry> entries)
        {
            return entries
                .Select(entry => CopyTocEntry(entry))
                .ToList();
        }

        public TocEntry CopyTocEntry(TocEntry entry)
        {
            return new TocEntry()
            {
                Title = entry.Title,
                Item = GetUpdatedItem(entry.Item),
                Children = CopyTocEntries(entry.Children)
            };
        }

        public void CopySpine()
        {
            foreach(var item in ToAppend.Opf.Spine)
            {
                InitialEpub.Opf.Spine.Add(GetUpdatedItem(item));
            }
        }

        public EpubItem GetUpdatedItem(EpubItem item)
        {
            var newId = NewItemIds[item.Id];
            return InitialEpub.Opf.IdIndex[newId];
        }

        public Epub InitialEpub { get; set; }
        public Epub ToAppend { get; set; }

        /// <summary>
        /// Map Item's old Absolute path to new Absolute path
        /// </summary>
        public Dictionary<string, string> NewAbsolutePaths { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Map Item's old IDs to new IDs
        /// </summary>
        public Dictionary<string, string> NewItemIds { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The hashes of images.  Used to eliminate duplicates
        /// </summary>
        public Dictionary<Int64, string> ImageHashes { get; set; } = new Dictionary<Int64, string>();
    }
}
