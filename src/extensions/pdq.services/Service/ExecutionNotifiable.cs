using System;
using pdq.common.Connections;

namespace pdq.services
{
    internal class ExecutionNotifiable :
        IExecutionNotifiable
    {
        private readonly IExecutionNotifiable query;
        private readonly IExecutionNotifiable command;

        public ExecutionNotifiable(
            IExecutionNotifiable query,
            IExecutionNotifiable command)
        {
            this.query = query;
            this.command = command;
        }

        protected ExecutionNotifiable(
            ITransient transient,
            Func<ITransient, IExecutionNotifiable> createQuery,
            Func<ITransient, IExecutionNotifiable> createCommand)
        {
            this.query = createQuery(transient);
            this.command = createCommand(transient);
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

