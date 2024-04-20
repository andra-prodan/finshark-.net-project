using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController(IStockRepository stockRepository) : ControllerBase
    {
        private readonly IStockRepository _stockRepository = stockRepository;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var stocks = await _stockRepository.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());

            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var stockModel = stockDto.ToStockFromCreate();
            await _stockRepository.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var stockModel = await _stockRepository.UpdateAsync(id, stockDto);
            if (stockModel == null) return NotFound();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _stockRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
