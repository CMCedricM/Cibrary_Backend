﻿using Cibrary_Backend.Services;
using Cibrary_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cibrary_Backend.Errors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Cibrary_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {

        private readonly BooksServices _context;
        public BooksController(BooksServices context)
        {
            _context = context;
        }


        private readonly BookProfile[] books =
        {
            new BookProfile
            {
                Title = "I am book",
                Isbn = "217316328",
                ID = 1,
                TotalCnt = 0,
            },
             new BookProfile
            {
                Title = "I am book",
                Isbn = "217316328",
                ID = 2,
                TotalCnt = 80,
            },
        };

        [HttpGet("count")]
        [Authorize]
        public async Task<ActionResult<int>> GetCount()
        {

            int cnt = await _context.GetBookCountAsync();
            return Ok(cnt);
        }

        [HttpGet("test")]
        [Authorize]
        public ActionResult<BookProfile[]> GetTestBooks()
        {
            return Ok(books);
        }


        [HttpPost("createABook")]
        [Authorize]
        public async Task<ActionResult<BookProfile>> CreateABook(BookProfile book)
        {
            try
            {
                var newBook = await _context.CreateBookAsync(book);
                return Ok(newBook);
            }
            catch (ConflictFound c)
            {
                return Conflict(c.ToErrorResponse());
            }

        }

        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<List<BookProfile>?>> FindABookTitle([FromQuery] BookSearch query)
        {

            var res = await _context.FindABook(query);
            if (res == null) return NotFound("No Books Matching Found!");

            return res;
        }


        [HttpGet("/isbn/{isbn}")]
        public async Task<ActionResult<BookProfile?>> GetBookByISBN(string isbn)
        {
            var aBook = await _context.GetBookByISBN(isbn);

            if (aBook == null) return NotFound();

            return Ok(aBook);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookProfile?>> GetBookById([FromRoute] int id)
        {
            var aBook = await _context.GetBookById(id);

            if (aBook == null) return NotFound();

            return Ok(aBook);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<BookProfile>> UpdateABook(int id, [FromBody] BookProfile req)
        {

            try
            {
                var updatedBook = await _context.UpdateBook(id, req);
                return Ok(updatedBook);
            }
            catch (ForbiddenFieldException err)
            {
                return BadRequest(err.ToErrorResponse());
            }

        }

    }
}
