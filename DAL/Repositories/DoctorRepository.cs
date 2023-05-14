using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class DoctorRepository : IRepository<Doctor>
    {
        private readonly HospitalContext _hospitalContext;
        private readonly ILogger<DoctorRepository> _logger;

        public DoctorRepository(HospitalContext hospitalContext, ILogger<DoctorRepository> logger)
        {
            _hospitalContext = hospitalContext;
            _logger = logger;
        }

        public Doctor Create(Doctor doctor)
        {
            try
            {
                if (doctor != null)
                {
                    var obj = _hospitalContext.Doctors.Add(doctor);
                    _hospitalContext.SaveChanges();
                    return obj.Entity;
                }
                else
                {
                    _logger.LogError("Order is null, return null");
                    return new Doctor();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Create Order");
                throw;
            }
        }

        public void Delete(Doctor doctor)
        {
            try
            {
                if (doctor != null)
                {
                    var obj = _hospitalContext.Remove(doctor);
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

        public IEnumerable<Doctor> GetAll()
        {
            try
            {
                IEnumerable<Doctor> doctors = _hospitalContext.Doctors.ToList();
                if (doctors != null)
                    return doctors;
                else
                    _logger.LogError("IEnumerable<Order> is null, return null");
                return Enumerable.Empty<Doctor>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetAll Orders");
                throw;
            }
        }

        public Doctor GetById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Doctor? doctor = _hospitalContext.Doctors.FirstOrDefault(x => x.DoctorId == id);
                    if (doctor != null)
                        return doctor;
                    else
                        _logger.LogError("Order GetById() is null, return null");
                    return new Doctor();
                }
                else
                {
                    _logger.LogError("Order id is incorrect, return null");
                    return new Doctor();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Get Order by Id");
                throw;
            }
        }

        public void Update(Doctor doctor)
        {
            try
            {
                if (doctor != null)
                {
                    var obj = _hospitalContext.Doctors.Update(doctor);
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
