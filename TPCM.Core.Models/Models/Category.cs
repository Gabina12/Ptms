using System;

namespace TPCM.Core.Models
{
	public class Category : IEntity<Guid>
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
