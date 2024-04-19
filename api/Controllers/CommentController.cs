using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepository, IStockRepository stockRepository) : ControllerBase
    {
        private readonly ICommentRepository _commentRepository = commentRepository;
        private readonly IStockRepository _stockRepository = stockRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentsModel = comments.Select(c => c.toCommentDto());

            return Ok(commentsModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment.toCommentDto());
        }

        [HttpPost]
        [Route("{stockId}")]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto commentDto, [FromRoute] int stockId)
        {
            if (!await _stockRepository.StockExists(stockId)) return BadRequest("Stock does not exist!");

            var commentModel = commentDto.toCommentFromCreate(stockId);
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.toCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequestDto commentModel, [FromRoute] int id)
        {
            var commentResponse = await _commentRepository.UpdateAsync(id, commentModel.toCommentFromUpdate());
            if (commentResponse == null) return NotFound("Comment not found!");

            return Ok(commentResponse.toCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentModel = await _commentRepository.DeleteAsync(id);

            if (commentModel == null) return NotFound("Comment not found!");

            return Ok(commentModel.toCommentDto());
        }
    }
}