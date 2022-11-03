using System.ComponentModel.DataAnnotations;


public class Attendance
{
    [Key]
    public int Id { get; set; }
    public double Brothers { get; set; }

    public double Sisters { get; set; }

    public MeetingTypes MeetingType { get; set; }

    public DateTime Date { get; set; }
}