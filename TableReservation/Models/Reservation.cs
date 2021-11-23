
using Microsoft.AspNetCore.Mvc;
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
        public string CustomerId { get; set; }
        [Display (Name = "First Name")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Phone]
        public string Phone { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Reservation Date and Time")]
        public DateTime ResDate { get; set; }
        [Display(Name = "Number of Guests")]
        public int PartySize { get; set; }
        public int TotalSeats { get; set; }
        public Customer Customer { get; set; }


    }
}
