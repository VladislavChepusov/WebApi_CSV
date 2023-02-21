using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Results
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FileName { get; set; } = "empty";
        public int AllTime { get; set; }
        public DateTimeOffset MinDateTime { get; set; }
        public int AverageTimeWork { get; set; }
        public float AverageValue { get; set; }
        public float MedianValue { get; set; }
        public float MaxValue { get; set; }
        public float MinValue { get; set; }
        public int CountRows { get; set; }
    }
}
