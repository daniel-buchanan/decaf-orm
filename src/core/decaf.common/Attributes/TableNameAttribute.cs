using System;

namespace decaf.common.Attributes
{
	public class TableNameAttribute : Attribute
	{
        public string Name { get; set; }

        public bool CaseSensitive { get; set; }

        public TableNameAttribute() { }

        public TableNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}

