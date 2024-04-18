using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StackController(ApplicationDBContext context) : ControllerBase
    {
        private readonly ApplicationDBContext _context = context;

        [HttpGet]
        public IActionResult GetAll(){
            var stocks = context.Stocks.ToList()
            .Select(s=>s.ToStockDto());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var stock = context.Stocks.Find(id);

            if(stock == null) return NotFound();

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreate();
            context.Stocks.Add(stockModel);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto){
            var stockModel = context.Stocks.FirstOrDefault(x=>x.Id == id);

            if( stockModel == null) return NotFound();

            stockModel.Symbol = stockDto.Symbol;
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.LastDiv = stockDto.LastDiv;
            stockModel.MarketCap = stockDto.MarketCap;

            context.SaveChanges();

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id){
            var stockModel = context.Stocks.Find(id);

            if (stockModel == null) return NotFound();

            context.Stocks.Remove(stockModel);
            context.SaveChanges();

            return NoContent();
            
        }
    }
}
