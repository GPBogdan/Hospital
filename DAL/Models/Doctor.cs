using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

public partial class Doctor
{
    public int DoctorId { get; set; } = 0;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int DepartmentId { get; set; } = 0;

    public string Email { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Department? Department { get; set; }
}
