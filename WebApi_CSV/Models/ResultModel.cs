namespace WebApi_CSV.Models
{
    public class ResultModel
    {
        public string FileName { get; set; } = "empty";
        public TimeSpan AllTime { get; set; } //???????
        public DateTimeOffset MinDateTime { get; set; }
        public double AverageTimeWork { get; set; }
        public double AverageValue { get; set; }
        public double MedianValue { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public int CountRows { get; set; }
    }
}
