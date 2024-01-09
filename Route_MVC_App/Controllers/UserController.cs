using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route.DAL.Models;
using Route_MVC_App.PL.Helpers;
using Route_MVC_App.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Route_MVC_App.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var user =await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync(); 
                return View(user);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);

                var mappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel>() { mappedUser });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var mappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);


            return View(viewName, mappedUser);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            //ViewData["Departments"] = _departmentRepository.GetAll(); 

            return await Details(id, "Edit");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                user.FName=userVM.FName;
                user.LName=userVM.LName;
                user.PhoneNumber=userVM.PhoneNumber;
                //user.Email=userVM.Email;
                //user.SecurityStamp=Guid.NewGuid().ToString();


                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ConfirmDelete( string id)

        {
    
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
            

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                return RedirectToAction("Error","Home");

            }
           

        }
    }
}
