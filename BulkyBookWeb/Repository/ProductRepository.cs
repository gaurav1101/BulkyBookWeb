using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class ProductRepository:Repository<Product> ,IProductRepository
    {
        internal ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db):base(db) 
        {
            _db= db;
        }
        public void Update(Product obj)
        {
            var objfromDB = GetFirstOrDefault(p=>p.id==obj.id);
            if (objfromDB != null)
            {
                objfromDB.ISBN = obj.ISBN;
                objfromDB.Author = obj.Author;
                objfromDB.Description = obj.Description;
                objfromDB.Price = obj.Price;
                objfromDB.ListPrice = obj.ListPrice;
                objfromDB.Price100 = obj.Price100;
                objfromDB.Price50 = obj.Price50;
                objfromDB.Category = obj.Category;
                if (obj.imgUrl != null)
                {
                    objfromDB.imgUrl= obj.imgUrl;
                }
            }
        }
    }
}
