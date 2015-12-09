using Bank.Models.Repositories;
using Bank.ViewModels;
using Common;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Web;
using System.Web.Mvc;

namespace Bank.Controllers
{
    public class BankController : Controller
    {
        BankRepository bankRepo = new BankRepository();
        // GET: Bank
        public ActionResult Index()
        {
            MoneyOrderVM model = new MoneyOrderVM();
            model.Requests = bankRepo.GetMoneyOrderRequests();
            model.CashRequests = bankRepo.GetCashRequests();
            
            return View(model);
        }

        public ActionResult SignMoneyOrder(int id)
        {
            bankRepo.SignMoneyOrder(id);
            return Json("Money Order Signed!!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CashMoneyOrder(int id)
        {
            if(bankRepo.VerifyMoneyOrder(id))
            {
                return Json("Money Order Cashed!!", JsonRequestBehavior.AllowGet);
            }
            return Json("Money Order Is not Valid!!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult StoreSignature()
        {
            AsymmetricCipherKeyPair keys = ServiceUtility.GenerateKeyPair();
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keys.Private);
            byte[] privateKey = privateKeyInfo.ToAsn1Object().GetDerEncoded();

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keys.Public);
            byte[] publicKey = publicKeyInfo.ToAsn1Object().GetDerEncoded();

            string serializedPublic = Convert.ToBase64String(publicKey);
            string serializedPrivate = Convert.ToBase64String(privateKey);

            bool added = bankRepo.StoreBankSecret(serializedPublic, serializedPrivate);
            if (!added)
            {
                return Json("Signature Not Stored", JsonRequestBehavior.AllowGet);
            }
            return Json("Signature Stored", JsonRequestBehavior.AllowGet);
        }


    }
}