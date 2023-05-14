using DAL.Models;
using DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Service
{
    public class DepartmentService
    {
        public readonly IRepository<Department> _repository;
        public readonly ILogger<DepartmentService> _logger;
        public DepartmentService(IRepository<Department> repository, ILogger<DepartmentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Department Create(Department department)
        {
            try
            {
                if (department != null)
                {
                    return _repository.Create(department);
                }
                else
                {
                    _logger.LogError("Department is null when Create");
                    throw new ArgumentNullException(nameof(department));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Create Department");
                throw;
            }
        }

        public void Delete(Department department)
        {
            try
            {
                if (department != null && department.DepartmentId > 0)
                {              
                    _repository.Delete(department);
                }
                else
                {
                    _logger.LogError("Failed while Delete Department, is null or empty");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Delete Department");
                throw;
            }
        }

        public void Update(Department department)
        {
            try
            {
                if (department != null)
                {
                    _repository.Update(department);
                }
                else
                {
                    _logger.LogError("Department is null when Update");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while Update Department");
                throw;
            }

        }

        public IEnumerable<Department> GetAll()
        {
            try
            {
                IEnumerable<Department> departments = _repository.GetAll();

                if (departments != null && departments.Count() > 0)
                {
                    return departments;
                }
                else
                {
                    _logger.LogError("Departments is null or empty when GetAll");
                    return Enumerable.Empty<Department>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while GetAll Departments");
                throw;
            }
        }

        public Department GetDepartmentById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Department order = _repository.GetById(id);
                    if (order != null)
                        return order;
                    else
                        _logger.LogError("Department is null while GetDepartmentById");
                    return new Department();
                }
                else
                {
                    _logger.LogError("Incorrect Id while GetDepartmentById");
                    return new Department();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetDepartmentById");
                throw;
            }
        }
    }
}
