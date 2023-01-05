using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
	public class OrderHeaderRepository: Repository<OrderHeader>,IOrderHeaderRepository
	{
		private readonly ApplicationDBContext _db;
		public OrderHeaderRepository(ApplicationDBContext db):base(db) 
		{
			_db = db;
		}

		public void Update(OrderHeader orderHeader)
		{
			_db.OrderHeaders.Update(orderHeader);
		}

		public void UpdateOrderStatus(int id, string orderStatus, string? paymentStatus)
		{
			var obj= _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if(obj != null)
			{
				obj.OrderStatus=orderStatus;
			}
			if(paymentStatus != null)
			{
				obj.PaymentStatus=paymentStatus;
			}
		}

		public void UpdateStripePaymentID(int id, string sessionId, string paymentId)
		{
			var obj = _db.OrderHeaders.FirstOrDefault(u=>u.Id==id);
			obj.SessionId = sessionId;
			obj.PaymentIntentId=paymentId;

		}
	}
}
