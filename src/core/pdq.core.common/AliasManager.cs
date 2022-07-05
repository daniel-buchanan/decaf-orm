using System;
using System.Linq;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.core-tests")]
namespace pdq.common
{
    internal class AliasManager : IAliasManager, IDisposable
	{
		private readonly List<ManagedAlias> knownAliases;
        private readonly SortedDictionary<string, int> aliasCounts;

		private AliasManager()
		{
            this.aliasCounts = new SortedDictionary<string, int>()
            {
                { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 }
            };
			this.knownAliases = new List<ManagedAlias>();
		}

        public static IAliasManager Create() => new AliasManager();

        /// <inheritdoc />
        public string Add(string alias, string assocWith)
        {
            if (string.IsNullOrWhiteSpace(assocWith))
                throw new ArgumentNullException(nameof(assocWith), "The assocWith parameter must be provided to either generate or add an alias.");

            if(string.IsNullOrWhiteSpace(alias))
            {
                alias = GenerateAlias(assocWith);
            }

            alias = alias.ToLower();
            var existing = this.knownAliases.FirstOrDefault(a => a.Name == alias);
            if (existing != null)
            {
                if (string.IsNullOrWhiteSpace(existing.Relation))
                    existing.Relation = assocWith;
                return alias;
            }

            this.knownAliases.Add(ManagedAlias.Create(alias, assocWith));
            return alias;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<ManagedAlias> All()
            => this.knownAliases.AsReadOnly();

        /// <inheritdoc />
        public void Dispose()
        {
            this.aliasCounts.Clear();
            this.knownAliases.Clear();
        }

        /// <inheritdoc />
        public IEnumerable<ManagedAlias> FindByAssociation(string assocWith)
            => this.knownAliases.Where(a => a.Relation == assocWith).ToList();

        /// <inheritdoc />
        public string GetAssociation(string alias)
        {
            var existing = this.knownAliases.FirstOrDefault(a => a.Name == alias);
            return existing?.Relation;
        }

        private string GenerateAlias(string assocWith)
        {
            var prefix = assocWith?.Substring(0, 1)?.ToLower() ?? "a";

            var aliasCount = this.aliasCounts[prefix];
            var alias = $"{prefix}{aliasCount}";
            this.aliasCounts[prefix] += 1;
            return alias;
        }
    }
}

