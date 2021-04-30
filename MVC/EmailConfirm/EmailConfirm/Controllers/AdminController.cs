using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailConfirm.Models;

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
        public ActionResult Dashboard(string searching,string list)
        {
            var result = (from e in _context.SellNotes
                      join d in _context.Downloads
                      on e.Id equals d.NoteId into j1
                      from j2 in j1.DefaultIfEmpty()
                      where e.Status == "Published"
                      select new 
                      { 
                          id = e.Id,
                          title = e.title,
                          cate =e.Category,
                          size ="10kb",
                          type =e.NoteType,
                          price=e.SellingPrice , 
                          publisher =e.Seller_Id, 
                          date = e.ModifiedDate,
                          no_dow = j2 == null ? 0 : 1
                      }).GroupBy(x=>new { a = x.id, b = x.no_dow,c=x.title,d=x.cate,e=x.size,f=x.type,
                                            h=x.price,i=x.publisher,k=x.date})
                     .Select(g=>new CountData{ id = g.Key.a,
                         title =g.Key.c,
                         cate = g.Key.d,
                         size = g.Key.e,
                         type = g.Key.f,
                         price = (int)g.Key.h,
                         publisher = g.Key.i,
                         date = (DateTime?)g.Key.k ?? DateTime.Now,
                         no_dow =g.Sum(q=>q.no_dow)}).OrderBy(x=>x.id);

            SelectList list1  = new SelectList(result, "Id", "title");
            ViewBag.list = list1;
            if (!String.IsNullOrEmpty(searching))
            {
               result= (IOrderedQueryable<CountData>)(from e in _context.SellNotes
                 join d in _context.Downloads
                 on e.Id equals d.NoteId into j1
                 from j2 in j1.DefaultIfEmpty()
                 where e.Status == "Published"
                 select new
                 {
                     id = e.Id,
                     title = e.title,
                     cate = e.Category,
                     size = "10kb",
                     type = e.NoteType,
                     price = e.SellingPrice,
                     publisher = e.Seller_Id,
                     date = e.ModifiedDate,
                     no_dow = j2 == null ? 0 : 1
                 }).GroupBy(x => new {
                     a = x.id,
                     b = x.no_dow,
                     c = x.title,
                     d = x.cate,
                     e = x.size,
                     f = x.type,
                     h = x.price,
                     i = x.publisher,
                     k = x.date
                 })
                        .Select(g => new CountData
                        {
                            id = g.Key.a,
                            title = g.Key.c,
                            cate = g.Key.d,
                            size = g.Key.e,
                            type = g.Key.f,
                            price = (int)g.Key.h,
                            publisher = g.Key.i,
                            date = (DateTime?)g.Key.k ?? DateTime.Now,
                            no_dow = g.Sum(q => q.no_dow)
                        }).Where(x => x.title.Contains(searching) || list == null);
            }
            if(!String.IsNullOrEmpty(list))
            {
                result = (IOrderedQueryable<CountData>)(from e in _context.SellNotes
                          join d in _context.Downloads
                          on e.Id equals d.NoteId into j1
                          from j2 in j1.DefaultIfEmpty()
                          where e.Status == "Published"
                          select new
                          {
                              id = e.Id,
                              title = e.title,
                              cate = e.Category,
                              size = "10kb",
                              type = e.NoteType,
                              price = e.SellingPrice,
                              publisher = e.Seller_Id,
                              date = e.ModifiedDate,
                              no_dow = j2 == null ? 0 : 1
                          }).GroupBy(x => new
                          {
                              a = x.id,
                              b = x.no_dow,
                              c = x.title,
                              d = x.cate,
                              e = x.size,
                              f = x.type,
                              h = x.price,
                              i = x.publisher,
                              k = x.date
                          })
                     .Select(g => new CountData
                     {
                         id = g.Key.a,
                         title = g.Key.c,
                         cate = g.Key.d,
                         size = g.Key.e,
                         type = g.Key.f,
                         price = (int)g.Key.h,
                         publisher = g.Key.i,
                         date = (DateTime?)g.Key.k ?? DateTime.Now,
                         no_dow = g.Sum(q => q.no_dow)
                     }).OrderBy(x => x.id).Where(x => x.title.Contains(list) || list == null);
            }
            var notes = _context.SellNotes.ToList().Count();
            ViewBag.notes = notes;
            var download = _context.Downloads.ToList().Count();
            ViewBag.download = download;
            var mem = _context.Registers.ToList().Count();
            ViewBag.mem = mem;
            
            return View(result.ToList());
        }

        public ActionResult NoteUnderReview(string searching,string list)
        {
            var result = _context.SellNotes.Where(x=>x.Status!="Published").ToList();
            SelectList list1 = new SelectList(result, "Id", "title");
            ViewBag.list = list1;
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.SellNotes
                          select e).Where(x => x.title.Contains(searching) || searching == null).ToList();
            }
            if (!String.IsNullOrEmpty(list))
            {
                result = (from e in _context.SellNotes
                          select e).Where(x => x.title.Contains(list) || list == null).ToList();
            }
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
        public ActionResult UnapproveNote(string id)
        {
            
            var result = _context.SellNotes.Where(x => x.title == id).FirstOrDefault();
            if (result != null)
            {
                result.Status = "Unpublished";
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
        public ActionResult AdminPublishNote(string searching,string list)
        {
            var result = (from e in _context.SellNotes
                          join d in _context.Downloads
                          on e.Id equals d.NoteId into j1
                          from j2 in j1.DefaultIfEmpty()
                          where e.Status == "Published"
                          select new
                          {
                              id = e.Id,
                              title = e.title,
                              cate = e.Category,
                              size = "10kb",
                              type = e.NoteType,
                              price = e.SellingPrice,
                              publisher = e.Seller_Id,
                              date = e.ModifiedDate,
                              no_dow = j2 == null ? 0 : 1
                          }).GroupBy(x => new {
                              a = x.id,
                              b = x.no_dow,
                              c = x.title,
                              d = x.cate,
                              e = x.size,
                              f = x.type,
                              h = x.price,
                              i = x.publisher,
                              k = x.date
                          })
                     .Select(g => new CountData
                     {
                         id = g.Key.a,
                         title = g.Key.c,
                         cate = g.Key.d,
                         size = g.Key.e,
                         type = g.Key.f,
                         price = (int)g.Key.h,
                         publisher = g.Key.i,
                         date = (DateTime?)g.Key.k ?? DateTime.Now,
                         no_dow = g.Sum(q => q.no_dow)
                     }).OrderBy(x => x.id);

            SelectList list1 = new SelectList(result, "Id", "title");
            ViewBag.list = list1;
            if (!String.IsNullOrEmpty(searching))
            {
                result = (IOrderedQueryable<CountData>)(from e in _context.SellNotes
                                                        join d in _context.Downloads
                                                        on e.Id equals d.NoteId into j1
                                                        from j2 in j1.DefaultIfEmpty()
                                                        where e.Status == "Published"
                                                        select new
                                                        {
                                                            id = e.Id,
                                                            title = e.title,
                                                            cate = e.Category,
                                                            size = "10kb",
                                                            type = e.NoteType,
                                                            price = e.SellingPrice,
                                                            publisher = e.Seller_Id,
                                                            date = e.ModifiedDate,
                                                            no_dow = j2 == null ? 0 : 1
                                                        }).GroupBy(x => new {
                                                            a = x.id,
                                                            b = x.no_dow,
                                                            c = x.title,
                                                            d = x.cate,
                                                            e = x.size,
                                                            f = x.type,
                                                            h = x.price,
                                                            i = x.publisher,
                                                            k = x.date
                                                        })
                         .Select(g => new CountData
                         {
                             id = g.Key.a,
                             title = g.Key.c,
                             cate = g.Key.d,
                             size = g.Key.e,
                             type = g.Key.f,
                             price = (int)g.Key.h,
                             publisher = g.Key.i,
                             date = (DateTime?)g.Key.k ?? DateTime.Now,
                             no_dow = g.Sum(q => q.no_dow)
                         }).Where(x => x.title.Contains(searching) || list == null);
            }
            if (!String.IsNullOrEmpty(list))
            {
                result = (IOrderedQueryable<CountData>)(from e in _context.SellNotes
                                                        join d in _context.Downloads
                                                        on e.Id equals d.NoteId into j1
                                                        from j2 in j1.DefaultIfEmpty()
                                                        where e.Status == "Published"
                                                        select new
                                                        {
                                                            id = e.Id,
                                                            title = e.title,
                                                            cate = e.Category,
                                                            size = "10kb",
                                                            type = e.NoteType,
                                                            price = e.SellingPrice,
                                                            publisher = e.Seller_Id,
                                                            date = e.ModifiedDate,
                                                            no_dow = j2 == null ? 0 : 1
                                                        }).GroupBy(x => new
                                                        {
                                                            a = x.id,
                                                            b = x.no_dow,
                                                            c = x.title,
                                                            d = x.cate,
                                                            e = x.size,
                                                            f = x.type,
                                                            h = x.price,
                                                            i = x.publisher,
                                                            k = x.date
                                                        })
                     .Select(g => new CountData
                     {
                         id = g.Key.a,
                         title = g.Key.c,
                         cate = g.Key.d,
                         size = g.Key.e,
                         type = g.Key.f,
                         price = (int)g.Key.h,
                         publisher = g.Key.i,
                         date = (DateTime?)g.Key.k ?? DateTime.Now,
                         no_dow = g.Sum(q => q.no_dow)
                     }).OrderBy(x => x.id).Where(x => x.title.Contains(list) || list == null);
            }

            return View(result.ToList());
        }
        public ActionResult AdminDownloadNote(string searching, string list, string ls1, string ls2)
        {
            var result = _context.Downloads.ToList();
            SelectList list1 = new SelectList(result, "Id", "NoteTitle");
            SelectList list2 = new SelectList(result, "Id", "Seller");
            SelectList list3 = new SelectList(result, "Id", "BuyerEmailId");
            ViewBag.list1 = list1;
            ViewBag.list2 = list2;
            ViewBag.list3 = list3;
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.Downloads
                          select e).Where(x => x.NoteTitle.Contains(searching) || searching == null).ToList();
            }
            if (!String.IsNullOrEmpty(list))
            {
                result = (from e in _context.Downloads
                          select e).Where(x => x.NoteTitle.Equals(list) || list == null).ToList();
            }
            if (!String.IsNullOrEmpty(ls1))
            {
                result = (from e in _context.Downloads
                          select e).Where(x => x.Seller.Equals(ls1)|| ls1 == null).ToList();
            }
            if (!String.IsNullOrEmpty(ls2))
            {
                result = (from e in _context.Downloads
                          select e).Where(x => x.BuyerEmailId.Equals(ls2) || ls2 == null).ToList();
            }
            return View(result);
        }
        public ActionResult AdminRejectNote(string searching,string list)
        {
            var result = (from e in _context.SellNotes
                          join d in _context.RejectedNotes on e.Id equals d.Seller_Id
                          select new RejectNote { 
                                id= d.Id,
                                title = d.NoteTitle,
                                categoey = e.Category,
                                dateadd = (DateTime?)e.ModifiedDate?? DateTime.Now,
                                rejectedby = "Admin",
                                remark = d.Remark,
                                seller = e.Seller_Id
                          });
            SelectList list1 = new SelectList(result, "Id", "title");
            ViewBag.list = list1;
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.SellNotes
                          join d in _context.RejectedNotes on e.Id equals d.Seller_Id
                          select new RejectNote
                          {
                              id = d.Id,
                              title = d.NoteTitle,
                              categoey = e.Category,
                              dateadd = (DateTime?)e.ModifiedDate ?? DateTime.Now,
                              rejectedby = "Admin",
                              remark = d.Remark,
                              seller = e.Seller_Id
                          }).Where(x => x.title.Contains(searching) || searching == null);
            }
            if (!String.IsNullOrEmpty(list))
            {
                result = (from e in _context.SellNotes
                          join d in _context.RejectedNotes on e.Id equals d.Seller_Id
                          select new RejectNote
                          {
                              id = d.Id,
                              title = d.NoteTitle,
                              categoey = e.Category,
                              dateadd = (DateTime?)e.ModifiedDate ?? DateTime.Now,
                              rejectedby = "Admin",
                              remark = d.Remark,
                              seller = e.Seller_Id
                          }).Where(x => x.title.Contains(list) || list == null);
            }
            return View(result.ToList());
        }
        public ActionResult Members(string searching)
        {
            List<Members> memb = new List<Members>();
            var result = (from e in _context.Registers
                         join d in _context.UserProfiles on e.Id equals d.User_Id 
                          select new Members{ 
                            LastName = e.LastName,
                            FirstName = e.FirstName,
                            Id = e.Id,
                            Email = e.EmailId,
                            Joiningdate = (DateTime)d.Created_Date
                          });
            if (!String.IsNullOrEmpty(searching))
            {
                result = (from e in _context.Registers
                          join d in _context.UserProfiles on e.Id equals d.User_Id
                          select new Members
                          {
                              LastName = e.LastName,
                              FirstName = e.FirstName,
                              Id = e.Id,
                              Email = e.EmailId,
                              Joiningdate = (DateTime)d.Created_Date
                          }).Where(x => x.FirstName.Contains(searching) ||x.LastName.Contains(searching) || x.Email.Contains(searching)|| searching == null);
            }

            return View(result);
        }
        public ActionResult MemberDetails(int id)
        {
            var result = (from e in _context.Registers
                          join d in _context.UserProfiles on e.Id equals d.User_Id
                          where e.Id == id
                          select new Members
                          {
                              LastName = e.LastName,
                              FirstName = e.FirstName,
                              Id = e.Id,
                              address1 = d.Address1,
                              address2 = d.Address2,
                              city = d.City,
                              college = d.College,
                              country =d.Country,
                              dob = (DateTime)d.DOB,
                              Email = d.User_email,
                              Joiningdate = (DateTime)d.Created_Date,
                              phone = d.Phone_number,
                              state = d.State,
                              zipcode = d.State
                          });
            var result1 = _context.SellNotes.Where(x => x.Seller_Id == id);
            return View(Tuple.Create(result.ToList(), result1.ToList()));
        }
        public ActionResult About()
        {
            var listEmp = _context.Registers.ToList();

            return View(listEmp);
        }
        public ActionResult AddEditEmployee(int EmployeeId)
        {
            var model = _context.SellNotes.Where(x => x.Id == EmployeeId).FirstOrDefault();
            
            ViewBag.msg = model.title;
            return View("partialRejectView",model);
        }
        [HttpPost]
        public ActionResult RejectNote(int Id,string Remark)
        {
            RejectedNote rn = new RejectedNote();
            var res = _context.SellNotes.Where(x => x.Id == Id).FirstOrDefault();
            rn.NoteTitle = res.title;
            rn.Remark = Remark;
            rn.Seller_Id = res.Id;
            rn.RejectedBy = "admin";
            res.Status = "Reject";
            _context.Entry(res).State = System.Data.Entity.EntityState.Modified;
            _context.RejectedNotes.Add(rn);
            _context.SaveChanges();
            return RedirectToAction("NoteUnderReview");
        }
        public ActionResult unpublishenote(int title)
        {
            var model = _context.SellNotes.Where(x => x.Id == title).FirstOrDefault();
            _context.SellNotes.Remove(model);
            ViewBag.msg = model.title;
            return View("partialUnpublishedView", model);
        }
        [HttpPost]
        public ActionResult UnpublishNote(int Id, string Remark)
        {
            var res = _context.SellNotes.Where(x => x.Id == Id).FirstOrDefault();
            res.Status = "Reject";
            _context.Entry(res).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("AdminPublishNote");
        }
    }
}