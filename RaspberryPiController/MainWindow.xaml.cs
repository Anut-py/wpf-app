using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RaspberryPiController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double multiplier;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private double GetSensitivity()
        {
            return Sensitivity.Value;
        }

        private async void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    UpKey.Background = new SolidColorBrush(Color.FromRgb(225, 225, 255));
                    await BackendConnector.Move(GetSensitivity(), MovementType.Forward);
                    break;
                case Key.A:
                    LeftKey.Background = new SolidColorBrush(Color.FromRgb(225, 225, 255));
                    await BackendConnector.Turn(GetSensitivity(), TurnType.Left);
                    PositionDot.RenderTransform = new RotateTransform(90 - (await BackendConnector.GetPosition()).RotationDegrees);
                    break;
                case Key.S:
                    DownKey.Background = new SolidColorBrush(Color.FromRgb(225, 225, 255));
                    await BackendConnector.Move(GetSensitivity(), MovementType.Backward);
                    break;
                case Key.D:
                    RightKey.Background = new SolidColorBrush(Color.FromRgb(225, 225, 255));
                    await BackendConnector.Turn(GetSensitivity(), TurnType.Right);
                    PositionDot.RenderTransform = new RotateTransform(90 - (await BackendConnector.GetPosition()).RotationDegrees);
                    break;
                case Key.Space:
                    if (e.IsRepeat) break;
                    SpaceKey.Background = new SolidColorBrush(Color.FromRgb(225, 225, 255));
                    await BackendConnector.Blink();
                    break;
            }

            var pos = await BackendConnector.GetPosition();
            PositionText.Text =
                $"Position (x: {pos.XPos}, y: {pos.YPos}, rotation: {pos.RotationDegrees})";

            var t = new Thickness((150 + pos.XPos) / multiplier, (110 - pos.YPos) / multiplier, 0, 0);
            if (t.Left >= 300 || t.Left <= 0 || t.Top <= 0 || t.Top >= 220) PositionDot.Text = "";
            else PositionDot.Text = "^";
            PositionDot.Margin = t;
            PositionDot.FontSize = 10 / multiplier;
            e.Handled = true;
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            UpKey.Background = new SolidColorBrush(Colors.White);
            LeftKey.Background = new SolidColorBrush(Colors.White);
            RightKey.Background = new SolidColorBrush(Colors.White);
            DownKey.Background = new SolidColorBrush(Colors.White);
            SpaceKey.Background = new SolidColorBrush(Colors.White);
            e.Handled = true;
        }

        private void Sensitivity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SensitivityLabel.Text = "Sensitivity (" + e.NewValue + ")";
        }

        private async void MainWindow_OnInitialized(object? sender, EventArgs e)
        {
            await BackendConnector.Blink();
            await BackendConnector.Blink();
            await BackendConnector.Blink();
            var pos = await BackendConnector.GetPosition();
            multiplier = 1;
            PositionText.Text =
                $"Position (x: {pos.XPos}, y: {pos.YPos}, rotation: {pos.RotationDegrees})";
            var t = new Thickness(150 + pos.XPos, 110 - pos.YPos, 0, 0);
            if (t.Left >= 300 || t.Left <= 0 || t.Top <= 0 || t.Top >= 220) PositionDot.Text = "";
            else PositionDot.Text = "^";
            PositionDot.Margin = t;
            PositionDot.RenderTransform = new RotateTransform(90 - (await BackendConnector.GetPosition()).RotationDegrees);
        }

        private async void Zoom_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var pos = await BackendConnector.GetPosition();
            multiplier = 1 / e.NewValue;
            var t = new Thickness((150 + pos.XPos) / multiplier, (110 - pos.YPos) / multiplier, 0, 0);
            if (t.Left >= 300 || t.Left <= 0 || t.Top <= 0 || t.Top >= 220) PositionDot.Text = "";
            else PositionDot.Text = "^";
            PositionDot.Margin = t;
            PositionDot.FontSize = 10 / multiplier;
        }
    }
}