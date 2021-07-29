using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Data.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAuthorAsync();
        Task<Author> GetAuthorByIdAsync(int? AuthorId);
        void AddAuthor(Author author);
        void DeleteAuthor(int AuthorId);
        void EditAuthor(AuthorDTO author);
    }
}
