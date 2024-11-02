using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; set; }

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
    // Create database connection
    static SqliteConnection CreateConnection()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=Library.db;");
        connection.Open();
        return connection;
    }

    // Initialize the database and create the Books table if it doesn't exist
    static void InitializeDatabase()
    {
        using (var connection = CreateConnection())
        {
            string createTableQuery = @"CREATE TABLE IF NOT EXISTS Books (
                                        Title TEXT NOT NULL,
                                        Author TEXT NOT NULL,
                                        ISBN TEXT PRIMARY KEY,
                                        IsAvailable BOOLEAN NOT NULL)";
            SqliteCommand command = new SqliteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }
    }

    // Add a new book to the database
    static void AddBook()
    {
        Console.Write("Enter title: ");
        string title = Console.ReadLine();
        Console.Write("Enter author: ");
        string author = Console.ReadLine();
        Console.Write("Enter ISBN: ");
        string isbn = Console.ReadLine();

        using (var connection = CreateConnection())
        {
            // Check for duplicate ISBN
            string checkQuery = "SELECT COUNT(*) FROM Books WHERE ISBN = @ISBN";
            SqliteCommand checkCommand = new SqliteCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@ISBN", isbn);
            int count = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (count > 0)
            {
                Console.WriteLine("A book with this ISBN already exists.");
                return;
            }

            // Insert the new book
            string insertQuery = "INSERT INTO Books (Title, Author, ISBN, IsAvailable) VALUES (@Title, @Author, @ISBN, @IsAvailable)";
            SqliteCommand command = new SqliteCommand(insertQuery, connection);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Author", author);
            command.Parameters.AddWithValue("@ISBN", isbn);
            command.Parameters.AddWithValue("@IsAvailable", true);

            command.ExecuteNonQuery();
            Console.WriteLine("Book added: " + title);
        }
    }

    // Display all books in the database
    static void DisplayAllBooks()
    {
        using (var connection = CreateConnection())
        {
            string query = "SELECT * FROM Books";
            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteDataReader reader = command.ExecuteReader();

            Console.WriteLine("\nList of Books:");
            while (reader.Read())
            {
                string title = reader["Title"].ToString();
                string author = reader["Author"].ToString();
                string isbn = reader["ISBN"].ToString();
                bool isAvailable = Convert.ToBoolean(reader["IsAvailable"]);
                Console.WriteLine($"Title: {title}, Author: {author}, ISBN: {isbn}, Available: {(isAvailable ? "Yes" : "No")}");
            }
        }
    }

    // Check out a book from the database by ISBN
    static void CheckoutBook()
    {
        Console.Write("Enter ISBN to check out: ");
        string isbn = Console.ReadLine();

        using (var connection = CreateConnection())
        {
            string query = "UPDATE Books SET IsAvailable = @IsAvailable WHERE ISBN = @ISBN AND IsAvailable = 1";
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IsAvailable", false);
            command.Parameters.AddWithValue("@ISBN", isbn);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected > 0 ? "Book checked out." : "Book is already checked out or not found.");
        }
    }

    // Return a book to the database by ISBN
    static void ReturnBook()
    {
        Console.Write("Enter ISBN to return: ");
        string isbn = Console.ReadLine();

        using (var connection = CreateConnection())
        {
            string query = "UPDATE Books SET IsAvailable = @IsAvailable WHERE ISBN = @ISBN AND IsAvailable = 0";
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@IsAvailable", true);
            command.Parameters.AddWithValue("@ISBN", isbn);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected > 0 ? "Book returned." : "Book was not checked out or not found.");
        }
    }

    // Remove a book from the database by ISBN
    static void RemoveBook()
    {
        Console.Write("Enter ISBN to remove: ");
        string isbn = Console.ReadLine();

        using (var connection = CreateConnection())
        {
            string query = "DELETE FROM Books WHERE ISBN = @ISBN";
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@ISBN", isbn);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected > 0 ? "Book removed." : "Book not found.");
        }
    }

    // Display the menu options
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
        InitializeDatabase();
        int choice;

        do
        {
            DisplayMenu();
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

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
