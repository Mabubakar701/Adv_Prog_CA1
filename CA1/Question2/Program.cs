using System;

namespace FileExtensionSystem
{
    // File extension data class
    class FileExtensionTypeInfo
    {
        public string Extension { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Uses { get; set; }
        public List<string> AssociatedApps { get; set; }

        public FileExtensionTypeInfo(string ext, string cat, string desc, string use, List<string> apps)
        {
            Extension = ext;
            Category = cat;
            Description = desc;
            Uses = use;
            AssociatedApps = apps;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"\n{'=',50}");
            Console.WriteLine($"Extension: {Extension}");
            Console.WriteLine($"Category: {Category}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Common Use: {Uses}");
            Console.WriteLine($"Associated Applications: {string.Join(", ", AssociatedApps)}");
            Console.WriteLine($"{'=',50}\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database();
            Menu menu = new Menu(database);
            bool running = true;

            menu.DisplayMainMenu();

            while (running)
            {
                menu.DisplayOptions();
                string? choice = Console.ReadLine()?.Trim();

                if (choice == "6")
                {
                    running = false;
                }
                else if (choice != null)
                {
                    menu.HandleUserChoice(choice);
                }

                if (running)
                {
                    menu.ShouldContinue();
              
              
                }
            
            
            }
       
        }
   
    }
}







