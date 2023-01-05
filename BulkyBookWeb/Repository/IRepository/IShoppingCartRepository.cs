using BulkyBook.Models;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Repository.IRepository
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        int IncrementCount(ShoppingCart cart,int count);
        int DecrementCount(ShoppingCart cart, int count);
    }
}
