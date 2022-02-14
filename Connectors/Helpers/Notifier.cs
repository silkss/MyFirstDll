using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Connectors
{
    /// <summary>
    /// Необходим для реализации всяких там модных MVVM в случае, если библиотека подключается к WPF проекту.
    /// </summary>
    public class Notifier : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(PropertyName);
            return true;
        }
    }
}
