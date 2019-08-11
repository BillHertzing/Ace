using System;
namespace ATAP.Utilities.Shared {

    #region interfaces for AMQP feature
    public interface IAMQPData {
        public IAMQPMessageService MQServer { get; }
        public IAMQPMessageFactory MessageFactory { get; }
        public IAMQPMessageQueueClientFactory MessageQueueClientFactory { get; }

    }
    public interface IAMQPMessageService : ServiceStack.Messaging.IMessageService {

    }
    public interface IAMQPMessage : ServiceStack.Messaging.IMessage {

    }
    public interface IAMQPMessage<T> : ServiceStack.Messaging.IMessage<T> {

    }
    public interface IAMQPMessageQueueClientFactory : ServiceStack.Messaging.IMessageQueueClientFactory {
        new IAMQPMessageQueueClient CreateMessageQueueClient();
    }

    public interface IAMQPMessageFactory : ServiceStack.Messaging.IMessageFactory {

    }

    public interface IAMQPMessageProducer : ServiceStack.Messaging.IMessageProducer {

    }
    public interface IAMQPMessageQueueClient : ServiceStack.Messaging.IMessageQueueClient, IAMQPMessageProducer {
        /// <summary>
        /// Publish the specified message into the durable queue @queueName
        /// </summary>
        void Publish(string queueName, IAMQPMessage message);

        /// <summary>
        /// Publish the specified message into the transient queue @queueName
        /// </summary>
         void Notify(string queueName, IAMQPMessage message);

        /// <summary>
        /// Synchronous blocking get.
        /// </summary>
        new IAMQPMessage<T> Get<T>(string queueName, TimeSpan? timeOut = null);

        /// <summary>
        /// Non blocking get message
        /// </summary>
        new IAMQPMessage<T> GetAsync<T>(string queueName);

        /// <summary>
        /// Acknowledge the message has been successfully received or processed
        /// </summary>
        new void Ack(IAMQPMessage message);

        /// <summary>
        /// Negative acknowledgment the message was not processed correctly
        /// </summary>
        new void Nak(IAMQPMessage message, bool requeue, Exception exception = null);

        /// <summary>
        /// Create a typed message from a raw MQ Response artefact
        /// </summary>
        new IAMQPMessage<T> CreateMessage<T>(object mqResponse);


    }

    public interface IAMQPMessageHandlerFactory : ServiceStack.Messaging.IMessageHandlerFactory {
        new IAMQPMessageHandler CreateMessageHandler();

    }
    public interface IAMQPMessageHandler : ServiceStack.Messaging.IMessageHandler {
        /// <summary>
        /// The MqClient processing the message
        /// </summary>
        new IAMQPMessageQueueClient MqClient { get; }
        /// <summary>
        /// Process all messages pending
        /// </summary>
        /// <param name="mqClient"></param>
        void Process(IAMQPMessageQueueClient mqClient);

        /// <summary>
        /// Process messages from a single queue.
        /// </summary>
        /// <param name="mqClient"></param>
        /// <param name="queueName">The queue to process</param>
        /// <param name="doNext">A predicate on whether to continue processing the next message if any</param>
        /// <returns></returns>
        int ProcessQueue(IAMQPMessageQueueClient mqClient, string queueName, Func<bool> doNext = null);

        /// <summary>
        /// Process a single message
        /// </summary>
        void ProcessMessage(IAMQPMessageQueueClient mqClient, object mqResponse);
    }
    public interface IAMQPMessageHandlerDisposer {
        void DisposeMessageHandler(IAMQPMessageHandler messageHandler);
    }
    #endregion

}
