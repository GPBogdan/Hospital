using BLL.Service;
using DAL.Constants;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly DoctorService _doctorService;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(ILogger<DepartmentController> logger, DepartmentService departmentService, DoctorService doctorService)
        {
            _departmentService = departmentService;
            _logger = logger;
            _doctorService = doctorService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Department>? medicalProcedureList = _departmentService.GetAll();

            return View(medicalProcedureList);
        }

        [Authorize(Roles = Constants.adminRoleName)]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = Constants.adminRoleName)]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                if (department != null)
                {
                    _departmentService.Create(department);
                }

                TempData["ResultOk"] = "Department Added Successfully!";
                return RedirectToAction("Index");
            }

            return View(department);
        }

        [Authorize(Roles = Constants.adminRoleName)]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                Department department = _departmentService.GetDepartmentById(id);
                if (department != null)
                {
                    return View(department);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = Constants.adminRoleName)]
        public IActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentService.Update(department);

                TempData["ResultOk"] = "Department data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(department);
        }

        [Authorize(Roles = Constants.adminRoleName)]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                Department department = _departmentService.GetDepartmentById(id);

                IEnumerable<Doctor> doctorsInDepartment = _doctorService.GetAll().Where(x => x.DepartmentId == department.DepartmentId);
                if (doctorsInDepartment != null && doctorsInDepartment.Count() > 0)
                {
                    _doctorService.DeleteListOfDoctors(doctorsInDepartment);
                    _departmentService.Delete(department);
                    return RedirectToAction("Index");
                }
                else
                {
                    _departmentService.Delete(department);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
