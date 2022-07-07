using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;
using Dapper.Contrib.Extensions;

namespace pdq.services
{
    public class Command<TEntity> :
        ServiceBase,
        ICommand<TEntity>
        where TEntity : class, IEntity
    {
        public Command(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        protected Command(ITransient transient) : base(transient) { }

        public static ICommand<TEntity> Create(ITransient transient) => new Command<TEntity>(transient);

        public TEntity Add(TEntity toAdd)
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

