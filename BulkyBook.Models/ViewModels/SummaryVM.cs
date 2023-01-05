using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
	public class SummaryVM
	{
		public ApplicationUser applicationUser { get; set; }
		public ShoppingCartVM shoppingCartVM { get; set; }
	}
}
