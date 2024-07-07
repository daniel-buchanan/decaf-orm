using decaf.services;

namespace decaf.tests.common.Models
{
	public class Address : Entity<int, int>
	{
        public Address()
			: base(nameof(Id), nameof(PersonId))
        {
        }

        public int Id { get; set; }

		public int PersonId { get; set; }

		public string Line1 { get; set; }

		public string Line2 { get; set; }

		public string Line3 { get; set; }

		public string Line4 { get; set; }

		public string PostCode { get; set; }

		public string City { get; set; }

		public string Region { get; set; }

		public string Country { get; set; }
	}
}

