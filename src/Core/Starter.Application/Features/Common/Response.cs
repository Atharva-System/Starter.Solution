using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starter.Application.Contracts.Responses;

namespace Starter.Application.Features.Common;
public class Response : IResponse
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; }
}
