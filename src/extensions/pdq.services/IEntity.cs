using System;
namespace pdq.services
{
	public interface IEntity { }

	public interface IEntity<TKey> : IEntity
    {
		Type KeyType { get; }

		string KeyName { get; }
    }
}

