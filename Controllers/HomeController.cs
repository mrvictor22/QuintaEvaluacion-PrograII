using QuintaEvaluacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace QuintaEvaluacion.Controllers
{
    public class HomeController : Controller
    {
        private adventure_works_db_Entities db = new adventure_works_db_Entities();

        public ActionResult Index()
        {
            var photos = db.Photos.ToList().Where(m => m.CreatedAt.DayOfYear == DateTime.Now.DayOfYear).ToList();
            return View(photos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public System.Linq.Expressions.Expression<Func<Photo, string>> AsTimeAgo(DateTime dateTime)
        {

            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return p => ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return p => "a minute ago";

            if (delta < 45 * MINUTE)
                return p => ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return p => "an hour ago";

            if (delta < 24 * HOUR)
                return p => ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return p => "yesterday";

            if (delta < 30 * DAY)
                return p => ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return p => months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return p => years <= 1 ? "one year ago" : years + " years ago";
            }
        }
    }
}