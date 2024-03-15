using decaf.common;
using decaf.common.Connections;

namespace decaf.services
{
    internal class Service<TEntity> :
        ServiceBase<TEntity>
        where TEntity : class, IEntity, new()
    {
        public Service(
            ISqlFactory sqlFactory,
            IQuery<TEntity> query,
            ICommand<TEntity> command)
            : base(sqlFactory, query, command) { }

        private Service(IUnitOfWork unitOfWork)
            : base(
                unitOfWork,
                (unitOfWork as IUnitOfWorkExtended)?.SqlFactory,
                Query<TEntity>.Create,
                Command<TEntity>.Create) { }

        public static IService<TEntity> Create(IUnitOfWork unitOfWork)
            => new Service<TEntity>(unitOfWork);
    }
}

