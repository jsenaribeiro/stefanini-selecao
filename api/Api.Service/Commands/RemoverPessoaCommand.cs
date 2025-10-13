using System.ComponentModel.DataAnnotations;
using Api.Domain.Pessoas;
using Api.Infrastructure.Attributes;
using Api.Service.Results;
using MediatR;

namespace Api.Service.Commands;

public record RemoverPessoaCommand(Guid Id) : IRequest<bool>;
