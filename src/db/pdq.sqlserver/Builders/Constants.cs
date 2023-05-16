using System;
namespace pdq.sqlserver.Builders
{
	public class Constants  : db.common.ANSISQL.Constants
	{
		public override string Ascending => "ascending";
		public override string Descending => "descending";
		public override string Returning => "output";
		public override string Limit => "top";
	}
}

