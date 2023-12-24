using System;
using pdq.common.Connections;

namespace pdq.services
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
            this.query = createQuery(unitOfWork);
            this.command = createCommand(unitOfWork);
        }

        /// <inheritdoc/>
        public event EventHandler<PreExecutionEventArgs> OnBeforeExecution
        {
            add
            {
                this.query.OnBeforeExecution += value;
                this.command.OnBeforeExecution += value;
            }

            remove
            {
                this.query.OnBeforeExecution -= value;
                this.command.OnBeforeExecution -= value;
            }
        }

        protected T GetQuery<T>() where T : IQuery => (T)this.query;

        protected T GetCommand<T>() where T : ICommand => (T)this.command;
    }
}

