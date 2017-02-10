using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SnakeGame.GameObjects.Enums;
using SnakeGame.ViewModels;

namespace SnakeGame.Views
{
    /// <summary>
    /// Interaction logic for SnakeControl.xaml
    /// </summary>
    public partial class SnakeControl : UserControl
    {
        public SnakeControl()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var gameEngine = this.DataContext as GameEngine;
            gameEngine?.StartGame();
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            Directions direction = Directions.Right;
            switch (e.Key)
            {
                case Key.Left:
                    direction = Directions.Left;
                    break;
                case Key.Right:
                    direction = Directions.Right;
                    break;
                case Key.Up:
                    direction = Directions.Up;
                    break;
                case Key.Down:
                    direction = Directions.Down;
                    break;
            }
            var gameEngine = DataContext as GameEngine;
            gameEngine?.ChangeDirection(direction);
        }
    }
}
