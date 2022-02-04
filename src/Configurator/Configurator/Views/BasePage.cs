using Configurator.Animations;
using Configurator.Properties;
using Configurator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Configurator
{
    public class BasePage : UserControl, ICanAnimate
    {
        public AnimateSlideProperty.Animation AnimationOut { get; set; }
        public AnimateSlideProperty.Animation AnimationIn { get; set; }

        public BasePage()
        {
            InitiliaseAnimations();
        }


        public async void AnimateIn()
        {
            await AnimationIn();
        }

        public async void AnimateOut()
        {
            await AnimationOut();
        }


        protected virtual void InitiliaseAnimations()
        {
            Loaded += BasePage_Loaded;

            AnimationIn = this.FadeIn;
            AnimationOut = this.FadeOut;
        }


        #region Private

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateIn();
        }

        #endregion
    }

    public class BasePage<ViewModel> : BasePage 
       where ViewModel : BaseViewModel, new()
    {

        public BasePage() : base()
        {
            DataContext = new ViewModel();
        }

        public BasePage(ViewModel Model)
        {
            DataContext = Model;
        }


    }
}
