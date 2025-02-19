using FC.CodeFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.Messaging.Common;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Genre;

public class DeleteGenreMessageHandler : IMessageHandler<GenrePayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteGenreMessageHandler> _logger;

    public DeleteGenreMessageHandler(IMediator mediator, ILogger<DeleteGenreMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(MessageModel<GenrePayloadModel> messageModel, CancellationToken cancellationToken)
    {
        try
        {
            var id = messageModel.Payload.Before.Id;
            var input = new DeleteGenreInput(id);
            await _mediator.Send(input, cancellationToken);

        }
        catch (NotFoundException ex)
        {

            _logger.LogError(ex, "Genre not found. {@message}", messageModel);
        }
    }
}
