using EmailConfirm.Models;
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
        public ActionResult Index()
        {
            SellNote sn = new SellNote();
            var data = _context.NoteTypes.ToList();
            ViewBag.NoteType = new SelectList(data, "Name", "Name");
            var item = _context.Countries.ToList();
            ViewBag.Country = new SelectList(item, "Name", "Name");
            var item1 = _context.NoteCatgories.ToList();
            ViewBag.Category = new SelectList(item1, "Name", "Name");
            var result = _context.SellNotes.ToList();
            ViewBag.University_Name = new SelectList(result, "University_Name", "University_Name");
            ViewBag.Course = new SelectList(result, "Course", "Course");
            
             return View(sn);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Index(SellNote model,SellerNotesAttachement mod, FormCollection fc,HttpPostedFileBase imgfile,HttpPostedFileBase pdffile)  
        {

            var data = _context.NoteTypes.ToList();
            ViewBag.NoteType = new SelectList(data, "Name", "Name");
            var item = _context.Countries.ToList();
            ViewBag.Country = new SelectList(item, "Name", "Name");
            var item1 = _context.NoteCatgories.ToList();
            ViewBag.Category = new SelectList(item1, "Name", "Name");
            
            if (ModelState.IsValid)
            {
                    var currenttime = DateTime.UtcNow;
                    if (imgfile != null)
                    {
                        model.Display_pic = new byte[imgfile.ContentLength];
                        imgfile.InputStream.Read(model.Display_pic, 0, imgfile.ContentLength);
                    }
                    if (pdffile != null) { 
                    String FileExt = Path.GetExtension(pdffile.FileName).ToUpper();
                    if (FileExt == ".PDF")
                    {
                        Stream str = pdffile.InputStream;
                        BinaryReader Br = new BinaryReader(str);
                        Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                        mod.FileName = model.FileName = pdffile.FileName;
                        mod.FileContent= model.Upload_note = FileDet;
                        mod.AttachmentSize = pdffile.ContentLength/1024;
                        
                    }
                    }
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
                    mod.CreatedBy= model.CreatedBy = User.Identity.Name;
                    mod.CreatedDate =model.CreatedDate = currenttime;
                    mod.ModifiedBy =model.ModifiedBy = User.Identity.Name;
                    mod.ModifiedDate =model.ModifiedDate = currenttime;
                    mod.NoteTitle = model.title;
                    _context.SellerNotesAttachements.Add(mod);
                    _context.SellNotes.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("ViewNotes");
                }
                else
                {
                    return View();
                }
            
        }
        public ActionResult updateNote(int id)
        {   SellNote sn = new SellNote();
            var data = _context.NoteTypes.ToList();
            ViewBag.NoteType = new SelectList(data, "Name", "Name");
            var item = _context.Countries.ToList();
            ViewBag.Country = new SelectList(item, "Name", "Name");
            var item1 = _context.NoteCatgories.ToList();
            ViewBag.Category = new SelectList(item1, "Name", "Name");
            var result = _context.SellNotes.ToList();
            ViewBag.University_Name = new SelectList(result, "University_Name", "University_Name");
            ViewBag.Course = new SelectList(result, "Course", "Course");
            var rs = _context.SellNotes.Where(x => x.Id == id).ToList();
            ViewData["data"] = rs;
            return View(sn);
        }
        [HttpPost]
        public ActionResult updateNote(SellNote model, FormCollection fc, HttpPostedFileBase imgfile, HttpPostedFileBase pdffile)
        {
            if (ModelState.IsValid)
            {
                var currenttime = DateTime.UtcNow;
                if (imgfile != null)
                {
                    model.Display_pic = new byte[imgfile.ContentLength];
                    imgfile.InputStream.Read(model.Display_pic, 0, imgfile.ContentLength);
                }
                if (pdffile != null)
                {
                    String FileExt = Path.GetExtension(pdffile.FileName).ToUpper();
                    if (FileExt == ".PDF")
                    {
                        Stream str = pdffile.InputStream;
                        BinaryReader Br = new BinaryReader(str);
                        Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                        model.FileName = pdffile.FileName;
                        model.Upload_note = FileDet;
                    }
                }
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
                model.ModifiedDate = currenttime;

                model.Seller_Id = _context.Registers.Where(x => x.EmailId == User.Identity.Name).Single().Id;
                _context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                return Redirect("ViewNotes");
            }
            else
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult ViewNotes(string searching,string search1)
        {
            var result = (from e in _context.SellNotes
                          join d in _context.Registers on e.Seller_Id equals d.Id
                          where d.EmailId == User.Identity.Name
                          select e).OrderByDescending(x=>x.CreatedDate);
            var data = (from e in _context.SellNotes
                        join d in _context.Registers on e.Seller_Id equals d.Id
                        where d.EmailId == User.Identity.Name
                        select e).Where(x => x.Status == "Published");
            if(!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.SellNotes
                          join d in _context.Registers on e.Seller_Id equals d.Id
                          where d.EmailId == User.Identity.Name
                          select e).Where(x => x.title.Contains(searching)|| x.Category.Contains(searching)|| x.Status.Contains(searching) || searching == null).OrderByDescending(x => x.CreatedDate);
            }
            if (!String.IsNullOrEmpty(search1))
            {
                data = (from e in _context.SellNotes
                          join d in _context.Registers on e.Seller_Id equals d.Id
                          where d.EmailId == User.Identity.Name
                          where e.Status == "Published"
                          select e).Where(x => x.title.Contains(search1) || x.Category.Contains(search1) || x.Status.Contains(search1) || search1 == null );
            }
            var download = (from e in _context.Downloads
                                join d in _context.Registers on e.Downloader equals d.Id
                                where e.BuyerEmailId == User.Identity.Name
                                select e).Count();
            ViewBag.download = download;
            var reject = (from e in _context.Registers
                          join d in _context.SellNotes on e.Id equals d.Seller_Id
                          join r in _context.RejectedNotes on d.Id equals r.Seller_Id
                          where e.EmailId == User.Identity.Name
                          select r).Count();
            ViewBag.reject = reject;
            var buyer = (from e in _context.Registers
                         join d in _context.SellNotes on e.Id equals d.Seller_Id
                         join r in _context.BuyerRequests on d.Seller_Id equals r.SellerId
                         where e.EmailId == User.Identity.Name
                         select r).Count();
            ViewBag.buyer = buyer;
            return View(Tuple.Create(result.ToList(),data.ToList()));
        }
        
        public ActionResult deleteNote(int id)
        {
            var res = _context.SellNotes.Where(x => x.Id == id).FirstOrDefault();
            var res1 = _context.BuyerRequests.Where(x => x.SellerId == id).FirstOrDefault();
            _context.SellNotes.Remove(res);
            if(res1!=null)
            { 
            _context.BuyerRequests.Remove(res1);
            }
            _context.SaveChanges();
            return RedirectToAction("ViewNotes");
        }
        [AllowAnonymous]
        public ActionResult SearchNotes(string Search, string list,string list1,string list2,string list3,string list4)
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
            var result1 = _context.SellNotes.Where(x => x.Status == "Published").ToList();

            if (!String.IsNullOrEmpty(Search))
            {
                result1 = (from e in _context.SellNotes
                          where e.Status == "Published"
                          select e).Where(x => x.title.Contains(Search) || Search == null).ToList();
            }
            if (!String.IsNullOrEmpty(list))
            {
                result1 = (from e in _context.SellNotes
                          where e.Status == "Published"
                          select e).Where(x => x.NoteType.Contains(list) || list == null).ToList();
            }
            
            if (!String.IsNullOrEmpty(list1))
            {
                result1 = (from e in _context.SellNotes
                          where e.Status == "Published"
                          select e).Where(x => x.Category.Contains(list1) || list1 == null).ToList();
            }
            if (!String.IsNullOrEmpty(list2))
            {
                result1 = (from e in _context.SellNotes
                          where e.Status == "Published"
                          select e).Where(x => x.University_Name.Contains(list2) || list2 == null).ToList();
            }
            if (!String.IsNullOrEmpty(list3))
            {
                result1 = (from e in _context.SellNotes
                          where e.Status == "Published"
                          select e).Where(x => x.Course.Contains(list3) || list3 == null).ToList();
            }
            if (!String.IsNullOrEmpty(list4))
            {
                result1 = (from e in _context.SellNotes
                          where e.Status == "Published"
                          select e).Where(x => x.Country.Contains(list4) || list4 == null).ToList();
            }
            return View(result1);
        }
        public ActionResult NoteDetails(string id)
        {
            var result = _context.SellNotes.Where(x => x.title == id).ToList();
            return View(result);
        }
        public ActionResult BuyerReqest(string searching)
        {
            var result = (from e in _context.Registers
                         join d in _context.SellNotes on e.Id equals d.Seller_Id
                         join r in _context.BuyerRequests on d.Seller_Id equals r.SellerId
                         where e.EmailId == User.Identity.Name
                         select r);
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.Registers
                          join d in _context.SellNotes on e.Id equals d.Seller_Id
                          join r in _context.BuyerRequests on d.Seller_Id equals r.SellerId
                          where e.EmailId == User.Identity.Name
                          select r).Where(x => x.Title.Contains(searching) || searching == null);
            }
            return View(result.ToList());
        }
        [HttpGet]
        public ActionResult Buyer(BuyerRequest model,Download model1, string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currenttime = DateTime.UtcNow;
                model1.NoteId =_context.SellNotes.Where(x => x.title == id).Single().Id;
                var SellerId = _context.SellNotes.Where(x => x.title == id).Single().Seller_Id;
                model1.Seller = _context.Registers.Where(x => x.Id == SellerId).Single().EmailId;
                model1.Downloader = _context.Registers.Where(x => x.EmailId == User.Identity.Name).Single().Id;
                model1.IsPaid =false;
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
                var FileById = (from FC in _context.SellNotes
                                where FC.title.Equals(id)
                                select new { FC.FileName, FC.Upload_note }).ToList().FirstOrDefault();

                return File(FileById.Upload_note, "application/pdf", FileById.FileName);

            }
            else
            {
                return RedirectToAction("Login", "RegisterUser");
            }
        }
        [HttpGet]
        public ActionResult download(string id)
        {
            var FileById = (from FC in _context.SellNotes
                            where FC.title.Equals(id)
                            select new { FC.FileName, FC.Upload_note }).ToList().FirstOrDefault();

            return File(FileById.Upload_note, "application/pdf", FileById.FileName);

        }
        public ActionResult DownloadNotes(string searching)
        {
            var result = _context.Downloads.Where(x => x.BuyerEmailId == User.Identity.Name);
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.Downloads 
                          where e.BuyerEmailId == User.Identity.Name
                          select e).Where(x => x.NoteTitle.Contains(searching) || searching == null);
            }
            return View(result.ToList());
        }
        public ActionResult SoldNotes(string searching)
        {
            var result = _context.Downloads.Where(x => x.BuyerEmailId == User.Identity.Name);
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.Downloads
                          where e.BuyerEmailId == User.Identity.Name
                          select e).Where(x => x.NoteTitle.Contains(searching) || searching == null);
            }
            return View(result.ToList());
        }
        public ActionResult RejectNotes(string searching)
        {
            var result = (from e in _context.Registers
                          join d in _context.SellNotes on e.Id equals d.Seller_Id
                          join r in _context.RejectedNotes on d.Id equals r.Seller_Id
                          where e.EmailId == User.Identity.Name
                          select new RejectNote {
                              id = r.Id,
                              title = r.NoteTitle,
                              categoey = d.Category,
                              remark = r.Remark
                            });

            if (!String.IsNullOrEmpty(searching))
            {
                 result = (from e in _context.Registers
                           join d in _context.SellNotes on e.Id equals d.Seller_Id
                           join r in _context.RejectedNotes on d.Id equals r.Seller_Id
                           where e.EmailId == User.Identity.Name
                           select new RejectNote{
                               id = r.Id,
                               title = r.NoteTitle,
                               categoey = d.Category,
                               remark = r.Remark
                           }).Where(x => x.title.Contains(searching) || searching == null);
                }
            return View(result.ToList());
        }
    }
}