using Gregslist.Data;
using Gregslist.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gregslist.Services
{
    public class EFPostingService : IPostingService
    {
        private readonly ApplicationDbContext _context;

        public EFPostingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Create(Product newData)
        {
            _context.Product.Add(newData);
            await _context.SaveChangesAsync();
            return newData;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _context.Product.FirstOrDefaultAsync(p => p.ID == id);
            if (product == null)
            {
                return false;
            }
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> Edit(Product newData)
        {
            var product = await _context.Product.Include(p => p.Owner).FirstOrDefaultAsync(p => p.ID == newData.ID);
            if (product == null)
            {
                return (await Create(newData)).ID;
            }

            product.Title = newData.Title;
            product.Description = newData.Description;
            product.Price = newData.Price;
            product.Owner = newData.Owner;
            await _context.SaveChangesAsync();
            return product.ID;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Product.Include(p => p.Owner).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllForUser(User user)
        {
            return await _context.Product.Include(p => p.Owner).Where(p => p.Owner.Id == user.Id).ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Product
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.ID == id);
        }
    }
}
