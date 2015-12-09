using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public static class ServiceUtility
    {
        public static byte[] GetRandomBytes(int count)
        {
            byte[] bytes = new byte[count];
            new SecureRandom().NextBytes(bytes);
            return bytes;
        }

        public static AsymmetricCipherKeyPair GenerateKeyPair()
        {
            
            RsaKeyPairGenerator generator = new RsaKeyPairGenerator();

            SecureRandom random = new SecureRandom();
            BigInteger bigNumber = new BigInteger("10001", 16);
            RsaKeyGenerationParameters parameters = new RsaKeyGenerationParameters(bigNumber, random, 2048, 80);

            generator.Init(parameters);

            return generator.GenerateKeyPair();
        }

    }
}