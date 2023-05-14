using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class PatientRepository: IRepository<Patient>
    {
        private readonly HospitalContext _hospitalContext;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(HospitalContext hospitalContext, ILogger<PatientRepository> logger)
        {
            _hospitalContext = hospitalContext;
            _logger = logger;
        }

        public Patient Create(Patient patient)
        {
            try
            {
                if (patient != null)
                {
                    var obj = _hospitalContext.Patients.Add(patient);
                    _hospitalContext.SaveChanges();
                    return obj.Entity;
                }
                else
                {
                    _logger.LogError("Order is null, return null");
                    return new Patient();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Create Patient");
                throw;
            }
        }

        public void Delete(Patient patient)
        {
            try
            {
                if (patient != null)
                {
                    var obj = _hospitalContext.Remove(patient);
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

        public IEnumerable<Patient> GetAll()
        {
            try
            {
                IEnumerable<Patient> patients = _hospitalContext.Patients.ToList();
                if (patients != null)
                    return patients;
                else
                    _logger.LogError("IEnumerable<Patient> is null, return null");
                return Enumerable.Empty<Patient>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetAll Patients");
                throw;
            }
        }

        public Patient GetById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Patient? patient = _hospitalContext.Patients.FirstOrDefault(x => x.PatientId == id);
                    if (patient != null)
                        return patient;
                    else
                        _logger.LogError("Patient GetById() is null, return null");
                    return new Patient();
                }
                else
                {
                    _logger.LogError("Patient id is incorrect, return null");
                    return new Patient();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Get Patient by Id");
                throw;
            }
        }

        public void Update(Patient patient)
        {
            try
            {
                if (patient != null)
                {
                    var obj = _hospitalContext.Patients.Update(patient);
                    if (obj != null)
                        _hospitalContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Update Patient");
                throw;
            }
        }
    }
}
