using System;
using pdq.services;

namespace pdq.tests.common.Models
{
	public class Person : Entity<int>
	{
        public Person() : base(nameof(Id)) { }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int AddressId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

