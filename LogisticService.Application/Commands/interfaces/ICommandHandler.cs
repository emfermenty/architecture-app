namespace LogisticService.Application.Commands.interfaces;

public interface ICommandHandler
{
    Task HandleAsync<T>(T command) where T : ICommand;
    Task UndoAsync();
    bool CanUndo { get; }
}