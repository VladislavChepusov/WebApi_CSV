using System.ComponentModel.DataAnnotations;

namespace WebApi_CSV.Models
{
    public class FilterModel
    {
        public string? FileName { get; set; } = null;
        public DateTimeOffset CreationDate_From { get; set; }
        public DateTimeOffset CreationDate_To { get; set; }

        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Показатель должен быть положительным")]
        public double AverageValue_From { get; set; } = -0.0000001;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Показатель должен быть положительным")]
        public double AverageValue_To { get; set; } = -0.0000001;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Показатель должен быть положительным")]
        public double AverageTimeWork_From { get; set; } = -0.0000001;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Показатель должен быть положительным")]
        public double AverageTimeWork_To { get; set; } = -0.0000001;
    }
}
