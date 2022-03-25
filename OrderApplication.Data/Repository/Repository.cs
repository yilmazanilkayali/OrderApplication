using Microsoft.EntityFrameworkCore;
using OrderApplication.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;//kalıtım alınabilen sınıflarda kullanılabilmek için bu şekilde tanımlandı.
        //Dependency Injection için ctor
        public Repository(ApplicationDbContext contex)
        {
            _context = contex; 
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable(); //_dbSet.AsQueryable();
            if(filter != null)
            {
                query = query.Where(filter);//Expression func tanımını where sorgusu içerisine yazdık. (x=>x.id==1)
            }
            if (includeProperties!=null)
            { 
                //"Product , Order..."
                //include ediilecek tablolar split ile birbirinden ayırıldı.
                foreach (var item in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
