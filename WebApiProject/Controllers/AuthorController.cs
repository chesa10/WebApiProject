using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Data.Interfaces;
using WebApiProject.Data.Repo;
using WebApiProject.DTOs;
using WebApiProject.Errors;
using WebApiProject.Models;

namespace WebApiProject.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddAuthor(AuthorDTO authorDTO)
        {
            var author = mapper.Map<Author>(authorDTO);
            unitOfWork.AuthorRepository.AddAuthor(author);
            await unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        //author/list
        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> GetAuthorList()
        {
            var authors = await unitOfWork.AuthorRepository.GetAuthorAsync();
            var authorDTO = mapper.Map<IEnumerable<AuthorDTO>>(authors);
            return Ok(authorDTO);
        }

        [HttpDelete("deletAuthor/{authorId}")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var author = await unitOfWork.AuthorRepository.GetAuthorByIdAsync(authorId);
            ApiError apiError = new ApiError();
            if (author == null)
            {
                apiError.ErrorCode = NotFound().StatusCode;
                apiError.ErrorMessage = "author not found.";
                return NotFound(apiError);
            }

            unitOfWork.AuthorRepository.DeleteAuthor(authorId);
            await unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpPut("editAuthor")]
        public async Task<IActionResult> EditAuthor(AuthorDTO authorDTO)
        {
            ApiError apiError = new ApiError();
            if (!ModelState.IsValid)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                //apiError.ErrorMessage = ModelState;
                return BadRequest(ModelState);
            }

            var author = await unitOfWork.AuthorRepository.GetAuthorByIdAsync(authorDTO.AuthorId);
            if (author == null)
            {
                apiError.ErrorCode = BadRequest().StatusCode;
                apiError.ErrorMessage = "author not found.";
                return NotFound(apiError);
            }

            unitOfWork.AuthorRepository.EditAuthor(authorDTO);
            await unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
