using BLL.Service;
using DAL.Constants;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Authorize(Constants.adminRoleName)]
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger, PatientService providerService)
        {
            _patientService = providerService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Patient> orderList = _patientService.GetAll();

            return View(orderList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]        
        public IActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                if (patient != null)
                {
                    _patientService.Create(patient);
                }

                return RedirectToAction("Index");
            }

            return View(patient);
        }

        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                Patient patient = _patientService.GetPatientById(id);
                if (patient != null)
                {
                    return View(patient);
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
        public IActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _patientService.Update(patient);

                return RedirectToAction("Index");
            }

            return View(patient);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                Patient patient = _patientService.GetPatientById(id);
                if (patient != null)
                {
                    var isDeleted = await _patientService.Delete(patient);
                    if (isDeleted) { _logger.LogInformation($"Patient with:{id} deleted successfully"); }

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
