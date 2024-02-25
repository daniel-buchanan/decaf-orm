using System;
namespace decaf.tests.common.Models
{
	public class User
	{
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int PersonId { get; set; }

        public Guid Subject { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

