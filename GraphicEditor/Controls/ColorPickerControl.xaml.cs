﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Controls
{
    public partial class ColorPickerControl : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPickerControl),
                new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public ColorPickerControl()
        {
            InitializeComponent();
            UpdateGradient();
        }

        private void UpdateGradient()
        {
            var hue = (float)HueSlider.Value;
            var color = ColorFromHsv(hue, 1, 1);
            SelectedHueStop.Color = color;
        }

        private void HueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateGradient();
        }

        private void ColorCanvas_MouseInteraction(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(ColorCanvas);
                var x = pos.X / ColorCanvas.ActualWidth;
                var y = pos.Y / ColorCanvas.ActualHeight;

                SelectedColor = ColorFromHsv(HueSlider.Value, x, 1 - y);
            }
        }

        private Color ColorFromHsv(double hue, double saturation, double value)
        {
            int hi = (int)(hue / 60) % 6;
            double f = hue / 60 - hi;

            double v = value * 255;
            double p = v * (1 - saturation);
            double q = v * (1 - f * saturation);
            double t = v * (1 - (1 - f) * saturation);

            return hi switch
            {
                0 => Color.FromArgb(255, (byte)v, (byte)t, (byte)p),
                1 => Color.FromArgb(255, (byte)q, (byte)v, (byte)p),
                2 => Color.FromArgb(255, (byte)p, (byte)v, (byte)t),
                3 => Color.FromArgb(255, (byte)p, (byte)q, (byte)v),
                4 => Color.FromArgb(255, (byte)t, (byte)p, (byte)v),
                _ => Color.FromArgb(255, (byte)v, (byte)p, (byte)q)
            };
        }

        private void ColorCanvas_MouseDown(object sender, MouseButtonEventArgs e) => ColorCanvas_MouseInteraction(sender, e);
        private void ColorCanvas_MouseMove(object sender, MouseEventArgs e) => ColorCanvas_MouseInteraction(sender, e);
        private void ColorCanvas_MouseUp(object sender, MouseButtonEventArgs e) => SelectedColor = SelectedColor; // Force update
    }
}