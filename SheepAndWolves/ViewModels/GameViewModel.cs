using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SheepAndWolves.Data;
using SheepAndWolves.Models;
using SheepAndWolves.ViewModels.Base;


namespace SheepAndWolves.ViewModels
{
    internal class GameViewModel : ViewModel
    {
        private FieldModel selectedField = null;
        public FieldModel SelectedField { get => selectedField; set => Set(ref selectedField, value); }

        private Color.Piece move = Color.Piece.Black;
        public Color.Piece Move { get => move; set => 
                Set(ref move, value); 
        }


        #region Field Observable Collection
        private ObservableCollection<ObservableCollection<FieldModel>> f = new();
        public ObservableCollection<ObservableCollection<FieldModel>> F { get => f; set => Set(ref f, value); }
        #endregion

        #region Constructor
        public GameViewModel()
        {
            StartGame();
        }
        #endregion

        #region Methods

        #region StartGame
        public void StartGame()
        {
            Move = Color.Piece.Black;
            GenerateField();
            StartingPosition();
        }
        #endregion

        #region SetPointCheck
        public void SetPointCheck(int checkposi, int checkposj)
        {
            if (CheckOnBoard(checkposi, checkposj))
            {
                if (SelectedField.PieceColor == Color.Piece.White)
                {
                    if (F[checkposi][checkposj].TexturePath == ImagesConstsPaths.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                }
                if (SelectedField.PieceColor == Color.Piece.Black)
                {
                    if (F[checkposi][checkposj].TexturePath == ImagesConstsPaths.Empty)
                    {
                        SetPoint(checkposi, checkposj);
                    }
                }
            }
        }

        #endregion

        #region Walk
        public void Walk()
        {
            int checkposi1 = SelectedField.I + 1;
            int checkposi2 = SelectedField.I - 1;

            int checkposj1 = SelectedField.J + 1;
            int checkposj2 = SelectedField.J - 1;

            SetPointCheck(checkposi2, checkposj2);
            SetPointCheck(checkposi2, checkposj1);
            SetPointCheck(checkposi1, checkposj2);
            SetPointCheck(checkposi1, checkposj1);


        }
        #endregion
        #region CheckOnBoard
        public static bool CheckOnBoard(int i, int j)
        {
            return i >= 0 && i < 8 && j >= 0 && j < 8;
        }
        #endregion

        #region Quit

        public static void Quit() { Application.Current.Shutdown(); }

        #endregion

        #region RestartGame

        public void RestartGame(string text)
        {
            MessageBoxResult mbr = MessageBox.Show($"{text} Начать заново?", "Конец игры", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes) StartGame();
            else Quit();
        }

        #endregion

        #region Win

        public void Win(Color.Piece color)
        {
            string colorName = (color == Color.Piece.White) ? "белые" : "черные";
            RestartGame($"Выйграли {colorName}.");
        }

        #endregion

        #region CheckWin
        public void CheckWin()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].PieceColor == Color.Piece.Black && F[i][j].I == 7)
                    {
                        Win(Color.Piece.Black);
                    }
                    if (F[i][j].PieceColor == Color.Piece.Black)
                    {
                        if ((!CheckOnBoard(i - 1, j - 1) || F[i - 1][j - 1].PieceColor != Color.Piece.Empty) && (!CheckOnBoard(i - 1, j + 1) || F[i - 1][j + 1].PieceColor != Color.Piece.Empty) &&
                            (!CheckOnBoard(i + 1, j - 1) || F[i + 1][j - 1].PieceColor != Color.Piece.Empty) && (!CheckOnBoard(i + 1, j + 1) || F[i + 1][j + 1].PieceColor != Color.Piece.Empty)) 
                        {
                            Win(Color.Piece.White);
                        }
                    }
                    

                }
            }
        }
        #endregion

        public void ReverseFigures(FieldModel field)
        {

            field.PieceColor = SelectedField.PieceColor;
            SelectedField.PieceColor = Color.Piece.Empty;
            SelectedField.Selected = false;
            SelectedField = null;
            ClearPoints();
            Move = Move == Color.Piece.White ? Color.Piece.Black : Color.Piece.White;
            CheckWin();
        }
        public void SetPoint(int i, int j)
        {
            if (((i >= 0 & i <= 7) & (j >= 0 & j <= 7)) && F[i][j].TexturePath == ImagesConstsPaths.Empty)
            {
                F[i][j].TexturePath = ImagesConstsPaths.Point;
            }
        }
        public void ClearPoints()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (F[i][j].TexturePath == ImagesConstsPaths.Point)
                    {
                        F[i][j].TexturePath = ImagesConstsPaths.Empty;
                    }
                }
            }
        }
        public void Click(FieldModel field)
        {
            if (field.TexturePath != ImagesConstsPaths.Empty && field.PieceColor == Move)
            {
                if (field.PieceColor == Move)
                {

                    if (SelectedField == null)
                    {
                        SelectedField = field;
                        SelectedField.Selected = true;
                        Walk();
                    }
                    else
                    {
                        if (field == SelectedField)
                        {
                            SelectedField.Selected = false;
                            SelectedField = null;
                            ClearPoints();
                            
                        }
                        else if (field != SelectedField)
                        {
                            SelectedField.Selected = false;
                            ClearPoints();
                            SelectedField = field;
                            SelectedField.Selected = true;
                            Walk();
                        }
                    }
                }
            }
            else
            {
                if (SelectedField != null)
                {
                    if (field.TexturePath == ImagesConstsPaths.Point)
                    {
                            ReverseFigures(field);
                    }
                }
            }
        }

        #region GenerateField
        public void GenerateField()
        {
            f.Clear();
            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<FieldModel> row = new();
                for (int j = 0; j < 8; j++)
                {
                    row.Add(new FieldModel() { I = i, J = j, BackgroundColor = (i + j) % 2 != 0 ? "#000000" : "#ffffff" });
                }
                f.Add(row);
            }
        }
        #endregion

        #endregion
        #region Starting position
        public void StartingPosition()
        {
            F[7][0].PieceColor = Color.Piece.White;
            F[7][2].PieceColor = Color.Piece.White;
            F[7][4].PieceColor = Color.Piece.White;
            F[7][6].PieceColor = Color.Piece.White;
            F[0][7].PieceColor = Color.Piece.Black;
        }

        #endregion
    }
}
