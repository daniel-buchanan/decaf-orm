namespace pdq.common
{
    internal class ManagedAlias
    {
        private ManagedAlias(string name, string assocWith)
        {
            Name = name;
            AssociatedWith = assocWith;
        }

        public string Name { get; }

        public string AssociatedWith { get; set; }

        public static ManagedAlias Create(string name, string assocWith) => new ManagedAlias(name, assocWith);
    }
}

