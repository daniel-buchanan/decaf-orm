using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using pdq.common;

namespace pdq.services
{
    public class Service<TEntity> :
        IService<TEntity>
        where TEntity : class, IEntity
    {
        private readonly IQuery<TEntity> query;
        private readonly ICommand<TEntity> command;

        public Service(
            IQuery<TEntity> query,
            ICommand<TEntity> command)
        {
            this.query = query;
            this.command = command;
        }

        private Service(ITransient transient)
        {
            this.query = Query<TEntity>.Create(transient);
            this.command = Command<TEntity>.Create(transient);
        }

        public static IService<TEntity> Create(ITransient transient)
            => new Service<TEntity>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => this.command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
            => this.query.All();

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => this.command.Delete(expression);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => this.query.Get(query);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);
    }
}

