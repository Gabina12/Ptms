using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SelectPdf;
using Stubble.Core.Builders;
using PTMS.Core.Extentions;
using PTMS.Core.Models;
using PTMS.Core.Repositories;
using PTMS.Core.Services.Interfaces;

namespace PTMS.Core.Services.Implementations
{
	public class RenderingService : IRenderingService
	{
		private static Stubble.Core.StubbleVisitorRenderer _renderer = new StubbleBuilder()
								.Configure(builder => {
									builder.SetIgnoreCaseOnKeyLookup(true);
								})
								.Build();

		private readonly IGeneralCache<Template> _cache;
		private readonly ICache<PartialCacheItem> _partials;
		private readonly ITemplateRepository _templateRepository;

		public RenderingService(IGeneralCache<Template> cache, ICache<PartialCacheItem> partials, ITemplateRepository templateRepository)
		{
			_cache = cache;
			_templateRepository = templateRepository;
			_partials = partials;
		}

		public async Task<string> RenderAsync(string id, string type, string version, object json)
		{
			var template = await _cache.Get(_templateRepository,id.ToCacheKey(version));
			var partials = await _partials.GetList("partials");
			return await _renderer.RenderAsync(template.TemplateBody, json, partials?.ToDictionary(x => x.Id, x => x.Template));
		}

		public async Task<byte[]> RenderPdfAsync(string id, string type, string version, object json) {
			var template = await _cache.Get(_templateRepository, id.ToCacheKey(version));
			var html = await _renderer.RenderAsync(template.TemplateBody, json);
			return SavePdf(template, html);
		}

		private byte[] SavePdf(Template template, string renderedHtml) {

			try {
				var options = template.Options;

				HtmlToPdf converter = new HtmlToPdf();
				converter.Options.PdfPageOrientation = options.Landscape ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
				converter.Options.PdfPageSize = GetPageSize(options.Format);
				converter.Options.MarginBottom = GetMargin(options.Margin.Bottom);
				converter.Options.MarginTop = GetMargin(options.Margin.Top);
				converter.Options.MarginLeft = GetMargin(options.Margin.Left);
				converter.Options.MarginRight = GetMargin(options.Margin.Right);
				converter.Options.ExternalLinksEnabled = template.LoadExternalSources;

				converter.Options.MinPageLoadTime = 2;
				converter.Options.MaxPageLoadTime = 360;

				if (template.Options.DisplayHeaderFooter) {
					converter.Header.Add(new PdfHtmlSection(options.HeaderTemplate, string.Empty));
					converter.Footer.Add(new PdfHtmlSection(options.FooterTemplate, string.Empty));
				}
				if (template.Options.PrintBackground) {

				}
				PdfDocument doc = converter.ConvertHtmlString(renderedHtml);
				byte[] pdfDoc = doc.Save();
				doc.Close();
				return pdfDoc;
			}catch(Exception ex) {
				throw;
            }
		}

		private PdfPageSize GetPageSize(string size) {
            PdfPageSize pdfPageSize;
            Enum.TryParse($"{size.ToUpper()[0]}{size[1..]}", out pdfPageSize);
			return pdfPageSize;
		}

        private int GetMargin(string margin) => Convert.ToInt32(margin[0..^2]);
    }
}
