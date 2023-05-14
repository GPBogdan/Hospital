using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class AppointmentRepository : IRepository<Appointment>
    {
        private readonly HospitalContext _hospitalContext;
        private readonly ILogger<AppointmentRepository> _logger;

        public AppointmentRepository(HospitalContext hospitalContext, ILogger<AppointmentRepository> logger)
        {
            _hospitalContext = hospitalContext;
            _logger = logger;
        }

        public Appointment Create(Appointment appointment)
        {
            try
            {
                if (appointment != null)
                {
                    var obj = _hospitalContext.Appointments.Add(appointment);
                    _hospitalContext.SaveChanges();
                    return obj.Entity;
                }
                else
                {
                    _logger.LogError("Order is null, return null");
                    return new Appointment();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Create Order");
                throw;
            }
        }

        public void Delete(Appointment appointment)
        {
            try
            {
                if (appointment != null)
                {
                    var obj = _hospitalContext.Remove(appointment);
                    if (obj != null)
                    {
                        _hospitalContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Delete Order");
                throw;
            }
        }

        public IEnumerable<Appointment> GetAll()
        {
            try
            {
                IEnumerable<Appointment> appointment = _hospitalContext.Appointments.ToList();
                if (appointment != null)
                    return appointment;
                else
                    _logger.LogError("IEnumerable<Order> is null, return null");
                return Enumerable.Empty<Appointment>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetAll Orders");
                throw;
            }
        }

        public Appointment GetById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Appointment? appointment = _hospitalContext.Appointments.FirstOrDefault(x => x.AppointmentId == id);
                    if (appointment != null)
                        return appointment;
                    else
                        _logger.LogError("Order GetById() is null, return null");
                    return new Appointment();
                }
                else
                {
                    _logger.LogError("Order id is incorrect, return null");
                    return new Appointment();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Get Order by Id");
                throw;
            }
        }

        public void Update(Appointment appointment)
        {
            try
            {
                if (appointment != null)
                {
                    var obj = _hospitalContext.Appointments.Update(appointment);
                    if (obj != null)
                        _hospitalContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Update Order");
                throw;
            }
        }
    }
}
