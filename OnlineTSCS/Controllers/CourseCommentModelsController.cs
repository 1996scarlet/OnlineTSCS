using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineTSCS.Models;

namespace OnlineTSCS.Controllers
{
    public class CourseCommentModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: CourseCommentModels
        public async Task<ActionResult> Index()
        {
            var courseCommentModels = db.CourseCommentModels.Include(c => c.Account).Include(c => c.Course);
            return View(await courseCommentModels.ToListAsync());
        }

        // GET: CourseCommentModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCommentModel courseCommentModel = await db.CourseCommentModels.FindAsync(id);
            if (courseCommentModel == null)
            {
                return HttpNotFound();
            }
            return View(courseCommentModel);
        }

        // GET: CourseCommentModels/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName");
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName");
            return View();
        }

        // POST: CourseCommentModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseCommentId,CourseId,Id,CommentDate,Comment,Frozen,Reported")] CourseCommentModel courseCommentModel)
        {
            if (ModelState.IsValid)
            {
                db.CourseCommentModels.Add(courseCommentModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", courseCommentModel.Id);
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", courseCommentModel.CourseId);
            return View(courseCommentModel);
        }

        // GET: CourseCommentModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCommentModel courseCommentModel = await db.CourseCommentModels.FindAsync(id);
            if (courseCommentModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", courseCommentModel.Id);
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", courseCommentModel.CourseId);
            return View(courseCommentModel);
        }

        // POST: CourseCommentModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseCommentId,CourseId,Id,CommentDate,Comment,Frozen,Reported")] CourseCommentModel courseCommentModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseCommentModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", courseCommentModel.Id);
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", courseCommentModel.CourseId);
            return View(courseCommentModel);
        }

        // GET: CourseCommentModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCommentModel courseCommentModel = await db.CourseCommentModels.FindAsync(id);
            if (courseCommentModel == null)
            {
                return HttpNotFound();
            }
            return View(courseCommentModel);
        }

        // POST: CourseCommentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCommentModel courseCommentModel = await db.CourseCommentModels.FindAsync(id);
            db.CourseCommentModels.Remove(courseCommentModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
