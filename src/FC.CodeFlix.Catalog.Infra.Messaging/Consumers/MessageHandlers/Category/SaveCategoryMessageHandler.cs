using FC.CodeFlix.Catalog.Infra.Messaging.Common;
using FC.CodeFlix.Catalog.Infra.Messaging.Models;
using MediatR;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Consumers.MessageHandlers.Category;

public class SaveCategoryMessageHandler : IMessageHandler<CategoryPayloadModel>
{
    private readonly IMediator _mediator;

    public SaveCategoryMessageHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleMessageAsync(MessageModel<CategoryPayloadModel> messageModel, CancellationToken cancellationToken)
    {
        var saveInput = messageModel.Payload.After.ToSaveCategoryInput();

        await _mediator.Send(saveInput, cancellationToken);
    }
}
