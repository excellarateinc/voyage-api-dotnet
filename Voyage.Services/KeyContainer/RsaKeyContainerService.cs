using System.Security.Cryptography;

namespace Voyage.Services.KeyContainer
{
    public class RsaKeyContainerService : IRsaKeyContainerService
    {
        /// <summary>
        /// Create Rsa provider object using a given key from key container. If key does not exist in key container new one will be created and use to create Rsa provider object.
        /// </summary>
        /// <returns></returns>
        public RSACryptoServiceProvider GetRsaCryptoServiceProviderFromKeyContainer(string keyName = "VoyageKey")
        {
            var publicPrivateKey = GetPrivateAndPublicKey(keyName);
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(publicPrivateKey);

            return rsaCryptoServiceProvider;
        }

        /// <summary>
        /// Delete key from key container
        /// </summary>
        /// <param name="keyName"></param>
        public void DeleteKeyFromContainer(string keyName = "VoyageKey")
        {
            var cp = new CspParameters { KeyContainerName = keyName };
            var rsa = new RSACryptoServiceProvider(cp) { PersistKeyInCsp = false };

            rsa.Clear();
        }

        /// <summary>
        /// Get public and private key as string
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private string GetPrivateAndPublicKey(string keyName)
        {
            var cp = new CspParameters { KeyContainerName = keyName };
            var rsa = new RSACryptoServiceProvider(2048, cp) { PersistKeyInCsp = true };

            var publicPrivateKey = rsa.ToXmlString(true);
            return publicPrivateKey;
        }
    }
}
