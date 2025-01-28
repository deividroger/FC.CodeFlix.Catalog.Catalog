using FC.CodeFlix.Catalog.Infra.Messaging.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Configuration;

public class KafkaConfiguration
{
    
    public KafkaConsumerConfiguration CategoryConsumer { get; set; } = null!;

}

public class KafkaConsumerConfiguration
{
    public string BoostrapServers { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string? RetryTopic { get; set; }
    public string? DlqTopic { get; set; }

    public int ConsumeDelaySeconds { get; private set; }

    public bool HasRetry => !string.IsNullOrEmpty(RetryTopic);

    public KafkaConsumerConfiguration CreateRetryConfiguration(int retryIndex, bool hasNextRetry)
        => new()
        {
            BoostrapServers = BoostrapServers,
            GroupId = GroupId,
            Topic = Topic.ToRetryTopic(retryIndex),
            RetryTopic = !hasNextRetry ? null : Topic.ToRetryTopic(retryIndex + 1),
            DlqTopic = DlqTopic,
            ConsumeDelaySeconds = (int)Math.Pow(2, retryIndex),    
        };
}