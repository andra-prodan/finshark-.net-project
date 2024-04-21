using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using Microsoft.AspNetCore.Identity;


namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    [Authorize]
    public class CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager) : ControllerBase
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IStockRepository _stockRepository = stockRepository;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var comments = await _commentRepository.GetAllAsync();
            var commentsModel = comments.Select(c => c.toCommentDto());

            return Ok(commentsModel);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment.toCommentDto());
        }

        [HttpPost]
        [Route("{stockId:int}")]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto commentDto, [FromRoute] int stockId)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (!await _stockRepository.StockExists(stockId)) return BadRequest("Stock does not exist!");

            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);

            var commentModel = commentDto.toCommentFromCreate(stockId, appUser.Id);
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.toCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequestDto commentModel, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var commentResponse = await _commentRepository.UpdateAsync(id, commentModel.toCommentFromUpdate());
            if (commentResponse == null) return NotFound("Comment not found!");

            return Ok(commentResponse.toCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var commentModel = await _commentRepository.DeleteAsync(id);

            if (commentModel == null) return NotFound("Comment not found!");

            return Ok(commentModel.toCommentDto());
        }
    }
}