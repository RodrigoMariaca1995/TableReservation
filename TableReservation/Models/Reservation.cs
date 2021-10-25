using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TableReservation.Models
{
    public class Reservation
    {
        [Key]
        public int ResId { get; set; }
        public string UserId { get; set; }
        public DateTime ResDate { get; set; }
        public int PartySize { get; set; }
        public ICollection<ReservedTable> ReservedTables { get; set; }
        public Customer Customer { get; set; }
    }
}
