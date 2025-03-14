using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.Messaging.Common;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.CastMember;

public class DeleteCastMemberMessageHandler: IMessageHandler<CastMemberPayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteCastMemberMessageHandler> _logger;

    public DeleteCastMemberMessageHandler(IMediator mediator, ILogger<DeleteCastMemberMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(MessageModel<CastMemberPayloadModel> messageModel, CancellationToken cancellationToken)
    {
        try
        {
            var deleteInput = messageModel.Payload.Before.ToDeleteCastMemberInput();
            await _mediator.Send(deleteInput, cancellationToken);
        }
        catch (NotFoundException)
        {
            _logger.LogError("Category not found. Message: {@message}", messageModel);
        }
    }
}
