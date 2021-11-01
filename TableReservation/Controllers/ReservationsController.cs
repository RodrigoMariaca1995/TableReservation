using System;
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

            if(User.Identity.IsAuthenticated)
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
        public async Task<IActionResult> Create([Bind("ResDate,PartySize")] Reservation reservation,  ReservedTable reservedTable)
        {
            //System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            if (User.Identity.IsAuthenticated)
            {
                var customer = User.FindFirstValue(ClaimTypes.NameIdentifier);
                reservation.CustomerId = customer;
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                var partSize = _context.Reservations.OrderByDescending(p => p.ResId).FirstOrDefault().PartySize;
                //const string connection = @"Server = (localdb)\\mssqllocaldb; Database = CU - 1; Trusted_Connection = True; MultipleActiveResultSets = true";
                //using (var conn = new SqlConnection(connection))
                //{
                //    var qry = "select * from Tables, ReservedTables, Reservations where Tables.TableId != ReservedTables.TablesTableId AND ReservedTables.ReservationResId = Reservations.ResId AND  Reservations.ResDate = '2021-11-10 15:40:00.0000000'";
                //    var cmd = new SqlCommand(qry, conn);
                //    conn.Open();
                //    SqlDataReader rdr = cmd.ExecuteReader();

                //    if(rdr.HasRows)
                //    {
                //        while(rdr.Read())
                //        {
                //            Console.WriteLine("{0}", rdr.GetString(0));
                //        }
                //    }
                //}
                //var numTable2 = _context.Tables.Where(p => p.Capacity == 2).Count();
                reservedTable.ReservationResId = _context.Reservations.OrderByDescending(p => p.ResId).FirstOrDefault().ResId;
                reservedTable.TablesTableId = 1;
                _context.Add(reservedTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
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
