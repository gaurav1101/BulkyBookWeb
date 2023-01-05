
using BulkyBook.Models;

namespace BulkyBookWeb.Repository.IRepository
{
    public interface ICoverType : IRepository<CoverType>
    {
        void update(CoverType obj);
    }
}
