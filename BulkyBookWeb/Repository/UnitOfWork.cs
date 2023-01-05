using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _db;
        
        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            category = new CategoryRepository(_db);
            covertype = new CoverTypeRepository(_db);
            productRepository = new ProductRepository(_db);
            companyRepository= new CompanyRepository(_db);
            applicationUserRepository=new ApplicationUserRepository(_db);
            shoppingCartRepository= new ShoppingCartRepository(_db);
            orderHeaderRepository= new OrderHeaderRepository(_db);
            orderDetailRepository= new OrderDetailRepository(_db);
        }
        public ICategoryRepository category { get; private set; }
        public ICoverType covertype { get; private set; }
        public IProductRepository productRepository { get; private set; }
        public ICompanyRepository companyRepository { get; private set; }
        public IApplicationUserRepository applicationUserRepository { get; private set; }
        public IShoppingCartRepository shoppingCartRepository { get; private set; }
		public IOrderHeaderRepository orderHeaderRepository { get; private set; }
		public IOrderDetailRepository orderDetailRepository { get; private set; }
		public void save()
        {
            _db.SaveChanges();
        }
    }
}
