using Bank.Models.Entities;
using Common;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank.Models.Repositories
{
    public class BankRepository : BaseRepository
    {
        public bool StoreBankSecret(string publicKey, string privateKey)
        {
            Signature sig =
                new Signature
                {
                    Public = publicKey,
                    Private = privateKey
                };
            try
            {
                db.Signatures.Add(sig);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            
            return true;
        }

        public byte[] GetPublicKey()
        {
            string serializedKey = (from sig in db.Signatures
                            where sig.Id == 0
                            select sig.Public).SingleOrDefault();
            
            byte[] publicKey = Convert.FromBase64String(serializedKey);
            return publicKey;
        }

        public byte[] GetPrivateKey()
        {
            string serializedKey = (from sig in db.Signatures
                                    where sig.Id == 0
                                    select sig.Private).SingleOrDefault();
            byte[] privateKey = Convert.FromBase64String(serializedKey);
            return privateKey;
        }

        
        public void SignMoneyOrder(int id)
        {
            var mo = (from m in db.MoneyOrders.Where(x => x.Id == id)
                            select m).SingleOrDefault();

            byte[] blindSignature = Convert.FromBase64String(mo.BlindSignature);
            RsaPrivateCrtKeyParameters privateKey = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(GetPrivateKey());

            // engine initialized with private key
            RsaEngine engine = new RsaEngine();
            engine.Init(true, privateKey);

            // the bank signs it
            byte[] bankSignature = engine.ProcessBlock(blindSignature, 0, blindSignature.Length);

            mo.Signature = Convert.ToBase64String(bankSignature);
            mo.Signed = true;

            db.SaveChanges();           

        }

        public List<Models.Entities.MoneyOrder> GetMoneyOrderRequests()
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

        public List<Entities.MoneyOrder> GetCashRequests()
        {

            var result = from mo in db.MoneyOrders.Where(m => m.CashRequest)
                         select mo;

            return result.ToList();
        }

        public MoneyOrderRequest GetMoneyOrderRequests(string uniqueId)
        {
            var moRequest = (from mo in db.MoneyOrders.Where(m => m.UniqueId == uniqueId)
                             select mo).SingleOrDefault();
            var result = new MoneyOrderRequest(Convert.FromBase64String(moRequest.UniqueId), Convert.FromBase64String(moRequest.BlindSignature), moRequest.Amount);

            return result;
        }

        /// <summary>
        /// Verifies the signature of the 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerifyMoneyOrder(int id)
        {
            var mo = (from m in db.MoneyOrders.Where(x => x.Id == id)
                      select m).SingleOrDefault();

            if (!mo.Cashed)
            {
                byte[] uniqueId = Convert.FromBase64String(mo.UniqueId);
                byte[] signature = Convert.FromBase64String(mo.Signature);

                PssSigner signer = new PssSigner(new RsaEngine(), new Sha1Digest(), 20);

                RsaKeyParameters publicKey = (RsaKeyParameters)PublicKeyFactory.CreateKey(GetPublicKey());

                signer.Init(false, publicKey);

                signer.BlockUpdate(uniqueId, 0, uniqueId.Length);
                if (signer.VerifySignature(signature))
                {
                    return true;
                }
                
                return false;
            }
            else
            {
                return false;
            }
        }


    }
}