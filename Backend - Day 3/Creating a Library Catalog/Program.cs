class Library
{
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Book> Books { get; set; }
    public List<MediaItem> MediaItems { get; set; }

    public Library(string name, string address)
    {
        Name = name;
        Address = address;
        Books = new List<Book>();
        MediaItems = new List<MediaItem>();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public void RemoveBook(Book book)
    {
        Books.Remove(book);
    }

    public void AddMediaItem(MediaItem item)
    {
        MediaItems.Add(item);
    }

    public void RemoveMediaItem(MediaItem item)
    {
        MediaItems.Remove(item);
    }

    public void PrintCatalog()
    {
        Console.WriteLine("\t\t\tBOOKS");
        foreach (var book in Books)
        {
            Console.WriteLine("{0} by {1} ({2})", book.Title, book.Author, book.PublicationYear);
        }

        Console.WriteLine("\n\tMEDIA ITEMS");
        foreach (var item in MediaItems)
        {
            Console.WriteLine("{0} \tType: {1} \tDuration: {2} minutes", item.Title, item.MediaType, item.Duration);
        }
    }

    public List<Book> SearchBooks(string searchQuery)
    {
        List<Book> results = new List<Book>();
        foreach (var book in Books)
        {
            if (book.Title.Contains(searchQuery) || book.Author.Contains(searchQuery) || book.ISBN.Contains(searchQuery))
            {
                results.Add(book);
            }
        }
        return results;
    }

    public List<MediaItem> SearchMediaItems(string searchQuery)
    {
        List<MediaItem> results = new List<MediaItem>();
        foreach (MediaItem item in MediaItems)
        {
            if (item.Title.Contains(searchQuery) || item.MediaType.Contains(searchQuery))
            {
                results.Add(item);
            }
        }
        return results;
    }
}

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }

    public Book(string title, string author, string isbn, int publicationYear)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        PublicationYear = publicationYear;
    }
}

class MediaItem
{
    public string Title { get; set; }
    public string MediaType { get; set; }
    public int Duration { get; set; }

    public MediaItem(string title, string mediaType, int duration)
    {
        Title = title;
        MediaType = mediaType;
        Duration = duration;
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library("Abrhot Library", "4 Kilo, Addis Ababa, Ethiopia");
        library.AddBook(new Book("Fiker Eske Mekabir", "Hadis Alemayehu", "0123456789", 1970));
        library.AddBook(new Book("Oromay", "Bealu Girma", "9876543210", 1980));
        library.AddBook(new Book("Sememen", "Sisay Nigusu", "1111111111", 1990));
        library.AddMediaItem(new MediaItem("Yewendoch Guday", "CD", 120));
        library.AddMediaItem(new MediaItem("Yeberedo Zemen", "DVD", 150));
        library.AddMediaItem(new MediaItem("400 Fikir", "CD", 120));

        Console.WriteLine($"\t\t\t{library.Name.ToUpper()}");

        string command;
        do
        {
            Console.WriteLine("\nEnter 'p' to print the catalogue, 's' to search for books or media and 'q' to quit: ");

            command = Console.ReadLine().ToLower();
            if (command == "p")
            {
                library.PrintCatalog();
            }
            else if(command == "s")
            {
                Console.WriteLine("\nEnter a search query (title, author, ISBN, or media type):");
                string searchQuery = Console.ReadLine();
                List<Book> books = library.SearchBooks(searchQuery);
                List<MediaItem> mediaItems = library.SearchMediaItems(searchQuery);

                if (books.Count > 0)
                {
                    Console.WriteLine("\t\t\tBOOKS");
                    foreach (Book book in books)
                    {
                        Console.WriteLine("{0} by {1} ({2})", book.Title, book.Author, book.PublicationYear);
                    }
                }
                else
                {
                    Console.WriteLine("\nNo matching books found.\n");
                }
                if (mediaItems.Count > 0)
                {
                    Console.WriteLine("\n\t\t\tMEDIA ITEMS");
                    foreach (MediaItem item in mediaItems)
                    {
                        Console.WriteLine("{0} \tType: {1} \tDuration: {2} minutes", item.Title, item.MediaType, item.Duration);
                    }
                }
                else
                {
                    Console.WriteLine("\nNo matching media items found.");
                }
                    }
            else if (command != "q")
            {
                Console.WriteLine("Invalid input.\n");
            }
        } while (command != "q");
    }
}
