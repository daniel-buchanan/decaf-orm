using System;
using pdq.common;

namespace pdq.services
{
    public class ServiceBase
    {
        private readonly ITransient transient;
        private readonly IUnitOfWork unitOfWork;

        protected ServiceBase(ITransient transient)
        {
            this.transient = transient;
        }

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected ITransient GetTransient()
        {
            if (this.transient != null) return this.transient;

            return this.unitOfWork.Begin();
        }
    }
}

