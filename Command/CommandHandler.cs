﻿using architectureProject.Command;

public class CommandHandler
{
    private readonly Stack<ICommand> _executedCommands = new();

    public async Task HandleAsync(ICommand command)
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

    public bool CanUndo => _executedCommands.Count > 0;
}