using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using Dapper.Contrib.Extensions;

namespace pdq.services
{
    public class Command<TEntity, TKey> :
        ServiceBase,
        ICommand<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        private Command(ITransient transient) : base(transient) { }

        public static ICommand<TEntity, TKey> Create(ITransient transient) => new Command<TEntity, TKey>(transient);

        public TEntity Add(TEntity toAdd)
        {
            throw new NotImplementedException();
        }

        public void Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Delete(params TKey[] keys)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<TKey> keys)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity toUpdate)
        {
            throw new NotImplementedException();
        }

        public void Update(dynamic toUpdate, TKey key)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}

