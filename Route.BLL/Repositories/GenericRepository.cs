using Microsoft.EntityFrameworkCore;
using Route.BLL.Interfaces;
using Route.DAL.Data;
using Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(T entity)
        {
            _dbContext.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public T Get(int id)
        {
          return  _dbContext.Find<T>(id);

        }

        public IEnumerable<T> GetAll()
        { 
            if (typeof(T) == typeof(Employee))
                return(IEnumerable<T>) _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
            else
                return _dbContext.Set<T>().AsNoTracking().ToList();
        }

          public int Update(T entity)
              {
            _dbContext.Update(entity);
            return (_dbContext.SaveChanges());
             }
    }
}
