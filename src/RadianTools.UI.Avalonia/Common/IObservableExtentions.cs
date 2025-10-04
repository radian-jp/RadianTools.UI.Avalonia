namespace RadianTools.UI.Avalonia.Common;

public static class IObservableExtentions
{
    private class ObserverWrapper<T> : IObserver<T>
    {
        private readonly Action<T> _onNext;

        public ObserverWrapper(Action<T> onNext)
        {
            _onNext = onNext;
        }

        public void OnNext(T value) => _onNext(value);
        public void OnError(Exception error) { }
        public void OnCompleted() { }
    }

    public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> action)
    {
        return observable.Subscribe(new ObserverWrapper<T>(action));
    }
}
