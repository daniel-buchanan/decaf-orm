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
            byte[] hash;
            var json = JsonConvert.SerializeObject(input);
            using(var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(json);
                memoryStream.Seek(0, SeekOrigin.Begin);
                hash = this.hashAlgorithm.ComputeHash(memoryStream);
            }

            return Convert.ToBase64String(hash);
        }
    }
}

