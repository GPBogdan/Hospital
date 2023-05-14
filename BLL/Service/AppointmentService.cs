using BLL.ViewModels;
using DAL.Models;
using DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Service
{
    public class AppointmentService
    {
        public readonly IRepository<Appointment> _repository;
        public readonly ILogger<AppointmentService> _logger;
        public AppointmentService(IRepository<Appointment> repository, ILogger<AppointmentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Appointment Create(Appointment appointment)
        {
            try
            {
                if (appointment != null)
                {
                    Appointment? isAppointment = _repository.GetAll()
                        .FirstOrDefault(x => x.AppointmentDate == appointment.AppointmentDate 
                        && x.StartTime == appointment.StartTime && x.EndTime == appointment.EndTime
                        || x.AppointmentDate == appointment.AppointmentDate && x.ProcedureId == appointment.ProcedureId
                        || x.DoctorId == appointment.DoctorId
                        && x.StartTime == appointment.StartTime && x.EndTime == appointment.EndTime);

                    if (isAppointment == null)
                    {
                        return _repository.Create(appointment);
                    }
                    else
                    {
                        _logger.LogError("Appointment for this date already exist, error when Create");
                        return new Appointment();
                    }                    
                }
                else
                {
                    _logger.LogError("Appointment is null when Create");
                    throw new ArgumentNullException(nameof(appointment));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Create Appointment");
                throw;
            }
        }

        public Appointment CreateAppointmentFromViewModel(AppointmentViewModel appointmentViewModel)
        {
            try
            {
                if (appointmentViewModel != null)
                {
                    string[] time = appointmentViewModel.Time.Split("-");
                    TimeSpan startTime = new TimeSpan(0, 0, 0);
                    TimeSpan endTime = new TimeSpan(0, 0, 0);
                    if (time.Length > 1)
                    {
                        startTime = TimeSpan.Parse(time[0]);
                        endTime = TimeSpan.Parse(time[1]);
                    }

                    Appointment data = new Appointment()
                    {
                        ProcedureId = appointmentViewModel.ProcedureId,
                        DoctorId = appointmentViewModel.DoctorId,
                        AppointmentDate = appointmentViewModel.AppointmentDate,
                        StartTime = startTime,
                        EndTime = endTime,
                        PatientId = appointmentViewModel.PatientId
                    };

                    var appointmentResult = Create(data);

                    return appointmentResult;
                }
                else
                {
                    _logger.LogError("AppointmentViewModel is null in CreateAppointmentFromViewModel");
                    return new Appointment();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while CreateAppointmentFromViewModel");
                return new Appointment();
            }
        }

        public void Delete(Appointment appointment)

        {
            try
            {
                if (appointment != null) { _repository.Delete(appointment); }
                else
                {
                    _logger.LogError("Appointment is null when Delete");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Delete Appointment");
                throw;
            }
        }

        public void DeleteListOfAppoitments(IEnumerable<Appointment> appointments)
        {
            try
            {
                if(appointments != null && appointments.Count() > 0)
                {
                    appointments.ToList().ForEach(_repository.Delete);
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

        public void Update(Appointment appointment)
        {
            try
            {
                if (appointment != null)
                {
                    _repository.Update(appointment);
                }
                else
                {
                    _logger.LogError("Appointment is null when Update");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Update Appointment");
                throw;
            }

        }

        public IEnumerable<Appointment> GetAll()
        {
            try
            {
                IEnumerable<Appointment> appointments = _repository.GetAll();

                if (appointments != null && appointments.Count() > 0)
                {
                    return appointments;
                }
                else
                {
                    _logger.LogError("Appointments is null or empty when GetAll");
                    return Enumerable.Empty<Appointment>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetAll Appointments");
                throw;
            }
        }

        public Appointment GetAppointmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Appointment appointment = _repository.GetById(id);
                    if (appointment != null)
                        return appointment;
                    else
                        _logger.LogError("Appointment is null while GetOrderById");
                    return new Appointment();
                }
                else
                {
                    _logger.LogError("Incorrect Id while GetAppointmentById");
                    return new Appointment();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetAppointmentById");
                throw;
            }
        }
    }
}
