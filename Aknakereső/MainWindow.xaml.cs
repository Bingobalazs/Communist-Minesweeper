// MainWindow.xaml.cs

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        private const int Rows = 8;
        private const int Columns = 10;
        private const int Mines = 10;

        private Button[,] gridButtons;
        private bool[,] mineGrid;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            mineGrid = GenerateMineGrid();
            CreateGridButtons();
            backgroundMusic.Open(new Uri(String.Format("{0}\\redalert.mp3", AppDomain.CurrentDomain.BaseDirectory)));
            backgroundMusic.Play();
        }

        private bool[,] GenerateMineGrid()
        {
            Random random = new Random();
            bool[,] grid = new bool[Rows, Columns];

            for (int i = 0; i < Mines; i++)
            {
                int row, col;

                do
                {
                    row = random.Next(0, Rows);
                    col = random.Next(0, Columns);
                } while (grid[row, col]);

                grid[row, col] = true;
            }

            return grid;
        }

        private void CreateGridButtons()
        {
            gridButtons = new Button[Rows, Columns];
            var bc = new BrushConverter();


            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Button button = new Button
                    {
                        Width = 40,
                        Height = 40,
                        Margin = new Thickness(2),
                        Tag = new Tuple<int, int>(i, j),
                        Background = (Brush)bc.ConvertFrom("#aa1921"),
                        Foreground = (Brush)bc.ConvertFrom("#fccb39"),
                        BorderBrush = null,
                        FontFamily = new FontFamily("Soviet"),
                        FontSize = 30,

                    };

                    button.Click += Button_Click;
                    button.MouseRightButtonDown += Button_Flag;
                    gridButtons[i, j] = button;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    MinefieldGrid.Children.Add(button);
                }
            }
        }
        private void Button_Flag(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int, int> coordinates = (Tuple<int, int>)button.Tag;
            int row = coordinates.Item1;
            int col = coordinates.Item2;

            if (button.Content == null)
            {
                button.Content = new Image
                {
                    Source = new BitmapImage(new Uri("/flag1.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Stretch = Stretch.Fill,
                    Height = 25,
                    Width = 25

                };
            }
            else if ((button.Content).GetType() == typeof(System.Windows.Controls.Image))
            {
                button.Content = null;
                button.ToolTip = null;
            }

            /* new Image
        {
            Source = new BitmapImage(new Uri("/WpfApplication1;component/image/add.jpg", UriKind.Relative)),
            VerticalAlignment = VerticalAlignment.Center,
            Stretch = Stretch.Fill,
            Height = 256,
            Width = 256
        }; */
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tuple<int, int> coordinates = (Tuple<int, int>)button.Tag;
            int row = coordinates.Item1;
            int col = coordinates.Item2;
            var bc = new BrushConverter();


            if (mineGrid[row, col])
            {
                MessageBox.Show("Észrevett a kulák és felrobbantott... \nJáték vége!", "GAME OVER", MessageBoxButton.OK, MessageBoxImage.Error);
                InitializeGame();
            }
            else
            {

                int adjacentMines = CountAdjacentMines(row, col);
                if (CountAdjacentMines(row, col) == 0)
                {
                    button.Content = " ";
                    button.Background = (Brush)bc.ConvertFrom("#1d1d1b");

                    
                }
                else
                    button.Content = adjacentMines.ToString();
            }
            CountAdjacentMines2();

        }
    
        

        private int CountAdjacentMines(int row, int col)
        {
            var bc = new BrushConverter();
            int count = 0;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < Rows && j >= 0 && j < Columns && mineGrid[i, j])
                    {
                        count++;
                    }
                }
            }
            if (count == 0)
            {
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = col - 1; j <= col + 1; j++)
                    {
                        if (i >= 0 && i < Rows && j >= 0 && j < Columns)
                        {



                            if (CountAdjacentMines3(i, j) == 0)
                            {
                                gridButtons[i, j].Content = " ";
                                gridButtons[i, j].Background = (Brush)bc.ConvertFrom("#1d1d1b");
                            }

                            else
                                gridButtons[i, j].Content = CountAdjacentMines3(i, j);
                        }
                    }
                }
            }


            return count;
        }
        private void CountAdjacentMines2()
        {

            for (int i = 0; i <= Rows; i++)
            {
                for (int j = 0; j <= Columns; j++)
                {
                    if (i >= 0 && i < Rows && j >= 0 && j < Columns && Convert.ToString( gridButtons[i, j].Content )== " ")
                    {
                        CountAdjacentMines(i, j);
                    }
                }
            }
        }
        private int CountAdjacentMines3(int row, int col)
        {


            int count = 0;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < Rows && j >= 0 && j < Columns && mineGrid[i, j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private MediaPlayer backgroundMusic = new MediaPlayer();

    }
}