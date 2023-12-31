﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Areas.Identity.Data;
using PracticeProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace PracticeProject.Controllers
{
    public class ResourceModelsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public ResourceModelsController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ResourceModels
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
              return _context.Resources != null ? 
                          View(await _context.Resources.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Resources'  is null.");
        }

        // GET: ResourceModels/Details/5
        [Authorize(Roles = "Admin")]
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
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRequestedResource(int id)
        {
            ResourceRequestModel request = await _context.ResourceRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (request.IsCompleted || request.IsRejected) { return RedirectToAction("ShowRequestsPage", "Resources"); }
            request.IsBeingWatched = false;
            _context.Update(request);
            await _context.SaveChangesAsync();
            ResourceModel resource = new ResourceModel()
            {
                Name = request.Name,
                Link = request.Link,
                ShortDescription = request.Description,
                RequestId = request.Id,
            };
            return View("Create", resource);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: ResourceModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResourceModel resourceModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (resourceModel.ImageFile != null)
                    {
                        resourceModel.CreatedAt = DateTime.Now;
                        resourceModel.UpdatedAt = DateTime.Now;
                        string userId = _userManager.GetUserId(User);
                        resourceModel.UserId = userId;
                        var fileNameOrig = Path.GetFileName(resourceModel.ImageFile.FileName);
                        string fileName = fileNameOrig;
                        int i = 1;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{resourceModel.FolderName}");
                        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
                        else return View(resourceModel);
                        var filePath = Path.Combine(path, fileName);
                        while (System.IO.File.Exists(filePath))
                        {
                            string fileNameEx = Path.GetFileNameWithoutExtension(fileNameOrig);
                            string ex = Path.GetExtension(fileNameOrig);
                            fileName = $"{fileNameEx}({i}){ex}";
                            i++;
                            filePath = Path.Combine(path, fileName);
                        }
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await resourceModel.ImageFile.CopyToAsync(stream);
                        }

                        resourceModel.ImageName = fileName;
                        if (resourceModel.RequestId != null)
                        {
                            var request = await _context.ResourceRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == resourceModel.RequestId);
                            request.IsCompleted = true;
                            resourceModel.RequestId = request.Id;
                            _context.Update(request);
                        }

                        _context.Add(resourceModel);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResourceModel resourceModel)
        {
            if (id != resourceModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    resourceModel.UpdatedAt = DateTime.Now;
                    var name = await _context.Resources.AsNoTracking().Where(x => x.Id == id).Select(x => new { x.FolderName, x.ImageName }).FirstOrDefaultAsync();
                    string folderName = name.FolderName;
                    resourceModel.ImageName = name.ImageName;
                    if (resourceModel.ImageFile != null)
                    {
                        var fileNameOrig = Path.GetFileName(resourceModel.ImageFile.FileName);
                        string fileName = fileNameOrig;
                        int i = 1;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folderName}");
                        var filePath = Path.Combine(path, fileName);
                        while (System.IO.File.Exists(filePath))
                        {
                            string fileNameEx = Path.GetFileNameWithoutExtension(fileNameOrig);
                            string ex = Path.GetExtension(fileNameOrig);
                            fileName = $"{fileNameEx}({i}){ex}";
                            i++;
                            filePath = Path.Combine(path, fileName);
                        }
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await resourceModel.ImageFile.CopyToAsync(stream);
                        }

                        resourceModel.ImageName = fileName;
                    }
                    _context.Update(resourceModel);
                    _context.Entry(resourceModel).Property(x => x.FolderName).IsModified = false;
                    _context.Entry(resourceModel).Property(x => x.UserId).IsModified = false;
                    _context.Entry(resourceModel).Property(x => x.CreatedAt).IsModified = false;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                try
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{resourceModel.FolderName}");
                    if (Directory.Exists(filePath)) { Directory.Delete(filePath, true); }
                    _context.Resources.Remove(resourceModel);
                }
                catch (Exception ex) { }
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
