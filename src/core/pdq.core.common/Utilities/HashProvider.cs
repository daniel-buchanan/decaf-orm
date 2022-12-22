using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace pdq.common.Utilities
{
    public interface IHashProvider
    {
        string GetHash<T>(T input);
    }

    public class HashProvider : IHashProvider
	{
        private readonly SHA256 hashAlgorithm;

		public HashProvider() => this.hashAlgorithm = SHA256.Create();

        /// <inheritdoc/>
        public string GetHash<T>(T input)
        {
            var json = JsonConvert.SerializeObject(input);
            var hash = this.hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(json));

            return Convert.ToBase64String(hash);
        }
    }
}

