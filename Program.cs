using System;
using System.Collections.Generic;

struct Book
{
    public string Title;
    public string Author;
    public string ISBN;
    public bool IsAvailable;

    // Constructor for Book struct
    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsAvailable = true;
    }
}

class Program
{
    // Collection of books in the library
    static List<Book> books = new List<Book>();

    // Add a new book to the library
    static void AddBook()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        Console.Write("Enter author: ");
        string author = Console.ReadLine();
        Console.Write("Enter ISBN: ");
        string isbn = Console.ReadLine();

        Book book = new Book(title, author, isbn);
        books.Add(book);
        Console.WriteLine("Book added: " + book.Title);
    }

    // Remove a book from the library by ISBN
    static void RemoveBook()
    {
        Console.Write("Enter ISBN to remove: ");
        string isbn = Console.ReadLine();

        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].ISBN == isbn)
            {
                Console.WriteLine("Book removed: " + books[i].Title);
                books.RemoveAt(i);
                return;
            }
        }
        Console.WriteLine("Book not found.");
    }

    // Display all books in the library
    static void DisplayAllBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books in the library.");
            return;
        }

        Console.WriteLine("\nList of Books:");
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}, Available: {(book.IsAvailable ? "Yes" : "No")}");
        }
    }

    // Check out a book from the library by ISBN
    static void CheckoutBook()
    {
        Console.Write("Enter ISBN to check out: ");
        string isbn = Console.ReadLine();

        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].ISBN == isbn)
            {
                if (books[i].IsAvailable)
                {
                    // Retrieve the struct, modify it, and reassign it
                    Book bookToCheckout = books[i];
                    bookToCheckout.IsAvailable = false;
                    books[i] = bookToCheckout;
                    Console.WriteLine("Book checked out: " + books[i].Title);
                }
                else
                {
                    Console.WriteLine("Book is already checked out.");
                }
                return;
            }
        }
        Console.WriteLine("Book not found.");
    }

    // Return a book to the library by ISBN
    static void ReturnBook()
    {
        Console.Write("Enter ISBN to return: ");
        string isbn = Console.ReadLine();

        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].ISBN == isbn)
            {
                if (!books[i].IsAvailable)
                {
                    // Retrieve the struct, modify it, and reassign it
                    Book bookToReturn = books[i];
                    bookToReturn.IsAvailable = true;
                    books[i] = bookToReturn;
                    Console.WriteLine("Book returned: " + books[i].Title);
                }
                else
                {
                    Console.WriteLine("Book was not checked out.");
                }
                return;
            }
        }
        Console.WriteLine("Book not found.");
    }

    // Function to display the menu options
    static void DisplayMenu()
    {
        Console.WriteLine("\nLibrary Management System");
        Console.WriteLine("1. Add Book");
        Console.WriteLine("2. Remove Book");
        Console.WriteLine("3. Display All Books");
        Console.WriteLine("4. Check Out Book");
        Console.WriteLine("5. Return Book");
        Console.WriteLine("0. Exit");
        Console.Write("Enter your choice: ");
    }

    // Main function to run the Library Management System
    static void Main()
    {
        int choice;

        do
        {
            DisplayMenu();
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    RemoveBook();
                    break;
                case 3:
                    DisplayAllBooks();
                    break;
                case 4:
                    CheckoutBook();
                    break;
                case 5:
                    ReturnBook();
                    break;
                case 0:
                    Console.WriteLine("Exiting the program.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        } while (choice != 0);
    }
}
