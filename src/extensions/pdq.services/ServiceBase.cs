using System;
using pdq.common;
using pdq.state.Utilities;

namespace pdq.services
{
    internal abstract class ServiceBase
    {
        private readonly ITransientInternal transient;
        private readonly IUnitOfWork unitOfWork;
        protected readonly bool disposeOnExit;

        protected readonly IReflectionHelper reflectionHelper;
        protected readonly IExpressionHelper expressionHelper;
        protected readonly IDynamicExpressionHelper dynamicExpressionHelper;

        public ServiceBase()
        {
            reflectionHelper = new ReflectionHelper();
            expressionHelper = new ExpressionHelper(reflectionHelper);
            dynamicExpressionHelper = new DynamicExpressionHelper(expressionHelper, new CallExpressionHelper(expressionHelper));
        }

        protected ServiceBase(ITransient transient) : this()
        {
            this.transient = transient as ITransientInternal;
            this.disposeOnExit = false;
        }

        public ServiceBase(IUnitOfWork unitOfWork) : this()
        {
            this.unitOfWork = unitOfWork;
            this.disposeOnExit = true;
        }

        internal ITransientInternal GetTransient()
        {
            if (this.transient != null) return this.transient;

            return this.unitOfWork.Begin() as ITransientInternal;
        }

        protected void ExecuteQuery(Action<IQuery> method)
        {
            var t = this.GetTransient();
            using(var q = t.Query())
            {
                method(q);
            }

            if (this.disposeOnExit) t.Dispose();
        }

        protected T ExecuteQuery<T>(Func<IQuery, T> method)
        {
            T result;
            var t = this.GetTransient();
            using (var q = t.Query())
            {
                result = method(q);
            }

            if (this.disposeOnExit) t.Dispose();

            return result;
        }
    }
}

