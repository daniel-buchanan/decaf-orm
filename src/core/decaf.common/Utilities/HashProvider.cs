using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace decaf.common.Utilities
{
    public interface IHashProvider
    {
        /// <summary>
        /// Get a hashed value for the provided input.
        /// </summary>
        /// <typeparam name="T">The type of the object to hash.</typeparam>
        /// <param name="input">The object to be hashed.</param>
        /// <returns>The Hash as a Bas64 encdoded string.</returns>
        string GetHash<T>(T input);

        /// <summary>
        /// Get a hashed value for the provided input and appendix.
        /// </summary>
        /// <typeparam name="T">The type of the object to hash.</typeparam>
        /// <param name="input">The object to be hashed.</param>
        /// <param name="appendix">The object to use as an appendix.</param>
        /// <returns></returns>
        string GetHash<T>(T input, object appendix);
    }

    public class HashProvider : IHashProvider
	{
        private readonly SHA256 hashAlgorithm;

		public HashProvider() => hashAlgorithm = SHA256.Create();

        public static IHashProvider Create() => new HashProvider();

        /// <inheritdoc/>
        public string GetHash<T>(T input)
        {
            var json = JsonConvert.SerializeObject(input);
            var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(json));

            return Convert.ToBase64String(hash);
        }

        /// <inheritdoc/>
        public string GetHash<T>(T input, object appendix)
        {
            var json = JsonConvert.SerializeObject(input);
            var hashBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(json));
            json = JsonConvert.SerializeObject(appendix);
            var appendixHashBytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(json));

            var hash = Convert.ToBase64String(hashBytes);
            var appendixHash = Convert.ToBase64String(appendixHashBytes);
            return $"{hash}:{appendixHash}";
        }
    }
}

