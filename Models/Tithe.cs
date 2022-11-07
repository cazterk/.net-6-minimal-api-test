using System.ComponentModel.DataAnnotations;
namespace ChurchSystem.Models
{

    public class Tithe
    {
        [Key]
        public int Id { get; set; }
        public MeetingTypes MeetingType { get; set; }

        public double CollectedAmount { get; set; }

        public DateTime Date { get; set; }
    }
}