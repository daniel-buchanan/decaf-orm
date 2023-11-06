using System;
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
        private IQuery<TEntity> Query => GetQuery<IQuery<TEntity>>();
        private ICommand<TEntity> Command => GetCommand<ICommand<TEntity>>();

        public Service(
            IQuery<TEntity> query,
            ICommand<TEntity> command)
            : base(query, command) { }

        private Service(ITransient transient)
            : base(transient,
                   Query<TEntity>.Create,
                   Command<TEntity>.Create)
        { }

        public static IService<TEntity> Create(ITransient transient)
            => new Service<TEntity>(transient);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => this.Command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => this.Command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
            => this.Command.Add(toAdd);

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
            => this.Query.All();

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default)
            => this.Query.AllAsync(cancellationToken);

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => this.Command.Delete(expression);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
            => this.Query.Find(expression);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => this.Query.FindAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.Command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
            => this.Command.Update(toUpdate, expression);

        /// <inheritdoc/>
        public async Task<TEntity> AddAsync(TEntity toAdd, CancellationToken cancellationToken = default)
            => await this.Command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default)
            => await this.Command.AddAsync(items, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public async Task UpdateAsync(dynamic toUpdate, Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.Command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public async Task DeleteAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
            => await this.Command.DeleteAsync(expression, cancellationToken);
    }
}

