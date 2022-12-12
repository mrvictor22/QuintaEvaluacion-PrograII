using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QuintaEvaluacion.Data;
using QuintaEvaluacion.Models;

namespace QuintaEvaluacion.Controllers
{
    public class PhotoController : Controller
    {
        private adventure_works_db_Entities db = new adventure_works_db_Entities();

        // GET: Photos
        public ActionResult Index()
        {
            return View();
        }

        // POST: Photo/Create
        public async Task<ActionResult> Create([Bind(Include="Title,Description,Image")] PhotoModel photo)
        {
            string path = Server.MapPath("~/Uploads/" + Session["user_id"] + "/");
            int userId = (int) Session["user_id"];
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if(photo.Image != null)
            {
                string fileName = Path.GetFileName(photo.Image.FileName);
                photo.Image.SaveAs(path + fileName);

                // Save into database
                db.Photos.Add(new Photo()
                {
                    Title = photo.Title,
                    Description = photo.Description,
                    Slug = fileName,
                    UserId = userId,
                    CreatedAt = DateTime.Now
                });

                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
                
        // GET: Photos/Details/5
        public ActionResult Comments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Comment/
        [HttpPost]
        public async Task<ActionResult> Comment(string title, string description, int id)
        {
            var userId = (int)Session["user_id"];
            var comment = new Comment() {
                Title = title,
                Content = description,
                PhotoId = id,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return RedirectToAction("Comments", "Photo", new { id });
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
