using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using DAL.Models;
using BLL.ViewModels;
using DAL.Constants;

namespace BLL.Service
{
    public class HospitalUserService
    {
        private readonly PatientService _patientService;
        private readonly DoctorService _doctorService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<HospitalUserService> _logger;
        private readonly IEmailSender _emailSender;

        public HospitalUserService(
            PatientService patientService,
            DoctorService doctorService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<HospitalUserService> logger,
            IEmailSender emailSender)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateHospitalUser(DoctorViewModel viewModel, string roleName)
        {
            if (viewModel != null)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, viewModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, viewModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("ASP User created a new account with password.");

                    IdentityRole? role = await _roleManager.FindByNameAsync(roleName);

                    if (role != null && !String.IsNullOrEmpty(role.Name))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    if(!String.IsNullOrEmpty(roleName) && roleName == Constants.patientRoleName)
                    {
                        Patient patient = new Patient()
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Address = viewModel.Address,
                            PhoneNumber = viewModel.PhoneNumber,
                            Gender = viewModel.Gender,
                            Email = viewModel.Email,
                            DateOfBirth = viewModel.DateOfBirth,
                            PhotoUrl = viewModel.PhotoUrl
                        };

                        var resultPatient = _patientService.Create(patient);

                        if (resultPatient != null && resultPatient.PatientId > 0)
                        {
                            _logger.LogInformation("Patient created successfully");
                            return true;
                        }
                        return false;
                    }
                    else if (!String.IsNullOrEmpty(roleName) && roleName == Constants.doctorRoleName)
                    {
                        Doctor doctor = new Doctor()
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            Address = viewModel.Address,
                            PhoneNumber = viewModel.PhoneNumber,
                            Gender = viewModel.Gender,
                            Email = viewModel.Email,
                            DateOfBirth = viewModel.DateOfBirth,
                            DepartmentId = viewModel.DepartmentId,
                            PhotoUrl = viewModel.PhotoUrl
                        };

                        var resultDoctor = _doctorService.Create(doctor);

                        if (resultDoctor != null && resultDoctor.DoctorId > 0)
                        {
                            _logger.LogInformation("Doctor created successfully");
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        _logger.LogError("Can not create Hospital user, Role did not exist");
                        return false;
                    }               
                }
            }

            return false;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
