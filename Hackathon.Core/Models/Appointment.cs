namespace Hackathon.Core.Models
{
    public class Appointment : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartAt { get; set;}
        public DateTime FinishAt { get; set; }
        public Guid DoctorId { get; set; }
        public Guid? PatientId { get; set; }
    }
}