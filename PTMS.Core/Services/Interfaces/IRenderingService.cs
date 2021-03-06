﻿using System;
using System.Threading.Tasks;

namespace PTMS.Core.Services.Interfaces
{
	public interface IRenderingService
	{
		Task<string> RenderAsync(string id, string type, string version, object json);
		Task<byte[]> RenderPdfAsync(string id, string type, string version, object json);
	}
}
