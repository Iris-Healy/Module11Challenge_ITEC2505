using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace MyApp.Namespace
{
    public class GenresModel : PageModel
    {
        // List that handles displaying DropdownList of genres
        public List<SelectListItem> GenreList { get; set; }

        // List storing retrieved book information from query
        public List<Book> BookList { get; set; }

        // Bound value for the selected genre
        [BindProperty]
        public string Genre { get; set; }

        public void OnGet()
        {
            // Loads list of genres for database on page load
            LoadGenresList();
        }

        // When user selects a genre from dropdown list, retrieves books associated with that genre
        public IActionResult OnPost()
        {
            LoadGenresList();

            if (!string.IsNullOrWhiteSpace(Genre))
            {
                GetBooksByGenre(Genre);
            }

            return Page();
        }
        //Helper method that loads list of genres from database
        private void LoadGenresList()
        {
            GenreList = new List<SelectListItem>();
            string connectionString = "Server=localhost;Database=Library;User Id=sa;Password=s3ntinal;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Genre_ID, Genre FROM Genres;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GenreList.Add(new SelectListItem
                            {
                                //Genre_ID as Value
                                Value = reader.GetInt32(0).ToString(),
                                //Genre as Text
                                Text = reader.GetString(1)
                            });
                        }
                    }
                }
            }
        }
        // Helper Method that retrieves List of Books By Parameterized Genre
        public void GetBooksByGenre(string genre)
        {
            BookList = new List<Book>();
            //Database ConnectionString
            string connectionString = "Server=localhost;Database=Library;User Id=sa;Password=s3ntinal;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Sql Query retrieving book information based on selected genre
                string sql = @"
                SELECT b.Book_ID, b.BK_Title, b.ISBN,
                    g.Genre, a.A_First_Name, a.A_Last_Name, b.BK_Publication_Year, p.P_Name
                FROM Books b
                JOIN Book_Genres bg 
                  ON b.Book_ID = bg.Book_ID
                JOIN Book_Authors ba 
                  ON b.Book_ID = ba.Book_ID
                JOIN Authors a
                  ON ba.Author_ID = a.Author_ID
                JOIN Publishers p
                  ON b.Publisher_ID = p.Publisher_ID
                JOIN Genres g ON bg.Genre_ID = g.Genre_ID
                WHERE g.Genre = @Genre;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //Parameterized Query
                    command.Parameters.AddWithValue("@Genre", genre);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Adds Retrieved Book Information to BookList
                            BookList.Add(new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                ISBN = reader.GetString(2),
                                Genre = reader.GetString(3),
                                Author_First_Name = reader.GetString(4),
                                Author_Last_Name = reader.GetString(5),
                                Publication = reader.GetString(6),
                                Publisher = reader.GetString(7)
                            });
                        }
                    }
                }
            }
        }
    }
}