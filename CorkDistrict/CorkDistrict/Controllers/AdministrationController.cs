using CorkDistrict.DAL;
using CorkDistrict.Models;
using CorkDistrict.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CorkDistrict.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        //
        // GET: /Administration/CreateCards
        public ActionResult CreateCards()
        {
            return View();
        }

        //
        // GET: /Administration/CreateCards
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCards([Bind(Include = "Amount,IsPromo,PromoOwner")] CreateCardViewModel model)
        {
            if(ModelState.IsValid)
            {
                CorkDistrictContext db = new CorkDistrictContext();
                var lastID = db.Cards.Max(card => card.CardID);
                for(int i = 1; i <= model.Amount; i++)
                {
                    var card = new Card() { CardID = lastID + i, Created = DateTime.Now, isPromo = model.IsPromo, Uses = 3 };
                    db.Cards.Add(card);
                    if(card.isPromo)
                    {
                        var activation = new Activation() { CardID = card.CardID, TimeStamp = DateTime.Now, UserID = model.PromoOwner };
                        db.Activations.Add(activation);
                    }
                }
                db.SaveChanges();
                db.Dispose();
                TempData["CrTCmessage"] = "Cards " + (lastID + 1) + " through " + (lastID + model.Amount) + " added.";
                return RedirectToAction("Menu");
            }
            TempData["CrTCmessage"] = "The information given is invalid";
            return View(model);
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult AdminCardStats(string currentFilter, string searchString, int? page)
        {
            var ApplicationDb = new ApplicationDbContext();
            var CorkDb = new CorkDistrictContext();
            var vm = new List<AdminCardStatsViewModel>();
            var cards = from c in CorkDb.Cards.Include("Activation").Include("Redemption") select c;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            int CardID;

            try 
	        {
                CardID = int.Parse(searchString);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is FormatException)
                {
                    CardID = -1;
                }
                else 
                { 
                    throw;
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                cards = cards.Where(c => c.Activation.UserID.ToLower().Contains(searchString.ToLower()) || c.CardID == CardID);
            }

            foreach (var card in cards)
            {
                if (card.Activation != null)
                {
                    var cardStat = new AdminCardStatsViewModel
                    {
                        CardID = card.CardID,
                        remainingUses = card.Uses,
                        owner = (card.isPromo ? card.Activation.UserID : ApplicationDb.Users.Find(card.Activation.UserID).Email),
                        purchasedAt = (card.isPromo ? "Promotional Card" : // is promo?
                                (card.CashPurchase != null ? ApplicationDb.Users.Find(card.CashPurchase.WineryID).UserName :  // is cash purchase?
                                (card.Purchase.Location.Equals("Online") ? card.Purchase.Location : // is online?
                                ApplicationDb.Users.Find(card.Purchase.Location).UserName))), // if here, then is in-winery cc purchase
                        vistedWineries = new string[card.Redemption.Count]
                    };

                    int i = 0;
                    foreach (var redemption in card.Redemption)
                    {
                        cardStat.vistedWineries[i] = ApplicationDb.Users.Find(redemption.WineryID).UserName;
                        i++;
                    }

                    vm.Add(cardStat);
                }
            }

            ApplicationDb.Dispose();
            CorkDb.Dispose();

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(vm.AsQueryable().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AdminPromoStats(string currentFilter, string searchString, int? page)
        {
            var ApplicationDb = new ApplicationDbContext();
            var CorkDb = new CorkDistrictContext();
            var vm = new List<AdminPromoStatsViewModel>();
            var cards = CorkDb.Cards.Include("Activation").Include("Redemption").Where(c => c.isPromo);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                cards = cards.Where(c => c.Activation.UserID.ToLower().Contains(searchString.ToLower()));
            }

            foreach (var card in cards)
            {
                var promoStat = new AdminPromoStatsViewModel 
                { 
                    CardID = card.CardID,
                    remainingUses = card.Uses,
                    vendorIssuer = card.Activation.UserID,
                    locationsUsed = new string[card.Redemption.Count]
                };

                int i = 0;
                foreach(var redemption in card.Redemption)
                {
                    promoStat.locationsUsed[i] = ApplicationDb.Users.Find(redemption.WineryID).UserName;
                    i++;
                }

                vm.Add(promoStat);
            }

            ApplicationDb.Dispose();
            CorkDb.Dispose();
            
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(vm.AsQueryable().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AdminWineryStats(string currentFilter, string searchString, int? page)
        {
            var ApplicationDb = new ApplicationDbContext();
            var CorkDb = new CorkDistrictContext();
            var vm = new List<AdminWineryStatsViewModel>();
            var redemptions = CorkDb.Redemptions.Include("Card");
            var users = from u in ApplicationDb.Users select u;
            
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(searchString.ToLower()) || u.Name.ToLower().Contains(searchString.ToLower()) || u.Email.ToLower().Contains(searchString.ToLower()));
            }
            
            foreach (var user in users)
            {
                if(user.Roles.Count(u => u.Role.Name == "Winery") > 0)
                {
                    vm.Add(new AdminWineryStatsViewModel
                    { 
                        siteID = user.UserName,
                        totalRedeemed = redemptions.Count(r => r.WineryID == user.Id),
                        soldCards = CorkDb.Purchases.Count(p => p.Location == user.Id) + CorkDb.CashPurchases.Count(cp => cp.WineryID == user.Id),
                        promosRedeemed = redemptions.Count(r => r.WineryID == user.Id && r.Card.isPromo)
                    });
                }
            }

            ApplicationDb.Dispose();
            CorkDb.Dispose();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(vm.AsQueryable().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CardReport (string searchMonth, string searchYear)
        {
            var CorkDb = new CorkDistrictContext();
            var cards = from c in CorkDb.Cards.Include("Activation") select c;

            int month, year;
            var succeeded = int.TryParse(searchMonth, out month);            
            if(succeeded)
            {
                cards = cards.Where(c => c.Activation.TimeStamp.Month == month);
            }
            succeeded = int.TryParse(searchYear, out year);
            if(!String.IsNullOrEmpty(searchYear))
            {
                cards = cards.Where(c => c.Activation.TimeStamp.Year == year);
            }

            var vm = new AggregateCardStatsViewModel 
            {
                Count = CorkDb.Cards.Count(),
                ActiveCount = CorkDb.Activations.Count(),
                NewActives = cards.Count(c => c.Activation != null),
                UsesRemaining = cards.Count(c => c.Activation != null && c.Uses > 0)
            };
            
            return View(vm);
        }
	}
}