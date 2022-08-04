using System;
using pdq.common;

namespace pdq.services
{
    public class ServiceBase
    {
        private readonly ITransientInternal transient;
        private readonly IUnitOfWork unitOfWork;

        protected ServiceBase(ITransient transient)
        {
            this.transient = transient as ITransientInternal;
        }

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal ITransientInternal GetTransient()
        {
            if (this.transient != null) return this.transient;

            return this.unitOfWork.Begin() as ITransientInternal;
        }
    }
}

