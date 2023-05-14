using DAL.Models;
using DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Service
{
    public class MedicalProcedureService
    {
        public readonly IRepository<MedicalProcedure> _repository;
        public readonly AppointmentService _appointmentService;
        public readonly ILogger<MedicalProcedureService> _logger;
        public MedicalProcedureService(IRepository<MedicalProcedure> repository, ILogger<MedicalProcedureService> logger, AppointmentService appointmentService)
        {
            _repository = repository;
            _logger = logger;
            _appointmentService = appointmentService;
        }

        public MedicalProcedure Create(MedicalProcedure medicalProcedure)
        {
            try
            {
                if(medicalProcedure != null)
                {
                    return _repository.Create(medicalProcedure);
                }
                else
                {
                    _logger.LogError("MedicalProcedure is null when Create");
                    throw new ArgumentNullException(nameof(medicalProcedure));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Create MedicalProcedure");
                throw;
            }
        }

        public void Delete(MedicalProcedure medicalProcedure)
        {
            try
            {
                IEnumerable<Appointment> procedureAppointments = _appointmentService.GetAll().Where(x => x.ProcedureId == medicalProcedure.ProcedureId);
                if (procedureAppointments != null && procedureAppointments.Count() > 0)
                {
                    _appointmentService.DeleteListOfAppoitments(procedureAppointments);
                    _repository.Delete(medicalProcedure);
                }
                else
                {
                    _repository.Delete(medicalProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Delete MedicalProcedure");
                throw;
            }
        }

        public void Update(MedicalProcedure medicalProcedure)
        {
            try
            {
                if (medicalProcedure != null)
                {
                    _repository.Update(medicalProcedure);
                }
                else
                {
                    _logger.LogError("MedicalProcedure is null when Update");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Update MedicalProcedure");
                throw;
            }

        }

        public IEnumerable<MedicalProcedure> GetAll()
        {
            try
            {
                IEnumerable<MedicalProcedure> procedures = _repository.GetAll();

                if (procedures != null && procedures.Count() > 0)
                {
                    return procedures;
                }
                else
                {
                    _logger.LogError("MedicalProcedure is null or empty when GetAll");
                    return Enumerable.Empty<MedicalProcedure>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetAll MedicalProcedure");
                throw;
            }
        }

        public MedicalProcedure GetMedicalProcedureById(int id)
        {
            try
            {
                if (id > 0)
                {
                    MedicalProcedure order = _repository.GetById(id);
                    if (order != null)
                        return order;
                    else
                        _logger.LogError("MedicalProcedure is null while GetMedicalProcedureById");
                    return new MedicalProcedure();
                }
                else
                {
                    _logger.LogError("Incorrect Id while GetMedicalProcedureById");
                    return new MedicalProcedure();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetMedicalProcedureById");
                throw;
            }
        }

        public IEnumerable<MedicalProcedure> GetProceduresByListOfIds(List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count() > 0)
                {
                    IEnumerable<MedicalProcedure> patients = _repository.GetAll();
                    var result = patients.Where(order => ids.Contains(order.ProcedureId));

                    if (result != null && result.Count() > 0)
                    {
                        return result;
                    }
                    else
                    {
                        _logger.LogError("MedicalProcedure is null or empty when GetProceduresByListOfIds");
                        return Enumerable.Empty<MedicalProcedure>();
                    }
                }
                else
                {
                    _logger.LogError("List of Ids is null or empty when GetProceduresByListOfIds");
                    return Enumerable.Empty<MedicalProcedure>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetProceduresByListOfIds");
                return Enumerable.Empty<MedicalProcedure>();
            }
        }
    }
}
