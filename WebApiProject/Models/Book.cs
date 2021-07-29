using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiProject.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string BookTitle { get; set; }
        public int TotalPages { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
    }
}
