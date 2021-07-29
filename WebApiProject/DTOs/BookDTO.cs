using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string ISBN { get; set; }
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurName { get; set; }
        public int TotalPages { get; set; }
        public string Description { get; set; }
    }
}
