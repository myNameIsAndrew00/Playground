using Configurator.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Configurator.Properties
{
    public class ShouldAnimateOutProperty : BaseProperty<ShouldAnimateOutProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var Control = (ContentControl)sender;
            (Control.Content as ICanAnimate)?.AnimateOut();
        }
    }
}
