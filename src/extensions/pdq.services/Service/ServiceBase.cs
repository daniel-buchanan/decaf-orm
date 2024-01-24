using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using pdq.common.Connections;
using pdq.common.Utilities;

namespace pdq.services
{
    internal abstract class ServiceBase<TEntity> :
        ExecutionNotifiable, 
        IService<TEntity>
        where TEntity : class, IEntity, new()
    {
        private IQuery<TEntity> Query => GetQuery<IQuery<TEntity>>();
        private ICommand<TEntity> Command => GetCommand<ICommand<TEntity>>();
        
        protected ServiceBase(
            IExecutionNotifiable query, 
            IExecutionNotifiable command) : 
            base(query, command) { }

        protected ServiceBase(
            IUnitOfWork unitOfWork, 
            Func<IUnitOfWork, IExecutionNotifiable> createQuery, 
            Func<IUnitOfWork, IExecutionNotifiable> createCommand) : 
            base(unitOfWork, createQuery, createCommand) { }

        /// <inheritdoc/>
        public IEnumerable<TEntity> All()
            => AllAsync().WaitFor();

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> AllAsync(CancellationToken cancellationToken = default)
            => Query.AllAsync(cancellationToken);

        /// <inheritdoc/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
            => FindAsync(expression).WaitFor();

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Query.FindAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public Task<TEntity> AddAsync(
            TEntity toAdd, 
            CancellationToken cancellationToken = default)
            => Command.AddAsync(toAdd, cancellationToken);

        /// <inheritdoc/>
        public Task<IEnumerable<TEntity>> AddAsync(
            IEnumerable<TEntity> items, 
            CancellationToken cancellationToken = default)
            => Command.AddAsync(items, cancellationToken);

        /// <inheritdoc/>
        public Task UpdateAsync(
            TEntity toUpdate,
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public Task UpdateAsync(
            dynamic toUpdate,
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Command.UpdateAsync(toUpdate, expression, cancellationToken);

        /// <inheritdoc/>
        public Task DeleteAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Command.DeleteAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public TEntity Add(TEntity toAdd)
            => AddAsync(toAdd).WaitFor();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(params TEntity[] toAdd)
            => AddAsync(toAdd?.ToArray()).WaitFor();

        /// <inheritdoc/>
        public IEnumerable<TEntity> Add(IEnumerable<TEntity> toAdd)
            => AddAsync(toAdd).WaitFor();

        /// <inheritdoc/>
        public void Update(TEntity toUpdate, Expression<Func<TEntity, bool>> expression)
            => UpdateAsync(toUpdate, expression).WaitFor();

        /// <inheritdoc/>
        public void Update(dynamic toUpdate, Expression<Func<TEntity, bool>> expression)
        {
            var t = UpdateAsync(toUpdate, expression);
            t.Wait();
        }

        /// <inheritdoc/>
        public void Delete(Expression<Func<TEntity, bool>> expression)
            => DeleteAsync(expression).WaitFor();
    }
}