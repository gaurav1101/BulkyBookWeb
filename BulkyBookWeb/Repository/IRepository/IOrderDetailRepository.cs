using BulkyBook.Models;

namespace BulkyBookWeb.Repository.IRepository
{
	public interface IOrderDetailRepository:IRepository<OrderDetail>
	{
		void Update(OrderDetail orderDetail);
	}
}
