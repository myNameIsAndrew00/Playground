using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Configurator.Animations
{
    public static class FrameworkElementExtensions
    {
        public static async Task SlideAndFadeInFromRight(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideFromRight(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeIn(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeInFromLeft(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideFromLeft(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeIn(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeInFromBottom(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideFromBottom(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeIn(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeInFromTop(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideFromTop(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeIn(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeOutToRight(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideToRight(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeOut(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeOutToLeft(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideToLeft(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeOut(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;

            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeOutToBottom(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideToBottom(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeOut(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;

            await Task.Delay((int)seconds * 1000);
        }

        public static async Task SlideAndFadeOutToTop(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            var mActualWidth = page.ActualWidth == 0 ? page.Width : page.ActualWidth;
            sb.AddSlideToTop(seconds, width == 0 ? mActualWidth : width, keepMargin: keepMargin);
            sb.AddFadeOut(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;

            await Task.Delay((int)seconds * 1000);
        }

        public static async Task FadeIn(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            sb.AddFadeIn(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }

        public static async Task FadeOut(this FrameworkElement page, float seconds = 0.3f, bool keepMargin = true, int width = 0)
        {
            var sb = new Storyboard();
            sb.AddFadeOut(seconds);
            sb.Begin(page);
            page.Visibility = System.Windows.Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }
    }
}

