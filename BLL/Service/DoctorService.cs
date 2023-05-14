using DAL.Repositories;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace BLL.Service
{
    public class DoctorService
    {
        public readonly IRepository<Doctor> _repository;
        public readonly AppointmentService _appointmentService;
        private readonly UserManager<IdentityUser> _userManager;
        public readonly ILogger<DoctorService> _logger;

        public DoctorService(IRepository<Doctor> repository,  ILogger<DoctorService> logger, AppointmentService appointmentService,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _appointmentService = appointmentService;
            _userManager = userManager;
        }

        public Doctor Create(Doctor doctor)
        {
            try
            {
                if (doctor != null)
                {
                    return _repository.Create(doctor);
                }
                else
                {
                    _logger.LogError("Doctor is null when Create");
                    throw new ArgumentNullException(nameof(doctor));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Create Doctor");
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                if (id < 0)
                {
                    var doctor = GetDoctorById(id);

                    if (doctor != null)
                    {
                        IEnumerable<Appointment> doctorAppointments = _appointmentService.GetAll().Where(x => x.DoctorId == doctor.DoctorId);
                        if (doctorAppointments != null && doctorAppointments.Count() > 0)
                        {
                            _appointmentService.DeleteListOfAppoitments(doctorAppointments);
                            _repository.Delete(doctor);

                            var user = await _userManager.FindByEmailAsync(doctor.Email);

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
                        else if (doctorAppointments != null && doctorAppointments.Count() == 0)
                        {
                            _repository.Delete(doctor);
                            var user = await _userManager.FindByEmailAsync(doctor.Email);

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
                _logger.LogError("Id is zero while delete Doctor");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Delete Doctor with");
                throw;
            }
        }

        public void DeleteListOfDoctors(IEnumerable<Doctor> doctors)
        {
            try
            {
                if (doctors != null && doctors.Count() > 0)
                {
                    doctors.ToList().ForEach(_repository.Delete);
                    _logger.LogError("DeleteListOfAppoitments completed successfully");
                }
                else
                {
                    _logger.LogError("Appointment list is null or empty when DeleteListOfAppoitments");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while DeleteListOfAppoitments Patient");
                throw;
            }
        }

        public void Update(Doctor doctor)

        {
            try
            {
                if (doctor != null)
                {
                    _repository.Update(doctor);
                }
                else
                {
                    _logger.LogError("Doctor is null when Update");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Update Doctor with");
                throw;
            }

        }

        public IEnumerable<Doctor> GetAll()
        {
            try
            {
                IEnumerable<Doctor> doctors = _repository.GetAll().AsQueryable();

                if (doctors != null && doctors.Count() > 0)
                {
                    return doctors;
                }
                else
                {
                    _logger.LogError("Doctors is null or empty when GetAll");
                    return Enumerable.Empty<Doctor>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetAll Doctors with");
                throw;
            }
        }

        public Doctor GetDoctorById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Doctor order = _repository.GetById(id);
                    if (order != null)
                        return order;
                    else
                        _logger.LogError("Doctor is null while GetOrderById");
                    return new Doctor();
                }
                else
                {
                    _logger.LogError("Incorrect Id while GetDoctorById");
                    return new Doctor();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetDoctorById with error:");
                throw;
            }
        }

        public IEnumerable<Doctor> GetDoctorsByListOfIds(List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    IEnumerable<Doctor> doctors = _repository.GetAll();
                    var result = doctors.Where(order => ids.Contains(order.DoctorId));

                    if (result != null && result.Count() > 0)
                    {
                        return result;
                    }
                    else
                    {
                        _logger.LogError("Doctors is null or empty when GetDoctorsByListOfIds");
                        return new List<Doctor>();
                    }
                }
                else
                {
                    _logger.LogError("List of Ids is null or empty when GetDoctorsByListOfIds");
                    return new List<Doctor>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetDoctorsByListOfIds with error");
                return new List<Doctor>();
            }
        }

    }
}