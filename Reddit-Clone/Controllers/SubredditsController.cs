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
    public class SubredditsController : Controller
    {
        private RedditCloneEntities db = new RedditCloneEntities();

        // GET: Subreddits
        public ActionResult Index()
        {
            var subreddits = db.Subreddits.Include(s => s.User);
            return View(subreddits.ToList());
        }

        // GET: Subreddits/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreddit subreddit = db.Subreddits.Find(id);
            if (subreddit == null)
            {
                return HttpNotFound();
            }
            return View(subreddit);
        }

        // GET: Subreddits/Create
        public ActionResult Create()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");
            ViewBag.username = new SelectList(db.Users, "username", "gender");
            return View();
        }

        // POST: Subreddits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subreddit_name,username,description")] Subreddit subreddit)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");
            if (ModelState.IsValid)
            {
                db.Subreddits.Add(subreddit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.username = new SelectList(db.Users, "username", "gender", subreddit.username);
            return View(subreddit);
        }

        public void Follow(string id)
        {

        }

        public ActionResult Subreddit(string id)
        {
            List<Post> post = ( from r in db.Subreddits
                        join p in db.Posts
                        on r.subreddit_name equals p.subreddit_name
                        where r.subreddit_name == id
                        select p).ToList();
            System.Diagnostics.Debug.WriteLine(post);
            return View(post);
        }

        // GET: Subreddits/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            var x = from s in db.Subreddits
                    where s.subreddit_name == id
                    select s.username;

            if (Session["user"].ToString() != x.First())
            {
                ViewBag.message = "Not allowed to delete other users!";
                return RedirectToAction("Index", "Subreddits");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreddit subreddit = db.Subreddits.Find(id);
            if (subreddit == null)
            {
                return HttpNotFound();
            }
            ViewBag.username = new SelectList(db.Users, "username", "gender", subreddit.username);
            return View(subreddit);
        }

        // POST: Subreddits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subreddit_name,username,description")] Subreddit subreddit)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            if (ModelState.IsValid)
            {
                db.Entry(subreddit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.username = new SelectList(db.Users, "username", "gender", subreddit.username);
            return View(subreddit);
        }

        // GET: Subreddits/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            var x = from s in db.Subreddits
                    where s.subreddit_name == id
                    select s.username;

            if (Session["user"].ToString() != x.First())
            {
                ViewBag.message = "Not allowed to delete other users!";
                return RedirectToAction("Index", "Subreddits");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subreddit subreddit = db.Subreddits.Find(id);
            if (subreddit == null)
            {
                return HttpNotFound();
            }
            return View(subreddit);
        }

        // POST: Subreddits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            var x = from s in db.Subreddits
                    where s.subreddit_name == id
                    select s.username;

            if (Session["user"].ToString() != x.First())
            {
                ViewBag.message = "Not allowed to delete other users!";
                return RedirectToAction("Index", "Subreddits");
            }

            Subreddit subreddit = db.Subreddits.Find(id);
            db.Subreddits.Remove(subreddit);
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
