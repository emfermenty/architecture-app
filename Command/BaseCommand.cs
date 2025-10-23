namespace architectureProject.Command;

public abstract class BaseCommand : ICommand
{
    protected readonly ILogger<BaseCommand> _logger;

    protected BaseCommand(ILogger<BaseCommand> logger)
    {
        _logger = logger;
    }

    public abstract Task ExecuteAsync();
    public abstract Task<bool> CanExecuteAsync();
    public abstract Task UndoAsync();

    protected void LogExecution(string commandName)
    {
        _logger.LogInformation("Command {CommandName} executed at {Time}", 
            commandName, DateTime.UtcNow);
    }
}