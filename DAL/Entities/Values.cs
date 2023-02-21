using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Values
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = "empty";
        public DateTimeOffset CreationDate { get; set; }
        public int WorkTime { get; set; }
        public float Value { get; set; }
    }
}
