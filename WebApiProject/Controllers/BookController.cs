using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.DTOs;
using WebApiProject.Errors;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    public class BookController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //book/list/1
        [HttpGet("list/")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookList(int? authorId = null)
        {
            var books = await unitOfWork.BookRepository.GetBooksAsync(authorId);
            var booksDTO = mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDTO);
        }

        [HttpGet("getAuthorBook/{authorId}")]
       // [Authorize]
        public async Task<IActionResult> GetAuthorBook(int authorId)
        {
            var books = await unitOfWork.BookRepository.GetBookByIdAsync(authorId);
            var booksDTO = mapper.Map<BookDTO>(books);
            return Ok(booksDTO);
        }

        //book/add
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddBook(BookDTO bookDTO)
        {
            var book = mapper.Map<Book>(bookDTO);
            unitOfWork.BookRepository.AddBook(book);
            await unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        [HttpDelete("deletBook/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await unitOfWork.BookRepository.GetBookByIdAsync(id);
            ApiError apiError = new ApiError();
            if (book == null)
            {
                apiError.ErrorCode = NotFound().StatusCode;
                apiError.ErrorMessage = "author not found.";
                return NotFound(apiError);
            }

            unitOfWork.BookRepository.DeleteBook(id);
            await unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPut("editBook")]
        public async Task<IActionResult> EditBook(BookDTO bookDTO)
        {
            ApiError apiError = new ApiError();
            if (!ModelState.IsValid)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                //apiError.ErrorMessage = ModelState;
                return BadRequest(ModelState);
            }

            var book = await unitOfWork.BookRepository.GetBookByIdAsync(bookDTO.Id);
            if (book == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "author not found.";
                return NotFound(apiError);
            }

            unitOfWork.BookRepository.EditBook(bookDTO);
            await unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
