using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt.Common;


namespace Models.Abstractions
{
    public interface ICommand : IRequest<Result<bool>> 
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>> where TResponse : class
        
    {
    }
}
