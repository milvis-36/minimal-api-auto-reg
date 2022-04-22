using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace SourceGenerators.Endpoints
{
	public interface IConvetionalEndpointRegistration
	{
		void Reg(IApplicationBuilder app);

	}
}
