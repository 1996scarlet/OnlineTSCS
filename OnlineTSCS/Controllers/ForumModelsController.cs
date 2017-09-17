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
    public class ForumModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: ForumModels
        public async Task<ActionResult> Index(int? mode)
        {
            var forumModels = db.ForumModels.Include(f => f.Account);
            switch (mode)
            {
                case 0: return View(await forumModels.OrderBy(x => x.Title).ToListAsync());
                case 1: return View(await forumModels.OrderByDescending(x => x.CommentDate).ToListAsync());
                case 2: return View(await forumModels.OrderByDescending(x => x.ForumComments.Count).ToListAsync());
                case 3: return View(await forumModels.Where(x=>!x.Frozen).ToListAsync());
                case 4: return View(await forumModels.Where(x=>!x.Reported).ToListAsync());
                case 5:
                default: return View(await forumModels.ToListAsync());
            }

        }

        // GET: ForumModels/Report/5
        public async Task<ActionResult> Report(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumModel forumModel = await db.ForumModels.FindAsync(id);
            if (forumModel == null)
            {
                return HttpNotFound();
            }
            forumModel.Reported = !forumModel.Reported;
            db.Entry(forumModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Manage");
        }

        // GET: ForumModels/Frozen/5
        public async Task<ActionResult> Frozen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumModel forumModel = await db.ForumModels.FindAsync(id);
            if (forumModel == null)
            {
                return HttpNotFound();
            }
            forumModel.Frozen = !forumModel.Frozen;
            db.Entry(forumModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Manage");
        }

        // GET: ForumModels/CommentReport/5
        public async Task<ActionResult> CommentReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumCommentModel forumCommentModel = await db.ForumCommentModels.FindAsync(id);
            if (forumCommentModel == null)
            {
                return HttpNotFound();
            }
            forumCommentModel.Reported = !forumCommentModel.Reported;
            db.Entry(forumCommentModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Manage");
        }

        // GET: ForumModels/CommentFrozen/5
        public async Task<ActionResult> CommentFrozen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumCommentModel forumCommentModel = await db.ForumCommentModels.FindAsync(id);
            if (forumCommentModel == null)
            {
                return HttpNotFound();
            }
            forumCommentModel.Frozen = !forumCommentModel.Frozen;
            db.Entry(forumCommentModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Manage()
        {
            return View(await db.ForumModels.Include(f => f.Account).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Posting(string title, string comment)
        {
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(comment))
            {
                db.ForumModels.Add(new ForumModel()
                {
                    Frozen = false,
                    Reported = false,
                    Id = int.Parse(Session["Id"].ToString()),
                    Comment = comment,
                    Title = title,
                    CommentDate = DateTime.Now
                });
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(int forumItemId, string comment)
        {
            if (!String.IsNullOrEmpty(comment))
            {
                db.ForumCommentModels.Add(new ForumCommentModel()
                {
                    Frozen = false,
                    Reported = false,
                    ForumItemId = forumItemId,
                    Id = int.Parse(Session["Id"].ToString()),
                    Comment = comment,
                    CommentDate = DateTime.Now
                });
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Details", routeValues: new { id = forumItemId });
        }

        // GET: ForumModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumModel forumModel = await db.ForumModels.FindAsync(id);
            if (forumModel == null)
            {
                return HttpNotFound();
            }
            return View(forumModel);
        }

        // GET: ForumModels/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName");
            return View();
        }

        // POST: ForumModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ForumItemId,Id,CommentDate,Title,Comment,Frozen,Reported")] ForumModel forumModel)
        {
            if (ModelState.IsValid)
            {
                db.ForumModels.Add(forumModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", forumModel.Id);
            return View(forumModel);
        }

        // GET: ForumModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumModel forumModel = await db.ForumModels.FindAsync(id);
            if (forumModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", forumModel.Id);
            return View(forumModel);
        }

        // POST: ForumModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ForumItemId,Id,CommentDate,Title,Comment,Frozen,Reported")] ForumModel forumModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forumModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", forumModel.Id);
            return View(forumModel);
        }

        // GET: ForumModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumModel forumModel = await db.ForumModels.FindAsync(id);
            if (forumModel == null)
            {
                return HttpNotFound();
            }
            return View(forumModel);
        }

        // POST: ForumModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ForumModel forumModel = await db.ForumModels.FindAsync(id);
            db.ForumModels.Remove(forumModel);
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
