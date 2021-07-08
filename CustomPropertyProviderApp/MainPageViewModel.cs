using System;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml.Data;

namespace CustomPropertyProviderApp
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _guid = System.Guid.NewGuid().ToString();
        public string Guid
        {
            get => _guid;
            set
            {
                _guid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guid)));
            }
        }

        private ICommand _updateGuidCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand UpdateGuidCommand => _updateGuidCommand ?? (_updateGuidCommand = new DelegateCommand(ExecuteUpdateGuid));

        private void ExecuteUpdateGuid()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }

    public class CustomPropertyProviderMainPageViewModel : ICustomPropertyProvider, INotifyPropertyChanged
    {
        public Model Model { get; } = new Model { Guid = Guid.NewGuid().ToString() };

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomPropertyProviderMainPageViewModel()
        {
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        public ICustomProperty GetCustomProperty(string name)
        {
            if (name == nameof(CustomPropertyProviderApp.Model.Guid))
            {
                return new GuidProperty();
            }
            else if (name == nameof(CustomPropertyProviderApp.Model.UpdateGuidCommand))
            {
                return new UpdateGuidCommandProperty();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public ICustomProperty GetIndexedProperty(string name, Type type) => throw new NotSupportedException();

        public string GetStringRepresentation()
        {
            return "MainPageViewModel";
        }

        public Type Type => typeof(Model);
    }

    public class Model : INotifyPropertyChanged
    {
        private string _guid;
        public string Guid
        {
            get => _guid;
            set
            {
                _guid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Guid)));
            }
        }

        private ICommand _updateGuidCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand UpdateGuidCommand => _updateGuidCommand ?? (_updateGuidCommand = new DelegateCommand(ExecuteUpdateGuid));

        private void ExecuteUpdateGuid()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
    }

    public class UpdateGuidCommandProperty : ICustomProperty
    {
        public object GetValue(object target)
        {
            var vm = (CustomPropertyProviderMainPageViewModel)target;
            return vm.Model.UpdateGuidCommand;
        }

        public void SetValue(object target, object value)
        {
            throw new NotSupportedException();
        }

        public object GetIndexedValue(object target, object index)
        {
            throw new NotSupportedException();
        }

        public void SetIndexedValue(object target, object value, object index)
        {
            throw new NotSupportedException();
        }

        public bool CanRead => true;

        public bool CanWrite => false;

        public string Name => nameof(Model.UpdateGuidCommand);

        public Type Type => typeof(ICommand);
    }

    public class GuidProperty : ICustomProperty
    {
        public object GetValue(object target)
        {
            var vm = (CustomPropertyProviderMainPageViewModel)target;
            return vm.Model.Guid;
        }

        public void SetValue(object target, object value)
        {
            var vm = (CustomPropertyProviderMainPageViewModel)target;
            vm.Model.Guid = (string)value;
        }

        public object GetIndexedValue(object target, object index) => throw new NotSupportedException();

        public void SetIndexedValue(object target, object value, object index) => throw new NotSupportedException();

        public bool CanRead => true;

        public bool CanWrite => true;

        public string Name => nameof(Model.Guid);

        public Type Type => typeof(string);
    }
}
