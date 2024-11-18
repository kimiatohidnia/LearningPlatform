using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LearningPlatform.Controllers
{
    public class ChapterController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ChapterController(ApplicationDBContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // دریافت شناسه کاربر جاری
            var course = await _context.Courses
                .Include(c => c.Chapters)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            // بررسی دسترسی برای دوره‌های پولی
            if (!course.IsFree)
            {
                var hasAccess = await _context.userCourses
                    .AnyAsync(uc => uc.CourseId == id && uc.UserId == userId);

                if (!hasAccess)
                {
                    return RedirectToAction("Buy", "UserCourse", new { courseId = id });
                }
            }

            return View(course);
        }


        #region Play
        [Authorize]
        public async Task<IActionResult> Play(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }
        #endregion

        #region Index
        // GET: Chapter
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Chapters.Include(c => c.Course);
            return View(await applicationDBContext.ToListAsync());
        }
        #endregion

        #region Data Analysis with Python
        // GET: Data Analysis with Python
        public async Task<IActionResult> DataAnalysiswithPython()
        {
            var DataAnalysiswithPythonDBContext = _context.Chapters.Include(c => c.Course);
            return View(await DataAnalysiswithPythonDBContext.ToListAsync());
        }
        #endregion

        #region Details
        // GET: Chapter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.ChapterId == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }
        #endregion

        #region Create
        // GET: Chapter/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title");
            return View();
        }

        // POST: Chapter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChapterId,Title,VideoUrl,Description,CourseId")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", chapter.CourseId);
            return View(chapter);
        }
        #endregion

        #region Edit
        // GET: Chapter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", chapter.CourseId);
            return View(chapter);
        }

        // POST: Chapter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChapterId,Title,VideoUrl,Description,CourseId")] Chapter chapter)
        {
            if (id != chapter.ChapterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.ChapterId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", chapter.CourseId);
            return View(chapter);
        }
        #endregion

        #region Delete
        // GET: Chapter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.ChapterId == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.ChapterId == id);
        }
        #endregion
    }
}
