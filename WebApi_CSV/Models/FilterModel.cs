namespace WebApi_CSV.Models
{
    public class FilterModel
    {
        public string? FileName { get; set; } = null;
        public DateTimeOffset CreationDate_From { get; set; }
        public DateTimeOffset CreationDate_To { get; set; }
        public double AverageValue_From { get; set; }
        public double AverageValue_To { get; set; }
        public double AverageTimeWork_From { get; set; }
        public double AverageTimeWork_To { get; set; }
    }
}
