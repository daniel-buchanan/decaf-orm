using System;
namespace pdq.Attributes
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

