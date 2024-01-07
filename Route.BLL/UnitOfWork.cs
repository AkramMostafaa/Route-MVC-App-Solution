using Route.BLL.Interfaces;
using Route.BLL.Repositories;
using Route.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.BLL
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly AppDbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get ; set ; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            EmployeeRepository = new EmployeeRepository(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
        }
        public int Complete()
        =>   _dbContext.SaveChanges();
        

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
