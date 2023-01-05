using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public ApplicationDBContext _db;
        public CompanyRepository(ApplicationDBContext context):base(context) 
        {
            _db = context;
        }
        public void Update(Company company)
        {
            _db.Companies.Update(company);
        }
    }
}
