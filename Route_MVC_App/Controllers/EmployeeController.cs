using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Route.BLL.Interfaces;
using Route.DAL.Models;
using Route_MVC_App.PL.Helpers;
using Route_MVC_App.PL.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Route_MVC_App.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepo;
        private readonly IMapper _mapper;


        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //[HttpGet] // /Employee/Index
        public IActionResult Index(string searchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(searchInp))
                 employees = _unitOfWork.EmployeeRepository.GetAll();  
            else 
                 employees= _unitOfWork.EmployeeRepository.SearchByName(searchInp.ToLower());

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

                return View(mappedEmps);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
              employeeVM.ImageName=  DocumentSetting.UploadFile(employeeVM.Image, "images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(mappedEmployee);

                //if (count > 0)
                //    TempData["Message"] = "Employee Created Successfully";

                //else
                //    TempData["Message"] = "An Error Has Occured, Employee Not Created :(";


                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }

        [HttpGet]
        public IActionResult Details(int? id ,string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            var employee= _unitOfWork.EmployeeRepository.Get(id.Value);
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee == null)
                return NotFound();

            return View(viewName,mappedEmp);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            //ViewData["Departments"] = _departmentRepository.GetAll(); 

            return Details(id, "Edit");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit([FromRoute]int id,EmployeeViewModel employeeVM)
        {
            if(id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty,ex.Message);
            }
            return View(employeeVM);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return Details(id,"Delete");

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute]int id,EmployeeViewModel employeeVM)

        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
              var count =   _unitOfWork.Complete();
                if (count > 0)
                    DocumentSetting.DeleteFile(employeeVM.ImageName, "images");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                
            }
            return View(employeeVM);
            
        }


    }
}
