using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TableReservation.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }
        public int Capacity { get; set; }
    }
}
