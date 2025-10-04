using System.Windows.Input;

namespace RadianTools.UI.Avalonia.Common;

public class Command : ICommand
{
    private Action _ActionExecute;

    public Command(Action actionExecute) => _ActionExecute = actionExecute;

#pragma warning disable CS0067
    public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067
    public virtual void Execute(object? parameter) => _ActionExecute.Invoke();
    public virtual bool CanExecute(object? parameter) => true;
}