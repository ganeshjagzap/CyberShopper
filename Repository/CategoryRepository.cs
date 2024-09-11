using Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Repository
{

    public class CategoryRepository<T> : ICategoryRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /*public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }*/
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
    }

}