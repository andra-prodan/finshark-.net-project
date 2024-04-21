using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortofolioRepository : IPortofolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortofolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetUserPortofolio(AppUser user)
        {
            return await _context.Portofolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync();
        }

        public async Task<Portofolio> CreateUserPortofolio(Portofolio portofolio)
        {
            await _context.Portofolios.AddAsync(portofolio);
            await _context.SaveChangesAsync();

            return portofolio;
        }

        public async Task<Portofolio?> DeletePortofolio(List<Stock> stocks, string symbol)
        {
            var stock = stocks.FirstOrDefault(s => s.Symbol == symbol);

            var portofolio = await _context.Portofolios.FirstOrDefaultAsync(p => p.Stock.Symbol == stock.Symbol);

            if (portofolio == null) return null;

            _context.Portofolios.Remove(portofolio);
            await _context.SaveChangesAsync();

            return portofolio;
        }
    }
}