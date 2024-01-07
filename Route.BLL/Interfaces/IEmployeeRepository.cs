using Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeeByAddress(string address);


    }
}
