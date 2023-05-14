using BLL.Service;
using DAL.Models;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.Constants;

namespace Hospital.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly DepartmentService _departmentService;
        private readonly HospitalUserService _hospitalUserService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger, DoctorService providerService, DepartmentService departmentService, 
            HospitalUserService hospitalUserService)
        {
            _doctorService = providerService;
            _departmentService = departmentService;
            _hospitalUserService = hospitalUserService;
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Doctor> doctors = _doctorService.GetAll();
            doctors.Where(x => x.Department == null).ToList().ForEach(d => d.Department = _departmentService.GetDepartmentById(d.DepartmentId));

            DoctorViewModel doctorViewModel = new DoctorViewModel()
            { 
                Doctors = doctors,
            };

            return View(doctorViewModel);
        }

        [Authorize(Constants.adminRoleName)]
        public IActionResult Create()
        {
            IEnumerable<Doctor> doctors = _doctorService.GetAll();
            IEnumerable<Department> departments = _departmentService.GetAll();

            DoctorViewModel doctorViewModel = new DoctorViewModel()
            {
                Doctors = doctors,
                Departments = departments
            };

            return View(doctorViewModel);
        }

        [HttpPost]
        [Authorize(Constants.adminRoleName)]
        public async Task<IActionResult> Create(DoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                bool result = await _hospitalUserService.CreateHospitalUser(doctorViewModel, Constants.doctorRoleName);

                if(result == true)
                {
                    _logger.LogInformation("Create Doctor successfully");
                }
                else
                {
                    _logger.LogInformation("Something went wrong while create a Doctor");
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }

            return View(doctorViewModel);
        }

        [Authorize(Constants.adminRoleName)]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                Doctor doctor = _doctorService.GetDoctorById(id);
                if (doctor != null && doctor.DoctorId > 0)
                {
                    DoctorViewModel viewModel = new DoctorViewModel();
                    viewModel.DoctorId = doctor.DoctorId;
                    viewModel.DepartmentId = doctor.DepartmentId;
                    viewModel.FirstName = doctor.FirstName;
                    viewModel.LastName = doctor.LastName;
                    viewModel.DateOfBirth = doctor.DateOfBirth;
                    viewModel.Email = doctor.Email;
                    viewModel.Gender = doctor.Gender;
                    viewModel.Address = doctor.Address;
                    viewModel.PhoneNumber = doctor.PhoneNumber;
                    viewModel.Departments = _departmentService.GetAll();

                    return View(viewModel);
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
        [Authorize(Constants.adminRoleName)]
        public IActionResult Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _doctorService.Update(doctor);

                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        [Authorize(Constants.adminRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _doctorService.Delete(id);        

            if (result) { _logger.LogInformation("Doctor and ASP.Net User deleted successfully"); return RedirectToAction("Index"); }

            return NotFound();            
        }
    }
}
