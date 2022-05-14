using System;
namespace pdq.core.Attributes
{
	public class TableNameAttribute : Attribute
	{
        public string Name { get; set; }

        public TableNameAttribute() { }

        public TableNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}

