using LogisticService.Application.Commands.interfaces;

namespace LogisticService.Application.Commands;

public class CommandHandler : ICommandHandler
{
    private readonly Stack<ICommand> _executedCommands = new();

    public async Task HandleAsync<T>(T command) where T : ICommand
    {
        try
        {
            if (await command.CanExecuteAsync())
            {
                await command.ExecuteAsync();
                _executedCommands.Push(command);
                Console.WriteLine($"Command executed: {command.GetType().Name}");
            }
            else
            {
                Console.WriteLine($"Command cannot be executed: {command.GetType().Name}");
                throw new InvalidOperationException("Command validation failed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing command {command.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    public async Task UndoAsync()
    {
        if (_executedCommands.Count > 0)
        {
            var command = _executedCommands.Pop();
            await command.UndoAsync();
            Console.WriteLine($"Command undone: {command.GetType().Name}");
        }
    }

    public virtual bool CanUndo => _executedCommands.Count > 0;
}