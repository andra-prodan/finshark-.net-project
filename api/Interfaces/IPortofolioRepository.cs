using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortofolioRepository
    {
        Task<List<Stock>> GetUserPortofolio(AppUser user);
        Task<Portofolio> CreateUserPortofolio(Portofolio portofolio);
        Task<Portofolio> DeletePortofolio(List<Stock> portofolios, string symbol);
    }
}