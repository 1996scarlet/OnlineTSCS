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
    public class HomeworkCommentModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: HomeworkCommentModels
        public async Task<ActionResult> Index()
        {
            var homeworkCommentModels = db.HomeworkCommentModels.Include(h => h.Account).Include(h => h.Homework);
            return View(await homeworkCommentModels.ToListAsync());
        }

        // GET: HomeworkCommentModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeworkCommentModel homeworkCommentModel = await db.HomeworkCommentModels.FindAsync(id);
            if (homeworkCommentModel == null)
            {
                return HttpNotFound();
            }
            return View(homeworkCommentModel);
        }

        // GET: HomeworkCommentModels/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName");
            ViewBag.HomeworkId = new SelectList(db.HomeworkModels, "HomeworkId", "Claim");
            return View();
        }

        // POST: HomeworkCommentModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HomeworkCommentId,Id,HomeworkId,PostDate,Score,Content,Frozen")] HomeworkCommentModel homeworkCommentModel)
        {
            if (ModelState.IsValid)
            {
                db.HomeworkCommentModels.Add(homeworkCommentModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", homeworkCommentModel.Id);
            ViewBag.HomeworkId = new SelectList(db.HomeworkModels, "HomeworkId", "Claim", homeworkCommentModel.HomeworkId);
            return View(homeworkCommentModel);
        }

        // GET: HomeworkCommentModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeworkCommentModel homeworkCommentModel = await db.HomeworkCommentModels.FindAsync(id);
            if (homeworkCommentModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", homeworkCommentModel.Id);
            ViewBag.HomeworkId = new SelectList(db.HomeworkModels, "HomeworkId", "Claim", homeworkCommentModel.HomeworkId);
            return View(homeworkCommentModel);
        }

        // POST: HomeworkCommentModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HomeworkCommentId,Id,HomeworkId,PostDate,Score,Content,Frozen")] HomeworkCommentModel homeworkCommentModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homeworkCommentModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", homeworkCommentModel.Id);
            ViewBag.HomeworkId = new SelectList(db.HomeworkModels, "HomeworkId", "Claim", homeworkCommentModel.HomeworkId);
            return View(homeworkCommentModel);
        }

        // GET: HomeworkCommentModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomeworkCommentModel homeworkCommentModel = await db.HomeworkCommentModels.FindAsync(id);
            if (homeworkCommentModel == null)
            {
                return HttpNotFound();
            }
            return View(homeworkCommentModel);
        }

        // POST: HomeworkCommentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HomeworkCommentModel homeworkCommentModel = await db.HomeworkCommentModels.FindAsync(id);
            db.HomeworkCommentModels.Remove(homeworkCommentModel);
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
