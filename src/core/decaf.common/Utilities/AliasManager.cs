using System;
using System.Linq;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("decaf.core.tests")]
namespace decaf.common.Utilities;

internal sealed class AliasManager : IAliasManager
{
    private readonly List<ManagedAlias> knownAliases;
    private readonly SortedDictionary<string, int> aliasCounts;

    private AliasManager()
    {
        aliasCounts = new SortedDictionary<string, int>()
        {
            { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 }
        };
        knownAliases = new List<ManagedAlias>();
    }

    public static IAliasManager Create() => new AliasManager();

    /// <inheritdoc />
    public string Add(string alias, string assocWith)
    {
        if (string.IsNullOrWhiteSpace(assocWith))
            throw new ArgumentNullException(nameof(assocWith), "The assocWith parameter must be provided to either generate or add an alias.");

        if (string.IsNullOrWhiteSpace(alias))
        {
            alias = GenerateAlias(assocWith);
        }

        alias = alias.ToLower();
        var existing = knownAliases.Find(a => a.Name == alias);
        if (existing != null)
        {
            if (string.IsNullOrWhiteSpace(existing.Relation))
                existing.Relation = assocWith;
            return alias;
        }

        knownAliases.Add(ManagedAlias.Create(alias, assocWith));
        return alias;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<ManagedAlias> All()
        => knownAliases.AsReadOnly();

    /// <inheritdoc />
    public void Dispose()
    {
        aliasCounts.Clear();
        knownAliases.Clear();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public IEnumerable<ManagedAlias> FindByAssociation(string assocWith)
        => knownAliases.Where(a => a.Relation == assocWith).ToList();

    /// <inheritdoc />
    public string GetAssociation(string alias)
    {
        var existing = knownAliases.Find(a => a.Name == alias);
        return existing?.Relation;
    }

    private string GenerateAlias(string assocWith)
    {
        if (string.IsNullOrWhiteSpace(assocWith) || assocWith.Length <= 1)
            assocWith = "a";
            
        var prefix = assocWith.Substring(0, 1).ToLower();
        var aliasCount = aliasCounts[prefix];
        var alias = $"{prefix}{aliasCount}";
        aliasCounts[prefix] += 1;
        return alias;
    }
}