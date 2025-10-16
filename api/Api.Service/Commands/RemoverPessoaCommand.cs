using MediatR;

namespace Api.Service.Commands;

public record RemoverPessoaCommand(Guid Id) : IRequest<bool>;
