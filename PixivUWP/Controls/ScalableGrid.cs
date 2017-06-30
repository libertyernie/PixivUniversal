using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace PixivUWP.Controls
{
    /// <summary>
    /// 引用自https://raw.githubusercontent.com/manupstairs/UWPSamples/master/UWPSamples/PhotosBrowser/ScalableGrid.cs的对象
    /// </summary>
    public class ScalableGrid : Grid
    {
        private TransformGroup transformGroup;
        private ScaleTransform scaleTransform;
        private TranslateTransform translateTransform;

        public ScalableGrid()
        {
            this.scaleTransform = new ScaleTransform();
            this.translateTransform = new TranslateTransform();
            this.transformGroup = new TransformGroup();
            this.transformGroup.Children.Add(scaleTransform);
            this.transformGroup.Children.Add(translateTransform);
            this.RenderTransform = transformGroup;

            this.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
            this.ManipulationDelta += ScalableGrid_ManipulationDelta;
            this.Loaded += ScalableGrid_Loaded;
            this.SizeChanged += (a, b) =>
            {
                this.scaleTransform.CenterX = this.ActualWidth / 2;
                this.scaleTransform.CenterY = this.ActualHeight / 2;
            };
            this.DoubleTapped += ScalableGrid_DoubleTapped;
        }

        private void ScalableGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
            this.translateTransform.X = 0;
            this.translateTransform.Y = 0;
            this.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
        }

        private void ScalableGrid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Loaded -= ScalableGrid_Loaded;
            scaleTransform.CenterX = this.ActualWidth / 2;
            scaleTransform.CenterY = this.ActualHeight / 2;
        }

        private void ScalableGrid_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
            => zoom(e.Delta.Scale, e.Delta.Translation.X, e.Delta.Translation.Y);

        private void zoom(double size,double x,double y)
        {
            if (scaleTransform.ScaleX == 1 && scaleTransform.ScaleY == 1)
            {
                this.ManipulationMode = ManipulationModes.System | ManipulationModes.Scale;
            }
            else
            {
                this.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale | ManipulationModes.TranslateInertia;
            }

            scaleTransform.ScaleX *= size;
            scaleTransform.ScaleY *= size;
            if (scaleTransform.ScaleY < 1)
            {
                scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
            }

            translateTransform.X += x;
            translateTransform.Y += y;
            StopWhenTranslateToEdge();
        }

        public void ZoomIn()
            => zoom(1.25, 0, 0);

        public void ZoomOut()
            => zoom(0.8, 0, 0);

        private void StopWhenTranslateToEdge()
        {
            double width = this.ActualWidth * (scaleTransform.ScaleX - 1) / 2;
            if (translateTransform.X > 0)
            {
                if (width < translateTransform.X)
                {
                    translateTransform.X = width;
                }
            }
            else if (translateTransform.X < 0)
            {
                if (-width > translateTransform.X)
                {
                    translateTransform.X = -width;
                }
            }

            double height = this.ActualHeight * (scaleTransform.ScaleY - 1) / 2;
            if (height < translateTransform.Y)
            {
                translateTransform.Y = height;
            }
            else if (translateTransform.Y < 0)
            {
                if (-height > translateTransform.Y)
                {
                    translateTransform.Y = -height;
                }
            }
        }

        private ManipulationModes SetCurrentMainpulationModes()
        {
            if (scaleTransform.ScaleX == 1 && scaleTransform.ScaleY == 1)
            {
                this.translateTransform.X = 0;
                this.translateTransform.Y = 0;
                return ManipulationModes.System | ManipulationModes.Scale;
            }

            var modes = ManipulationModes.Scale;
            var parentElement = this.Parent as FrameworkElement;
            if (parentElement == null)
            {
                throw new NotImplementedException("ScalableGrid should be child of one FrameworkElement");
            }

            var position = this.TransformToVisual(parentElement).TransformPoint(new Point());
            if (position.Y <= 0 || position.Y + this.ActualHeight >= parentElement.ActualHeight)
            {
                modes = modes | ManipulationModes.TranslateY;
            }

            if (position.X <= 0 || position.X + this.ActualWidth >= parentElement.ActualWidth)
            {
                modes = modes | ManipulationModes.TranslateX;
            }

            return modes;
        }
    }
}
