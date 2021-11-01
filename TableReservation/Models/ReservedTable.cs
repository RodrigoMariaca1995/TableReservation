using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TableReservation.Models
{
    public class ReservedTable
    {
        [Key]
        public int ResTableId { get; set; }
        public int ReservationResId { get; set; }
        public int TablesTableId { get; set; }
        public Table Tables { get; set; }
        public Reservation Reservation { get; set; }
    }
}
