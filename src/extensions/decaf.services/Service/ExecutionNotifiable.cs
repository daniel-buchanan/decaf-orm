using System;
using decaf.common.Connections;

namespace decaf.services
{
    internal class ExecutionNotifiable :
        IExecutionNotifiable
    {
        private readonly IExecutionNotifiable query;
        private readonly IExecutionNotifiable command;

        protected ExecutionNotifiable(
            IExecutionNotifiable query,
            IExecutionNotifiable command)
        {
            this.query = query;
            this.command = command;
        }

        protected ExecutionNotifiable(
            IUnitOfWork unitOfWork,
            Func<IUnitOfWork, IExecutionNotifiable> createQuery,
            Func<IUnitOfWork, IExecutionNotifiable> createCommand)
        {
            query = createQuery(unitOfWork);
            command = createCommand(unitOfWork);
        }

        /// <inheritdoc/>
        public event EventHandler<PreExecutionEventArgs> OnBeforeExecution
        {
            add
            {
                query.OnBeforeExecution += value;
                command.OnBeforeExecution += value;
            }

            remove
            {
                query.OnBeforeExecution -= value;
                command.OnBeforeExecution -= value;
            }
        }

        protected T GetQuery<T>() where T : IQuery => (T)query;

        protected T GetCommand<T>() where T : ICommand => (T)command;
    }
}

