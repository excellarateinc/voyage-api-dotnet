using System.Security.Cryptography;

namespace Voyage.Services.KeyContainer
{
    public interface IRsaKeyContainerService
    {
        /// <summary>
        /// Create Rsa provider object using a given key from key container. If key does not exist in key container new one will be created and use to create Rsa provider object.
        /// </summary>
        /// <returns></returns>
        RSACryptoServiceProvider GetRsaCryptoServiceProviderFromKeyContainer(string keyName = "VoyageKey");

        /// <summary>
        /// Delete key from key container
        /// </summary>
        /// <param name="keyName"></param>
        void DeleteKeyFromContainer(string keyName = "VoyageKey");
    }
}
