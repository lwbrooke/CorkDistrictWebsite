using CorkDistrict.DAL;
using CorkDistrict.Models;
using CorkDistrict.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CorkDistrict.Controllers
{
    [Authorize(Roles = "Winery, Admin")]//Restricts Access to only users that sign in as winery/admin. 
    public class RedemptionController : Controller
    {
        
        private CorkDistrictContext db = new CorkDistrictContext();

        // GET: /Redemption/
        public ActionResult Index(int? page)
        {
            var vm = new List<RedemptionIndexViewModel>();
            var redemptions = db.Redemptions.Include("Card");            
            var AccountDb = new ApplicationDbContext();

            if (User.IsInRole("Admin"))
            {
                foreach(var r in redemptions)
                {
                    var rvm = new RedemptionIndexViewModel
                    {
                        CardID = r.CardID,
                        owner = (r.Card.Activation.UserID.StartsWith("promo ") ? r.Card.Activation.UserID : AccountDb.Users.Find(r.Card.Activation.UserID).Email),
                        TimeStamp = r.TimeStamp,
                        WineryID = AccountDb.Users.Find(r.WineryID).Name,
                        uses = r.Card.Uses
                    };

                    vm.Add(rvm);
                }
                AccountDb.Dispose();
                //return View(vm);
            }
            else if (User.IsInRole("Winery"))
            {
                foreach (var r in redemptions)
                {
                    if (User.Identity.GetUserId().Equals(r.WineryID))
                    {
                        vm.Add(new RedemptionIndexViewModel
                        {
                            CardID = r.CardID,
                            owner = (r.Card.Activation.UserID.StartsWith("promo ") ? r.Card.Activation.UserID : AccountDb.Users.Find(r.Card.Activation.UserID).Email),
                            TimeStamp = r.TimeStamp,
                            WineryID = User.Identity.Name,
                            uses = r.Card.Uses
                        });
                    }
                }
                AccountDb.Dispose();
                //return View(vm);
            }
            else
            {
                AccountDb.Dispose();
                return RedirectToAction("Index", "Home");
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(vm.AsQueryable().ToPagedList(pageNumber, pageSize));
        }

        // GET: /Redemption/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Redemption/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CardID")] RedeemViewModel model)
        {

            if (ModelState.IsValid)
            {
                bool isActive = db.Activations.Find(Convert.ToInt32(model.CardID)) != null;
                if (isActive)
                {
                    var redemption = new Redemption();
                    redemption.Card = db.Cards.Find(Convert.ToInt32(model.CardID));
                    if (redemption.Card.Uses > 0)
                    {
                        redemption.TimeStamp = DateTime.Now;
                        redemption.WineryID = User.Identity.GetUserId();
                        redemption.Card.Uses--;
                        db.Entry(redemption.Card).State = EntityState.Modified;
                        db.Redemptions.Add(redemption);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Rmessage"] = "This card has been fully redeemed";
                        return View(model);
                    }
                }
                TempData["Rmessage"] = "This card has not been activated";
                return View(model);
            }
            TempData["Rmessage"] = "The supplied information is invalid";
            return View(model);
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
