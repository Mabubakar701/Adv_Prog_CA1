using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExtensionSystem
{
    class Database
    {
        private Dictionary<string, FileExtensionTypeInfo> extensionDatabase;
        private Dictionary<string, List<string>> categoryIndex;

        public Database()
        {
            extensionDatabase = new Dictionary<string, FileExtensionTypeInfo>(StringComparer.OrdinalIgnoreCase);
            categoryIndex = new Dictionary<string, List<string>>();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Video Extensions
            AddExtension(new FileExtensionTypeInfo(
                ".mp4", "Video",
                "MPEG-4 multimedia container format",
                "Storing video - audio, used for streaming",
                new List<string> { "VLC", "Windows Media Player" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".avi", "Video",
                "Audio Video Interleave multimedia container",
                "Storing audio and video data",
                new List<string> { "VLC", "Windows Media Player", "Media Player Classic" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".mkv", "Video",
                "Matroska Multimedia Container",
                "High quality video with multiple audio tracks",
                new List<string> { "VLC", "Windows Media Player" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".mov", "Video",
                "QuickTime Movie format",
                "Apple's proprietary video format",
                new List<string> { "QuickTime Player", "VLC", "iMovie" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".webm", "Video",
                "WebM video format designed for web",
                "HTML5 video streaming",
                new List<string> { "Web Browsers", "VLC", "Chrome" }
            ));

            // Audio Ext
            AddExtension(new FileExtensionTypeInfo(
                ".mp3", "Audio",
                "MPEG Audio Layer III",
                "Compressed audio for music and podcasts",
                new List<string> { "iTunes", "Windows Media Player", "VLC" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".wav", "Audio",
                "Waveform Audio File Format",
                "Uncompressed audio, high quality",
                new List<string> { "Audacity", "Windows Media Player", "Adobe Audition" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".flac", "Audio",
                "Free Lossless Audio Codec",
                "Lossless audio compression",
                new List<string> { "VLC", "Foobar2000", "Audacity" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".aac", "Audio",
                "Advanced Audio Coding",
                "Better sound quality than MP3 on same bitrate",
                new List<string> { "iTunes", "VLC", "Windows Media Player" }
            ));

            // Image Ext
            AddExtension(new FileExtensionTypeInfo(
                ".jpg", "Image",
                "Joint Photographic Experts Group",
                "Compressed image format for photos",
                new List<string> { "Photos", "Paint", "Photoshop" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".png", "Image",
                "Portable Network Graphics",
                "image format which is lossless and have transparency support",
                new List<string> { "Photos", "GIMP", "Photoshop" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".gif", "Image",
                "Graphics Interchange Format",
                "Animated images and simple graphics",
                new List<string> { "Web Browsers", "Photoshop", "GIMP" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".svg", "Image",
                "Scalable Vector Graphics",
                "Vector graphics for web and print",
                new List<string> { "Web Browsers", "Inkscape", "Adobe Illustrator" }
            ));

            // Doc Ext
            AddExtension(new FileExtensionTypeInfo(
                ".pdf", "Document",
                "Portable Document Format",
                "Universal document format ",
                new List<string> { "Adobe Acrobat", "Web Browsers", "Preview" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".docx", "Document",
                "Microsoft Word Document",
                "Word processing documents",
                new List<string> { "Microsoft Word", "Google Docs", "LibreOffice Writer" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".xlsx", "Document",
                "Microsoft Excel Spreadsheet",
                "Spreadsheets and data analysis",
                new List<string> { "Microsoft Excel", "Google Sheets", "LibreOffice Calc" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".pptx", "Document",
                "Microsoft PowerPoint Presentation",
                "for presentations",
                new List<string> { "Microsoft PowerPoint", "Google Slides", "LibreOffice Impress" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".txt", "Document",
                "Plain Text File",
                "Text documents",
                new List<string> { "Notepad", "TextEdit", "Vim" }
            ));

            // Compressed Ext
            AddExtension(new FileExtensionTypeInfo(
                ".zip", "Compressed",
                "ZIP Archive",
                "Compressed file archive",
                new List<string> { "WinZip", "7-Zip", "Windows Explorer" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".rar", "Compressed",
                "RAR Archive",
                "Compressed file archive",
                new List<string> { "WinRAR", "7-Zip", "The Unarchiver" }
            ));

            // prog ext
            AddExtension(new FileExtensionTypeInfo(
                ".py", "Programming",
                "Python Source Code",
                "Python programming language scripts",
                new List<string> { "PyCharm", "VS Code", "Sublime Text" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".java", "Programming",
                "Java Source Code",
                "Java programming language files",
                new List<string> { "IntelliJ IDEA", "Eclipse", "NetBeans" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".html", "Programming",
                "HyperText Markup Language",
                "Web page structure and contnt",
                new List<string> { "Web Browsers", "VS Code", "Notepad++" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".css", "Programming",
                "Cascading Style Sheets",
                "Web page style and layout",
                new List<string> { "VS Code", "Sublime Text", "Web Browsers" }
            ));

            AddExtension(new FileExtensionTypeInfo(
                ".json", "Programming",
                "JavaScript Object Notation",
                "Data interchange format",
                new List<string> { "VS Code", "Notepad++" }
            ));
        }

        private void AddExtension(FileExtensionTypeInfo info)
        {
            extensionDatabase[info.Extension] = info;

            if (!categoryIndex.ContainsKey(info.Category))
            {
                categoryIndex[info.Category] = new List<string>();
            }
            categoryIndex[info.Category].Add(info.Extension);
        }

        public void SearchExtension(string extension)
        {
            if (!extension.StartsWith("."))
                extension = "." + extension;

            if (extensionDatabase.ContainsKey(extension))
            {
                extensionDatabase[extension].DisplayInfo();
            }
            else
            {
                Console.WriteLine($"\n Sorry, information about '{extension}' is not available in this database.");
                Console.WriteLine("Would you like to search for a different extension or browse categories.\n");
                SuggestSimilarExtensions(extension);
            }
        }

        private void SuggestSimilarExtensions(string extension)
        {
            var similar = extensionDatabase.Keys
                .Where(e => e.Length > 2 && extension.Length > 2 && 
                       e.Substring(0, 3).Equals(extension.Substring(0, Math.Min(3, extension.Length)), 
                       StringComparison.OrdinalIgnoreCase))
                .Take(3)
                .ToList();

            if (similar.Any())
            {
                Console.WriteLine("Do you mean one of these?");
                foreach (var ext in similar)
                {
                    Console.WriteLine($"  - {ext} ({extensionDatabase[ext].Category})");
                }
                Console.WriteLine();
            }
        }

        public void ListAllExtensions()
        {
            Console.WriteLine("\n=== All Supported File Extensions ===\n");
            foreach (var category in categoryIndex.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"{category}:");
                foreach (var ext in categoryIndex[category].OrderBy(e => e))
                {
                    Console.WriteLine($"  {ext}");
                }
                Console.WriteLine();
            }
        }

        public void BrowseByCategory(string category)
        {
            if (categoryIndex.ContainsKey(category))
            {
                Console.WriteLine($"\n=== {category} File Extensions ===\n");
                foreach (var ext in categoryIndex[category].OrderBy(e => e))
                {
                    Console.WriteLine($"{ext} - {extensionDatabase[ext].Description}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"\n[!] Category '{category}' not found.");
                Console.WriteLine("Available categories: " + string.Join(", ", categoryIndex.Keys));
                Console.WriteLine();
            }
        }

        public void ShowCategories()
        {
            Console.WriteLine("\n=== Available Categories ===\n");
            foreach (var cat in categoryIndex.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"  {cat} ({categoryIndex[cat].Count} extensions)");
            }
            Console.WriteLine();
        }

        public void ShowStatistics()
        {
            Console.WriteLine("\n=== Database Statistics ===");
            Console.WriteLine($"Total Extensions: {extensionDatabase.Count}");
            Console.WriteLine($"Total Categories: {categoryIndex.Count}");
            Console.WriteLine("\nBreakdown by Category:");
            foreach (var cat in categoryIndex.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"  {cat}: {categoryIndex[cat].Count} extensions");
            }
            Console.WriteLine();
        }
   
   
   
    }
}
