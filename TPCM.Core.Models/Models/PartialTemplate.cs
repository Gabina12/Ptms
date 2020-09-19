using System;

namespace TPCM.Core.Models
{
	public class PartialTemplate : IEntity<Guid>, ITemplateInfo, IUserInfo
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Category { get; set; }

		public string Body { get; set; }

		public string Header { get; set; }

		public string Footer { get; set; }

		public DateTime Created { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? Modifed { get; set; }
	}
}
