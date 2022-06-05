using System;
using System.Linq;
using System.Collections.Generic;

namespace pdq.common
{
	public interface IAliasManager
    {
		string Add(string alias, string name);
		IReadOnlyCollection<string> All();
        string FindByAssociation(string name);
        string GetAssociation(string alias);
    }

	public class AliasManager : IAliasManager
	{
		private readonly List<Alias> knownAliases;
        private readonly Dictionary<string, int> aliasCounts;

		public AliasManager()
		{
            this.aliasCounts = new Dictionary<string, int>()
            {
                { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 }
            };
			this.knownAliases = new List<Alias>();
		}

        public string Add(string alias, string name)
        {
            if(string.IsNullOrWhiteSpace(alias))
            {
                alias = GenerateAlias();
            }

            alias = alias.ToLower();
            var existing = this.knownAliases.FirstOrDefault(a => a.Name == alias);
            if (existing != null)
            {
                if (string.IsNullOrWhiteSpace(existing.AssociatedWith))
                    existing.AssociatedWith = name;
                return alias;
            }

            this.knownAliases.Add(Alias.Create(alias, name));
            return alias;
        }

        public IReadOnlyCollection<string> All() => this.knownAliases.Select(a => a.Name).ToList().AsReadOnly();

        public string FindByAssociation(string name)
        {
            var found = this.knownAliases.FirstOrDefault(a => a.AssociatedWith == name);
            return found?.Name;
        }

        public string GetAssociation(string alias)
        {
            var existing = this.knownAliases.FirstOrDefault(a => a.Name == alias);
            return existing?.AssociatedWith;
        }

        private string GenerateAlias()
        {
            string foundAlias = null;
            var minAliasCount = this.aliasCounts.Min(t => t.Value);
            foreach(var a in this.aliasCounts)
            {
                if (a.Value != minAliasCount) continue;
                foundAlias = a.Key;
            }

            var alias = $"{foundAlias}{minAliasCount}";
            this.aliasCounts[foundAlias] += 1;
            return alias;
        }

        private class Alias
        {
            private Alias(string name, string assocWith)
            {
                Name = name;
                AssociatedWith = assocWith;
            }

            public string Name { get; }

            public string AssociatedWith { get; set; }

            public static Alias Create(string name, string assocWith) => new Alias(name, assocWith);
        }
    }
}

