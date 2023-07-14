using Chat;

public class TestMessageService : IHostedService
{
    private readonly IChat _chat;

    //logger
    private readonly ILogger<TestMessageService> _logger;

    public TestMessageService(IChat chat, ILogger<TestMessageService> logger)
    {
        _chat = chat;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TestMessageService is starting.");
        Task.Run(() => _chat.AddTestMessagesAsync());

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}