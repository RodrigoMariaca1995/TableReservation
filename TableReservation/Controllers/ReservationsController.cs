﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notfy;


        public ReservationsController(ApplicationDbContext context, INotyfService notfy)
        {
            _context = context;
            _notfy = notfy;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(string searchString)
        {
            var customer = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _context.Reservations.Include(r => r.Customer).AsNoTracking();
            var curDate = DateTime.Now;
       
            if (User.Identity.IsAuthenticated)
            {
                //set the list of reservations to current user and only shows future reservations
                reservations = reservations.Where(s => s.CustomerId == customer && s.ResDate > curDate); 
               
            }
            else
            {
                //set the list of reservations to the search keyword
                ViewData["CurrentFilter"] = searchString;
                reservations = reservations.Where(s => s.Email.Contains(searchString)
                                           || s.Email.Contains(searchString));


                //notification 
                _notfy.Information("Please Consider Registering!", 5);

            }
            return View(await reservations.ToListAsync());
     

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
        public async Task<IActionResult> Create([Bind("ResDate, PartySize")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                
                var endHour = reservation.ResDate.AddHours(1.5);
                var startHour = reservation.ResDate.AddHours(-1.5);

                var prevDateS = reservation.ResDate.Date.AddYears(-1);
                var prevDateE = reservation.ResDate.Date.AddYears(-1).AddDays(1);

                var partySize = reservation.PartySize;

                var restaurantCapacity = from res in _context.Reservations
                                         where res.ResDate > startHour && res.ResDate < endHour
                                         select res.TotalSeats;
                var prevRes = from res in _context.Reservations
                               where res.ResDate >= prevDateS && res.ResDate <= prevDateE
                              select res.PartySize;

                

                List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };
                int total = tables.Sum();
                System.Diagnostics.Debug.WriteLine(total);
                int currentCapacity = 0;

                foreach (var i in restaurantCapacity)
                {
                    currentCapacity += i;
                }

                int prevCap = 0;
                foreach (var i in prevRes)
                {
                    prevCap += i;
                }

                if (prevCap > 100)
                {
                    _notfy.Error("You have selected a high traffic day. A $10 fee is required to create a reservation. Please enter your credit card information", 3);
                    return View(reservation);
                }
                if ((total - currentCapacity) < partySize)
                {
                    _notfy.Error("No reservations available for this time", 3);
                    return View(reservation);
                }
                else
                {
                    reservation.CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    var curUser = from user in _context.Customer where user.Id == reservation.CustomerId select user;

                    foreach (var i in curUser)
                    {
                        reservation.FName = i.FristName;
                        reservation.LName = i.LastName;
                        reservation.Phone = i.PhoneNumber;
                        reservation.Email = i.Email;
                    }

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


        public IActionResult GCreate( )
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GCreate([Bind("FName, LName, Phone, Email, ResDate, PartySize")] Reservation reservation)
        {

            if (ModelState.IsValid)
            {

                var endHour = reservation.ResDate.AddHours(1.5);
                var startHour = reservation.ResDate.AddHours(-1.5);

                var prevDateS = reservation.ResDate.Date.AddYears(-1);
                var prevDateE = reservation.ResDate.Date.AddYears(-1).AddDays(1);

                var partySize = reservation.PartySize;

                var restaurantCapacity = from res in _context.Reservations
                                         where res.ResDate > startHour && res.ResDate < endHour
                                         select res.TotalSeats;

                List<int> tables = new List<int>() { 2, 2, 2, 4, 4, 4, 6, 6, 6 };
                int total = tables.Sum();
                System.Diagnostics.Debug.WriteLine(total);
                int currentCapacity = 0;

                var prevRes = from res in _context.Reservations
                              where res.ResDate >= prevDateS && res.ResDate <= prevDateE
                              select res.PartySize;

                int prevCap = 0;
                foreach (var i in prevRes)
                {
                    prevCap += i;
                }

                if (prevCap > 100)
                {
                    _notfy.Error("You have selected a high traffic day. A $10 fee is required to create a reservation. Please enter your credit card information", 3);
                    return View(reservation);
                }

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
                var curUser = from user in _context.Customer where user.Id == reservation.CustomerId select user;

                foreach (var i in curUser)
                {
                    reservation.FName = i.FristName;
                    reservation.LName = i.LastName;
                    reservation.Phone = i.PhoneNumber;
                    reservation.Email = i.Email;
                }
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
