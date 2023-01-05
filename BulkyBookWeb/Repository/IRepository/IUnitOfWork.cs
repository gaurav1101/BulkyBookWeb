namespace BulkyBookWeb.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository category { get; }
        ICoverType covertype { get; }
        IProductRepository productRepository { get; }
        ICompanyRepository companyRepository { get; }
        IApplicationUserRepository applicationUserRepository { get; }
        IShoppingCartRepository shoppingCartRepository { get; }
		IOrderHeaderRepository orderHeaderRepository { get; }
		IOrderDetailRepository orderDetailRepository { get; }
		void save();
    }
}
