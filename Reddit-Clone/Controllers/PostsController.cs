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
    public class PostsController : Controller
    {
        private RedditCloneEntities db = new RedditCloneEntities();

        // GET: Posts
        
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                //return RedirectToAction("Login", "UsersManage");
                var posts = db.Posts.Include(p => p.Subreddit).Include(p => p.User);
                return View(posts.ToList());
            }
            else
            {
                /*var posts = from s in db.Subreddits join p in db.Posts
                            on s.subreddit_name equals p.subreddit_name into groupjoin_subreddit_post
                            join f in db.Followings
                            on s.subreddit_name equals f.subreddit_name;*/
                List<Post> posts = (from f in db.Followings join s in db.Subreddits 
                                   on f.subreddit_name equals s.subreddit_name
                                   where f.username == Session["user"].ToString()
                                   join p in db.Posts
                                   on s.subreddit_name equals p.subreddit_name
                                   select p).ToList();

                return View(posts);
            }

            
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        
        // GET: Posts/Create
        public ActionResult Create()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "UsersManage");

            ViewBag.subreddit_name = new SelectList(db.Subreddits, "subreddit_name", "username");
            ViewBag.username = new SelectList(db.Users, "username", "gender");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "post_id,username,subreddit_name,title,content,image,upvotes,downvotes,edit_flag")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.subreddit_name = new SelectList(db.Subreddits, "subreddit_name", "username", post.subreddit_name);
            ViewBag.username = new SelectList(db.Users, "username", "gender", post.username);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.subreddit_name = new SelectList(db.Subreddits, "subreddit_name", "username", post.subreddit_name);
            ViewBag.username = new SelectList(db.Users, "username", "gender", post.username);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "post_id,username,subreddit_name,title,content,image,upvotes,downvotes,edit_flag")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.subreddit_name = new SelectList(db.Subreddits, "subreddit_name", "username", post.subreddit_name);
            ViewBag.username = new SelectList(db.Users, "username", "gender", post.username);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
