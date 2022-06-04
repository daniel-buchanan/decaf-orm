using System;
namespace pdq.Attributes
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

