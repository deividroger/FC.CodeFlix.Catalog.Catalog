using FC.CodeFlix.Catalog.Application.UseCases.Genre.SaveGenre;
using FC.CodeFlix.Catalog.Infra.Messaging.Common;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using MediatR;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Genre;

public class SaveGenreMessageHandler<T>
    : IMessageHandler<T>
    where T : GenrePayloadModel
    
{
    private readonly IMediator _mediator;

    public SaveGenreMessageHandler(IMediator mediator)
        => _mediator = mediator;

    public async Task HandleMessageAsync(MessageModel<T> messageModel, CancellationToken cancellationToken)
    {
        var id = messageModel.Payload.After?.Id ?? messageModel.Payload.Before.Id; 
        var input = new  SaveGenreInput(id);

        await _mediator.Send(input,cancellationToken);
    }
}
