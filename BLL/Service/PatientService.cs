using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace BLL.Service
{
    public class PatientService
    {
        public readonly IRepository<Patient> _repository;
        public readonly AppointmentService _appointmentService;
        private readonly UserManager<IdentityUser> _userManager;
        public readonly ILogger<PatientService> _logger;
        public PatientService(IRepository<Patient> repository, ILogger<PatientService> logger, AppointmentService appointmentService,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _appointmentService = appointmentService;
            _userManager = userManager;
        }

        public Patient Create(Patient patient)
        {
            try
            {
                if (patient != null)
                {
                    return _repository.Create(patient);
                }
                else
                {
                    _logger.LogError("Patient is null when Create");
                    throw new ArgumentNullException(nameof(patient));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Create Patient");
                throw;
            }
        }

        public async Task<bool> Delete(Patient patient)
        {
            try
            {
                if (patient != null)
                {
                    IEnumerable<Appointment> patientAppointments = _appointmentService.GetAll().Where(x => x.PatientId == patient.PatientId);
                    if (patientAppointments != null && patientAppointments.Count() > 0)
                    {
                        _appointmentService.DeleteListOfAppoitments(patientAppointments);
                        _repository.Delete(patient);
                        var user = await _userManager.FindByEmailAsync(patient.Email);

                        if (user != null)
                        {
                            var result = await _userManager.DeleteAsync(user);

                            if (result.Succeeded) { return true; } 
                            else { return false; }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if(patientAppointments != null && patientAppointments.Count() == 0)
                    {
                        _repository.Delete(patient);
                        var user = await _userManager.FindByEmailAsync(patient.Email);

                        if (user != null)
                        {
                            var result = await _userManager.DeleteAsync(user);

                            if (result.Succeeded) { return true; }
                            else { return false; }
                        };

                        return false;
                    }
                    else { return false; }                
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Delete Patient with");
                return false;
                throw;            
            }
        }

        public void Update(Patient patient)

        {
            try
            {
                if (patient != null)
                {
                    _repository.Update(patient);
                }
                else
                {
                    _logger.LogError("Patient is null when Update");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Update Patient with");
                throw;
            }

        }

        public IEnumerable<Patient> GetAll()
        {
            try
            {
                IEnumerable<Patient> patients = _repository.GetAll().AsQueryable();

                if (patients != null && patients.Count() > 0)
                {
                    return patients;
                }
                else
                {
                    _logger.LogError("Patients is null or empty when GetAll");
                    return Enumerable.Empty<Patient>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetAll Patients with");
                throw;
            }
        }

        public Patient GetPatientById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Patient patient = _repository.GetById(id);
                    if (patient != null)
                        return patient;
                    else
                        _logger.LogError("Patient is null while GetPatientById");
                    return new Patient();
                }
                else
                {
                    _logger.LogError("Incorrect Id while GetPatientById");
                    return new Patient();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetPatientById with error:");
                throw;
            }
        }

        public IEnumerable<Patient> GetPatientsByListOfIds(List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    IEnumerable<Patient> patients = _repository.GetAll();
                    var result = patients.Where(order => ids.Contains(order.PatientId));

                    if (result != null && result.Count() > 0)
                    {
                        return result;
                    }
                    else
                    {
                        _logger.LogError("Doctors is null or empty when GetDoctorsByListOfIds");
                        return Enumerable.Empty<Patient>();
                    }
                }
                else
                {
                    _logger.LogError("List of Ids is null or empty when GetDoctorsByListOfIds");
                    return Enumerable.Empty<Patient>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetDoctorsByListOfIds with error");
                return Enumerable.Empty<Patient>();
            }
        }

        public Patient GetPatientByEmail(string? email)
        {
            try
            {
                if(email != null && !String.IsNullOrEmpty(email))
                {
                    var result = _repository.GetAll().FirstOrDefault(x => x.Email == email);

                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        _logger.LogError("Patient not found");
                        return new Patient();
                    }
                }
                else
                {
                    _logger.LogError("Email is empty, null or incorrect");
                    return new Patient();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed while GetPatientByEmail");
                return new Patient();
            }
        }
    }
}
