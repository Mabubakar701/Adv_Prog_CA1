using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactBookApplication
{
    // Contact class representing individual contact with encapsulation
    class Contact
    {
        // Private fields for encapsulation
        private string firstName;
        private string lastName;
        private string company;
        private string mobileNumber;
        private string email;
        private DateTime birthdate;

        // Auto-implemented property for ContactId
        public int ContactId { get; private set; }

        // Properties with validation and access modifiers
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be empty.");
                firstName = value.Trim();
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be empty.");
                lastName = value.Trim();
            }
        }

        public string Company
        {
            get { return company; }
            set { company = string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim(); }
        }

        public string MobileNumber
        {
            get { return mobileNumber; }
            set
            {
                if (!ValidateMobileNumber(value))
                    throw new ArgumentException("Mobile number must be a 9-digit positive number.");
                mobileNumber = value.Trim();
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!ValidateEmail(value))
                    throw new ArgumentException("Invalid email format.");
                email = value.Trim();
            }
        }

        public DateTime Birthdate
        {
            get { return birthdate; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Birthdate cannot be in the future.");
                birthdate = value;
            }
        }

        // Computed property
        public string FullName => $"{FirstName} {LastName}";

        public int Age
        {
            get
            {
                int age = DateTime.Now.Year - birthdate.Year;
                if (DateTime.Now < birthdate.AddYears(age))
                    age--;
                return age;
            }
        }

        // Constructor with validation
        public Contact(int id, string firstName, string lastName, string company, 
                      string mobileNumber, string email, DateTime birthdate)
        {
            ContactId = id;
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            MobileNumber = mobileNumber;
            Email = email;
            Birthdate = birthdate;
        }

        // Private validation methods
        private bool ValidateMobileNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            // Remove any spaces or dashes
            string cleanNumber = number.Replace(" ", "").Replace("-", "");

            // Check if it's exactly 9 digits and all characters are digits
            return cleanNumber.Length == 9 && cleanNumber.All(char.IsDigit) && 
                   !cleanNumber.StartsWith("0");
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Basic email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        // Method to display contact information
        public void DisplayContact()
        {
            Console.WriteLine($"\n{"═",60}");
            Console.WriteLine($"Contact ID: {ContactId}");
            Console.WriteLine($"Name: {FullName}");
            Console.WriteLine($"Company: {Company}");
            Console.WriteLine($"Mobile: {MobileNumber}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Birthdate: {Birthdate:dd MMM yyyy}");
            Console.WriteLine($"Age: {Age} years");
            Console.WriteLine($"{"═",60}\n");
        }

        // Method overloading - Display with custom format
        public void DisplayContact(bool brief)
        {
            if (brief)
            {
                Console.WriteLine($"[{ContactId}] {FullName,-25} | {MobileNumber,-12} | {Email}");
            }
            else
            {
                DisplayContact();
            }
        }

        // Override ToString for easy printing
        public override string ToString()
        {
            return $"{ContactId}: {FullName} - {MobileNumber}";
        }
    }
}
