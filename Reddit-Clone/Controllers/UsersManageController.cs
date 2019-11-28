using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Reddit_Clone.Models;

namespace Reddit_Clone.Controllers
{
    public class UsersManageController : Controller
    {
        private RedditCloneEntities db = new RedditCloneEntities();

        // GET: UsersManage
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: UsersManage/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login()
        {
            if (Session["user"] != null)
            {
                ViewBag.message = "Alreay logged in!";
                return RedirectToAction("Subreddit" , "Subreddits");
            }
            return View();
        }

        public ActionResult Login(string id, string password)
        {
            if (ModelState.IsValid)
            {
                if (Session["user"] != null)
                {
                    ViewBag.message = "Already Logged In!";
                    return RedirectToAction("Index", "UsersManage");
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    ViewBag.message1 = "User does not exist";
                    return RedirectToAction("Create", "UsersManage");
                }
                else
                {
                    if (user.password != password)
                    {
                        ViewBag.message = "Wrong Password";
                        
                    }
                    if (true)
                    {
                        Session["user"] = user.username;
                        return RedirectToAction("Index", "UsersManage");
                    }
                }
            }
            return View();
        }

        public ActionResult Follow()
        {
            return View();
        }

        public ActionResult Follow(string submitValue)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            if (ModelState.IsValid)
            {
                
                var user = db.Users.Find(Session["user"]);
                user.followers += 1;
                db.Users.Add(user);
                Following f = new Following();
                f.username = Session["user"].ToString();
                f.subreddit_name = submitValue;
                db.Followings.Add(f);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "UsersManage");
            //return View();
        }

        // GET: UsersManage/Create
        public ActionResult Create()
        {
            if (Session["user"] != null)
            {
                ViewBag.message = "Logout first!";
                return RedirectToAction("Login", "UsersManage");
            }

            return View();
        }

        // POST: UsersManage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,creation_time,gender,followers,password")] User user)
        {
            if (Session["user"] != null)
            {
                ViewBag.message = "Logout first!";
                return RedirectToAction("Login", "UsersManage");
            }

            if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            return View(user);
        }

        // GET: UsersManage/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            if (Session["user"].ToString() != id)
            {
                ViewBag.message = "Not allowed to delete other users!";
                return RedirectToAction("Index", "Subreddits");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UsersManage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,creation_time,gender,followers,password")] User user)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: UsersManage/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            if (Session["user"].ToString() != id)
            {
                ViewBag.message = "Not allowed to delete other users!";
                return RedirectToAction("Index", "Subreddits");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UsersManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            if (Session["user"].ToString() != id)
            {
                ViewBag.message = "Not allowed to delete other users!";
                return RedirectToAction("Index", "Subreddits");
            }

            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
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
