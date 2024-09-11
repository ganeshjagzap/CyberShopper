using System.Collections.Generic;

namespace Ecommerce.Repository
{
    public interface ICategoryRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}