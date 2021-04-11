using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EmailConfirm.Controllers
{
    public class NotesController : Controller
    {
        private NotMarketPlaceEntities _context;
        
        public NotesController()
        {
            _context = new NotMarketPlaceEntities();
        }
        //GET: /Notes/Index
        public ActionResult Index()
        {
            SellNote sn = new SellNote();
            var data = _context.NoteTypes.ToList();
            ViewBag.NoteType = new SelectList(data,"Name","Name");
            var item = _context.Countries.ToList();
            ViewBag.Country = new SelectList(item, "Name", "Name");
            var item1 = _context.NoteCatgories.ToList();
            ViewBag.Category = new SelectList(item1, "Name", "Name");
            var result = _context.SellNotes.ToList();
            ViewBag.University_Name = new SelectList(result, "University_Name", "University_Name");
            ViewBag.Course = new SelectList(result, "Course", "Course");

            return View(sn);

        }
        // POST: /Notes/Index
        [HttpPost]
        [Authorize]
        public ActionResult Index(SellNote model, FormCollection fc,HttpPostedFileBase imgfile,HttpPostedFileBase pdffile)  
        {
            var currenttime = DateTime.UtcNow;
            if (imgfile != null)
            {
                model.Display_pic = new byte[imgfile.ContentLength];
                imgfile.InputStream.Read(model.Display_pic, 0, imgfile.ContentLength);
            }

            String FileExt = Path.GetExtension(pdffile.FileName).ToUpper();
            if (FileExt == ".PDF")
            {
                Stream str = pdffile.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                model.Upload_note = FileDet;
            }

            var data = _context.NoteTypes.ToList();
            ViewBag.NoteType = new SelectList(data, "Name", "Name");
            var item = _context.Countries.ToList();
            ViewBag.Country = new SelectList(item, "Name", "Name");
            var item1 = _context.NoteCatgories.ToList();
            ViewBag.Category = new SelectList(item1, "Name", "Name");
            if (fc["save"] == "SAVE")
            {
                model.Status = "Draft";
            }
            if (fc["publish"] == "PUBLISH")
            {
                model.Status = " Submitted for Review";
            }
            var res = _context.Registers.Where(x => x.EmailId == User.Identity.Name).Single().Id;
            model.Seller_Id = res;
            _context.SellNotes.Add(model);
            _context.SaveChanges();
            return View(model);
        }
        [Authorize]
        public ActionResult ViewNotes()
        {
            var result = (from e in _context.SellNotes
                          join d in _context.Registers on e.Seller_Id equals d.Id
                          where d.EmailId == User.Identity.Name
                          select e);
            return View(result);
        }
        [AllowAnonymous]
        public ActionResult SearchNotes()
        {
            var data = _context.NoteTypes.ToList();
            ViewBag.NoteType = new SelectList(data, "Name", "Name");
            var item = _context.Countries.ToList();
            ViewBag.Country = new SelectList(item, "Name", "Name");
            var item1 = _context.NoteCatgories.ToList();
            ViewBag.Category = new SelectList(item1, "Name", "Name");
            var result = _context.SellNotes.Where(x => x.Status =="Published").ToList();
            ViewBag.University_Name = new SelectList(result, "University_Name", "University_Name");
            ViewBag.Course = new SelectList(result, "Course", "Course");
            return View(result);
        }
        public ActionResult NoteDetails(string id)
        {
            var result = _context.SellNotes.Where(x => x.title == id).ToList();
            return View(result);
        }
        public ActionResult BuyerReqest()
        {
            var result = _context.BuyerRequests.ToList();
            return View(result);
        }
        public ActionResult Buyer(BuyerRequest model,Download model1, string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currenttime = DateTime.UtcNow;
                model1.NoteId =_context.SellNotes.Where(x => x.title == id).Single().Id;
                var SellerId = _context.SellNotes.Where(x => x.title == id).Single().Seller_Id;
                model1.Seller = _context.Registers.Where(x => x.Id == SellerId).Single().EmailId;
                model1.Downloader = _context.Registers.Where(x => x.EmailId == User.Identity.Name).Single().Id;
                model1.IsPaid =_context.SellNotes.Where(x => x.title == id).Single().IsPaid;
                model1.NoteId =_context.SellNotes.Where(x => x.title == id).Single().Id;
                model1.NoteId =_context.SellNotes.Where(x => x.title == id).Single().Id;
                model1.NoteTitle = _context.SellNotes.Where(x => x.title == id).Single().title;
                model1.NoteCategory = _context.SellNotes.Where(x => x.title == id).Single().Category;
                model1.PurchasedPrice = _context.SellNotes.Where(x => x.title == id).Single().SellingPrice;
                model1.BuyerEmailId = User.Identity.Name;

                model.Title = _context.SellNotes.Where(x => x.title == id).Single().title;
                model.Category = _context.SellNotes.Where(x => x.title == id).Single().Category;
                model.SellType = _context.SellNotes.Where(x => x.title == id).Single().NoteType;
                model.Price = _context.SellNotes.Where(x => x.title == id).Single().SellingPrice;
                model.SellerId = _context.SellNotes.Where(x => x.title ==id).Single().Id;
                model.BuyerId = _context.UserProfiles.Where(x => x.User_email == User.Identity.Name).Single().Id;
                model.PhoneNo = _context.UserProfiles.Where(x => x.User_email == User.Identity.Name).Single().Phone_number;
            
                model.DownloadDateTime = currenttime;
                model1.AttachmentDownloadedDate = currenttime;
                model1.CreatedBy = User.Identity.Name;
                model1.ModifiedBy = User.Identity.Name;
                model1.CreatedDate = currenttime;
                model1.ModifiedDate = currenttime;
                _context.BuyerRequests.Add(model);
                _context.SaveChanges();
                _context.Downloads.Add(model1);
                _context.SaveChanges();
                return RedirectToAction("DownloadNotes");
            }
            else
            {
                return RedirectToAction("Login", "RegisterUser");
            }
        }
        public ActionResult DownloadNotes()
        {
            var result = _context.Downloads.Where(x => x.Seller == User.Identity.Name).ToList();
            return View(result);
        }
        public ActionResult SoldNotes()
        {
            var result = _context.Downloads.Where(x=>x.Seller==User.Identity.Name).ToList();
            return View(result);
        }
    }
}