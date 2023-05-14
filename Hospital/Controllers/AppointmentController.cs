using BLL.Service;
using BLL.ViewModels;
using DAL.Constants;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;
        private readonly DepartmentService _departmentService;
        private readonly MedicalProcedureService _medicalProcedureService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(ILogger<AppointmentController> logger, AppointmentService providerService, 
            DoctorService doctorService , MedicalProcedureService medicalProcedureService, PatientService patientService, DepartmentService departmentService)
        {
            _appointmentService = providerService;
            _patientService = patientService;
            _doctorService = doctorService;
            _medicalProcedureService = medicalProcedureService;
            _departmentService = departmentService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Appointment>? appointmentList = null;
            IEnumerable<Patient>? patients = null;

            if (User.IsInRole(Constants.adminRoleName)) { appointmentList = _appointmentService.GetAll(); patients = _patientService.GetAll();  }
            else if(User.IsInRole(Constants.doctorRoleName)) 
            { 
                appointmentList = _appointmentService.GetAll().Where(x => x.Doctor.Email == User.Identity.Name).OrderBy(y => y.AppointmentDate);
                patients = _patientService.GetAll();
            }
            else { appointmentList = _appointmentService.GetAll().Where(x => x.Patient.Email == User.Identity.Name).OrderBy(y => y.AppointmentDate);
                patients = new List<Patient>();
            }
            
            IEnumerable<Doctor> doctorList = _doctorService.GetAll();
            doctorList.Where(x => x.Department == null).ToList().ForEach(d => d.Department = _departmentService.GetDepartmentById(d.DepartmentId));
            IEnumerable<MedicalProcedure> medicalProcedure = _medicalProcedureService.GetAll();

            Patient patient = _patientService.GetPatientByEmail(User.Identity.Name);

            AppointmentViewModel appointmentViewModel = new AppointmentViewModel(appointmentList, doctorList, medicalProcedure, patients, patient.PatientId);

            return View(appointmentViewModel);
        }

        public IActionResult Create()
        {
            IEnumerable<Appointment> appointmentList = _appointmentService.GetAll().Where(x => x.Patient.Email == User.Identity.Name).OrderBy(y => y.AppointmentDate);
            IEnumerable<Doctor> doctorList = _doctorService.GetAll();
            doctorList.Where(x => x.Department == null).ToList().ForEach(d => d.Department = _departmentService.GetDepartmentById(d.DepartmentId));

            IEnumerable<MedicalProcedure> medicalProcedure = _medicalProcedureService.GetAll();

            Patient patient = _patientService.GetPatientByEmail(User.Identity.Name);

            IEnumerable<Patient> patients = _patientService.GetAll();

           AppointmentViewModel appointmentViewModel = new AppointmentViewModel(appointmentList, doctorList, medicalProcedure, patients, patient.PatientId);

            return View(appointmentViewModel);
        }

        [HttpPost]
        public IActionResult Create(AppointmentViewModel appointmentViewModel)
        {
            if (ModelState.IsValid)
            {

                Appointment appointmentResult = _appointmentService.CreateAppointmentFromViewModel(appointmentViewModel);

                if (appointmentResult != null && appointmentResult.AppointmentId > 0)
                {
                    ViewBag.Message = "Enrollment successful, thank you for being with us";
                    return RedirectToAction("Index");
                }
                else
                {
                    IEnumerable<Appointment> appointmentList = new List<Appointment>();
                    IEnumerable<Doctor> doctorList = _doctorService.GetAll();
                    doctorList.Where(x => x.Department == null).ToList().ForEach(d => d.Department = _departmentService.GetDepartmentById(d.DepartmentId));

                    IEnumerable<MedicalProcedure> medicalProcedure = _medicalProcedureService.GetAll();

                    Patient patient = _patientService.GetPatientByEmail(User.Identity.Name);
                    IEnumerable<Patient> patients = _patientService.GetAll();

                    AppointmentViewModel viewModel = new AppointmentViewModel(appointmentList, doctorList, medicalProcedure, patients, patient.PatientId);
                    ModelState.AddModelError("Error", "Cannot make an appointment, please choose another date or procedure");
                    return View(viewModel);
                }             
            }

            return ValidationProblem();
        }

        [Authorize(Roles = Constants.adminRoleName + "," + Constants.doctorRoleName)]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                Appointment appointment = _appointmentService.GetAppointmentById(id);
                if (appointment != null && appointment.AppointmentId > 0)
                {
                    AppointmentViewModel viewModel = new AppointmentViewModel();
                    viewModel.AppointmentId = appointment.AppointmentId;
                    viewModel.AppointmentDate = appointment.AppointmentDate;
                    viewModel.Time = appointment.StartTime + "-" + appointment.EndTime;
                    viewModel.DoctorId = appointment.DoctorId;
                    viewModel.DoctorList = _doctorService.GetAll();
                    viewModel.Patients = _patientService.GetAll();
                    viewModel.MedicalProcedureList = _medicalProcedureService.GetAll();
                    viewModel.ProcedureId = appointment.ProcedureId;
                    viewModel.PatientId = appointment.PatientId;
                    viewModel.AppointmentConclusion = appointment.AppointmentConclusion;
                    viewModel.AppointmentStatus = appointment.AppointmentStatus;

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
        [Authorize(Roles = Constants.adminRoleName + "," + Constants.doctorRoleName)]
        public IActionResult Edit(AppointmentViewModel appointmentViewModel)
        {
            if (ModelState.IsValid)
            {
                string[] time = appointmentViewModel.Time.Split("-");
                TimeSpan startTime = new TimeSpan(0, 0, 0);
                TimeSpan endTime = new TimeSpan(0, 0, 0);
                if (time.Length > 1)
                {
                    startTime = TimeSpan.Parse(time[0]);
                    endTime = TimeSpan.Parse(time[1]);
                }

                Appointment newAppointment = new Appointment();

                newAppointment.AppointmentId = appointmentViewModel.AppointmentId;
                newAppointment.DoctorId = appointmentViewModel.DoctorId;
                newAppointment.PatientId = appointmentViewModel.PatientId;
                newAppointment.ProcedureId = appointmentViewModel.ProcedureId;
                newAppointment.AppointmentDate = appointmentViewModel.AppointmentDate;
                newAppointment.StartTime = startTime;
                newAppointment.EndTime = endTime;

                _appointmentService.Update(newAppointment);

                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return ValidationProblem();
        }

        [Authorize(Roles = Constants.adminRoleName + "," + Constants.doctorRoleName)]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                Appointment appointment = _appointmentService.GetAppointmentById(id);
                if (appointment != null)
                {
                    _appointmentService.Delete(appointment);
                    return RedirectToAction("Index");
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
    }
}