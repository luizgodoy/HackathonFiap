namespace Hackathon.Core.Models
{
    public class Appointment : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set;}        
        public long DoctorId { get; set; }
        public long PatientId { get; set; }

        public virtual User? Doctor { get; set; }
        public virtual User? Patient { get; set;}
    }
}