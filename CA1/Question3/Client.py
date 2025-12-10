import socket
import json
import sys

class DBSAdmissionClient:
    def __init__(self, host='127.0.0.1', port=5555):
        self.host = host
        self.port = port
        self.secret_key = "DBS_SECURE_KEY_2024"
    
    def display_header(self):
        """Display application header"""
        print("\n" + "=" * 60)
        print(" " * 15 + "DBS ADMISSION APPLICATION")
        print("=" * 60)
        print("\nAvailable Courses:")
        print("  1. MSc in Cyber Security")
        print("  2. MSc Information Systems & Computing")
        print("  3. MSc Data Analytics")
        print("=" * 60 + "\n")
    
    def get_course_selection(self):
        """Get course selection from user"""
        courses = {
            '1': 'MSc in Cyber Security',
            '2': 'MSc Information Systems & Computing',
            '3': 'MSc Data Analytics'
        }
        
        while True:
            choice = input("Select course (1-3): ").strip()
            if choice in courses:
                return courses[choice]
            print("Invalid selection. Please choose 1, 2, or 3.")
    
    def get_start_date(self):
        """Get intended start year and month"""
        months = ['January', 'February', 'March', 'April', 'May', 'June',
                  'July', 'August', 'September', 'October', 'November', 'December']
        
        while True:
            try:
                year = int(input("Intended start year (e.g., 2025): ").strip())
                if 2024 <= year <= 2030:
                    break
                print("Please enter a valid year between 2024 and 2030.")
            except ValueError:
                print("Please enter a valid year.")
        
        print("\nAvailable months:")
        for i, month in enumerate(months, 1):
            print(f"  {i:2d}. {month}")
        
        while True:
            try:
                month_num = int(input("\nSelect month (1-12): ").strip())
                if 1 <= month_num <= 12:
                    return year, months[month_num - 1]
                print("Please enter a number between 1 and 12.")
            except ValueError:
                print("Please enter a valid number.")
    
    def collect_application_data(self):
        """Collect all application data from user"""
        self.display_header()
        
        # Collect applicant information
        name = input("Full Name: ").strip()
        while not name:
            print("Name cannot be empty.")
            name = input("Full Name: ").strip()
        
        address = input("Address: ").strip()
        while not address:
            print("Address cannot be empty.")
            address = input("Address: ").strip()
        
        qualifications = input("Educational Qualifications: ").strip()
        while not qualifications:
            print("Educational qualifications cannot be empty.")
            qualifications = input("Educational Qualifications: ").strip()
        
        print()
        course = self.get_course_selection()
        
        print()
        start_year, start_month = self.get_start_date()
        
        # Prepare application data
        application_data = {
            'name': name,
            'address': address,
            'educational_qualifications': qualifications,
            'course': course,
            'start_year': start_year,
            'start_month': start_month,
            'auth_key': self.secret_key
        }
        
        return application_data
    
    def send_application(self, application_data):
        """Send application to server and receive registration number"""
        try:
            # Create TCP socket and use context manager to ensure close
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as client_socket:
                # Set timeout for connection
                client_socket.settimeout(10)

                print("\n" + "-" * 60)
                print(f"[*] Connecting to DBS Admission Server...")

                # Connect to server
                client_socket.connect((self.host, self.port))
                print(f"[✓] Connected to {self.host}:{self.port}")

                # Send application data as JSON
                message = json.dumps(application_data)
                client_socket.sendall(message.encode('utf-8'))
                print(f"[*] Application submitted...")

                # Receive response from server (read until timeout/close)
                client_socket.settimeout(5.0)
                response_chunks = []
                while True:
                    try:
                        chunk = client_socket.recv(4096)
                        if not chunk:
                            break
                        response_chunks.append(chunk)
                    except socket.timeout:
                        # no more data arriving
                        break

                response = b''.join(response_chunks).decode('utf-8') if response_chunks else ''
                if response:
                    result = json.loads(response)
                else:
                    result = {'success': False, 'error': 'No response from server'}

                print("-" * 60 + "\n")
            
            # Display result
            if result.get('success'):
                print("=" * 60)
                print(" " * 15 + "APPLICATION SUCCESSFUL!")
                print("=" * 60)
                print(f"\nYour Registration Number: {result['registration_number']}")
                print("\nPlease save this number for future reference.")
                print("You will need it for all correspondence with DBS.")
                print("\n" + "=" * 60)
                return True
            else:
                print("=" * 60)
                print(" " * 15 + "APPLICATION FAILED")
                print("=" * 60)
                print(f"\nError: {result.get('error', 'Unknown error')}")
                print("\nPlease try again or contact DBS support.")
                print("\n" + "=" * 60)
                return False
        
        except socket.timeout:
            print("\n[!] Connection timeout. Server is not responding.")
            print("    Please check if the server is running and try again.")
            return False
        
        except ConnectionRefusedError:
            print("\n[!] Connection refused. Server is not running.")
            print("    Please ensure the server is started before submitting application.")
            return False
        
        except Exception as e:
            print(f"\n[!] Error: {e}")
            return False
    
    def run(self):
        """Run the client application"""
        try:
            # Collect application data
            application_data = self.collect_application_data()
            
            # Display summary and confirm
            print("\n" + "=" * 60)
            print(" " * 20 + "APPLICATION SUMMARY")
            print("=" * 60)
            print(f"Name: {application_data['name']}")
            print(f"Address: {application_data['address']}")
            print(f"Qualifications: {application_data['educational_qualifications']}")
            print(f"Course: {application_data['course']}")
            print(f"Start Date: {application_data['start_month']} {application_data['start_year']}")
            print("=" * 60)
            
            confirm = input("\nSubmit application? (yes/no): ").strip().lower()
            
            if confirm in ['yes', 'y']:
                # Send application to server
                self.send_application(application_data)
            else:
                print("\n[*] Application cancelled.")
        
        except KeyboardInterrupt:
            print("\n\n[*] Application cancelled by user.")
        
        except Exception as e:
            print(f"\n[!] Unexpected error: {e}")

if __name__ == "__main__":
    print("\n" + "=" * 60)
    print(" " * 10 + "Welcome to DBS Admission System")
    print("=" * 60)
    
    client = DBSAdmissionClient()
    client.run()
    
    print("\n[*] Thank you for using DBS Admission System.")
    print()

    