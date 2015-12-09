using Common;
using CustomerClient.Models.Repositories;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerClient.Models
{
    public class CustomerRepository : BaseRepository
    {
        //private readonly byte[] MoneyOrderID;
        //private readonly RsaBlindingParameters blindingParams;


        public void CreateMoneyOrderRequest(decimal amount)
        {
            byte[] MoneyOrderID = ServiceUtility.GetRandomBytes(16);

            var serializedKey = GetPublicKey();
            RsaKeyParameters publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(serializedKey);

            RsaBlindingFactorGenerator blindingFactorGenerator = new RsaBlindingFactorGenerator();
            blindingFactorGenerator.Init(publicKey);

            BigInteger blindingFactor = blindingFactorGenerator.GenerateBlindingFactor();

            RsaBlindingParameters blindingParams = new RsaBlindingParameters(publicKey, blindingFactor);

            // Blind Signing
            PssSigner signer = new PssSigner(new RsaBlindingEngine(), new Sha1Digest(), 20);
            signer.Init(true, blindingParams);

            signer.BlockUpdate(MoneyOrderID, 0, MoneyOrderID.Length);

            byte[] blindSignature = signer.GenerateSignature();

            CustomerClient.Models.Entities.MoneyOrder mo = new Entities.MoneyOrder();
            mo.Amount = amount;
            mo.BlindSignature = Convert.ToBase64String(blindSignature);
            mo.UniqueId = Convert.ToBase64String(MoneyOrderID);

            db.MoneyOrders.Add(mo);
            db.SaveChanges();
            
        }

        public bool RequestCash(int id)
        {
            var result = (from mo in db.MoneyOrders.Where(m => m.Id == id)
                         select mo).SingleOrDefault();
            result.CashRequest = true;
            db.SaveChanges();
            return true;
        }

        public List<Entities.MoneyOrder> GetMoneyOrderRequests()
        {
            var result = from mo in db.MoneyOrders.Where(m => !m.Signed)
                         select mo;
            
            return result.ToList();
            
        }

        public List<Entities.MoneyOrder> GetMoneyOrders()
        {

            var result = from mo in db.MoneyOrders.Where(m => m.Signed)
                         select mo;


            return result.ToList();
        }

        public MoneyOrderRequest GetMoneyOrderByID(int id)
        {
            var result = (from mo in db.MoneyOrders.Where(m => m.Id == id)
                         select mo).SingleOrDefault();
                        
            var moReq = new MoneyOrderRequest();
            moReq.Amount = result.Amount;
            moReq.BlindSignatureBase64 = result.BlindSignature;
            moReq.IsRequested = !result.Signed;
            moReq.UniqueIDBase64 = result.UniqueId;

            return moReq;
        }

        public void UpdateRequested()
        { }

        public byte[] GetPublicKey()
        {
            string serializedKey = (from key in db.Signatures.Where(k => k.Id == 0)
                                    select key.Public).SingleOrDefault();
            byte[] result = Convert.FromBase64String(serializedKey);
            return result;
        }
    }
}