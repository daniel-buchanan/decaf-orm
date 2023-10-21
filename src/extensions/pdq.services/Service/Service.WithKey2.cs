﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;

namespace pdq.services
{
    internal class Service<TEntity, TKey1, TKey2> :
        ExecutionNotifiable,
        IService<TEntity, TKey1, TKey2>
        where TEntity : class, IEntity<TKey1, TKey2>, new()
    {
        private IQuery<TEntity, TKey1, TKey2> query
            => GetQuery<IQuery<TEntity, TKey1, TKey2>>();

        private ICommand<TEntity, TKey1, TKey2> command
            => GetCommand<ICommand<TEntity, TKey1, TKey2>>();

        public Service(
            IQuery<TEntity, TKey1, TKey2> query,
            ICommand<TEntity, TKey1, TKey2> command)
            : base(query, command) { }

        private Service(ITransient transient)
            : base(transient,
                   t => Query<TEntity, TKey1, TKey2>.Create(t),
                   t => Command<TEntity, TKey1, TKey2>.Create(t))
        { }

        public static IService<TEntity, TKey1, TKey2> Create(ITransient transient)
            => new Service<TEntity, TKey1, TKey2>(transient);

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
        public void Delete(TKey1 key1, TKey2 key2)
            => this.command.Delete(key1, key2);

        /// <inheritdoc/>
        public void Delete(params ICompositeKeyValue<TKey1, TKey2>[] keys)
            => this.command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys)
            => this.command.Delete(keys);

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => this.command.Delete(expression);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query)
            => this.query.Get(query);

        /// <inheritdoc/>
        public TEntity Get(TKey1 key1, TKey2 key2)
            => this.query.Get(key1, key2);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(params ICompositeKeyValue<TKey1, TKey2>[] keys)
            => this.query.Get(keys);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Get(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys)
            => this.query.Get(keys);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, TKey1 key1, TKey2 key2)
            => this.command.Update(toUpdate, key1, key2);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate)
            => this.command.Update(toUpdate);

        public async Task UpdateAsync(TEntity toUpdate, CancellationToken cancellationToken = default)
            => await this.command.UpdateAsync(toUpdate, cancellationToken);

        public async Task UpdateAsync(dynamic toUpdate, TKey1 key1, TKey2 key2, CancellationToken cancellationToken = default)
            => await this.command.UpdateAsync(toUpdate, key1, key2, cancellationToken);

        public async Task DeleteAsync(TKey1 key1, TKey2 key2, CancellationToken cancellationToken = default)
            => await this.command.DeleteAsync(key1, key2, cancellationToken);

        public async Task DeleteAsync(ICompositeKeyValue<TKey1, TKey2>[] keys, CancellationToken cancellationToken = default)
            => await this.command.DeleteAsync(keys, cancellationToken);

        public async Task DeleteAsync(IEnumerable<ICompositeKeyValue<TKey1, TKey2>> keys, CancellationToken cancellationToken = default)
            => await this.command.DeleteAsync(keys, cancellationToken);

        public async Task<TEntity> AddAsync(TEntity toAdd, CancellationToken cancellationToken = default)
            => await this.command.AddAsync(toAdd, cancellationToken);

        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> toAdd, CancellationToken cancellationToken = default)
            => await this.command.AddAsync(toAdd, cancellationToken);

        public async Task UpdateAsync(TEntity toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.command.UpdateAsync(toUpdate, expression, cancellationToken);

        public async Task UpdateAsync(dynamic toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.command.UpdateAsync(toUpdate, expression, cancellationToken);

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.command.DeleteAsync(expression, cancellationToken);
    }
}

