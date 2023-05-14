using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BLL.ViewModels
{
    public class AppointmentViewModel
    {
        public IEnumerable<Appointment>? AppointmentList { get; set; }
        public IEnumerable<Doctor>? DoctorList { get; set; }
        public IEnumerable<MedicalProcedure>? MedicalProcedureList { get; set; }
        public IEnumerable<Patient>? Patients { get; set; }

        public string Time { get; set; } = String.Empty;

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int ProcedureId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Appointment date is required.")]
        public DateTime AppointmentDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public AppointmentViewModel() { }

        public AppointmentViewModel(IEnumerable<Appointment> appointmentList, IEnumerable<Doctor> doctorList,
            IEnumerable<MedicalProcedure> medicalProcedure, IEnumerable<Patient> patients, int patientId)
        {
            AppointmentList = appointmentList;
            DoctorList = doctorList;
            MedicalProcedureList = medicalProcedure;
            PatientId = patientId;
            Patients = patients;
        }

    }
}
