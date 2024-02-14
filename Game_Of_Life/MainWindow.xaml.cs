﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Game_Of_Life.Logic;
using Game_Of_Life.Models;

namespace Game_Of_Life
{
    public partial class MainWindow : Window
    {
        private int GridSize = 10; // Set your initial grid size
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
            timer.Interval = TimeSpan.FromSeconds(2); // Set your desired interval
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    cellStates[i, j] = false;
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
    }
}
