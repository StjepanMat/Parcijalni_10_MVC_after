using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ispit.Todo.Data;
using Ispit.Todo.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ispit.Todo.Controllers
{
    [Authorize]
    public class TodolistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodolistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todolist
        public async Task<IActionResult> Index()
        {
              return _context.Todolists != null ? 
                          View(await _context.Todolists.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Todolists'  is null.");
        }

        // GET: Todolist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Todolists == null)
            {
                return NotFound();
            }

            var todolist = await _context.Todolists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todolist == null)
            {
                return NotFound();
            }

            return View(todolist);
        }

        // GET: Todolist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todolist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,UserId,Description")] Todolist todolist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todolist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todolist);
        }

        // GET: Todolist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Todolists == null)
            {
                return NotFound();
            }

            var todolist = await _context.Todolists.FindAsync(id);
            if (todolist == null)
            {
                return NotFound();
            }
            return View(todolist);
        }

        // POST: Todolist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UserId,Description")] Todolist todolist)
        {
            if (id != todolist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todolist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodolistExists(todolist.Id))
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
            return View(todolist);
        }

        // GET: Todolist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Todolists == null)
            {
                return NotFound();
            }

            var todolist = await _context.Todolists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todolist == null)
            {
                return NotFound();
            }

            return View(todolist);
        }

        // POST: Todolist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Todolists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Todolists'  is null.");
            }
            var todolist = await _context.Todolists.FindAsync(id);
            if (todolist != null)
            {
                _context.Todolists.Remove(todolist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodolistExists(int id)
        {
          return (_context.Todolists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
