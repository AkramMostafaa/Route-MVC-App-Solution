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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
      

        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
            return _dbContext.Employees.Where(E=>E.Address.ToLower().Contains(address.ToLower()));
            
        }

        public IQueryable<Employee> SearchByName(string name)
        => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name));
    }
}
