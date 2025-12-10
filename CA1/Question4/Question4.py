import requests
from bs4 import BeautifulSoup
import csv
import re

# Function to clean price text and extract numeric value
def clean_price(price_text):
    # Remove € symbol and any extra whitespace
    price_text = price_text.strip().replace('€', '').replace(',', '').strip()
    # Extract first number found (in case of multiple prices)
    numbers = re.findall(r'\d+', price_text)
    if numbers:
        return numbers[0]
    return 'N/A'

# Function to scrape hotel data from a website
def scrape_hotel_data(url, source_name):
    print(f"Scraping data from {source_name}...")
    
    try:
        # Send GET request
        response = requests.get(url)
        response.raise_for_status()
        
        # Parse HTML
        soup = BeautifulSoup(response.content, 'html.parser')
        
        hotels = []
        
        # Find all hotel listings - they appear to be in divs or sections
        # Looking for patterns in the HTML structure
        hotel_sections = soup.find_all(['div', 'section'])
        
        hotel_count = 0
        for section in hotel_sections:
            # Look for hotel name (usually in headings or strong tags)
            name_tag = section.find(['h3', 'h4', 'strong'])
            if not name_tag:
                continue
                
            hotel_name = name_tag.get_text(strip=True)
            
            # Skip if name is too short or contains common non-hotel text
            if len(hotel_name) < 5 or hotel_name in ['About', 'Contact', 'Home', 'Hotels']:
                continue
            
            # Look for location
            location = 'Dublin, Ireland'
            location_tags = section.find_all(string=re.compile(r'Dublin|km from'))
            if location_tags:
                for loc in location_tags:
                    if 'Dublin' in loc:
                        location = loc.strip()
                        break
            
            # Look for room type
            room_type = 'Standard Room'
            room_tags = section.find_all(string=re.compile(r'Room|bed|Apartment|Studio'))
            if room_tags:
                for room in room_tags:
                    if 'bed' in room.lower() or 'room' in room.lower():
                        room_type = room.strip()
                        break
            
            # Look for price (€ symbol)
            price = 'N/A'
            price_tags = section.find_all(string=re.compile(r'€\s*\d+'))
            if price_tags:
                # Get the last price (usually the actual price, not crossed-out)
                price_text = price_tags[-1]
                price = clean_price(price_text)
            
            # Look for rating
            rating = 'N/A'
            rating_tags = section.find_all(string=re.compile(r'^\d+\.\d+$'))
            if rating_tags:
                rating = rating_tags[0].strip()
            
            # Look for reviews count
            reviews = 'N/A'
            review_tags = section.find_all(string=re.compile(r'\d+.*reviews'))
            if review_tags:
                reviews_text = review_tags[0]
                numbers = re.findall(r'\d+', reviews_text.replace(',', ''))
                if numbers:
                    reviews = numbers[0]
            
            # Only add if we have at least a name and price
            if hotel_name and price != 'N/A':
                hotels.append({
                    'Hotel Name': hotel_name,
                    'Location': location,
                    'Room Type': room_type,
                    'Price (EUR)': price,
                    'Rating': rating,
                    'Number of Reviews': reviews,
                    'Source': source_name
                })
                
                hotel_count += 1
                if hotel_count >= 10:  # Stop after 10 hotels from each source
                    break
        
        print(f"Found {len(hotels)} hotels from {source_name}")
        return hotels
        
    except Exception as e:
        print(f"Error scraping {source_name}: {e}")
        return []

# Main function
def main():
    print("=" * 60)
    print("Hotel Price Data Scraper")
    print("Seasonal Period: December 20-30, 2025")
    print("=" * 60)
    print()
    
    # URLs to scrape
    url1 = "https://booking-hotels2.tiiny.site/"
    url2 = "https://hotel1.tiiny.site"
    
    # Scrape data from both websites
    all_hotels = []
    all_hotels.extend(scrape_hotel_data(url1, "DublinStays"))
    all_hotels.extend(scrape_hotel_data(url2, "LuxeHaven"))
    
    print()
    print(f"Total hotels scraped: {len(all_hotels)}")
    print()
    
    # Save to CSV file
    csv_filename = 'hotel_prices.csv'
    
    if all_hotels:
        print(f"Saving data to {csv_filename}...")
        
        # Define CSV columns
        fieldnames = ['Hotel Name', 'Location', 'Room Type', 'Price (EUR)', 
                     'Rating', 'Number of Reviews', 'Source']
        
        # Write to CSV
        with open(csv_filename, 'w', newline='', encoding='utf-8') as csvfile:
            writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
            writer.writeheader()
            writer.writerows(all_hotels)
        
        print(f"Data successfully saved to {csv_filename}")
    else:
        print("No data to save!")
    
    print()
    print("=" * 60)
    print("Retrieving and Displaying Data from CSV File")
    print("=" * 60)
    print()
    
    # Read and display data from CSV
    try:
        with open(csv_filename, 'r', encoding='utf-8') as csvfile:
            reader = csv.DictReader(csvfile)
            
            print(f"{'No.':<5} {'Hotel Name':<35} {'Location':<25} {'Price':<10} {'Rating':<8} {'Source':<15}")
            print("-" * 105)
            
            for idx, row in enumerate(reader, 1):
                hotel_name = row['Hotel Name'][:33] + '..' if len(row['Hotel Name']) > 35 else row['Hotel Name']
                location = row['Location'][:23] + '..' if len(row['Location']) > 25 else row['Location']
                price = f"€{row['Price (EUR)']}"
                rating = row['Rating']
                source = row['Source']
                
                print(f"{idx:<5} {hotel_name:<35} {location:<25} {price:<10} {rating:<8} {source:<15}")
            
            print("-" * 105)
            print()
            
    except FileNotFoundError:
        print(f"Error: {csv_filename} not found!")
    except Exception as e:
        print(f"Error reading CSV: {e}")
    
    print("Program completed successfully!")
    print()

if __name__ == "__main__":
    main()