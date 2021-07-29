using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Data.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync(int? authoerId);
        void AddBook(Book book);
        Task<Book> GetBookByIdAsync(int? id);
        void DeleteBook(int id);
        void EditBook(BookDTO bookDTO);
    }
}
