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
using OnlineTSCS.Filters;

namespace OnlineTSCS.Controllers
{
    public class AccountModelsController : Controller
    {
        private OTSCSModel db = new OTSCSModel();

        // GET: AccountModels
        public async Task<ActionResult> Index(int? type, string name, string school, string mobile)
        {
            var accounts = db.AccountModels.AsQueryable();

            if (type != null) accounts = accounts.Where(x => (int)x.Type == type);

            if (!String.IsNullOrEmpty(name)) accounts = accounts.Where(x => x.AccountName.Contains(name));

            if (!String.IsNullOrEmpty(school)) accounts = accounts.Where(x => x.School.Contains(school));

            if (!String.IsNullOrEmpty(mobile)) accounts = accounts.Where(x => x.Mobile.Contains(mobile));

            return View(await accounts.ToListAsync());
        }

        // GET: AccountModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountModel accountModel = await db.AccountModels.FindAsync(id);
            if (accountModel == null)
            {
                return HttpNotFound();
            }
            return View(accountModel);
        }

        // GET: AccountModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountModels/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AccountName,Password,Type,Credit,School,Mobile,Email")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                db.AccountModels.Add(accountModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(accountModel);
        }

        // GET: AccountModels/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: AccountModels/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "Id,AccountName,Password,Type")] AccountModel accountModel)
        {
            if (ModelState.IsValid && !await db.AccountModels.Where(x => x.AccountName == accountModel.AccountName).AnyAsync())
            {
                db.AccountModels.Add(accountModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("AccountName", "用户名已存在");
            }

            return View(accountModel);
        }

        // GET: AccountModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountModel accountModel = await db.AccountModels.FindAsync(id);
            if (accountModel == null)
            {
                return HttpNotFound();
            }
            return View(accountModel);
        }

        // POST: AccountModels/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [Operation]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AccountName,Password,Type,Credit,School,Mobile,Email")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(accountModel);
        }

        // GET: AccountModels/PasswordEdit/5
        public async Task<ActionResult> PasswordEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountModel accountModel = await db.AccountModels.FindAsync(id);
            if (accountModel == null)
            {
                return HttpNotFound();
            }

            accountModel.Password = "";
            return View(accountModel);
        }

        // POST: AccountModels/PasswordEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PasswordEdit([Bind(Include = "Id,AccountName,ConfirmPassword,Password,Type,Credit,School,Mobile,Email")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                accountModel.Password = accountModel.ConfirmPassword;
                db.Entry(accountModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("Password", "原密码错误");
            return View(accountModel);
        }

        // GET: AccountModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountModel accountModel = await db.AccountModels.FindAsync(id);
            if (accountModel == null)
            {
                return HttpNotFound();
            }
            return View(accountModel);
        }

        [Operation]
        // POST: AccountModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AccountModel accountModel = await db.AccountModels.FindAsync(id);
            db.AccountModels.Remove(accountModel);
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

        [Authentication(Checked = false)]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Operation]
        [Authentication(Checked = false)]
        public async Task<ActionResult> Login([Bind(Include = "AccountName,Password,Type")] AccountModel accountModel)
        {
            if (ModelState.IsValid)
            {
                var find = await db.AccountModels.FirstOrDefaultAsync(x => x.Type == accountModel.Type
                && x.AccountName == accountModel.AccountName
                && x.Password == accountModel.Password);

                if (find != null)
                {
                    Session["AccountName"] = accountModel.AccountName;
                    Session["Type"] = accountModel.Type.ToString();
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "用户名或密码错误！");
            }

            return View(accountModel);
        }
        
        [Authentication(Checked = false)]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
