using BulkyBook.DataAccess;
using BulkyBook.Models;
using BulkyBookWeb.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;


namespace BulkyBookWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BulkyBook.DataAccess.ApplicationDBContext _db;
        internal DbSet<T> _dbset;
        public Repository(BulkyBook.DataAccess.ApplicationDBContext db)
        {
            _db = db;
            this._dbset = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

       public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties=null)
        {

            IQueryable<T> query = _dbset;
            if(filter!= null)
            {
                query = query.Where(filter);
            }
           // query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach(var prop in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
           return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = _dbset;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            
            _dbset.Remove(entity);
        }

       public void RemoveRange(IEnumerable<T> entity)
        {
            _dbset.RemoveRange(entity);
        }
    }   
}
