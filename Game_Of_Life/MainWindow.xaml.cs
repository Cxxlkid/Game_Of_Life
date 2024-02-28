using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Game_Of_Life.Logic;
using Game_Of_Life.Models;
using Game_Of_Life.Utils;

namespace Game_Of_Life
{
    public partial class MainWindow : Window
    {
        private int GridSize = SaveUtil.Load().X;
        private Rectangle[,] cells;
        private DispatcherTimer timer;
        private bool[,] cellStates;
        private GameLogic gameLogic;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGrid();
            InitializeTimer();
        }

        private void InitializeGameGrid()
        {
            double cellSize = (GridSize * 25) / GridSize; // Ajustez la taille de la grille principale (300) selon vos besoins

            GameGrid.Width = GridSize*25;
            GameGrid.Height = GridSize*25;
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < GridSize; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize) });
            }

            for (int j = 0; j < GridSize; j++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize) });
            }

            cells = new Rectangle[GridSize, GridSize];
            gameLogic = new GameLogic(GridSize);
            cellStates = new bool[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Rectangle cell = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Margin = new Thickness(0) // Aucune marge entre les cellules
                    };

                    cell.MouseLeftButtonDown += Cell_Click;
                    cell.Cursor = Cursors.Hand;
                    GameGrid.Children.Add(cell);
                    cells[i, j] = cell;
                    cellStates[i, j] = false;

                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                }
            }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.5);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Background = Brushes.LightGreen;
            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.Background = Brushes.LightGray;
            timer.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StopButton_Click(sender, e);
            StepIndicator.Text = "0";
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    cellStates[i, j] = false;
                    gameLogic.GetGrid()[i,j].IsAlive = false;
                    cells[i, j].Fill = Brushes.White;
                }
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Rectangle clickedCell = (Rectangle)sender;
            int row = Grid.GetRow(clickedCell);
            int col = Grid.GetColumn(clickedCell);

            // Mettre à jour l'état de la cellule dans la logique du jeu
            cellStates[row, col] = !cellStates[row, col];
            gameLogic.GetGrid()[row, col].IsAlive = cellStates[row, col];

            // Mettre à jour la couleur de la cellule visuelle
            clickedCell.Fill = cellStates[row, col] ? Brushes.Black : Brushes.White;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            gameLogic.CalculateNextGeneration();
            StepIndicator.Text = (int.Parse(StepIndicator.Text) + 1).ToString(); 
            UpdateVisualGrid();
        }

        private void UpdateVisualGrid()
        {
            Cell[,] grid = gameLogic.GetGrid();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Rectangle cell = cells[i, j];
                    cell.Fill = grid[i, j].IsAlive ? Brushes.Black : Brushes.White;
                }
            }
        }
        
        private void GenerateRandomGrid(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    cellStates[i, j] = random.Next(0, 2) == 1;
                    cells[i, j].Fill = cellStates[i, j] ? Brushes.Black : Brushes.White;
                    gameLogic.GetGrid()[i, j].IsAlive = cellStates[i, j];
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F9:
                    StopButton_Click(sender, e);
                    GenerateRandomGrid(sender, e);
                    break;
                case Key.Escape:
                    Exit(sender, e);
                    break;
                case Key.F2:
                    ChangeSize(sender, e);
                    break;
                case Key.F8:
                    InvertGrid(sender, e);
                    break;
            }
        }

        private void ChangeSize(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveGridSizeDialog();
            dialog.ShowDialog();
    
            StepIndicator.Text = "0";
        }

        private void InvertGrid(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    cellStates[i, j] = !cellStates[i, j];
                    cells[i, j].Fill = cellStates[i, j] ? Brushes.Black : Brushes.White;
                    gameLogic.GetGrid()[i, j].IsAlive = cellStates[i, j];
                }
            }
            
            StepIndicator.Text = "0";
        }
    }
}
