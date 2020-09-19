namespace TPCM.Core.Models
{
	public interface IStoreDatabaseSettings
	{
		string CollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}
