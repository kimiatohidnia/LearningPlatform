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
    public class UserCourseController : Controller
    {
        private readonly ApplicationDBContext _context;

        public UserCourseController(ApplicationDBContext context)
        {
            _context = context;
        }

        #region Buy
        [Authorize]
        public async Task<IActionResult> Buy(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // دریافت ID کاربر جاری

            // بررسی اینکه آیا کاربر این دوره را قبلاً خریداری کرده است
            var hasAccess = await _context.userCourses
                .AnyAsync(uc => uc.CourseId == courseId && uc.UserId == userId);

            if (hasAccess)
            {
                return RedirectToAction("Details", "Course", new { id = courseId }); // اگر خرید کرده باشد
            }

            // ثبت خرید جدید
            var userCourse = new UserCourse
            {
                UserId = userId,
                CourseId = courseId
            };

            _context.userCourses.Add(userCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Course", new { id = courseId }); // هدایت به جزئیات دوره
        }

        #endregion

        #region Index
        // GET: UserCourse
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.userCourses.Include(u => u.Course).Include(u => u.User);
            return View(await applicationDBContext.ToListAsync());
        }
        #endregion

        #region Details
        // GET: UserCourse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.userCourses
                .Include(u => u.Course)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserCourseId == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }
        #endregion

        #region Create
        // GET: UserCourse/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserCourse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserCourseId,UserId,CourseId,PurchaseDate,HasAccess")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCourse.UserId);
            return View(userCourse);
        }
        #endregion

        #region Edit
        // GET: UserCourse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.userCourses.FindAsync(id);
            if (userCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCourse.UserId);
            return View(userCourse);
        }

        // POST: UserCourse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserCourseId,UserId,CourseId,PurchaseDate,HasAccess")] UserCourse userCourse)
        {
            if (id != userCourse.UserCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCourseExists(userCourse.UserCourseId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Title", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCourse.UserId);
            return View(userCourse);
        }
        #endregion

        #region Delete
        // GET: UserCourse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.userCourses
                .Include(u => u.Course)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserCourseId == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // POST: UserCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCourse = await _context.userCourses.FindAsync(id);
            if (userCourse != null)
            {
                _context.userCourses.Remove(userCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCourseExists(int id)
        {
            return _context.userCourses.Any(e => e.UserCourseId == id);
        }
        #endregion
    }
}
