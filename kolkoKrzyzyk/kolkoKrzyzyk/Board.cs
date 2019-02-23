using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kolkoKrzyzyk
{
    /// <summary>
    ///Klasa cref="board" /> odpowiadająca za planszę gry kółko i krzyżyk.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Rozmiar szachownicy, który jest równy liczbie rzędów i liczbie kolumn szachownicy.
        /// </summary>
        public const int Size = 5;

        /// <summary>
        /// Tło planszy
        /// </summary>
        private readonly ChessPiece[,] board;

        /// <summary>
        /// Pobiera wartość wskazującą, czy ta board jest pełna niepustych .
        /// </summary>
        public bool IsFull
        {
            get
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (this[i, j] == ChessPiece.Clear)
                            return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        ///  komórki z określonym indeksem wiersza i indeksem kolumny.
        /// </summary>
        /// <param name="rowIndex">Indeks wierszy określonej komórki</param>
        /// <param name="columnIndex">Indeks kolumny określonej komórki.</param>
        /// <returns> komórki z określonym indeksem wiersza i indeksem kolumny</returns>
        public ChessPiece this[int rowIndex, int columnIndex]
        {
            get { return board[rowIndex, columnIndex]; }
            private set { board[rowIndex, columnIndex] = value; }
        }

        /// <summary>
        /// Inicjuje nową instancję <see cref = "board" /> z pustymi komórkami.
        /// </summary>
        public Board()
        {
            board = new ChessPiece[Board.Size, Board.Size];
            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                    board[i, j] = ChessPiece.Clear;
            }
        }

        /// <summary>
        /// Dodaje sztukę w określonej komórce .
        /// </summary>
        /// <param name="rowIndex">Indeks wiersza komórki, aby dodać nowy element planszy.</param>
        /// <param name="columnIndex">Indeks kolumny komórki, aby dodać nowy element planszy.</param>
        /// <param name="chessPiece">Plansza.</param>
        /// <returns>Prawda, jeśli element planszy został pomyślnie dodany (tzn. Komórka była pusta), w przeciwnym razie false.</returns>
        public bool AddChessPiece(int rowIndex, int columnIndex, ChessPiece chessPiece)
        {
            if (this[rowIndex, columnIndex] == ChessPiece.Clear)
            {
                this[rowIndex, columnIndex] = chessPiece;
                return true;
            }
            return false;
        }

        /// <summary>
        /// czyści <see cref="Board" />.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    this[i, j] = ChessPiece.Clear;
            }
        }

        /// <summary>
        /// Zwraca głęboką kopię tego <see cref = "Board" /> z tym samym stanem.
        /// </summary>
        /// <returns>Głęboka kopia tego <see cref = "Board" /> z tym samym stanem.</returns>
        public Board Copy()
        {
            Board copy = new Board();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    copy[i, j] = this[i, j];
            }

            return copy;
        }
    }
}
