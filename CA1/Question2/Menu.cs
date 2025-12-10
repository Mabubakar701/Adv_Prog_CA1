using System;

namespace FileExtensionSystem
{
    class Menu
    {
        private Database database;

        public Menu(Database db)
        {
            database = db;
        }

        public void DisplayMainMenu()
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║   File Extension Info System                   ║");
            Console.WriteLine("║   Guide containing info of File Formats        ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");
        }

        public void DisplayOptions()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("  1. Search for a file Extension");
            Console.WriteLine("  2. Browse by Category");
            Console.WriteLine("  3. List all Extensions");
            Console.WriteLine("  4. Show Categories");
            Console.WriteLine("  5. Show Statistics");
            Console.WriteLine("  6. Exit");
            Console.Write("\nEnter your choice (1-6): ");
        }

        public void HandleUserChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    HandleSearchExtension();
                    break;

                case "2":
                    HandleBrowseByCategory();
                    break;

                case "3":
                    database.ListAllExtensions();
                    break;

                case "4":
                    database.ShowCategories();
                    break;

                case "5":
                    database.ShowStatistics();
                    break;

                case "6":
                    Console.WriteLine("\nThank you for using File Extension Information System! \n");
                    break;

                default:
                    Console.WriteLine("\n Invalid choice!!! Please enter a number between 1 and 6.\n");
                    break;
            }
        }

        private void HandleSearchExtension()
        {
            Console.Write("\nEnter file extension (e.g., .mp3 or .mp4 or .docx): ");
            string? ext = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(ext))
            {
                database.SearchExtension(ext);
            }
            else
            {
                Console.WriteLine("\n Invalid input!!! Please enter a valid extension.\n");
            }
        }

        private void HandleBrowseByCategory()
        {
            database.ShowCategories();
            Console.Write("Enter category name: ");
            string? cat = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(cat))
            {
                database.BrowseByCategory(cat);
            }
        }

        public bool ShouldContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            return true;
        
        
        }
    
    }
}
