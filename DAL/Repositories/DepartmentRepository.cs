using DAL.Models;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class DepartmentRepository : IRepository<Department>
    {
        private readonly HospitalContext _hospitalContext;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(HospitalContext hospitalContext, ILogger<DepartmentRepository> logger)
        {
            _hospitalContext = hospitalContext;
            _logger = logger;
        }

        public Department Create(Department department)
        {
            try
            {
                if (department != null)
                {
                    var obj = _hospitalContext.Departments.Add(department);
                    _hospitalContext.SaveChanges();
                    return obj.Entity;
                }
                else
                {
                    _logger.LogError("Order is null, return null");
                    return new Department();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Create Order");
                throw;
            }
        }

        public void Delete(Department department)
        {
            try
            {
                if (department != null)
                {
                    var obj = _hospitalContext.Remove(department);
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

        public IEnumerable<Department> GetAll()
        {
            try
            {
                IEnumerable<Department> department = _hospitalContext.Departments.ToList();
                if (department != null)
                    return department;
                else
                    _logger.LogError("IEnumerable<Order> is null, return null");
                return Enumerable.Empty<Department>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while GetAll Orders");
                throw;
            }
        }

        public Department GetById(int id)
        {
            try
            {
                if (id > 0)
                {
                    Department? department = _hospitalContext.Departments.FirstOrDefault(x => x.DepartmentId == id);
                    if (department != null)
                        return department;
                    else
                        _logger.LogError("Order GetById() is null, return null");
                    return new Department();
                }
                else
                {
                    _logger.LogError("Order id is incorrect, return null");
                    return new Department();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed while Get Order by Id");
                throw;
            }
        }

        public void Update(Department department)
        {
            try
            {
                if (department != null)
                {
                    var obj = _hospitalContext.Departments.Update(department);
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
