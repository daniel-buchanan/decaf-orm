﻿using System;
using decaf.common.Connections;
using Microsoft.Extensions.Configuration;

namespace decaf.db.common;

public interface IDatabaseOptions
{
	/// <summary>
	/// The SQL connection details to use.
	/// </summary>
	IConnectionDetails ConnectionDetails { get; }

	/// <summary>
	/// Whether or not to quote identifiers.
	/// </summary>
	bool QuotedIdentifiers { get; }
}

public interface IDatabaseOptionsExtensions : IDatabaseOptions
{
	Func<IServiceProvider, IConnectionDetails> ConnectionDetailsServiceProviderFactory { get; }

	Func<IConfiguration, IConnectionDetails> ConnectionDetailsConfigurationFactory { get; }

	IConnectionDetails GetConnectionDetails(IServiceProvider provider);
        
	bool IncludeParameterPrefix { get; }
}