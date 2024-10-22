using LanguageExt.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstractions
{
    public interface IQuery : IRequest<Result<bool>>
    {
    }

    public interface IQuery<TResponse> : IRequest<Result<TResponse>> where TResponse : class
        
    {
    }
}
