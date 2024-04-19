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
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto comment, [FromRoute] int stockId)
        {
            var stock = await _stockRepository.GetByIdAsync(stockId);
            if (stock == null) return BadRequest();

            var commentModel = comment.toCommentFromCreate();
            commentModel.StockId = stockId;
            commentModel.Stock = stock;
            var commentResponse = await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.toCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequestDto commentModel, [FromRoute] int id)
        {
            var commentResponse = await _commentRepository.UpdateAsync(id, commentModel);

            if (commentResponse == null) return NotFound();

            return Ok(commentResponse);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _commentRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}