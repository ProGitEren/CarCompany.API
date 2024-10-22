using LanguageExt.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstractions
{
    public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result<bool>>
        where TQuery : IQuery
    {
    }

    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
        where TResponse : class
    {
    }
}
