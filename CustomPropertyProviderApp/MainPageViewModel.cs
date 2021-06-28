using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace CustomPropertyProviderApp
{
    public class MainPageViewModel
    {
        private string _guid = System.Guid.NewGuid().ToString();
        public string Guid => _guid;
    }

    public class CustomPropertyProviderMainPageViewModel : ICustomPropertyProvider
    {
        private Model _model = new Model { Guid = Guid.NewGuid().ToString() };
        public ICustomProperty GetCustomProperty(string name)
        {
            if (name == nameof(Model.Guid))
            {
                return new GuidProperty(_model);
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

    public class Model
    {
        public string Guid { get; set; }
    }

    public class GuidProperty : ICustomProperty
    {
        private readonly Model _model;

        public GuidProperty(Model model)
        {
            _model = model;
        }

        public object GetValue(object target)
        {
            return _model.Guid;
        }

        public void SetValue(object target, object value)
        {
            _model.Guid = (string)value;
        }

        public object GetIndexedValue(object target, object index) => throw new NotSupportedException();

        public void SetIndexedValue(object target, object value, object index) => throw new NotSupportedException();

        public bool CanRead => true;

        public bool CanWrite => true;

        public string Name => nameof(Model.Guid);

        public Type Type => typeof(string);
    }
}
