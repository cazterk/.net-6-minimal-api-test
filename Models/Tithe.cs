using System.ComponentModel.DataAnnotations;
namespace ChurchSystem;

public class Tithe
{
    [Key]
    public int Id { get; set; }
    public MeetingTypes MeetingType { get; set; }

    public double CollectionedAmount { get; set; }

    public DateTime Date { get; set; }
}