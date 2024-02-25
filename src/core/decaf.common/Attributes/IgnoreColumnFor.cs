namespace decaf.common.Attributes
{
    public static class IgnoreColumnFor
    {
        /// <summary>
        /// Ignore the field this is applied to for UPDATE commands
        /// </summary>
        public class AllAttribute : IgnoreColumnForAttribute
        {
            public AllAttribute() : 
                base(QueryTypes.Select |
                     QueryTypes.Insert |
                     QueryTypes.Update |
                     QueryTypes.Delete) { }
        }

        /// <summary>
        /// Ignore the field this is applied to for UPDATE commands
        /// </summary>
        public class UpdateAttribute : IgnoreColumnForAttribute
        {
            public UpdateAttribute() : base(QueryTypes.Update) { }
        }

        /// <summary>
        /// Ingore the field this is applied to for INSERT commands
        /// </summary>
        public class InsertAttribute : IgnoreColumnForAttribute
        {
            public InsertAttribute() : base(QueryTypes.Insert) { }
        }

        /// <summary>
        /// Ingore the field this is applied to for DELETE commands
        /// </summary>
        public class DeleteAttribute : IgnoreColumnForAttribute
        {
            public DeleteAttribute() : base(QueryTypes.Delete) { }
        }

        /// <summary>
        /// Ignore the field this is applied to for SELECT commands
        /// </summary>
        public class SelectAttribute : IgnoreColumnForAttribute
        {
            public SelectAttribute() : base(QueryTypes.Select) { }
        }

        /// <summary>
        /// Ignore the field this is applied to for INSERT and UPDATE commands
        /// </summary>
        public class InsertAndUpdateAttribute : IgnoreColumnForAttribute
        {
            public InsertAndUpdateAttribute() : base(QueryTypes.Insert | QueryTypes.Update) { }
        }
    }
}

