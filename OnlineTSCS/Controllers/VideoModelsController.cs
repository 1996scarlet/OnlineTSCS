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
    public class VideoModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: VideoModels
        public async Task<ActionResult> Index()
        {
            var videoModels = db.VideoModels.Include(v => v.Account).Include(v => v.Course);
            return View(await videoModels.ToListAsync());
        }

        public async Task<ActionResult> CourseVideoIndex(int? courseId)
        {
            var videoModels = db.VideoModels.Include(v => v.Account).Include(v => v.Course);
            return View(await videoModels.Where(x => x.CourseId == courseId.Value).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(int videoId, string comment)
        {
            if (!String.IsNullOrEmpty(comment))
            {
                db.VideoCommentModels.Add(new VideoCommentModel()
                {
                    Id = int.Parse(Session["Id"].ToString()),
                    VideoId = videoId,
                    CommentDate = DateTime.Now,
                    Comment = comment,
                    Frozen = false,
                    Reported = false
                });
                await db.SaveChangesAsync();

            }

            return RedirectToAction("Details", routeValues: new { id = videoId });
        }

        // GET: VideoModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoModel videoModel = await db.VideoModels.FindAsync(id);
            if (videoModel == null)
            {
                return HttpNotFound();
            }

            videoModel.Times++;
            db.Entry(videoModel).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return View(videoModel);
        }

        // GET: VideoModels/Create
        public ActionResult Create()
        {
            var accountId = int.Parse(Session["Id"].ToString());

            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName");
            if (Session["Type"].ToString() == "教师") ViewBag.CourseId = new SelectList(db.CourseModels.Where(x => x.UploadId == accountId && !x.Frozen), "CourseId", "CourseName");
            else ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName");

            return View();
        }

        // POST: VideoModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VideoId,CourseId,VideoName,Description,BiliAid,BiliCid")] VideoModel videoModel)
        {
            var accountId = int.Parse(Session["Id"].ToString());
            if (ModelState.IsValid)
            {
                videoModel.Id = accountId;
                videoModel.Frozen = false;
                videoModel.Times = 0;
                videoModel.UpLoadDate = DateTime.Now;
                videoModel.Reported = false;

                db.VideoModels.Add(videoModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", videoModel.Id);
            if (Session["Type"].ToString() == "教师")
            {
                ViewBag.CourseId = new SelectList(db.CourseModels.Where(x => x.UploadId == accountId && !x.Frozen), "CourseId", "CourseName", videoModel.CourseId);
            }
            else
            {
                ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", videoModel.CourseId);
            }
            return View(videoModel);
        }

        // GET: VideoModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoModel videoModel = await db.VideoModels.FindAsync(id);
            if (videoModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", videoModel.Id);
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", videoModel.CourseId);
            return View(videoModel);
        }

        // POST: VideoModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VideoId,CourseId,Id,UpLoadDate,VideoName,Description,BiliAid,BiliCid,Times,Frozen,Reported")] VideoModel videoModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(videoModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AccountModels, "Id", "AccountName", videoModel.Id);
            ViewBag.CourseId = new SelectList(db.CourseModels, "CourseId", "CourseName", videoModel.CourseId);
            return View(videoModel);
        }

        // GET: VideoModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoModel videoModel = await db.VideoModels.FindAsync(id);
            if (videoModel == null)
            {
                return HttpNotFound();
            }
            return View(videoModel);
        }

        // POST: VideoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VideoModel videoModel = await db.VideoModels.FindAsync(id);
            db.VideoModels.Remove(videoModel);
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
