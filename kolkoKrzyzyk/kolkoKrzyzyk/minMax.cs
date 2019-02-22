using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kolkoKrzyzyk
{
    public class minMax
    {
        /// <summary>
        ///  Obliczanie najlepszej pozycji AI do przemieszczania się po planszy w aktualnym stanie. 
        /// </summary>
        /// <param name="currentState">Aktualny stan planszy.</param>
        /// <param name="playerMark">playerMark (symbol gracza) do przesunięcia.</param>
        /// <param name="rowIndex">Indeks wiersza najlepszej pozycji do przeniesienia obliczony przez Min-Max-Pruning.</param>
        /// <param name="columnIndex">Indeks kolumny najlepszej pozycji do przeniesienia obliczony przez Min-Max-Pruning.</param>
        /// <remarks>
        /// "player" tutaj nie oznacza człowieka grającego w tę grę, oznacza tylko AI lub gracza tej gry.
        /// </remarks>
        public static void GetBestPosition(Board currentState, ChessPiece playerMark, out int rowIndex, out int columnIndex)
        {
            // przydzielenie początkowych ustawień.
            rowIndex = 0;
            columnIndex = 0;

            // Pobieranie symbolu przeciwnika.
            ChessPiece opponentMark = playerMark == ChessPiece.X ? ChessPiece.O : ChessPiece.X;

            // Inicjalizowanie zmiennej "max" z wartością "int.MiValue".
            int max = int.MinValue;

            // Iterowanie przez każdą pustą komórkę dla bieżącego gracza.
            for (int playerRowIndex = 0; playerRowIndex < Board.Size; playerRowIndex++)
            {
                for (int playerColumnIndex = 0; playerColumnIndex < Board.Size; playerColumnIndex++)
                {
                    if (currentState[playerRowIndex, playerColumnIndex] == ChessPiece.Blank)
                    {
                        // Wykonanie kopii bieżącego stanu.
                        Board nextStep = currentState.Copy();
                        nextStep.AddChessPiece(playerRowIndex, playerColumnIndex, playerMark);

                        // Inicjalizowanie zmiennej "min" z wartością "int.MaxValue".
                        int min = int.MaxValue;

                        // Iterowanie przez każdą pustą komórkę dla przeciwnika bieżącego gracza.
                        for (int opponentRowIndex = 0; opponentRowIndex < Board.Size; opponentRowIndex++)
                        {
                            for (int opponentColumnIndex = 0; opponentColumnIndex < Board.Size; opponentColumnIndex++)
                            {
                                if (nextStep[opponentRowIndex, opponentColumnIndex] == ChessPiece.Blank)
                                {
                                    //Wykonanie kopii bieżącego stanu i umieszczenie symbolu przeciwnika w pustej komórce kopii.
                                    Board nextNextStep = nextStep.Copy();
                                    nextNextStep.AddChessPiece(opponentRowIndex, opponentColumnIndex, opponentMark);

                                    // Ocenianie stanu według wyniku gracza i wyniku przeciwnika.
                                    int stateScore = PlayerScore(nextNextStep, playerMark) - OpponentScore(nextNextStep, opponentMark);

                                    // Śledzene minimalnej wartości.
                                    if (stateScore < min)
                                        min = stateScore;
                                }
                            }
                        }

                        // Jeśli min > max, then current (playerRowIndex, playerColumnIndex) is better than any other cell searched before.
                        if (min > max)
                        {
                            max = min;
                            rowIndex = playerRowIndex;
                            columnIndex = playerColumnIndex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obliczanie ile jest możliwych liń, po których zajęciu gracz wygrywa.
        /// </summary>
        /// <param name="state">Symulowany stan planszy.</param>
        /// <param name="playerMark">Symbol gracza.</param>
        /// <returns>Liczba lini, po których gracz może wygrać.</returns>

        private static int PlayerScore(Board state, ChessPiece playerMark)
        {
            Board filledBoard = FillBoard(state, playerMark);
            return CalculateScore(filledBoard, playerMark);
        }

        /// <summary>
        /// Obliczanie ile jest możliwych liń, po których zajęciu przeciwnik wygrywa.
        /// </summary>
        /// <param name="state">Symulowany stan planszy.</param>
        /// <param name="opponentMark">Symbol przeciwnika.</param>
        /// <returns>Liczba lini, po których przeciwnik może wygrać.</returns>
        private static int OpponentScore(Board state, ChessPiece opponentMark)
        {
            Board filledBoard = FillBoard(state, opponentMark);
            return CalculateScore(filledBoard, opponentMark);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="Board">The input (original) <see cref="Board" />.</param>
        /// <param name="mark">The chess piece mark to fill the blank cells.</param>
        /// <returns>A deep copy of the input <see cref="Board" /> will all blank cells are filled with the specified mark.</returns>
        private static Board FillBoard(Board Board, ChessPiece mark)
        {
            Board filledBoard = Board.Copy();
            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    if (filledBoard[i, j] == ChessPiece.Blank)
                        filledBoard.AddChessPiece(i, j, mark);
                }
            }

            return filledBoard;
        }

        /// <summary>
        /// Obliczanie ile możliwych lini może gracz jeszcze zaznaczyć by wygrać.
        /// </summary>
        /// <param name="filledBoard">Instancja planszy <see cref="Board" /> bez pustych komórek.</param>
        /// <param name="mark">Symbol gracza.</param>
        /// <returns>Liczba linii, po których zaznaczeniu gracz może wygrać.</returns>
        /// <remarks>
        /// "player" tutaj nie oznacza człowieka grającego w tę grę, oznacza tylko AI lub gracza tej gry.
        /// </remarks>
        private static int CalculateScore(Board filledBoard, ChessPiece mark)
        {
            // Inicjowanie zmiennej score, zwiększanie jej za każdym razem, gdy zostanie znaleziony możliwy wiersz.
            int score = 0;

            // Zmienna potwierdzanąca czy grać wygrał (true).
            bool add;

            // Wynik z wierszy.
            for (int i = 0; i < Board.Size; i++)
            {
                add = true;
                for (int j = 0; j < Board.Size; j++)
                {
                    if (filledBoard[i, j] != mark)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    score++;
            }

            // Wynik z kolumn.
            for (int j = 0; j < Board.Size; j++)
            {
                add = true;
                for (int i = 0; i < Board.Size; i++)
                {
                    if (filledBoard[i, j] != mark)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    score++;
            }

            // Główna przekątna.
            add = true;
            for (int i = 0; i < Board.Size; i++)
            {
                if (filledBoard[i, i] != mark)
                {
                    add = false;
                    break;
                }
            }
            if (add)
                score++;

            // Przekątna.
            add = true;
            for (int i = 0; i < Board.Size; i++)
            {
                if (filledBoard[i, Board.Size - 1 - i] != mark)
                {
                    add = false;
                    break;
                }
            }
            if (add)
                score++;

            return score;
        }
    }
}