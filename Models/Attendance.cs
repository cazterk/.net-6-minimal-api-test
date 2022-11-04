using System.ComponentModel.DataAnnotations;


public class Attendance
{
    [Key]
    public int Id { get; set; }
    public double Brothers { get; set; }

    public double Sisters { get; set; }

    public MeetingTypes MeetingType { get; set; }

    // [DataType(DataType.Date, ErrorMessage = "Date only")]
    // [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = false)]
    public DateTime Date { get; set; }
}