using System.ComponentModel.DataAnnotations;
namespace ChurchSystem.Models
{

    public class Tithe
    {
        [Key]
        public int Id { get; set; }
        public MeetingTypes MeetingType { get; set; }

        public double CollectionedAmount { get; set; }

        public DateOnly Date { get; set; }
    }
}