using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
	public class OrderDetailRepository: Repository<OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDBContext _db;
		public OrderDetailRepository(ApplicationDBContext db) : base(db)
		{
			_db = db;
		}

		public void Update(OrderDetail orderDetail)
		{
			_db.OrderDetails.Update(orderDetail);
		}
	}
}
