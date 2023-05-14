using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class DoctorViewModel
    {
        [Required(ErrorMessage = "DoctorId is required.")]
        [Display(Name = "DoctorId")]
        public int DoctorId { get; set; } = 0;

        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(150)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(150)]
        [Display(Name = "LastName")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Date Of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(5)]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!;

        [Phone]
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+38)?\(?\d{3}\)?[\s\.-]?(\d{7}|\d{3}[\s\.-]\d{2}[\s\.-]\d{2}|\d{3}-\d{4}))",
                ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; } = 0;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        [Display(Name = "Email")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Not a valid password")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        public IEnumerable<Doctor>? Doctors { get; set; }

        public IEnumerable<Department>? Departments { set; get; } = null!;
    }
}
