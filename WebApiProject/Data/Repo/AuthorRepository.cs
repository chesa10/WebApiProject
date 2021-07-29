using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Data.Repo
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext dataContext;

        public AuthorRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public void AddAuthor(Author author)
        {
            dataContext.Authors.Add(author);
        }

        public  void DeleteAuthor(int authorId)
        {
            var author = dataContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId).Result;
            if (author != null)
            {
                var books = author.Books;
                dataContext.Books.RemoveRange(books);
                dataContext.Authors.Remove(author);
            }
        }

        public void EditAuthor(AuthorDTO author)
        {
            var autho = dataContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == author.AuthorId).Result;
            if (author != null)
            {
                autho.LastName = author.LastName;
                autho.FirstName = author.FirstName;
                dataContext.Update(autho);
            }
        }

        public async Task<IEnumerable<Author>> GetAuthorAsync()
        {
            var authors = await dataContext.Authors.ToListAsync();
            return authors;
        }

        public async Task<Author> GetAuthorByIdAsync(int? authorId)
        {
            var author = await dataContext.Authors
                .Include(b => b.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);
            return author;
        }
    }
}
