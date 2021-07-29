using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.Data.Repo;

namespace WebApiProject.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IBookRepository BookRepository => new BookRepository(dataContext);

        public IAuthorRepository AuthorRepository => new AuthorRepository(dataContext);

        public IUserRepository UserRepository => new UserRepository(dataContext);

        public async Task<bool> SaveAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }
    }
}
