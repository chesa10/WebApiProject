using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Data.Repo
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext dataContext;

        public BookRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void AddBook(Book book)
        {
            var author = dataContext.Authors.Include(b => b.Books).FirstOrDefault(a => a.AuthorId == book.AuthorId);
            author.Books.Add(book);
        }

        public void DeleteBook(int id)
        {
            var book =  GetBookByIdAsync(id).Result;
            if (book != null)
            {
                dataContext.Books.Remove(book);
            }
        }

        public void EditBook(BookDTO bookDTO)
        {
            var book = dataContext.Books.FirstOrDefaultAsync(a => a.Id == bookDTO.Id).Result;
            if (book != null)
            {
                book.ISBN = bookDTO.ISBN;
                book.BookTitle = bookDTO.BookTitle;
                book.Description = bookDTO.Description;
                book.TotalPages = bookDTO.TotalPages;
                dataContext.Update(book);
            }
        }

        public async Task<Book> GetBookByIdAsync(int? id)
        {
            var book = await dataContext.Books
                .Include(a => a.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(int? authoerId)
        {
            var books = await dataContext.Books
                .Where(b => authoerId == null || b.AuthorId == authoerId)
                .Include(c => c.Author)
                .ToListAsync();

            return books;
        }
    }
}
