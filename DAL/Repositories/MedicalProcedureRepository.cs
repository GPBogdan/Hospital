using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class MedicalProcedureRepository : IRepository<MedicalProcedure>
    {
        private readonly HospitalContext _hospitalContext;
        private readonly ILogger<MedicalProcedureRepository> _logger;

        public MedicalProcedureRepository(HospitalContext hospitalContext, ILogger<MedicalProcedureRepository> logger)
        {
            _hospitalContext = hospitalContext;
            _logger = logger;
        }

        public MedicalProcedure Create(MedicalProcedure medicalProcedure)
        {
            try
            {
                if (medicalProcedure != null)
                {
                    var obj = _hospitalContext.MedicalProcedures.Add(medicalProcedure);
                    _hospitalContext.SaveChanges();
                    return obj.Entity;
                }
                else
                {
                    _logger.LogError("Order is null, return null");
                    return new MedicalProcedure();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Create Order");
                throw;
            }
        }

        public void Delete(MedicalProcedure medicalProcedure)
        {
            try
            {
                if (medicalProcedure != null)
                {
                    var obj = _hospitalContext.Remove(medicalProcedure);
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

        public IEnumerable<MedicalProcedure> GetAll()
        {
            try
            {
                IEnumerable<MedicalProcedure> medicalProcedure = _hospitalContext.MedicalProcedures.ToList();
                if (medicalProcedure != null)
                    return medicalProcedure;
                else
                    _logger.LogError("IEnumerable<Order> is null, return null");
                return Enumerable.Empty<MedicalProcedure>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetAll Orders");
                throw;
            }
        }

        public MedicalProcedure GetById(int id)
        {
            try
            {
                if (id > 0)
                {
                    MedicalProcedure? medicalProcedure = _hospitalContext.MedicalProcedures.FirstOrDefault(x => x.ProcedureId == id);
                    if (medicalProcedure != null)
                        return medicalProcedure;
                    else
                        _logger.LogError("Order GetById() is null, return null");
                    return new MedicalProcedure();
                }
                else
                {
                    _logger.LogError("Order id is incorrect, return null");
                    return new MedicalProcedure();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Get Order by Id");
                throw;
            }
        }

        public void Update(MedicalProcedure medicalProcedure)
        {
            try
            {
                if (medicalProcedure != null)
                {
                    var obj = _hospitalContext.MedicalProcedures.Update(medicalProcedure);
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