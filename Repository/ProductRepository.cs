using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ecommerce.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ecommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext con;

        public ProductRepository(ApplicationDbContext context)
        {
            con = context;
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            return con.OrderDetails.GroupBy(o => o.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    TotalSum = x.Sum(o => o.Quantity)
                })
                .OrderByDescending(x => x.TotalSum)
                .Take(12)
                .Join(con.Products,
                x => x.ProductId,
                p => p.ProductId,
                (x, p) => new Product
                {
                    ProductId= p.ProductId,
                    ModelName= p.ModelName,
                    UnitCost= p.UnitCost,
                    Description= p.Description
                }).ToList();
        }
    }
}
