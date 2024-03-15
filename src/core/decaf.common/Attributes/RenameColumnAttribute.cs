using System;

namespace decaf.common.Attributes
{
	public class RenameColumnAttribute : Attribute
	{
        public string Name { get; set; }

        public RenameColumnAttribute() { }

        public RenameColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}

