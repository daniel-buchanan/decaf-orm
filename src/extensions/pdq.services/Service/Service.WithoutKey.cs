using pdq.common.Connections;

namespace pdq.services
{
    internal class Service<TEntity> :
        ServiceBase<TEntity>
        where TEntity : class, IEntity, new()
    {
        public Service(
            IQuery<TEntity> query,
            ICommand<TEntity> command)
            : base(query, command) { }

        private Service(IUnitOfWork unitOfWork)
            : base(unitOfWork,
                   Query<TEntity>.Create,
                   Command<TEntity>.Create)
        { }

        public static IService<TEntity> Create(IUnitOfWork unitOfWork)
            => new Service<TEntity>(unitOfWork);
    }
}

