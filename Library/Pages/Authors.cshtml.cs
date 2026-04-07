using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace MyApp.Namespace
{
    public class AuthorsModel : PageModel
    {
        // List that handles displaying DropdownList of authors
        public List<SelectListItem> AuthorList { get; set; }

        // List storing retrieved book information from query
        public List<Book> BookList { get; set; }

        // Bound value for the selected author last name
        [BindProperty]
        public string Author { get; set; }

        public void OnGet()
        {
            // Loads list of authors for database on page load
            LoadAuthorsList();
        }

        // When user selects an author from dropdown list, retrieves books associated with that author
        public IActionResult OnPost()
        {
            LoadAuthorsList();

            if (!string.IsNullOrWhiteSpace(Author))
            {
                GetBooksByAuthor(Author);
            }

            return Page();
        }
        //Helper method that loads list of authors from database
        private void LoadAuthorsList()
        {
            AuthorList = new List<SelectListItem>();
            string connectionString = "Server=localhost;Database=Library;User Id=sa;Password=s3ntinal;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Author_ID, A_Last_Name FROM Authors;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AuthorList.Add(new SelectListItem
                            {
                                //Author_ID as Value
                                Value = reader.GetInt32(0).ToString(),
                                //Author LastName as Text
                                Text = reader.GetString(1)
                            });
                        }
                    }
                }
            }
        }
        // Helper Method that retrieves List of Books By Parameterized Author
        public void GetBooksByAuthor(string author)
        {
            BookList = new List<Book>();
            //Database ConnectionString
            string connectionString = "Server=localhost;Database=Library;User Id=sa;Password=s3ntinal;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //Sql Query retrieving book information based on selected author
                string sql = @"
                SELECT b.Book_ID, b.BK_Title, b.ISBN,
                    a.A_First_Name, a.A_Last_Name,
                    b.BK_Publication_Year, p.P_Name
                FROM Books b
                JOIN Book_Authors ba ON b.Book_ID = ba.Book_ID
                JOIN Publishers p ON b.Publisher_ID = p.Publisher_ID
                JOIN Authors a ON ba.Author_ID = a.Author_ID
                WHERE a.A_Last_Name = @Author;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //Parameterized Query
                    command.Parameters.AddWithValue("@Author", author);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookList.Add(new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                ISBN = reader.GetString(2),
                                Author_First_Name = reader.GetString(3),
                                Author_Last_Name = reader.GetString(4),
                                Publication = reader.GetInt32(5).ToString(),
                                Publisher = reader.GetString(6)
                            });
                        }
                    }
                }
            }
        }
    }
    public class Book
    {
        public int Id {get; set;}
        public string Title {get; set;}
        public string ISBN {get; set;}
        public string Author_First_Name {get; set;}
        public string Author_Last_Name {get; set;}
        public string Publication {get; set;}
        public string Publisher {get; set;}
    }
}
