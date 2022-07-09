using pdq.common;

namespace pdq.Attributes
{
    public class IgnoreColumnFor
    {
        /// <summary>
        /// Ignore the field this is applied to for UPDATE commands
        /// </summary>
        public class Update : IgnoreColumnForAttribute
        {
            public Update() : base(QueryTypes.Update) { }
        }

        /// <summary>
        /// Ingore the field this is applied to for INSERT commands
        /// </summary>
        public class Insert : IgnoreColumnForAttribute
        {
            public Insert() : base(QueryTypes.Insert) { }
        }

        /// <summary>
        /// Ingore the field this is applied to for DELETE commands
        /// </summary>
        public class Delete : IgnoreColumnForAttribute
        {
            public Delete() : base(QueryTypes.Delete) { }
        }

        /// <summary>
        /// Ignore the field this is applied to for SELECT commands
        /// </summary>
        public class Select : IgnoreColumnForAttribute
        {
            public Select() : base(QueryTypes.Select) { }
        }

        /// <summary>
        /// Ignore the field this is applied to for INSERT and UPDATE commands
        /// </summary>
        public class InsertAndUpdate : IgnoreColumnForAttribute
        {
            public InsertAndUpdate() : base(QueryTypes.Insert | QueryTypes.Update) { }
        }
    }
}

