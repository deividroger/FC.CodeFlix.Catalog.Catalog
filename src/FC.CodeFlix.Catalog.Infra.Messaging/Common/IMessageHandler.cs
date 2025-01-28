using FC.CodeFlix.Catalog.Infra.Messaging.Models;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Common;

public interface IMessageHandler<T>
    where T : class
{
    Task HandleMessageAsync(MessageModel<T> messageModel, CancellationToken cancellationToken);
}
