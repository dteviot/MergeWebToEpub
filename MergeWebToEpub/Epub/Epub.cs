using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public partial class Epub
    {
        public class ImageUseIndex : Dictionary<string, HashSet<EpubItem>> { }

        public Epub()
        {

        }

        public void ReadFile(string fileName)
        {
            var options = new ReadOptions() { Encoding = System.Text.Encoding.UTF8 };
            using (ZipFile zip = ZipFile.Read(fileName, options))
            {
                Container = new Container(zip.ExtractXml(Epub.ContainerPath));
                var containerPath = Container.FullPath;
                Opf = new Opf(zip.ExtractXml(containerPath), containerPath);
                foreach (var item in Opf.Manifest)
                {
                    item.AddRawDataToItem(zip);
                }
                RebuildImageUseIndexes();
                var ncxItem = Opf.NcxItem;
                ToC = new ToC(zip.ExtractXml(ncxItem.AbsolutePath), ncxItem, Opf.AbsolutePathIndex);
            }
        }

        public void WriteFile(string fileName)
        {
            using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
            {
                AddMimeType(zip);
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                Container.WriteTo(zip);
                Opf.WriteTo(zip);
                ToC.WriteToNcxEpubItem();
                Opf.WriteManifestTo(zip);

                zip.Save(fileName);
            }
        }

        public void WriteAsTextFiles(string fileName)
        {
            using (var raw = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite))
            using (var output = new ZipOutputStream(raw))
            {
                Opf.WriteContentToTextFiles(output);
            }
        }

        private static void AddMimeType(ZipFile zip)
        {
            // Setting this makes mimetype correctly format.
            // refer https://stackoverflow.com/questions/33726113/the-mimetype-file-has-an-extra-field-of-length-n-the-use-of-the-extra-field-fea/33726439#33726439
            zip.EmitTimesInWindowsFormatWhenSaving = false;
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
            zip.AddEntry(Epub.MimeType, "application/epub+zip");
        }

        public void DeleteItems(List<EpubItem> items)
        {
            foreach (var item in items)
            {
                Opf.DeleteItem(item);
                ToC.DeleteItem(item);
            }
        }

        public void DeleteImages(List<EpubItem> items)
        {
            foreach (var item in items)
            {
                HashSet<EpubItem> usedChapters = null;
                if (ImagesUsedIndex.TryGetValue(item.AbsolutePath, out usedChapters))
                {
                    foreach(var chapter in usedChapters)
                    {
                        chapter.RemoveImageLink(item.AbsolutePath);
                    }
                }
                ImagesUsedIndex.Remove(item.AbsolutePath);
                Opf.DeleteImage(item);
            }
        }

        public List<string> Validate()
        {
            var errors = new List<string>();
            errors.AddRange(ValidateImages());
            errors.AddRange(ValidateXhtml());
            errors.AddRange(CheckForMissingChapters());
            errors.AddRange(ValidateImageLinks());
            return errors;
        }

        public IEnumerable<string> ValidateImages()
        {
            return Opf.GetImageItems()
                .Where(EpubUtils.IsWebp)
                .Select(item => $"Image '{item.AbsolutePath}' is Webp. Convert to jpeg");
        }

        public List<string> ValidateImageLinks()
        {
            var errors = new List<string>(UnusedImages.Select(item => $"Image '{item.AbsolutePath}' is not used"));
            errors.AddRange(MissingImages.Select(item => $"Image '{item}' is missing"));
            return errors;
        }

        public List<string> ValidateXhtml()
        {
            var errors = new List<string>();
            foreach(var item in Opf.Manifest)
            {
                errors.AddRange(item.ValidateXhtml());
            }
            return errors;
        }

        public List<string> CheckForMissingChapters()
        {
            var srcToTitle = ToC.BuildScrToTitleMap();
            var titles = new List<string>();
            int previousChapterNumber = Epub.NoChapterNum;
            Signature previousSignature = new Signature();
            foreach (var item in Opf.Spine)
            {
                string title = null;
                srcToTitle.TryGetValue(item.AbsolutePath, out title);
                int currentChapterNumber = title.ExtractProbableChapterNumber();
                bool missing = ((currentChapterNumber != Epub.NoChapterNum)
                        && (previousChapterNumber != Epub.NoChapterNum)
                        && (currentChapterNumber != (previousChapterNumber + 1)));
                previousChapterNumber = currentChapterNumber;
                if (missing)
                {
                    titles.Add($"Might be a missing chapter before \"{title}\"");
                }
                var sig = item.RawBytes.ToXhtml().CalcSignature();
                string possibleError = item.CheckForErrors(sig, previousSignature);
                if (possibleError != null)
                {
                    titles.Add(possibleError);
                }
                previousSignature = sig;
            }
            return titles;
        }

        public void InsertChapter(EpubItem chapter, TocEntry tocEntry, EpubItem preceedingItem)
        {
            Opf.InsertChapter(new List<EpubItem>() { chapter }, preceedingItem);
            ToC.InsertChapter(new List<TocEntry>() { tocEntry }, preceedingItem);
        }

        public void InsertChapters(List<EpubItem> chapters, List<TocEntry> tocEntries, EpubItem insertAt)
        {
            Opf.InsertChapter(chapters, insertAt);
            ToC.InsertChapter(tocEntries, insertAt);
        }

        public void RenumberItemIds(int index)
        {
            Opf.RenumberItemIds(index);
        }

        public void SortSpineByChapterNumber()
        {
            var srcToTitle = ToC.BuildScrToTitleMap();
            int Comparison(EpubItem x, EpubItem y)
            {
                string title = null;
                srcToTitle.TryGetValue(x.AbsolutePath, out title);
                int xChapterNumber = title.ExtractProbableChapterNumber();
                title = null;
                srcToTitle.TryGetValue(y.AbsolutePath, out title);
                int yChapterNumber = title.ExtractProbableChapterNumber();
                return xChapterNumber - yChapterNumber;
            }
            Opf.Spine.Sort(Comparison);
            ToC.GenerateToCFromChapters(Opf.Spine, srcToTitle);
        }

        public void RebuildImageUseIndexes()
        {
            UnusedImages.Clear();
            ImagesUsedIndex.Clear();
            foreach (var item in Opf.GetPageItems())
            {
                item.AddImagesTo(ImagesUsedIndex);
            }
            MissingImages = new HashSet<string>(ImagesUsedIndex.Keys);
            foreach (var item in Opf.GetImageItems())
            {
                if (ImagesUsedIndex.ContainsKey(item.AbsolutePath))
                {
                    MissingImages.Remove(item.AbsolutePath);
                }
                else
                {
                    UnusedImages.Add(item);
                }
            }
        }

        public Container Container { get; set; }
        public Opf Opf { get; set; }
        public ToC ToC { get; set; }
        public ImageUseIndex ImagesUsedIndex { get; set; } = new ImageUseIndex();
        public HashSet<EpubItem> UnusedImages { get; set; } = new HashSet<EpubItem>();
        public HashSet<string> MissingImages { get; set; } = new HashSet<string>();
    }
}
