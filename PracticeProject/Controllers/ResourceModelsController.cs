using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Areas.Identity.Data;
using PracticeProject.Models;

namespace PracticeProject.Controllers
{
    public class ResourceModelsController : Controller
    {
        private readonly ApplicationContext _context;

        public ResourceModelsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: ResourceModels
        public async Task<IActionResult> Index()
        {
              return _context.Resources != null ? 
                          View(await _context.Resources.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Resources'  is null.");
        }

        // GET: ResourceModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resources == null)
            {
                return NotFound();
            }

            var resourceModel = await _context.Resources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceModel == null)
            {
                return NotFound();
            }

            return View(resourceModel);
        }

        // GET: ResourceModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResourceModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResourceModel resourceModel)
        {
            resourceModel.CreatedAt = DateTime.Now;
            resourceModel.UpdatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (resourceModel.ImageFile != null)
                {
                    var fileName = Path.GetFileName(resourceModel.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resourceModel.ImageFile.CopyToAsync(stream);
                    }

                    resourceModel.ImageName = "/images/" + fileName;
                }

                _context.Add(resourceModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resourceModel);
        }

        // GET: ResourceModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Resources == null)
            {
                return NotFound();
            }

            var resourceModel = await _context.Resources.FindAsync(id);
            if (resourceModel == null)
            {
                return NotFound();
            }
            return View(resourceModel);
        }

        // POST: ResourceModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShortDescription,LongDescription,Link,ImageName,Type,UpdatedAt,CreatedAt")] ResourceModel resourceModel)
        {
            if (id != resourceModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resourceModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceModelExists(resourceModel.Id))
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
            return View(resourceModel);
        }

        // GET: ResourceModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Resources == null)
            {
                return NotFound();
            }

            var resourceModel = await _context.Resources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resourceModel == null)
            {
                return NotFound();
            }

            return View(resourceModel);
        }

        // POST: ResourceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Resources == null)
            {
                return Problem("Entity set 'ApplicationContext.Resources'  is null.");
            }
            var resourceModel = await _context.Resources.FindAsync(id);
            if (resourceModel != null)
            {
                _context.Resources.Remove(resourceModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResourceModelExists(int id)
        {
          return (_context.Resources?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
