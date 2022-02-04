using Configurator.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Configurator.Properties
{
    /// <summary>
    /// A base class to run animation when a boolean is set to true or false ( In or Out )
    /// </summary>
    /// <typeparam name="Parent"></typeparam>


    public class AnimateBaseProperty<Parent> : BaseProperty<Parent, bool>
        where Parent : BaseProperty<Parent, bool>, new()
    {
        public bool FirstLoad { get; set; } = true;
        protected bool secureAnimation = true;

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            if (!(sender is FrameworkElement element)) return;
            if ((bool)sender.GetValue(ValueProperty) == (bool)value && !secureAnimation)
                return;

            secureAnimation = false;
            if (FirstLoad)
            {
                if (!(bool)value) element.Visibility = Visibility.Hidden;

                RoutedEventHandler onLoaded = null;
                onLoaded = async (ss, ee) =>
                {

                    element.Loaded -= onLoaded;
                    await Task.Delay(5);
                    DoAnimation(element, (bool)value);
                    FirstLoad = false;
                };
                element.Loaded += onLoaded;

            }
            else DoAnimation(element, (bool)value);
          
        }

        protected virtual void DoAnimation(FrameworkElement element, bool value) { }
    }

    public class AnimateSlideProperty : AnimateBaseProperty<AnimateSlideProperty>
    {
        public delegate Task Animation(float seconds = 0.25f, bool keepMargin = true, int width = 0);
        private static Animation AnimationOut;
        private static Animation AnimationIn;
        private float mSeconds = 0.7f;
        private bool mKeepMargin = false;
        private int mWidth;
        protected override async void DoAnimation(FrameworkElement element, bool value)
        {
            InitialiseAnimations(element, value);
            if (value)
            {
                await AnimationIn(FirstLoad ? 0 : mSeconds, mKeepMargin, mWidth);
                element.IsHitTestVisible = true;
            }
            else
            {
                await AnimationOut(FirstLoad ? 0 : mSeconds, mKeepMargin, mWidth);
                element.IsHitTestVisible = false;
            }

        }

        /// <summary>
        /// Initialise a page content container with an animation
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        private void InitialiseAnimations(FrameworkElement element, bool value)
        {
            mWidth = 0;
            mSeconds = 0.55f;


            AnimationIn = element.FadeIn;
            AnimationOut = element.FadeOut;
        }
    }
}
