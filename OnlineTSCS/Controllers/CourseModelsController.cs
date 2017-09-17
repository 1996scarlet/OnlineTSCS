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
    public class CourseModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: CourseModels
        public async Task<ActionResult> Index(int? type, string name, string description, string upload)
        {
            var courses = db.CourseModels.AsQueryable();

            if (type != null) courses = courses.Where(x => (int)x.Type == type);

            if (!String.IsNullOrEmpty(name)) courses = courses.Where(x => x.CourseName.Contains(name));

            if (!String.IsNullOrEmpty(description)) courses = courses.Where(x => x.Description.Contains(description));

            if (!String.IsNullOrEmpty(upload))
            {
                var courseList = db.ACMappingModels.Where(x => (int)x.Account.Type == 3 && x.Account.AccountName.Contains(upload));
                courses = courses.Where(x => courseList.Select(y => y.CourseId).Contains(x.CourseId));
            }

            return View(await courses.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(int courseId, string comment)
        {
            if (!String.IsNullOrEmpty(comment))
            {
                db.CourseCommentModels.Add(new CourseCommentModel()
                {
                    Id = int.Parse(Session["Id"].ToString()),
                    CourseId = courseId,
                    CommentDate = System.DateTime.Now,
                    Comment = comment,
                    Frozen = false,
                    Reported = false
                });
                await db.SaveChangesAsync();

            }

            return RedirectToAction("Details", routeValues: new { id = courseId });
        }

        public async Task<ActionResult> Pick()
        {
            if (Session["Type"].ToString() == "学生")
            {
                var accountId = int.Parse(Session["Id"].ToString());
                ViewBag.StudentPick = await db.ACMappingModels.Where(x => x.Id == accountId).Select(x => x.CourseId).ToListAsync();
                return View(await db.CourseModels.Where(x => !x.Frozen && x.Checked).ToListAsync());
            }
            else
            {
                ViewBag.StudentPick = await db.ACMappingModels.Select(x => x.CourseId).ToListAsync();
            }
            return View(await db.CourseModels.ToListAsync());
        }

        public async Task<ActionResult> Choose(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //CourseModel courseModel = await db.CourseModels.FindAsync(id);
            var accountId = int.Parse(Session["Id"].ToString());
            var find = await db.ACMappingModels.FirstOrDefaultAsync(x => x.CourseId == id && x.Id == accountId);
            if (find == null)
            {
                db.ACMappingModels.Add(new ACMappingModel()
                {
                    Id = accountId,
                    CourseId = id.Value
                });
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Pick");
        }

        public async Task<ActionResult> Revoke(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var accountId = int.Parse(Session["Id"].ToString());
            var find = await db.ACMappingModels.FirstOrDefaultAsync(x => x.CourseId == id && x.Id == accountId);
            if (find != null)
            {
                db.ACMappingModels.Remove(find);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Pick");
        }

        public async Task<ActionResult> Mapping()
        {
            return View(await db.ACMappingModels.ToListAsync());
        }

        public async Task<ActionResult> Check()
        {
            return View(await db.CourseModels.Where(x => !x.Checked).ToListAsync());
        }

        // GET: CourseModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = await db.CourseModels.FindAsync(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }

            //ViewBag.Comment = await db.CourseCommentModels.Where(x => x.CourseId == id).ToListAsync();

            return View(courseModel);
        }

        // GET: CourseModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseId,CourseName,Description,Type,Capacity,Lesson,StartDate")] CourseModel courseModel)
        {
            if (ModelState.IsValid)
            {
                courseModel.Checked = false;
                courseModel.Frozen = false;
                courseModel.UploadId = int.Parse(Session["Id"].ToString());
                db.CourseModels.Add(courseModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseModel);
        }

        // GET: CourseModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = await db.CourseModels.FindAsync(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            return View(courseModel);
        }

        // POST: CourseModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CourseId,CourseName,Description,Type,Capacity,Lesson,StartDate,Checked,Frozen,UploadId,JudgeId")] CourseModel courseModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(courseModel);
        }

        // GET: CourseModels/Delete/5
        public async Task<ActionResult> Pass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = await db.CourseModels.FindAsync(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }

            courseModel.Checked = true;
            courseModel.JudgeId = int.Parse(Session["Id"].ToString());
            db.Entry(courseModel).State = EntityState.Modified;

            db.ACMappingModels.Add(new ACMappingModel()
            {
                CourseId = courseModel.CourseId,
                Id = courseModel.UploadId
            });

            await db.SaveChangesAsync();
            return RedirectToAction("Check");
        }

        // GET: CourseModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = await db.CourseModels.FindAsync(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            return View(courseModel);
        }

        // POST: CourseModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseModel courseModel = await db.CourseModels.FindAsync(id);
            db.CourseModels.Remove(courseModel);
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
