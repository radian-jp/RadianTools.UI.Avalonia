using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RadianTools.UI.Avalonia.Utils;

public class AsyncRelayCommand<T> : ICommand
{
    private readonly Func<T, Task> _execute;
    private readonly Predicate<T>? _canExecute;

    public AsyncRelayCommand(Func<T, Task> execute, Predicate<T>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        if (_canExecute == null) return true;
        return parameter is T t && _canExecute(t);
    }

    public async void Execute(object? parameter)
    {
        if (parameter is T t)
            await _execute(t);
    }

    public event EventHandler? CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
