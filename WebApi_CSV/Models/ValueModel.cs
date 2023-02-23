namespace WebApi_CSV.Models
{
    public class ValueModel
    {
        public string FileName { get; set; } = "empty";
        public DateTimeOffset CreationDate { get; set; }
        public int WorkTime { get; set; }
        public float Value { get; set; }
    }
}
