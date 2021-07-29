using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.DTOs;
using WebApiProject.Models;

namespace WebApiProject.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(m => m.AuthorName, map => map.MapFrom(book => book.Author.FirstName))
                .ForMember(m => m.AuthorSurName, map => map.MapFrom(book => book.Author.LastName));

            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<AuthorDTO, Author>();

            CreateMap<BookDTO, Book>().ReverseMap()
                .ForMember(m => m.AuthorName, map => map.MapFrom(book => book.Author.FirstName))
                .ForMember(m => m.AuthorSurName, map => map.MapFrom(book => book.Author.LastName));
        }
    }
}
