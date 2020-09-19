using System;

namespace TPCM.Core.Models
{
	public interface IUserInfo
	{
		DateTime Created { get; }
		string CreatedBy { get; }
		DateTime? Modifed { get; }
	}
}
