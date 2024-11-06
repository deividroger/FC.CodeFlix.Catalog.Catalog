using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.CodeFlix.Catalog.Infra.Messaging.Configuration;

public class KafkaConfiguration
{
    public string BoostrapServers { get; set; } = null!;

    public KafkaConsumerConfiguration CategoryConsumer { get; set; } = null!;

}

public class KafkaConsumerConfiguration
{
    public string GroupId { get; set; } = null!;
    public string Topic { get; set; } = null!;

}