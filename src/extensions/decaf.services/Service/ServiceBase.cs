using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using decaf.common;
using decaf.common.Connections;
using decaf.common.Utilities;

namespace decaf.services
{
    internal abstract class ServiceBase<TEntity> :
        ExecutionNotifiable, 
        IService<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly ISqlFactory sqlFactory;
        private IQuery<TEntity> Query => GetQuery<IQuery<TEntity>>();
        private ICommand<TEntity> Command => GetCommand<ICommand<TEntity>>();

        protected ServiceBase(
            ISqlFactory sqlFactory,
            IExecutionNotifiable query,
            IExecutionNotifiable command) :
            base(query, command)
        {
            this.sqlFactory = sqlFactory;
            OnBeforeExecution += HandleLastExecutedSql;
        }

        protected ServiceBase(
            IUnitOfWork unitOfWork,
            ISqlFactory sqlFactory,
            Func<IUnitOfWork, IExecutionNotifiable> createQuery,
            Func<IUnitOfWork, IExecutionNotifiable> createCommand) :
            base(unitOfWork, createQuery, createCommand)
        {
            this.sqlFactory = sqlFactory;
            OnBeforeExecution += HandleLastExecutedSql;
        }

        private void HandleLastExecutedSql(object sender, PreExecutionEventArgs e)
        {
            var template = sqlFactory.ParseTemplate(e.Context);
            LastExecutedSql = template.Sql;
        }
        
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
        public TEntity Single(Expression<Func<TEntity, bool>> expression)
            => SingleAsync(expression).WaitFor();

        /// <inheritdoc/>
        public Task<TEntity> SingleAsync(
            Expression<Func<TEntity, bool>> expression, 
            CancellationToken cancellationToken = default)
            => Query.SingleAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
            => SingleOrDefaultAsync(expression).WaitFor();

        /// <inheritdoc/>
        public Task<TEntity> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Query.SingleOrDefaultAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public TEntity First(Expression<Func<TEntity, bool>> expression)
            => FirstAsync(expression).WaitFor();

        /// <inheritdoc/>
        public Task<TEntity> FirstAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Query.FirstAsync(expression, cancellationToken);

        /// <inheritdoc/>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
            => FirstOrDefaultAsync(expression).WaitFor();

        /// <inheritdoc/>
        public Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression,
            CancellationToken cancellationToken = default)
            => Query.FirstOrDefaultAsync(expression, cancellationToken);

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

        public string LastExecutedSql { get; private set; }
    }
}