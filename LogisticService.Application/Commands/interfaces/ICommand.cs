namespace LogisticService.Application.Commands.interfaces;

public interface ICommand
{
    Task ExecuteAsync();
    Task<bool> CanExecuteAsync();
    Task UndoAsync();
}