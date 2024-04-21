using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using api.Models;
using api.Interfaces;
using api.Extensions;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PortofolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortofolioRepository _portofolioRepository;
        public PortofolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortofolioRepository portofolioRepository)
        {
            _stockRepository = stockRepository;
            _userManager = userManager;
            _portofolioRepository = portofolioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortofolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortofolio = await _portofolioRepository.GetUserPortofolio(appUser);

            return Ok(userPortofolio);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserPortofolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepository.GetBySymbolAsync(symbol);

            if (stock == null) return NotFound();

            var userPortofolio = await _portofolioRepository.GetUserPortofolio(appUser);

            if (userPortofolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest();

            var portofolioModel = new Portofolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portofolioRepository.CreateUserPortofolio(portofolioModel);

            if (portofolioModel == null) return StatusCode(500, "Could not create");

            return Created();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePorofolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var stocks = await _portofolioRepository.GetUserPortofolio(appUser);

            var portofolio = await _portofolioRepository.DeletePortofolio(stocks, symbol);

            if (portofolio == null) return BadRequest("Portofolio not found");

            return Ok(portofolio);
        }
    }
}