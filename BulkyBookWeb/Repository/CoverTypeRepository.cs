using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;

namespace BulkyBookWeb.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverType
    {
        private ApplicationDBContext _db;
        public CoverTypeRepository(ApplicationDBContext db):base(db)
        {
            _db = db;
        }
        public void update(CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
