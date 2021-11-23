using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TableReservation.Models
{
    public class Customer : IdentityUser
    {

        [Display(Name = "First Name")]
        public string FristName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

    }
}
