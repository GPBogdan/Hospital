namespace DAL.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public int ProcedureId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public string AppointmentStatus { get; set; } = "0";

    public string AppointmentConclusion { get; set; } = "Write conclusion here";

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual MedicalProcedure Procedure { get; set; } = null!;
}
