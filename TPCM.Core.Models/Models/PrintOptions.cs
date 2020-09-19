namespace TPCM.Core.Models
{
	public class PrintOptions
	{
		public int Scale { get; set; }
		public bool DisplayHeaderAndFooter { get; set; }
		public string HeaderTemplate { get; set; }
		public string FooterTemplate { get; set; }
		public bool PrintBackground { get; set; }
		public bool LandScape { get; set; }
		public string Format { get; set; }
		public Margin Margins { get; set; }
	}
}
