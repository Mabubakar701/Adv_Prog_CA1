using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBookApplication
{
    // ContactBook class managing all contacts - demonstrates Object Relationships
    class ContactBook
    {
        // Private collection for encapsulation
        private Dictionary<int, Contact> contacts;
        private int nextId;

        // Property to get contact count
        public int ContactCount => contacts.Count;

        // Constructor
        public ContactBook()
        {
            contacts = new Dictionary<int, Contact>();
            nextId = 1;
            InitializeDefaultContacts();
        }

        // Initialize with 20+ default contacts
        private void InitializeDefaultContacts()
        {
            try
            {
                AddContact("Emily", "Blackwell", "Dublin Business School", "871111111", 
                          "emily.blackwell@dbs.ie", new DateTime(1990, 1, 1));
                AddContact("James", "Murphy", "TechCorp Ltd", "872222222", 
                          "james.murphy@techcorp.ie", new DateTime(1985, 3, 15));
                AddContact("Sarah", "O'Connor", "Green Energy Solutions", "873333333", 
                          "sarah.oconnor@greenenergy.ie", new DateTime(1992, 7, 22));
                AddContact("Michael", "Walsh", "Dublin City Council", "874444444", 
                          "michael.walsh@dcc.ie", new DateTime(1988, 11, 30));
                AddContact("Lisa", "Brennan", "Healthcare Plus", "875555555", 
                          "lisa.brennan@healthcare.ie", new DateTime(1995, 5, 8));
                AddContact("David", "Kelly", "Financial Services Group", "876666666", 
                          "david.kelly@fsg.ie", new DateTime(1983, 9, 17));
                AddContact("Emma", "Ryan", "Digital Marketing Agency", "877777777", 
                          "emma.ryan@digitalma.ie", new DateTime(1991, 2, 14));
                AddContact("Patrick", "Doyle", "Construction Ltd", "878888888", 
                          "patrick.doyle@construction.ie", new DateTime(1987, 12, 25));
                AddContact("Aoife", "McCarthy", "Trinity College Dublin", "879999999", 
                          "aoife.mccarthy@tcd.ie", new DateTime(1993, 4, 3));
                AddContact("Sean", "O'Brien", "Restaurant Group", "871234567", 
                          "sean.obrien@restaurant.ie", new DateTime(1989, 8, 19));
                AddContact("Claire", "Fitzgerald", "Law Firm Associates", "872345678", 
                          "claire.fitzgerald@lawfirm.ie", new DateTime(1986, 6, 11));
                AddContact("Conor", "Gallagher", "Sports Academy", "873456789", 
                          "conor.gallagher@sports.ie", new DateTime(1994, 10, 7));
                AddContact("Niamh", "Collins", "Art Gallery Dublin", "874567890", 
                          "niamh.collins@artgallery.ie", new DateTime(1990, 1, 28));
                AddContact("Brian", "Kennedy", "Insurance Ireland", "875678901", 
                          "brian.kennedy@insurance.ie", new DateTime(1984, 5, 16));
                AddContact("Rachel", "Byrne", "Travel Agency", "876789012", 
                          "rachel.byrne@travel.ie", new DateTime(1992, 9, 23));
                AddContact("Kevin", "Nolan", "Engineering Solutions", "877890123", 
                          "kevin.nolan@engineering.ie", new DateTime(1988, 3, 12));
                AddContact("Michelle", "Dunne", "Fashion Retail", "878901234", 
                          "michelle.dunne@fashion.ie", new DateTime(1991, 7, 5));
                AddContact("Thomas", "Quinn", "Pharmaceutical Research", "879012345", 
                          "thomas.quinn@pharma.ie", new DateTime(1987, 11, 18));
                AddContact("Jennifer", "Lynch", "Media Production", "871111222", 
                          "jennifer.lynch@media.ie", new DateTime(1993, 2, 9));
                AddContact("Daniel", "Kavanagh", "Real Estate Services", "872222333", 
                          "daniel.kavanagh@realestate.ie", new DateTime(1985, 12, 27));
                AddContact("Sinead", "Moore", "University College Cork", "873333444", 
                          "sinead.moore@ucc.ie", new DateTime(1990, 4, 14));

                Console.WriteLine($"[✓] Loaded {ContactCount} default contacts successfully.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error loading default contacts: {ex.Message}");
            }
        }

        // Method overloading - Add contact with all parameters
        public bool AddContact(string firstName, string lastName, string company, 
                              string mobileNumber, string email, DateTime birthdate)
        {
            try
            {
                Contact newContact = new Contact(nextId, firstName, lastName, company, 
                                                mobileNumber, email, birthdate);
                contacts.Add(nextId, newContact);
                nextId++;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error adding contact: {ex.Message}");
                return false;
            }
        }

        // Method overloading - Add contact object
        public bool AddContact(Contact contact)
        {
            try
            {
                if (contact == null)
                    throw new ArgumentNullException(nameof(contact));

                contacts.Add(nextId, contact);
                nextId++;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error adding contact: {ex.Message}");
                return false;
            }
        }

        // Show all contacts
        public void ShowAllContacts()
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("\n[!] No contacts available.\n");
                return;
            }

            Console.WriteLine($"\n{"═",80}");
            Console.WriteLine($"  All Contacts ({ContactCount} total)");
            Console.WriteLine($"{"═",80}");
            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Mobile",-12} {"Email"}");
            Console.WriteLine($"{"─",80}");

            foreach (var contact in contacts.Values.OrderBy(c => c.LastName))
            {
                Console.WriteLine($"{contact.ContactId,-5} {contact.FullName,-25} {contact.MobileNumber,-12} {contact.Email}");
            }
            Console.WriteLine($"{"═",80}\n");
        }

        // Show contact details by ID
        public void ShowContactDetails(int contactId)
        {
            if (contacts.ContainsKey(contactId))
            {
                contacts[contactId].DisplayContact();
            }
            else
            {
                Console.WriteLine($"\n[!] Contact with ID {contactId} not found.\n");
            }
        }

        // Method overloading - Show contact by name
        public void ShowContactDetails(string name)
        {
            var matchingContacts = contacts.Values
                .Where(c => c.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchingContacts.Count == 0)
            {
                Console.WriteLine($"\n[!] No contacts found matching '{name}'.\n");
            }
            else if (matchingContacts.Count == 1)
            {
                matchingContacts[0].DisplayContact();
            }
            else
            {
                Console.WriteLine($"\n[i] Found {matchingContacts.Count} contacts matching '{name}':");
                foreach (var contact in matchingContacts)
                {
                    contact.DisplayContact(true);
                }
                Console.WriteLine();
            }
        }

        // Update contact
        public bool UpdateContact(int contactId)
        {
            if (!contacts.ContainsKey(contactId))
            {
                Console.WriteLine($"\n[!] Contact with ID {contactId} not found.\n");
                return false;
            }

            Contact contact = contacts[contactId];
            Console.WriteLine("\nCurrent contact details:");
            contact.DisplayContact();

            Console.WriteLine("What would you like to update?");
            Console.WriteLine("  1. First Name");
            Console.WriteLine("  2. Last Name");
            Console.WriteLine("  3. Company");
            Console.WriteLine("  4. Mobile Number");
            Console.WriteLine("  5. Email");
            Console.WriteLine("  6. Birthdate");
            Console.WriteLine("  0. Cancel");
            Console.Write("\nEnter choice: ");

            string choice = Console.ReadLine()?.Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter new First Name: ");
                        contact.FirstName = Console.ReadLine();
                        Console.WriteLine("[✓] First name updated successfully.");
                        break;
                    case "2":
                        Console.Write("Enter new Last Name: ");
                        contact.LastName = Console.ReadLine();
                        Console.WriteLine("[✓] Last name updated successfully.");
                        break;
                    case "3":
                        Console.Write("Enter new Company: ");
                        contact.Company = Console.ReadLine();
                        Console.WriteLine("[✓] Company updated successfully.");
                        break;
                    case "4":
                        Console.Write("Enter new Mobile Number (9 digits): ");
                        contact.MobileNumber = Console.ReadLine();
                        Console.WriteLine("[✓] Mobile number updated successfully.");
                        break;
                    case "5":
                        Console.Write("Enter new Email: ");
                        contact.Email = Console.ReadLine();
                        Console.WriteLine("[✓] Email updated successfully.");
                        break;
                    case "6":
                        Console.Write("Enter new Birthdate (dd/MM/yyyy): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newDate))
                        {
                            contact.Birthdate = newDate;
                            Console.WriteLine("[✓] Birthdate updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("[!] Invalid date format.");
                            return false;
                        }
                        break;
                    case "0":
                        Console.WriteLine("[i] Update cancelled.");
                        return false;
                    default:
                        Console.WriteLine("[!] Invalid choice.");
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error updating contact: {ex.Message}");
                return false;
            }
        }

        // Delete contact
        public bool DeleteContact(int contactId)
        {
            if (!contacts.ContainsKey(contactId))
            {
                Console.WriteLine($"\n[!] Contact with ID {contactId} not found.\n");
                return false;
            }

            Contact contact = contacts[contactId];
            Console.WriteLine("\nContact to be deleted:");
            contact.DisplayContact(true);

            Console.Write("\nAre you sure you want to delete this contact? (yes/no): ");
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "yes" || confirmation == "y")
            {
                contacts.Remove(contactId);
                Console.WriteLine("[✓] Contact deleted successfully.\n");
                return true;
            }
            else
            {
                Console.WriteLine("[i] Deletion cancelled.\n");
                return false;
            }
        }

        // Search contacts
        public void SearchContacts(string searchTerm)
        {
            var results = contacts.Values
                .Where(c => c.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           c.Company.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine($"\n[!] No contacts found matching '{searchTerm}'.\n");
            }
            else
            {
                Console.WriteLine($"\n[i] Found {results.Count} contact(s):\n");
                foreach (var contact in results)
                {
                    contact.DisplayContact(true);
                }
                Console.WriteLine();
            }
        }

        // Check if contact exists
        public bool ContactExists(int contactId)
        {
            return contacts.ContainsKey(contactId);
        }
    }
}
