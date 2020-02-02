using Gregslist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gregslist.Services
{
    public interface IPostingService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetAllForUser(User user);
        Task<Product> GetById(int id);
        Task<Product> Create(Product newData);
        Task<int> Edit(Product newData);
        Task<bool> Delete(int id);
    }
}
