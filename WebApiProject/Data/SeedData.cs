using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Data
{
    public class SeedData
    {
        private readonly DataContext dataContext;

        public SeedData(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Seed()
        {
            if (dataContext.Database.CanConnect())
            {
                if (!dataContext.Users.Any())
                {
                    InsertSampleData();
                }
            }
        }

        private void InsertSampleData()
        {
            byte[] passwordHash, passwordKey;
            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Chesa@10"));
            }

            var user = new User()
            {
                Username = "Chesa",
                Password = passwordHash,
                PasswordKey = passwordKey,
                Email = "Chesa@gmail.com",
                Mobile = "0799610258"
            };

            var authors = new List<Author>
            {
                new Author
                {
                    FirstName = "William",
                    LastName = "Mmako",
                    Books = new List<Book>
                    {
                        new Book
                        {
                            ISBN = "dfdfs45354335",
                            BookTitle = "Programing 101",
                            TotalPages = 155,
                            Description = "Programing 101"
                        },
                        new Book
                        {
                            ISBN = "dfddfsffs45354335",
                            BookTitle = "Data Structure",
                            TotalPages = 150,
                            Description = "Data Structure"
                        },
                    }
                },
                new Author
                {
                    FirstName = "Bob",
                    LastName = "Jones",
                    Books = new List<Book>
                    {
                        new Book
                        {
                            ISBN = "dfd45354335",
                            BookTitle = "Programing for real",
                            TotalPages = 155,
                            Description = "Programing for real"
                        },
                        new Book
                        {
                            ISBN = "dfddffs45355",
                            BookTitle = "Data for dummies",
                            TotalPages = 150,
                            Description = "Data for dummies"
                        }
                    }
                }
            };

            dataContext.AddRange(authors);
            dataContext.Users.Add(user);
        }

    }
}
