
using System;

namespace ContactBookApplication
{
    // Main Program (keeps entry point & UI only)
    class Program
    {
        static void Main(string[] args)
        {
            ContactBook contactBook = new ContactBook();
            bool running = true;

            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║       Phone Contact Book Application           ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddNewContact(contactBook);
                        break;
                    case "2":
                        contactBook.ShowAllContacts();
                        break;
                    case "3":
                        ShowContactDetailsMenu(contactBook);
                        break;
                    case "4":
                        UpdateContactMenu(contactBook);
                        break;
                    case "5":
                        DeleteContactMenu(contactBook);
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("\n[✓] Thank you for using Contact Book. Goodbye!\n");
                        break;
                    default:
                        Console.WriteLine("\n[!] Invalid choice. Please select 0-5.\n");
                        break;
                }

                if (running)
                {
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("─────────────────────────────────────────────────");
            Console.WriteLine("                   Main Menu                     ");
            Console.WriteLine("─────────────────────────────────────────────────");
            Console.WriteLine("  1: Add Contact");
            Console.WriteLine("  2: Show All Contacts");
            Console.WriteLine("  3: Show Contact Details");
            Console.WriteLine("  4: Update Contact");
            Console.WriteLine("  5: Delete Contact");
            Console.WriteLine("  0: Exit");
            Console.WriteLine("─────────────────────────────────────────────────");
            Console.Write("Enter your choice: ");
        }

        static void AddNewContact(ContactBook contactBook)
        {
            Console.WriteLine("\n═══ Add New Contact ═══\n");

            try
            {
                Console.Write("First Name: ");
                string firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();

                Console.Write("Company: ");
                string company = Console.ReadLine();

                Console.Write("Mobile Number (9 digits, no leading zero): ");
                string mobile = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Birthdate (dd/MM/yyyy): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthdate))
                {
                    Console.WriteLine("\n[!] Invalid date format. Contact not added.\n");
                    return;
                }

                if (contactBook.AddContact(firstName, lastName, company, mobile, email, birthdate))
                {
                    Console.WriteLine("\n[✓] Contact added successfully!\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[!] Error: {ex.Message}\n");
            }
        }

        static void ShowContactDetailsMenu(ContactBook contactBook)
        {
            Console.Write("\nEnter Contact ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                contactBook.ShowContactDetails(id);
            }
            else
            {
                Console.WriteLine("\n[!] Invalid ID format.\n");
            }
        }

        static void UpdateContactMenu(ContactBook contactBook)
        {
            Console.Write("\nEnter Contact ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                contactBook.UpdateContact(id);
            }
            else
            {
                Console.WriteLine("\n[!] Invalid ID format.\n");
            }
        }

        static void DeleteContactMenu(ContactBook contactBook)
        {
            Console.Write("\nEnter Contact ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                contactBook.DeleteContact(id);
            }
            else
            {
                Console.WriteLine("\n[!] Invalid ID format.\n");
            }
        }
    }
}