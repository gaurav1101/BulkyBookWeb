using BulkyBook.DataAccess;
using BulkyBook.Models.ViewModels;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class ShoppingCartRepository:Repository<ShoppingCart> ,IShoppingCartRepository
    {
        internal ApplicationDBContext _db;
        public ShoppingCartRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }
       public int IncrementCount(ShoppingCart cart, int count)
        {
            cart.count += count;
            return cart.count;
        }

        public int DecrementCount(ShoppingCart cart, int count)
        {
            cart.count -= count;
            return cart.count;
        }
    }
}
