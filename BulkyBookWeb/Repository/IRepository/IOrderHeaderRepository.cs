using BulkyBook.Models;

namespace BulkyBookWeb.Repository.IRepository
{
	public interface IOrderHeaderRepository: IRepository<OrderHeader>
	{
		void Update(OrderHeader orderHeader);
		void UpdateOrderStatus(int id, string orderStatus,string? paymentStatus=null);
		void UpdateStripePaymentID(int id, string sessionId, string paymentId);
	}
}
