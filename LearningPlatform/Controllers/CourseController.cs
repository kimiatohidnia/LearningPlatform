using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningPlatform.Data;
using LearningPlatform.Models;

namespace LearningPlatform.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CourseController(ApplicationDBContext context)
        {
            _context = context;
        }

        #region Index
        // GET: Course
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Courses.Include(c => c.Category);
            return View(await applicationDBContext.ToListAsync());
        }
        #endregion

        #region Programming Language
        // GET: Programming-Language Course
        public async Task<IActionResult> ProgrammingLanguage()
        {
            var ProgrammingLanguageDBContext = _context.Courses.Include(c => c.Category).Where(c => c.CategoryId == 4);
            return View(await ProgrammingLanguageDBContext.ToListAsync());
        }
        #endregion
        #region Python
        // GET: Python Course
        public async Task<IActionResult> Python()
        {
            var PythonDBContext = _context.Courses.Include(c => c.Category).Where(c => c.KeyWord == "python");
            return View(await PythonDBContext.ToListAsync());
        }
        #endregion
        #region C#
        // GET: CSharp Course
        public async Task<IActionResult> CSharp()
        {
            var CSharpDBContext = _context.Courses.Include(c => c.Category).Where(c => c.KeyWord == "csharp");
            return View(await CSharpDBContext.ToListAsync());
        }
        #endregion

        #region Web Design
        // GET: WebDesign Course
        public async Task<IActionResult> WebDesign()
        {
            var WebDesignDBContext = _context.Courses.Include(c => c.Category).Where(c => c.CategoryId == 7);
            return View(await WebDesignDBContext.ToListAsync());
        }
        #endregion

        #region Database
        // GET: Database Course
        public async Task<IActionResult> Database()
        {
            var DatabaseDBContext = _context.Courses.Include(c => c.Category).Where(c => c.CategoryId == 8);
            return View(await DatabaseDBContext.ToListAsync());
        }
        #endregion

        #region Details
        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        #endregion

        #region Create
        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.ParentId != null), "CategoryId", "Name");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,Description,ImageUrl,Teacher,IsFree,Price,CategoryId,KeyWord")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.ParentId != null), "CategoryId", "Name", course.CategoryId);
            return View(course);
        }
        #endregion

        #region Edit
        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", course.CategoryId);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Description,ImageUrl,Teacher,IsFree,Price,CategoryId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", course.CategoryId);
            return View(course);
        }
        #endregion

        #region Delete
        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
        #endregion
    }
}
