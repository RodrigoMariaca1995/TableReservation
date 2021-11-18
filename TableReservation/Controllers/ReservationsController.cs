using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TableReservation.Data;
using TableReservation.Models;


namespace TableReservation.Controllers
{
    public class ReservationsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDbContext _context;


        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var customer = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _context.Reservations.Include(r => r.Customer).AsNoTracking();
            var curDate = DateTime.Now;

            if (User.Identity.IsAuthenticated)
            {
                reservations = reservations.Where(s => s.CustomerId == customer && s.ResDate > curDate); //set the list of reservations to current user and only shows future reservations
               
            }
            else
            {
                //reservations = reservations.Where(s => s.UserId == ""); //might want to change this in the future
            }
            return View(await reservations.ToListAsync());
     
            //return View(await _context.Reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResDate,PartySize")] Reservation reservation)
        {
            
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            if (User.Identity.IsAuthenticated)
            {
                var customer = User.FindFirstValue(ClaimTypes.NameIdentifier);
                reservation.CustomerId = customer;
            }
            if (ModelState.IsValid)
            {

                var reservationDate = reservation.ResDate;
                var partySize = reservation.PartySize;

                var restaurantCapacity = from res in _context.Reservations
                                         where res.ResDate == reservationDate
                                         select res.TotalSeats;

                List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };
                int total = tables.Sum();
                System.Diagnostics.Debug.WriteLine(total);
                int currentCapacity = 0;

                foreach (var i in restaurantCapacity)
                {
                    currentCapacity += i;
                }

                if ((total - currentCapacity) < partySize)
                {
                    return View(reservation);
                }
                else
                {
                    foreach (var i in restaurantCapacity)
                    {
                        int temp = TablesReserved(ref tables, i);
                    }
                    foreach (var i in tables)
                    {
                        System.Diagnostics.Debug.WriteLine(i);
                    }

                    int capacity = TablesReserved(ref tables, partySize);

                    reservation.TotalSeats = capacity;
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(reservation);
        }


        // GET: Reservations/Edit/5

        public int TablesReserved(ref List<int> tables, int partySize)
        {
            int capacity = 0;
            while(capacity < partySize)
            {
                if(partySize > 4)
                {
                    if (tables.Exists(x => x == 6))
                    {
                        int index = tables.FindIndex(x => x == 6);
                        tables.RemoveAt(index);
                        capacity += 6;
                    }
                    else if (tables.Exists(x => x == 4))
                    {
                        int index = tables.FindIndex(x => x == 4);
                        tables.RemoveAt(index);
                        capacity += 4;
                    }
                    else
                    {
                        int index = tables.FindIndex(x => x == 2);
                        tables.RemoveAt(index);
                        capacity += 2;
                    }
                }
                else if (partySize > 2)
                {
                    if (tables.Exists(x => x == 4))
                    {
                        int index = tables.FindIndex(x => x == 4);
                        tables.RemoveAt(index);
                        capacity += 4;
                    }
                    else if (tables.Exists(x => x == 2))
                    {
                        int index = tables.FindIndex(x => x == 2);
                        tables.RemoveAt(index);
                        capacity += 2;
                    }
                    else
                    {
                        int index = tables.FindIndex(x => x == 6);
                        tables.RemoveAt(index);
                        capacity += 6;
                    }
                }
                else
                {
                    if (tables.Exists(x => x == 2))
                    {
                        int index = tables.FindIndex(x => x == 2);
                        tables.RemoveAt(index);
                        capacity += 2;
                    }
                    else if (tables.Exists(x => x == 4))
                    {
                        int index = tables.FindIndex(x => x == 4);
                        tables.RemoveAt(index);
                        capacity += 4;
                    }
                    else
                    {
                        int index = tables.FindIndex(x => x == 6);
                        tables.RemoveAt(index);
                        capacity += 6;
                    }
                }

            }
            return capacity;
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResId,UserId,ResDate,PartySize")] Reservation reservation)
        {
            if (id != reservation.ResId)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var customer = User.FindFirstValue(ClaimTypes.NameIdentifier);
                reservation.CustomerId = customer;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ResId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ResId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ResId == id);
        }

        

    }
}
