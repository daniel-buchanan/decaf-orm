﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Service<TEntity> :
        ExecutionNotifiable,
        IService<TEntity>
        where TEntity : class, IEntity, new()
    {
        private IQuery<TEntity> query => GetQuery<IQuery<TEntity>>();
        private ICommand<TEntity> command => GetCommand<ICommand<TEntity>>();

        public Service(
            IQuery<TEntity> query,
            ICommand<TEntity> command)
            : base(query, command) { }

        private Service(ITransient transient)
            : base(transient,
                   t => Query<TEntity>.Create(t),
                   t => Command<TEntity>.Create(t))
        { }

        public static IService<TEntity> Create(ITransient transient)
            => new Service<TEntity>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => this.command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => this.command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
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

        /// <inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity toAdd, CancellationToken cancellationToken = default)
            => await this.command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> toAdd, CancellationToken cancellationToken = default)
            => await this.command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(dynamic toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.command.DeleteAsync(expression, cancellationToken);
    }
}

