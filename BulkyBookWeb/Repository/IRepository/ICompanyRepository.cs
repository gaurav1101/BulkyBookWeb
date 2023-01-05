using BulkyBook.Models;

namespace BulkyBookWeb.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public void Update(Company company);
    }
}
