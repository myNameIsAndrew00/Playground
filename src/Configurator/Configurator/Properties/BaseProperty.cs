using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Configurator.Properties
{
    /// <summary>   
    /// A base attached property to a xml tag
    /// </summary>
    /// <typeparam name="Parent"> The parent class to be attached to property</typeparam>
    /// <typeparam name="Property"> The type of property </typeparam>
    public class BaseProperty<Parent, Property>
        where Parent : new()
    {

        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };
        public event Action<DependencyObject, object> ValueUpdated = (sender, value) => { };

        public static Parent Instance { get; private set; } = new Parent();

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(Property),
            typeof(BaseProperty<Parent, Property>),
            new PropertyMetadata(default(Property),
                                new PropertyChangedCallback(OnValuePropertyChanged),
                                new CoerceValueCallback(OnValuePropertyUpdated)
                                ));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (Instance as BaseProperty<Parent, Property>)?.OnValueChanged(d, e);
            (Instance as BaseProperty<Parent, Property>)?.ValueChanged(d, e);
        }

        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            (Instance as BaseProperty<Parent, Property>)?.OnValueUpdated(d, value);
            (Instance as BaseProperty<Parent, Property>)?.ValueUpdated(d, value);

            return value;
        }

        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);


        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        public virtual void OnValueUpdated(DependencyObject sender, object value) { }
    }

}
