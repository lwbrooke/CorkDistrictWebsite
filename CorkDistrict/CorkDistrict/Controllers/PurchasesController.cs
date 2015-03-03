using CorkDistrict.DAL;
using CorkDistrict.Models;
using CorkDistrict.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CorkDistrict.Controllers
{
    [Authorize(Roles = "User, Winery, Admin")]
    public class PurchasesController : Controller
    {
        private CorkDistrictContext db = new CorkDistrictContext();

        // GET: /Purchases/
        public ActionResult Index(int? page)
        {
            var vm = new List<PurchaseIndexViewModel>();
            var cashP = db.CashPurchases.Include("Card");
            var ccP = db.Purchases.Include("Card");
            var AccountDb = new ApplicationDbContext();
            
            if (User.IsInRole("Admin"))
            {
                foreach (var cash in cashP)
                {
                    vm.Add(new PurchaseIndexViewModel
                        {
                            CardID = cash.CardID,
                            method = "Cash",
                            purchaseLocation = AccountDb.Users.Find(cash.WineryID).Name,
                            timestamp = cash.TimeStamp,
                            uses = cash.Card.Uses,
                            owner = AccountDb.Users.Find(cash.Card.Activation.UserID).Email
                        });
                }
                foreach (var cc in ccP)
                {
                    vm.Add(new PurchaseIndexViewModel
                        {
                            CardID = cc.CardID,
                            method = cc.CreditCardNum,
                            purchaseLocation = (cc.Location.Equals("Online") ? cc.Location : AccountDb.Users.Find(cc.Location).Name),
                            timestamp = cc.TimeStamp,
                            uses = cc.Card.Uses,
                            owner = AccountDb.Users.Find(cc.Card.Activation.UserID).Email
                        });
                }
                AccountDb.Dispose();
                //return View(vm);
            }
            else if (User.IsInRole("Winery"))
            {
                foreach (var cash in cashP)
                {
                    if (User.Identity.GetUserId().Equals(cash.WineryID))
                    {
                        vm.Add(new PurchaseIndexViewModel
                        {
                            CardID = cash.CardID,
                            method = "Cash",
                            purchaseLocation = User.Identity.Name,
                            timestamp = cash.TimeStamp,
                            uses = cash.Card.Uses,
                            owner = AccountDb.Users.Find(cash.Card.Activation.UserID).Email
                        });
                    }
                }
                foreach (var cc in ccP)
                {
                    if (User.Identity.GetUserId().Equals(cc.Location))
                    {
                        vm.Add(new PurchaseIndexViewModel
                        {
                            CardID = cc.CardID,
                            method = cc.CreditCardNum,
                            purchaseLocation = User.Identity.Name,
                            timestamp = cc.TimeStamp,
                            uses = cc.Card.Uses,
                            owner = AccountDb.Users.Find(cc.Card.Activation.UserID).Email
                        });
                    }
                }
                AccountDb.Dispose();
                //return View(vm);
            }
            else if (User.IsInRole("User"))
            {
                foreach (var cash in cashP)
                {
                    if (User.Identity.GetUserId().Equals(cash.Card.Activation.UserID))
                    {
                        vm.Add(new PurchaseIndexViewModel
                        {
                            CardID = cash.CardID,
                            method = "Cash",
                            purchaseLocation = AccountDb.Users.Find(cash.WineryID).Name,
                            timestamp = cash.TimeStamp,
                            uses = cash.Card.Uses,
                            owner = AccountDb.Users.Find(cash.Card.Activation.UserID).Email
                        });
                    }
                }
                foreach (var cc in ccP)
                {
                    if (User.Identity.GetUserId().Equals(cc.Card.Activation.UserID))
                    {
                        vm.Add(new PurchaseIndexViewModel
                        {
                            CardID = cc.CardID,
                            method = cc.CreditCardNum,
                            purchaseLocation = (cc.Location.Equals("Online") ? cc.Location : AccountDb.Users.Find(cc.Location).Name),
                            timestamp = cc.TimeStamp,
                            uses = cc.Card.Uses,
                            owner = AccountDb.Users.Find(cc.Card.Activation.UserID).Email
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

        // GET: /Purchases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CardID,FirstName,MiddleName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,Country,CreditCardNum,ExpirationMonth,ExpirationYear,CVC")] HomeCCViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Cards.Find(Convert.ToInt32(model.CardID)) != null)
                {
                    if (db.Activations.Find(Convert.ToInt32(model.CardID)) == null) 
                    {
                        try
                        {
                            var myToken = new StripeTokenCreateOptions();

                            myToken.CardAddressCountry = model.Country;
                            myToken.CardAddressLine1 = model.AddressLine1;
                            myToken.CardAddressLine2 = model.AddressLine2;
                            myToken.CardAddressCity = model.City;
                            myToken.CardAddressState = model.State;
                            myToken.CardAddressZip = model.ZipCode.ToString();
                            myToken.CardCvc = model.CVC.ToString();
                            myToken.CardExpirationMonth = model.ExpirationMonth.ToString();
                            myToken.CardExpirationYear = model.ExpirationYear.ToString();
                            myToken.CardName = model.FirstName + " " + model.MiddleName + " " + model.LastName;
                            myToken.CardNumber = model.CreditCardNum;

                            var tokenService = new StripeTokenService();
                            StripeToken stripeToken = tokenService.Create(myToken);

                            var myCharge = new StripeChargeCreateOptions();

                            myCharge.Amount = 1500;
                            myCharge.Currency = "usd";
                            myCharge.Description = "Card CardID: " + model.CardID;

                            myCharge.TokenId = stripeToken.Id;

                            var chargeService = new StripeChargeService();
                            StripeCharge stripeCharge = chargeService.Create(myCharge);
                        }
                        catch (StripeException e) 
                        {
                            TempData["PCHmessage"] = e.Message;
                            return View(model);
                        }

                        model.CreditCardNum = "Card ending with " + model.CreditCardNum.Substring(model.CreditCardNum.Length - 4, 4);
                        Activation activation = new Activation { CardID = Convert.ToInt32(model.CardID), TimeStamp = DateTime.Now, UserID = User.Identity.GetUserId() };
                        Purchase purchase = new Purchase { CardID = Convert.ToInt32(model.CardID), TimeStamp = DateTime.Now, CreditCardNum = model.CreditCardNum, Location = "Online" };
                        db.Activations.Add(activation);
                        db.Purchases.Add(purchase);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["PCHmessage"] = "This tasting card has already been activated"; 
                        return View(model);
                    }
                }
                TempData["PCHmessage"] = "This tasting card number is invalid"; 
                return View(model);
            }
            TempData["PCHmessage"] = "The information given is invalid";
            return View(model);
        }

        [Authorize(Roles = "Winery, Admin")]
        public ActionResult TypeChoice()
        {
            return View();
        }

        // GET: /Purchases/CreateCash
        [Authorize(Roles = "Winery, Admin")]
        public ActionResult CreateCash()
        {
            return View();
        }

        // POST: /Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Winery, Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCash([Bind(Include = "CardID, Email")] CashViewModel cashModel)
        {
            if(ModelState.IsValid) // if model is valid
            {
                if (db.Cards.Find(Convert.ToInt32(cashModel.CardID)) != null) // if card exists
                {
                    if (db.Activations.Find(Convert.ToInt32(cashModel.CardID)) == null) // if card is inactive
                    {
                        CashPurchase cashPurchase = new CashPurchase { CardID = Convert.ToInt32(cashModel.CardID), TimeStamp = DateTime.Now, WineryID = User.Identity.GetUserId() };

                        var user = cashModel.GetUser(); // get who the card will belong to, if there is no account, create one
                        if(user.UserName.EndsWith("" + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day))
                        {
                            TempData["NewUser"] = "This user has been assigned the account: " + user.UserName + " | password is defaulted to: password";
                        }
                        var userId = user.Id;

                        Activation activation = new Activation { CardID = Convert.ToInt32(cashModel.CardID), TimeStamp = DateTime.Now, UserID = userId };
                        db.Activations.Add(activation);
                        db.CashPurchases.Add(cashPurchase);
                        db.SaveChanges();
                        return RedirectToAction("TypeChoice");
                    }
                    else
                    {
                        TempData["PCaWmessage"] = "This tasting card is already activated";
                        return View(cashModel);
                    }
                }
                TempData["PCaWmessage"] = "This tasting card number is invalid"; 
                return View(cashModel);
            }
            TempData["PCaWmessage"] = "The information given is invalid";
            return View(cashModel);
        }

        // GET: /Purchases/Create
        [Authorize(Roles = "Winery, Admin")]
        public ActionResult CreateCC()
        {
            return View();
        }

        // POST: /Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Winery, Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCC([Bind(Include = "CardID,Email,FirstName,MiddleName,LastName,AddressLine1,AddressLine2,City,State,ZipCode,Country,CreditCardNum,ExpirationMonth,ExpirationYear,CVC")] CCViewModel ccModel)
        {
            if (ModelState.IsValid)
            {
                if (db.Cards.Find(Convert.ToInt32(ccModel.CardID)) != null)
                {
                    if (db.Activations.Find(Convert.ToInt32(ccModel.CardID)) == null)
                    {
                        try
                        {
                            var myToken = new StripeTokenCreateOptions();

                            myToken.CardAddressCountry = ccModel.Country;
                            myToken.CardAddressLine1 = ccModel.AddressLine1;
                            myToken.CardAddressLine2 = ccModel.AddressLine2;
                            myToken.CardAddressCity = ccModel.City;
                            myToken.CardAddressState = ccModel.State;
                            myToken.CardAddressZip = ccModel.ZipCode;
                            myToken.CardCvc = ccModel.CVC;
                            myToken.CardExpirationMonth = ccModel.ExpirationMonth;
                            myToken.CardExpirationYear = ccModel.ExpirationYear;
                            myToken.CardName = ccModel.FirstName + " " + ccModel.MiddleName + " " + ccModel.LastName;
                            myToken.CardNumber = ccModel.CreditCardNum;

                            var tokenService = new StripeTokenService();
                            StripeToken stripeToken = tokenService.Create(myToken);

                            var myCharge = new StripeChargeCreateOptions();

                            myCharge.Amount = 1500;
                            myCharge.Currency = "usd";
                            myCharge.Description = "Card CardID: " + ccModel.CardID;

                            myCharge.TokenId = stripeToken.Id;

                            var chargeService = new StripeChargeService();
                            StripeCharge stripeCharge = chargeService.Create(myCharge);
                        }
                        catch (StripeException e)
                        {
                            TempData["PCWmessage"] = e.Message;
                            return View(ccModel);
                        }

                        ccModel.CreditCardNum = "Card ending with " + ccModel.CreditCardNum.Substring(ccModel.CreditCardNum.Length - 4, 4);
                        Purchase purchase = new Purchase { CardID = Convert.ToInt32(ccModel.CardID), CreditCardNum = ccModel.CreditCardNum, Location = User.Identity.GetUserId(), TimeStamp = DateTime.Now };

                        var user = ccModel.GetUser(); // get who the card will belong to, if there is no account, create one
                        if (user.UserName.EndsWith(String.Concat(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)))
                        {
                            TempData["NewUser"] = "This user has been assigned the account: " + user.UserName + " | password is defaulted to: password";
                        }
                        var userId = user.Id;

                        Activation activation = new Activation { CardID = Convert.ToInt32(ccModel.CardID), TimeStamp = DateTime.Now, UserID = userId };
                        db.Activations.Add(activation);
                        db.Purchases.Add(purchase);
                        db.SaveChanges();
                        return RedirectToAction("TypeChoice");
                    }
                    else
                    {
                        TempData["PCWmessage"] = "This tasting card has already been activated";
                        return View(ccModel);
                    }
                }
                TempData["PCWmessage"] = "This tasting card number is invalid";
                return View(ccModel);
            }
            TempData["PCWmessage"] = "The information given is invalid";
            return View(ccModel);
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
