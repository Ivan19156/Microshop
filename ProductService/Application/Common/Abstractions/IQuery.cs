using MediatR;

namespace Application.Common.Abstractions;

public interface IQuery<TResponse> : IRequest<TResponse> { }
