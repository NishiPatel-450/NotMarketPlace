using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmailConfirm.Controllers
{
    public class AdminController : Controller
    {
        private NotMarketPlaceEntities _context;

        public AdminController()
        {
            _context = new NotMarketPlaceEntities();
        }
        // GET: Admin
        public ActionResult Dashboard()
        {
            var result = _context.SellNotes.ToList();
            return View(result);
        }

        public ActionResult NoteUnderReview()
        {
            var result = _context.SellNotes.ToList();
            return View(result);
        }
        public ActionResult ApproveNote(int id)
        {
            ViewBag.status = id;
            var result = _context.SellNotes.Where(x => x.Id == id).FirstOrDefault();
            if(result!= null)
            {
                result.Status = "Published";
                _context.Entry(result).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("NoteUnderReview");
        }
        public ActionResult InReviewNote(int id)
        {
            var result = _context.SellNotes.Where(x => x.Id == id).FirstOrDefault();
            if(result!= null)
            {
                result.Status = "InReview";
                _context.Entry(result).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("NoteUnderReview");
        }
        public ActionResult RejectNote(int id)
        {
            var result = _context.SellNotes.Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {
                result.Status = "Reject";
                _context.Entry(result).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
            return RedirectToAction("NoteUnderReview");
        }
        public ActionResult AdminPublishNote()
        {
            var result = _context.SellNotes.ToList();
            return View(result);
        }
        public ActionResult AdminDownloadNote()
        {
            var result = _context.Downloads.ToList();
            return View(result);
        }
        public ActionResult AdminRejectNote()
        {
            var result = _context.SellNotes.Where(x=>x.Status=="Reject").ToList();
            return View(result);
        }
    }
}