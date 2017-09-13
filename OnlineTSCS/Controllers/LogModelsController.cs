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
    public class LogModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: LogModels
        public async Task<ActionResult> Index()
        {
            return View(await db.LogModels.ToListAsync());
        }

        // GET: LogModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogModel logModel = await db.LogModels.FindAsync(id);
            if (logModel == null)
            {
                return HttpNotFound();
            }
            return View(logModel);
        }

        // GET: LogModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LogModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ActionName,ControllerName,ActionParameters,AccessDate,ActionAccount")] LogModel logModel)
        {
            if (ModelState.IsValid)
            {
                db.LogModels.Add(logModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(logModel);
        }

        // GET: LogModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogModel logModel = await db.LogModels.FindAsync(id);
            if (logModel == null)
            {
                return HttpNotFound();
            }
            return View(logModel);
        }

        // POST: LogModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ActionName,ControllerName,ActionParameters,AccessDate,ActionAccount")] LogModel logModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(logModel);
        }

        // GET: LogModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogModel logModel = await db.LogModels.FindAsync(id);
            if (logModel == null)
            {
                return HttpNotFound();
            }
            return View(logModel);
        }

        // POST: LogModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LogModel logModel = await db.LogModels.FindAsync(id);
            db.LogModels.Remove(logModel);
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
