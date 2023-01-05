using BulkyBook.Models;
using NuGet.Protocol;

namespace BulkyBookWeb.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product obj);
    }
}
