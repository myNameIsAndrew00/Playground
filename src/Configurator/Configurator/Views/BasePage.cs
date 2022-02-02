using Configurator.Animations;
using Configurator.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Configurator
{
    public class BasePage<ViewModel> : UserControl, ICanAnimate
       where ViewModel : class, new()
    {

        public AnimateSlideProperty.Animation AnimationOut { get; set; }
        public AnimateSlideProperty.Animation AnimationIn { get; set; }

        public BasePage()
        {
            DataContext = new ViewModel();
            initiliaseAnimations();
        }

        public BasePage(ViewModel Model)
        {
            DataContext = Model;
            initiliaseAnimations();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateIn();
        }

        public async void AnimateIn()
        {
            await AnimationIn();
        }

        public async void AnimateOut()
        {
            await AnimationOut();
        }


        #region Private

        private void initiliaseAnimations()
        {
            Loaded += BasePage_Loaded;

            AnimationIn = this.FadeIn;
            AnimationOut = this.FadeOut;
        }

        #endregion
    }
}
