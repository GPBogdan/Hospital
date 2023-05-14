using BLL.Service;
using DAL.Constants;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Authorize]
    public class MedicalProcedureController : Controller
    {
        private readonly MedicalProcedureService _medicalProcedureService;
        private readonly ILogger<MedicalProcedureController> _logger;

        public MedicalProcedureController(ILogger<MedicalProcedureController> logger, MedicalProcedureService providerService)
        {
            _medicalProcedureService = providerService;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<MedicalProcedure>? medicalProcedureList = _medicalProcedureService.GetAll();

            return View(medicalProcedureList);
        }

        [Authorize(Constants.adminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Constants.adminRoleName)]
        public IActionResult Create(MedicalProcedure medicalProcedure)
        {
            if (ModelState.IsValid)
            {
                if (medicalProcedure != null)
                {
                    _medicalProcedureService.Create(medicalProcedure);
                }

                TempData["ResultOk"] = "MedicalProcedure Added Successfully!";
                return RedirectToAction("Index");
            }

            return View(medicalProcedure);
        }

        [Authorize(Constants.adminRoleName)]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                MedicalProcedure medicalProcedure = _medicalProcedureService.GetMedicalProcedureById(id);
                if (medicalProcedure != null)
                {
                    return View(medicalProcedure);
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
        public IActionResult Edit(MedicalProcedure medicalProcedure)
        {
            if (ModelState.IsValid)
            {
                _medicalProcedureService.Update(medicalProcedure);

                TempData["ResultOk"] = "MedicalProcedure data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(medicalProcedure);
        }

        [Authorize(Constants.adminRoleName)]
        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var medicalProcedure = _medicalProcedureService.GetMedicalProcedureById(id);
                if (medicalProcedure != null)
                {
                    _medicalProcedureService.Delete(medicalProcedure);
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
