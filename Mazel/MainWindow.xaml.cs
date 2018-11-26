using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;
using System.Windows.Threading;

namespace Mazel
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Maze mainMaze;

        public MainWindow()
        {
            InitializeComponent();

            mainMaze = new Maze(new ArrayPoint2D(10, 10), new ArrayPoint2D(0, 0), new ArrayPoint2D(9, 10));
            
            Prepare();
            ShowMaze();
        }

        public void Prepare()
        {
            // CLEAR ALL ENTITIES IN GRID //
            MazeGrid.Children.Clear();
            MazeGrid.RowDefinitions.Clear();
            MazeGrid.ColumnDefinitions.Clear();
            //

            // MAKE DEFINITIONS //
            MazeGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MazeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < mainMaze.GetSize().r; i++)
            {
                MazeGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
                MazeGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < mainMaze.GetSize().c; i++)
            {
                MazeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                MazeGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // DRAW DOTS //
            for (int i = 1; i < mainMaze.GetSize().r; i++)
            {
                for (int j = 1; j < mainMaze.GetSize().c; j++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Fill = Brushes.Black,
                        StrokeThickness = 0
                    };

                    MazeGrid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i * 2);
                    Grid.SetColumn(rectangle, j * 2);
                }
            }

            // DRAW 4 BIG WALLS
            Rectangle leftWall = new Rectangle { Fill = Brushes.Black, StrokeThickness = 0 };
            Rectangle rightWall = new Rectangle { Fill = Brushes.Black, StrokeThickness = 0 };
            Rectangle upWall = new Rectangle { Fill = Brushes.Black, StrokeThickness = 0 };
            Rectangle downWall = new Rectangle { Fill = Brushes.Black, StrokeThickness = 0 };

            MazeGrid.Children.Add(leftWall);
            Grid.SetRow(leftWall, 0);
            Grid.SetColumn(leftWall, 0);
            Grid.SetRowSpan(leftWall, mainMaze.GetSize().r * 2 + 1);

            MazeGrid.Children.Add(rightWall);
            Grid.SetRow(rightWall, 0);
            Grid.SetColumn(rightWall, mainMaze.GetSize().r * 2);
            Grid.SetRowSpan(rightWall, mainMaze.GetSize().r * 2 + 1);

            MazeGrid.Children.Add(upWall);
            Grid.SetRow(upWall, 0);
            Grid.SetColumn(upWall, 0);
            Grid.SetColumnSpan(upWall, mainMaze.GetSize().c * 2 + 1);

            MazeGrid.Children.Add(downWall);
            Grid.SetRow(downWall, mainMaze.GetSize().c * 2);
            Grid.SetColumn(downWall, 0);
            Grid.SetColumnSpan(downWall, mainMaze.GetSize().c * 2 + 1);
        }

        public void ShowMaze()
        {
            for (int i = 0; i < mainMaze.GetSize().r - 1; i++)
            {
                for (int j = 0; j < mainMaze.GetSize().c; j++)
                {
                    if (mainMaze.HolWalls[i][j])
                    {
                        Rectangle rectangle = new Rectangle
                        {
                            Fill = Brushes.Black,
                            StrokeThickness = 0
                        };

                        MazeGrid.Children.Add(rectangle);
                        Grid.SetRow(rectangle, i * 2 + 2);
                        Grid.SetColumn(rectangle, j * 2 + 1);
                    }
                }
            }

            for (int i = 0; i < mainMaze.GetSize().r; i++)
            {
                for (int j = 0; j < mainMaze.GetSize().c - 1; j++)
                {
                    if (mainMaze.VerWalls[i][j])
                    {
                        Rectangle rectangle = new Rectangle
                        {
                            Fill = Brushes.Black,
                            StrokeThickness = 0
                        };

                        MazeGrid.Children.Add(rectangle);
                        Grid.SetRow(rectangle, i * 2 + 1);
                        Grid.SetColumn(rectangle, j * 2 + 2);
                    }
                }
            }
        }

        private void MenuGenerateButton(object sender, RoutedEventArgs e)
        {
            ArrayPoint2D size = new ArrayPoint2D(int.Parse(RowSizeComboBox.SelectedItem.ToString()), int.Parse(ColSizeComboBox.SelectedItem.ToString()));

            mainMaze = new Maze(size, new ArrayPoint2D(0, 0), new ArrayPoint2D(9, 10));

            switch (GenerateAlgComboBox.SelectedIndex)
            {
                case 0:
                    MazeGenerator.RecursiveBacktracker(mainMaze);
                    break;

                case 1:
                    MazeGenerator.HuntAndKill(mainMaze);
                    break;

                default:
                    break;
            }
            
            
            Prepare();
            ShowMaze();
        }

        private void MenuSolveButton(object sender, RoutedEventArgs e)
        {

        }

        private void MenuGenerateAlgComboChange(object sender, SelectionChangedEventArgs e)
        {

        }
        
    }
}
