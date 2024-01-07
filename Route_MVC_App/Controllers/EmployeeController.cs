using Microsoft.AspNetCore.Mvc;
using Route.BLL.Interfaces;
using Route.DAL.Models;
using System;

namespace Route_MVC_App.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepo/*,IDepartmentRepository departmentRepository*/)
        {
            _employeeRepo = employeeRepo;
            //_departmentRepository = departmentRepository;
        }


        [HttpGet] // /Employee/Index
        public IActionResult Index()
        {
            //  Binding Through View's Dictionary : Transfer Data From Action to View   [One Way ]

            // 1. ViewData 
            ViewData["Message"] = "Hello View Data ";
            // 2. ViewBag
            ViewBag.Bag = "Hello View Bag";

            var employees = _employeeRepo.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
               
               var count =  _employeeRepo.Add(employee);

                if (count > 0)
                    TempData["Message"] = "Employee Created Successfully";

                else
                    TempData["Message"] = "An Error Has Occured, Employee Not Created :(";
                
                return RedirectToAction(nameof(Index));

            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Details(int? id ,string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            var employee= _employeeRepo.Get(id.Value);

            if (employee == null)
                return NotFound();

            return View(viewName,employee);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            //ViewData["Departments"] = _departmentRepository.GetAll(); 

            return Details(id, "Edit");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit([FromRoute]int id,Employee employee)
        {
            if(id != employee.Id)
                return BadRequest();
            try
            {
                _employeeRepo.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty,ex.Message);
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return Details(id,"Delete");

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute]int id,Employee employee)

        {
            if (id != employee.Id)
                return BadRequest();
            try
            {
                _employeeRepo.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                
            }
            return View(employee);
            
        }


    }
}
