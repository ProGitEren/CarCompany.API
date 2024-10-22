using LanguageExt.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Abstractions
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result<bool>> 
        where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
        where TResponse : class
    {
    }
}
