using System;
using System.Collections.Generic;

namespace decaf.common.Utilities;

public interface IAliasManager : IDisposable
{
    /// <summary>
    /// Add an Alias, if none is provided one will be generated
    /// </summary>
    /// <param name="assocWith">Entity to create or associate this alias with.</param>
    /// <param name="alias">The alias to use (optional).</param>
    /// <returns>The alias that has been associated with the Entity.</returns>
    string Add(string alias, string assocWith);

    /// <summary>
    /// Get all known Aliases.
    /// </summary>
    /// <returns>A read only enumeration of all known Aliases</returns>
    IReadOnlyCollection<ManagedAlias> All();

    /// <summary>
    /// Find Aliases by their associated Entity.
    /// </summary>
    /// <param name="assocWith">The entity the alias is associated with</param>
    /// <returns>An Enumeration of all known aliases for the provided resource.</returns>
    IEnumerable<ManagedAlias> FindByAssociation(string assocWith);

    /// <summary>
    /// Get the associated Entity for a given Alias.
    /// </summary>
    /// <param name="alias">The alias to retrieve the associated Entity for.</param>
    /// <returns>The entity associated with the provided Alias.</returns>
    string GetAssociation(string alias);
}