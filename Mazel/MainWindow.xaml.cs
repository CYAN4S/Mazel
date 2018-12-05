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
using System.Threading;

namespace Mazel
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Maze mainMaze;

        List<List<Rectangle>> CellRects;
        List<List<Rectangle>> HolWallsRects;
        List<List<Rectangle>> VerWallsRects;

        public static int delayTime = 0;

        public MainWindow()
        {
            InitializeComponent();

            mainMaze = new Maze(new ArrayPoint2D(5, 5), new ArrayPoint2D(0, 0), new ArrayPoint2D(9, 10));

            Prepare();
            ShowMaze();
        }

        public void Prepare()
        {
            // CLEAR ALL ENTITIES IN GRID //
            MazeGrid.Children.Clear();
            MazeGrid.RowDefinitions.Clear();
            MazeGrid.ColumnDefinitions.Clear();

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

            // DRAW 4 BIG WALLS //
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
            Grid.SetColumn(rightWall, mainMaze.GetSize().c * 2);
            Grid.SetRowSpan(rightWall, mainMaze.GetSize().r * 2 + 1);

            MazeGrid.Children.Add(upWall);
            Grid.SetRow(upWall, 0);
            Grid.SetColumn(upWall, 0);
            Grid.SetColumnSpan(upWall, mainMaze.GetSize().c * 2 + 1);

            MazeGrid.Children.Add(downWall);
            Grid.SetRow(downWall, mainMaze.GetSize().r * 2);
            Grid.SetColumn(downWall, 0);
            Grid.SetColumnSpan(downWall, mainMaze.GetSize().c * 2 + 1);

            // DRAW HORIZONTAL WALLS //
            HolWallsRects = new List<List<Rectangle>>();
            for (int i = 0; i < mainMaze.GetSize().r - 1; i++)
            {
                HolWallsRects.Add(new List<Rectangle>());
                for (int j = 0; j < mainMaze.GetSize().c; j++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Fill = Brushes.Black,
                        StrokeThickness = 0
                    };
                    HolWallsRects[i].Add(rectangle);
                    MazeGrid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i * 2 + 2);
                    Grid.SetColumn(rectangle, j * 2 + 1);
                }
            }

            // DRAW VERTICAL WALLS //
            VerWallsRects = new List<List<Rectangle>>();
            for (int i = 0; i < mainMaze.GetSize().r; i++)
            {
                VerWallsRects.Add(new List<Rectangle>());
                for (int j = 0; j < mainMaze.GetSize().c - 1; j++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Fill = Brushes.Black,
                        StrokeThickness = 0
                    };
                    VerWallsRects[i].Add(rectangle);
                    MazeGrid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i * 2 + 1);
                    Grid.SetColumn(rectangle, j * 2 + 2);
                }
            }

            CellRects = new List<List<Rectangle>>();
            for (int i = 0; i < mainMaze.GetSize().r; i++)
            {
                CellRects.Add(new List<Rectangle>());
                for (int j = 0; j < mainMaze.GetSize().c; j++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Fill = Brushes.White,
                        StrokeThickness = 0
                    };
                    CellRects[i].Add(rectangle);
                    MazeGrid.Children.Add(rectangle);
                    Grid.SetRow(rectangle, i * 2 + 1);
                    Grid.SetColumn(rectangle, j * 2 + 1);
                }
            }

            return;
        }

        public void ShowMaze()
        {
            for (int i = 0; i < mainMaze.GetSize().r - 1; i++)
            {
                for (int j = 0; j < mainMaze.GetSize().c; j++)
                {
                    HolWallsRects[i][j].Fill = mainMaze.HolWalls[i][j] ? Brushes.Black : Brushes.White;
                }
            }

            for (int i = 0; i < mainMaze.GetSize().r; i++)
            {
                for (int j = 0; j < mainMaze.GetSize().c - 1; j++)
                {
                    VerWallsRects[i][j].Fill = mainMaze.VerWalls[i][j] ? Brushes.Black : Brushes.White;
                }
            }

            for (int i = 0; i < mainMaze.GetSize().r; i++)
            {
                for (int j = 0; j < mainMaze.GetSize().c; j++)
                {
                    switch (mainMaze.Cells[i][j])
                    {
                        case 0:
                            CellRects[i][j].Fill = Brushes.White;
                            break;

                        case 4:
                            CellRects[i][j].Fill = Brushes.Aqua;
                            break;

                        case 5:
                            CellRects[i][j].Fill = Brushes.Pink;
                            break;

                        default:
                            CellRects[i][j].Fill = null;
                            break;
                    }
                }
            }
            Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
        }

        public void Delay()
        {
            Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
            Thread.Sleep(delayTime);
        }

        #region NEW EXCEPTION
        class InvaildDelayTimeException : Exception
        {
            public InvaildDelayTimeException(string message) : base(message) { }
        }

        class InvaildMazeSizeException : Exception
        {
            public InvaildMazeSizeException(string msg) : base(msg) { }
        }
        #endregion

        private void MenuGenerateButton(object sender, RoutedEventArgs e)
        {
            int inputRow = 0, inputCol = 0;

            #region EXCEPTION
            try
            {
                delayTime = int.Parse(DelayTimeTextBox.Text);
                if (delayTime < 0)
                {
                    throw new InvaildDelayTimeException("지연 시간은 0 이상이여야 합니다.");
                }
            }
            catch (FormatException)
            {
                MessageBoxResult result = MessageBox.Show("지연 시간 입력란에는 숫자만 입력할 수 있습니다.", "Wait...");
                return;
            }
            catch (InvaildDelayTimeException)
            {
                MessageBoxResult result = MessageBox.Show("지연 시간은 0 이상이여야 합니다.", "Wait...");
                return;
            }

            try
            {
                inputRow = int.Parse(RowInputTextBox.Text);
                inputCol = int.Parse(ColInputTextBox.Text);
                if (inputRow < 1 || inputCol < 1)
                {
                    throw new InvaildMazeSizeException("미로의 행과 열의 크기는 2 이상이여야 합니다.");
                }
            }
            catch (FormatException)
            {
                MessageBoxResult result = MessageBox.Show("미로의 크기 입력란에는 숫자만 입력할 수 있습니다.", "Wait...");
                return;
            }
            catch (InvaildMazeSizeException)
            {
                MessageBoxResult result = MessageBox.Show("미로의 행과 열의 크기는 2 이상이여야 합니다.", "Wait...");
                return;
            }
            #endregion

            ArrayPoint2D size = new ArrayPoint2D(inputRow, inputCol);
            mainMaze = new Maze(size, new ArrayPoint2D(0, 0), new ArrayPoint2D(9, 10));
            Prepare();

            switch (GenerateAlgComboBox.SelectedIndex)
            {
                case 0:
                    MazeGenerator.RecursiveBacktracker(mainMaze, ShowMaze);
                    break;

                case 1:
                    MazeGenerator.Kruskal(mainMaze, ShowMaze);
                    break;

                case 2:
                    MazeGenerator.HuntAndKill(mainMaze, ShowMaze);
                    break;

                default:
                    mainMaze.isMaze = false;
                    break;
            }

            mainMaze.isMaze = true;
            ShowMaze();
        }

        private void MenuSolveButton(object sender, RoutedEventArgs e)
        {
            int inputSR = 0, inputSC = 0, inputER = 0, inputEC = 0;

            #region EXCEPTION
            if (!mainMaze.isMaze)
            {
                MessageBoxResult result = MessageBox.Show("미로를 먼저 생성해주세요.", "Wait...");
                return;
            }

            try
            {
                inputSR = int.Parse(SRTextBox.Text); inputSC = int.Parse(SCTextBox.Text);
                inputER = int.Parse(ERTextBox.Text); inputEC = int.Parse(ECTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBoxResult result = MessageBox.Show("시작점/종료점 입력란에는 숫자만 입력할 수 있습니다.", "Wait...");
                return;
            }
            #endregion

            switch (SolveAlgComboBox.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    break;

                default:
                    break;
            }
        }

        private void SaveMazeButton(object sender, RoutedEventArgs e)
        {
            if (!mainMaze.isMaze)
            {
                MessageBoxResult result = MessageBox.Show("미로를 먼저 생성해주세요.", "Wait...");
                return;
            }
            MazeConverter.Save(mainMaze);
        }

        private void OpenMazeButton(object sender, RoutedEventArgs e)
        {
            if (MazeConverter.Open(ref mainMaze))
            {
                Prepare();
                ShowMaze();
                mainMaze.isMaze = true;
            }
        }

        private void BMPExportButton(object sender, RoutedEventArgs e)
        {

        }

        private void SVGExportButton(object sender, RoutedEventArgs e)
        {

        }

        private void OpenLogButton(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("test.log");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBoxResult result = MessageBox.Show("test.log 파일이 존재하지 않습니다.", "Wait...");
                return;
            }
        }

        private void HelpButton(object sender, RoutedEventArgs e)
        {

        }

        private void MenuPlayButton(object sender, RoutedEventArgs e)
        {
            if (MazeGame.isRunning)
            {
                MazeGame.isRunning = false;

                for (int i = 0; i < mainMaze.GetSize().r; i++)
                {
                    for (int j = 0; j < mainMaze.GetSize().c; j++)
                    {
                        mainMaze.Cells[i][j] = 0;
                    }
                }
                ShowMaze();

                return;
            }

            int inputSR = 0, inputSC = 0, inputER = 0, inputEC = 0;

            #region EXCEPTION
            if (!mainMaze.isMaze)
            {
                MessageBoxResult result = MessageBox.Show("미로를 먼저 생성해주세요.", "Wait...");
                return;
            }
            try
            {
                inputSR = int.Parse(SRTextBox.Text); inputSC = int.Parse(SCTextBox.Text);
                inputER = int.Parse(ERTextBox.Text); inputEC = int.Parse(ECTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBoxResult result = MessageBox.Show("시작점/종료점 입력란에는 숫자만 입력할 수 있습니다.", "Wait...");
                return;
            }
            #endregion

            MazeGame.isRunning = true;

            MazeGame.playerPos.r = inputSR;
            MazeGame.playerPos.c = inputSC;

            mainMaze.Cells[inputSR][inputSC] = 4;
            mainMaze.Cells[inputER][inputEC] = 5;

            ShowMaze();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (MazeGame.MovePlayer(mainMaze, e.Key) && MazeGame.isRunning)
            {
                ShowMaze();
            }
        }
    }
}
