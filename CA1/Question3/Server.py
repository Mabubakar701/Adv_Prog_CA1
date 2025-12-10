import socket
import json
import sqlite3
import hashlib
import secrets
from datetime import datetime
import threading
import os

class DBSAdmissionServer:
    def __init__(self, host='127.0.0.1', port=5555):
        self.host = host
        self.port = port
        self.server_socket = None
        self.running = True
        # Get the directory where this script is located
        self.script_dir = os.path.dirname(os.path.abspath(__file__))
        self.db_path = os.path.join(self.script_dir, 'dbs_admissions.db')
        self.init_database()
        # Generate a shared secret key for basic authentication
        self.secret_key = "DBS_SECURE_KEY_2024"
        
    def init_database(self):
        """Initialize SQLite database with applicants table"""
        conn = sqlite3.connect(self.db_path)
        cursor = conn.cursor()
        
        cursor.execute('''
            CREATE TABLE IF NOT EXISTS applicants (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                registration_number TEXT UNIQUE NOT NULL,
                name TEXT NOT NULL,
                address TEXT NOT NULL,
                educational_qualifications TEXT NOT NULL,
                course TEXT NOT NULL,
                start_year INTEGER NOT NULL,
                start_month TEXT NOT NULL,
                application_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        ''')
        
        conn.commit()
        conn.close()
        print(f"✓ Database initialized successfully at: {self.db_path}")
    
    def generate_registration_number(self, course, year):
        """Generate unique registration number"""
        # Extract course code
        course_codes = {
            'MSc in Cyber Security': 'CS',
            'MSc Information Systems & Computing': 'ISC',
            'MSc Data Analytics': 'DA'
        }
        
        course_code = course_codes.get(course, 'GEN')
        year_code = str(year)[-2:]
        
        # Generate unique ID with timestamp and random component
        unique_id = secrets.token_hex(4).upper()
        
        return f"DBS-{course_code}-{year_code}-{unique_id}"
    
    def verify_request(self, data):
        """Basic security verification"""
        try:
            return data.get('auth_key') == self.secret_key
        except:
            return False
    
    def save_application(self, app_data):
        """Save application to database and return registration number"""
        conn = None
        try:
            print(f"[DEBUG] Attempting to save to database at: {self.db_path}")
            conn = sqlite3.connect(self.db_path)
            cursor = conn.cursor()
            
            # Generate unique registration number
            reg_number = self.generate_registration_number(
                app_data['course'], 
                app_data['start_year']
            )
            
            print(f"[DEBUG] Generated registration number: {reg_number}")
            print(f"[DEBUG] Inserting applicant: {app_data['name']}")
            
            # Insert into database
            cursor.execute('''
                INSERT INTO applicants 
                (registration_number, name, address, educational_qualifications, 
                 course, start_year, start_month)
                VALUES (?, ?, ?, ?, ?, ?, ?)
            ''', (
                reg_number,
                app_data['name'],
                app_data['address'],
                app_data['educational_qualifications'],
                app_data['course'],
                app_data['start_year'],
                app_data['start_month']
            ))
            
            print(f"[DEBUG] SQL INSERT executed, committing...")
            conn.commit()
            print(f"[DEBUG] Data committed successfully")
            
            # Verify the data was saved
            cursor.execute('SELECT COUNT(*) FROM applicants')
            count = cursor.fetchone()[0]
            print(f"[DEBUG] Total records in database: {count}")
            
            conn.close()
            print(f"[✓] Application saved to database: {reg_number}")
            
            return {'success': True, 'registration_number': reg_number}
        
        except sqlite3.IntegrityError as ie:
            # In case of duplicate, regenerate and try again
            print(f"[!] Duplicate registration detected: {ie}, regenerating...")
            if conn:
                conn.close()
            return self.save_application(app_data)
        except Exception as e:
            print(f"[!] Database error: {e}")
            if conn:
                conn.close()
            return {'success': False, 'error': str(e)}
    
    def handle_client(self, client_socket, client_address):
        """Handle individual client connection"""
        print(f"\n[+] New connection from {client_address}")
        
        try:
            # Receive data from client (read until no more data arrives)
            client_socket.settimeout(1.0)
            data_chunks = []
            while True:
                try:
                    chunk = client_socket.recv(4096)
                    if not chunk:
                        break
                    data_chunks.append(chunk)
                    # small optimization: if chunk is smaller than buffer, likely end of message
                    if len(chunk) < 4096:
                        # continue to allow for remaining small fragments
                        pass
                except socket.timeout:
                    # no more data arriving, proceed with what we have
                    break

            if not data_chunks:
                print(f"[-] No data received from {client_address}")
                return

            data = b''.join(data_chunks).decode('utf-8')
            print(f"[>] Received {len(data)} bytes from {client_address} on thread {threading.current_thread().name}")
            
            # Parse JSON data
            request_data = json.loads(data)
            print(f"[*] Received application from: {request_data.get('name', 'Unknown')}")
            
            # Verify authentication
            if not self.verify_request(request_data):
                response = {
                    'success': False,
                    'error': 'Authentication failed'
                }
                client_socket.send(json.dumps(response).encode('utf-8'))
                print(f"[!] Authentication failed for {client_address}")
                return
            
            # Save application
            result = self.save_application(request_data)
            
            # Send response back to client
            client_socket.sendall(json.dumps(result).encode('utf-8'))
            
            if result['success']:
                print(f"[✓] Application processed successfully")
                print(f"    Registration Number: {result['registration_number']}")
            else:
                print(f"[✗] Application failed: {result.get('error')}")
        
        except json.JSONDecodeError:
            error_response = {'success': False, 'error': 'Invalid data format'}
            try:
                client_socket.sendall(json.dumps(error_response).encode('utf-8'))
            except Exception:
                pass
            print(f"[!] Invalid JSON from {client_address}")
        
        except Exception as e:
            error_response = {'success': False, 'error': str(e)}
            try:
                client_socket.sendall(json.dumps(error_response).encode('utf-8'))
            except Exception:
                pass
            print(f"[!] Error handling client {client_address}: {e}")
        
        finally:
            client_socket.close()
            print(f"[-] Connection closed with {client_address}")
    
    def start(self):
        """Start the server"""
        try:
            # Create TCP socket
            self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
            
            # Set socket timeout so accept() doesn't block indefinitely
            self.server_socket.settimeout(1.0)
            
            # Bind to address and port
            self.server_socket.bind((self.host, self.port))
            
            # Listen for connections (max 5 queued connections)
            self.server_socket.listen(5)
            
            print("=" * 60)
            print("DBS ADMISSION SERVER")
            print("=" * 60)
            print(f"[*] Server started on {self.host}:{self.port}")
            print(f"[*] Waiting for connections...")
            print(f"[*] Press Ctrl+C to stop the server\n")
            
            while self.running:
                try:
                    # Accept client connection
                    client_socket, client_address = self.server_socket.accept()
                    
                    # Handle client in a separate thread (daemon so it stops with main)
                    client_thread = threading.Thread(
                        target=self.handle_client,
                        args=(client_socket, client_address),
                        daemon=True
                    )
                    client_thread.start()
                
                except socket.timeout:
                    # Socket timeout is expected, just continue
                    continue
        
        except KeyboardInterrupt:
            print("\n\n[*] Server shutting down...")
            self.running = False
        
        except Exception as e:
            print(f"[!] Server error: {e}")
            self.running = False
        
        finally:
            if self.server_socket:
                self.server_socket.close()
            self.running = False
            print("[*] Server stopped")

if __name__ == "__main__":
    server = DBSAdmissionServer()
    server.start()

    