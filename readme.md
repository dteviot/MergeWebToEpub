# MergeWebToEpub
(c) 2020 David Teviotdale   

Tool to allow joining Epubs created by WebToEpub into a single Epub.

The intended purpose is for web novels that are released over time.
This means that as the novel is published, you don't need to redownload all the chapters to get a single Epub.
Instead, just download the new chapters and merge them into the previous download.

I built this for my own use.  If it works for you, great.  If it doesn't, feel free to create an issue, but I make no promise I will do anything about it.

## How to use
1. Start MergeWebToEpub
2. On file menu, select open and pick the Epub that will be the first chapters.
3. Then, on file menu, select "Append to End" for next chapters to add to end of the Epub.
4. Repeat step 3 if there are additional Chapters to add.
5. If there are chapters to remove (e.g. Duplicated chapters, unwanted covers, etc) select the chapters, right click and select "Delete Item"
6. On file menu, select "Save"

You can also move chapters around in the Spine.
1. Select chapters to move.
2. Right click and select "Cut Item(s)"
3. Select position in ToC where you want to add the cut chapters.
4. Right click and slelct "Paste Item(s)".


## License information
Licenced under GPLv3.

## MergeWebToEpub uses the following libraries:
* DotNetZip  https://documentation.help/DotNetZip/CSharp.htm
* HtmlAgilityPack https://html-agility-pack.net/

