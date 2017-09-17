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
    public class HomeworkModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: HomeworkModels
        public async Task<ActionResult> Index()
        {
            var homeworkModels = db.HomeworkModels.Include(h => h.Course);
            return View(await homeworkModels.ToListAsync());
        }

        // GET: HomeworkModels/Edit/5
        public async Task<ActionResult> Mark(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var homeworkCommentModel = await db.HomeworkCommentModels.FindAsync(id);
            if (homeworkCommentModel == null)
            {
                return HttpNotFound();
            }
            return View(homeworkCommentModel);
        }

        // POST: HomeworkModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Mark([Bind(Include = "HomeworkCommentId,Score")] HomeworkCommentModel homeworkCommentModel)
        {
            var finder = await db.HomeworkCommentModels.FindAsync(homeworkCommentModel.HomeworkCommentId);

            if (finder != null)
            {
                finder.Score = homeworkCommentModel.Score;
                db.Entry(finder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(homeworkCommentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(int homeworkId, string comment)
        {
            if (!String.IsNullOrEmpty(comment))
            {
                db.HomeworkCommentModels.Add(new HomeworkCommentModel()
                {
                    Score = 0,
                    Id = int.Parse(Session["Id"].ToString()),
                    HomeworkId = homeworkId,
                    PostDate = DateTime.Now,
                    Content = comment,
                    Frozen = false
                });
                await db.SaveChangesAsync();

            }

            return RedirectToAction("Details", routeValues: new { id = homeworkId });
        }

        // GET: HomeworkModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeworkModel homeworkModel = await db.HomeworkModels.FindAsync(id);
            if (homeworkModel == null)
            {
                return HttpNotFound();
            }
            return View(homeworkModel);
        }

        // GET: HomeworkModels/Create
        public ActionResult Create()
        {
            var accountId = int.Parse(Session["Id"].ToString());

            if (Session["Type"].ToString() == "教师") ViewBag.CourseId = new SelectList(db.CourseModels.Where(x => x.UploadId == accountId && !x.Frozen), "CourseId", "CourseName");
            else ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName");
            return View();
        }

        // POST: HomeworkModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HomeworkId,CourseId,StartDate,EndDate,Claim")] HomeworkModel homeworkModel)
        {
            var accountId = int.Parse(Session["Id"].ToString());

            if (ModelState.IsValid)
            {
                homeworkModel.Frozen = false;
                db.HomeworkModels.Add(homeworkModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            if (Session["Type"].ToString() == "教师")
            {
                ViewBag.CourseId = new SelectList(db.CourseModels.Where(x => x.UploadId == accountId && !x.Frozen), "CourseId", "CourseName", homeworkModel.CourseId);
            }
            else
            {
                ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", homeworkModel.CourseId);
            }
            return View(homeworkModel);
        }

        // GET: HomeworkModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeworkModel homeworkModel = await db.HomeworkModels.FindAsync(id);
            if (homeworkModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", homeworkModel.CourseId);
            return View(homeworkModel);
        }

        // POST: HomeworkModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HomeworkId,CourseId,StartDate,EndDate,Claim,Frozen")] HomeworkModel homeworkModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homeworkModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", homeworkModel.CourseId);
            return View(homeworkModel);
        }

        // GET: HomeworkModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeworkModel homeworkModel = await db.HomeworkModels.FindAsync(id);
            if (homeworkModel == null)
            {
                return HttpNotFound();
            }
            return View(homeworkModel);
        }

        // POST: HomeworkModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HomeworkModel homeworkModel = await db.HomeworkModels.FindAsync(id);
            db.HomeworkModels.Remove(homeworkModel);
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
