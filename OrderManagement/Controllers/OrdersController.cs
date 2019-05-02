using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderManagementContext _context;

        public OrdersController(OrderManagementContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,FirstName,LastName,City,State,DOB,RowVersion")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,FirstName,LastName,City,State,DOB,RowVersion")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var orderToUpdate = await _context.Order.FirstOrDefaultAsync(m => m.OrderID == order.OrderID);

                if (orderToUpdate == null)
                {
                    Order deletedorder = new Order();
                    await TryUpdateModelAsync(orderToUpdate);
                    ModelState.AddModelError(string.Empty,
                        "Unable to save changes. The Order was deleted by another user.");
                    return View(deletedorder);
                }

                _context.Entry(orderToUpdate).Property("RowVersion").OriginalValue = order.RowVersion;
                _context.Entry(orderToUpdate).Property("FirstName").OriginalValue = order.FirstName;
                _context.Entry(orderToUpdate).Property("LastName").OriginalValue = order.LastName;
                _context.Entry(orderToUpdate).Property("City").OriginalValue = order.City;
                _context.Entry(orderToUpdate).Property("State").OriginalValue = order.State;

                if (await TryUpdateModelAsync<Order>(
                    orderToUpdate,
                    "",
                    s => s.FirstName, s => s.LastName, s => s.City, s => s.State, s => s.DOB))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var exceptionEntry = ex.Entries.Single();
                        var clientValues = (Order)exceptionEntry.Entity;
                        var databaseEntry = exceptionEntry.GetDatabaseValues();
                        if (databaseEntry == null)
                        {
                            ModelState.AddModelError(string.Empty,
                                "Unable to save changes. The order was deleted by another user.");
                        }
                        else
                        {
                            var databaseValues = (Order)databaseEntry.ToObject();

                            if (databaseValues.FirstName != clientValues.FirstName)
                            {
                                ModelState.AddModelError("FirstName", $"Current value: {databaseValues.FirstName}");
                            }
                            if (databaseValues.LastName != clientValues.LastName)
                            {
                                ModelState.AddModelError("LastName", $"Current value: {databaseValues.LastName}");
                            }
                            if (databaseValues.City != clientValues.City)
                            {
                                ModelState.AddModelError("City", $"Current value: {databaseValues.City}");
                            }

                            if (databaseValues.State != clientValues.State)
                            {
                                ModelState.AddModelError("State", $"Current value: {databaseValues.State}");
                            }

                            ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                    + "was modified by another user after you got the original value. The "
                                    + "edit operation was canceled and the current values in the database "
                                    + "have been displayed. If you still want to edit this record, click "
                                    + "the Save button again. Otherwise click the Back to List hyperlink.");
                            orderToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                            ModelState.Remove("RowVersion");
                        }
                    }
                }
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
