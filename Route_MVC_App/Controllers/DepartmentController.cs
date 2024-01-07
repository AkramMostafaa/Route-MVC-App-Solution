using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Route.BLL.Interfaces;
using Route.DAL.Models;
using Route_MVC_App.PL.ViewModels;
using System;
using System.Collections.Generic;

namespace Route_MVC_App.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]  // GET : /Department/Index
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            var mappedDepartment= _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDepartment);
        }

      // /Department/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.DepartmentRepository.Add(mappedDepartment);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    return RedirectToAction(nameof(Index));

            }
            return View(departmentVM);
        }

        [HttpGet] // Departments/Details/1
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            var department = _unitOfWork.DepartmentRepository.Get(id.Value);
            var mappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);


            if (department is null)
                return NotFound();

            return View(viewName,mappedDepartment);

        }

        // /Department/Edit/1
        public IActionResult Edit(int? id)
        {
            ///if (!id.HasValue)
            ///    return BadRequest();
            ///var department = _departmentRepo.Get(id.Value);
            ///if(department is null)
            ///    return NotFound();
            ///return View(department);

            return Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                    _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                    
                }
                
            
            }
            return View(departmentVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id,"Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id,DepartmentViewModel departmentVM)
        {
            if(id != departmentVM.Id)
                return BadRequest();
            if(ModelState.IsValid)
                try
                {
                    var mappedDepartment = _mapper.Map<DepartmentViewModel,Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Delete(mappedDepartment);
                    _unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            return View(departmentVM);

        }
    }
}
