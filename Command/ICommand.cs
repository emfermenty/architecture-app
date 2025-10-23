namespace architectureProject.Command;

public interface ICommand
{
    Task ExecuteAsync();
    Task<bool> CanExecuteAsync();
    Task UndoAsync();
}