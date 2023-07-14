using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace StarWars;


public class MessageSubscription : ObjectGraphType
{
    private readonly IChat _chat;
    public MessageSubscription(IChat chat)
    {
        _chat = chat;
        AddField(new FieldType
        {
            Name = "messageAdded",
            Type = typeof(MessageType),
            StreamResolver = new SourceStreamResolver<Message>(Subscribe)
        });

        // test
        Task.Run(() => _chat.AddTestMessagesAsync());
    }

    private IObservable<Message> Subscribe(IResolveFieldContext context)
    {
        return _chat.Messages();
    }
}

public class MessageType : ObjectGraphType<Message>
{
    public MessageType()
    {
        Field(o => o.Content);
        Field(o => o.SentAt);
        Field(o => o.From, false, typeof(MessageFromType)).Resolve(ResolveFrom);
    }

    private MessageFrom ResolveFrom(IResolveFieldContext<Message> context)
    {
        var message = context.Source;
        return message.From;
    }
}

public class MessageFromType : ObjectGraphType<MessageFrom>
{
    public MessageFromType()
    {
        Field(o => o.Id);
        Field(o => o.DisplayName);
    }
}

public class Message
{
    public MessageFrom From { get; set; }

    public string Content { get; set; }

    public DateTime SentAt { get; set; }
}

public class MessageFrom
{
    public string Id { get; set; }

    public string DisplayName { get; set; }
}

public class ReceivedMessage
{
    public string FromId { get; set; }

    public string Content { get; set; }

    public DateTime SentAt { get; set; }
}

public interface IChat
{
    ConcurrentStack<Message> AllMessages { get; }

    Message AddMessage(Message message);

    IObservable<Message> Messages();
    IObservable<List<Message>> MessagesGetAll();

    Message AddMessage(ReceivedMessage message);

    Task<IObservable<Message>> MessagesAsync();

   Task AddTestMessagesAsync();
}

public class Chat : IChat
{
    private readonly ISubject<Message> _messageStream = new ReplaySubject<Message>(1);
    private readonly ISubject<List<Message>> _allMessageStream = new ReplaySubject<List<Message>>(1);

    // logger
    private readonly ILogger<Chat> _logger;

    public Chat(ILogger<Chat> logger)
    {
        _logger = logger;
        AllMessages = new ConcurrentStack<Message>();
        Users = new ConcurrentDictionary<string, string>
        {
            ["1"] = "developer",
            ["2"] = "tester"
        };
    }

    // テストメッセージのためのカウンタ
    private int testMessageCounter = 0;

    // メッセージを定期的に追加するためのメソッド
    public async Task AddTestMessagesAsync()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(5)); // 5秒ごとにメッセージを追加

            var testMessage = new Message
            {
                Content = $"Test Message {testMessageCounter++}",
                SentAt = DateTime.UtcNow,
                From = new MessageFrom
                {
                    DisplayName = "Test User",
                    Id = "0"
                }
            };

            AddMessage(testMessage);
            _logger.LogInformation($"Test Message Added: {testMessage.Content}");
        }
    }

    public ConcurrentDictionary<string, string> Users { get; set; }

    public ConcurrentStack<Message> AllMessages { get; }

    public Message AddMessage(ReceivedMessage message)
    {
        if (!Users.TryGetValue(message.FromId, out string displayName))
        {
            displayName = "(unknown)";
        }

        return AddMessage(new Message
        {
            Content = message.Content,
            SentAt = message.SentAt,
            From = new MessageFrom
            {
                DisplayName = displayName,
                Id = message.FromId
            }
        });
    }

    public async Task<IObservable<Message>> MessagesAsync()
    {
        //pretend we are doing something async here
        await Task.Delay(100).ConfigureAwait(false);
        return Messages();
    }

    public List<Message> AddMessageGetAll(Message message)
    {
        AllMessages.Push(message);
        var l = new List<Message>(AllMessages);
        _allMessageStream.OnNext(l);
        return l;
    }

    public Message AddMessage(Message message)
    {
        AllMessages.Push(message);
        _messageStream.OnNext(message);
        return message;
    }

    public IObservable<Message> Messages()
    {
        return _messageStream.AsObservable();
    }

    public IObservable<List<Message>> MessagesGetAll()
    {
        return _allMessageStream.AsObservable();
    }

    public void AddError(Exception exception)
    {
        _messageStream.OnError(exception);
    }
}
