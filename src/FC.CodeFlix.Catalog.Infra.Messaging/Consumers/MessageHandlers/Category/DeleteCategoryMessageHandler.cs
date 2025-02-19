using FC.CodeFlix.Catalog.Domain.Exceptions;
using FC.CodeFlix.Catalog.Infra.Messaging.Common;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Category;

public class DeleteCategoryMessageHandler : IMessageHandler<CategoryPayloadModel>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeleteCategoryMessageHandler> _logger;

    public DeleteCategoryMessageHandler(IMediator mediator, ILogger<DeleteCategoryMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleMessageAsync(MessageModel<CategoryPayloadModel> messageModel, CancellationToken cancellationToken)
    {
        try
        {
            var deleteInput = messageModel.Payload.Before.ToDeleteCategoryInput();
            await _mediator.Send(deleteInput, cancellationToken);
        }
        catch (NotFoundException)
        {
            _logger.LogError("Category not found. Message: {@message}", messageModel);
        }
    }
}
