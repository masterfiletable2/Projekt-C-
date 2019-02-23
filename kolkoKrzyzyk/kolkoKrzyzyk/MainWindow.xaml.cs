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

namespace kolkoKrzyzyk
{
    /// <summary>
    /// MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Ścieżka zaznaczenia kół.
        /// </summary>
        private const string CircleImagePath = "Images/o.png";

        /// <summary>
        /// Ścieżka zaznaczenia krzyży.
        /// </summary>
        private const string CrossImagePath = "Images/x.png";

        /// <summary>
        /// Ścieżka niewidoczna.
        /// </summary>
        private const string TransparentImagePath = "Images/Transparent.bmp";

        /// <summary>
        /// Znak gracza jest ustawiony na "O".
        /// </summary>
        private const ChessPiece PlayerMark = ChessPiece.O;

        /// <summary>
        /// Znak AI jest ustawiony na "X".
        /// </summary>
        private const ChessPiece AiMark = ChessPiece.X;

        /// <summary>
        /// Obraz znaku gracza.
        /// </summary>
        private readonly BitmapImage PlayerMarkImage = new BitmapImage(new Uri(CircleImagePath, UriKind.Relative));

        /// <summary>
        /// Obraz znaku AI.
        /// </summary>
        private readonly BitmapImage AiMarkImage = new BitmapImage(new Uri(CrossImagePath, UriKind.Relative));

        /// <summary>
        /// Przezroczysty obraz.
        /// </summary>
        private readonly BitmapImage TransparentImage = new BitmapImage(new Uri(TransparentImagePath, UriKind.Relative));

        /// <summary>
        /// Tablica tła gry "Kółko i krzyżyk".
        /// </summary>
        private readonly Board board;

        /// <summary>
        /// Odpowiednia tablica 2-D przycisków, z których każdy reprezentuje komórkę interfejsu użytkownika.
        /// </summary>
        private Button[,] chessPositions;

        /// <summary>
        /// Wartość logiczna wskazująca, czy gra jest uruchomiona.
        /// </summary>
        private bool gameStarted;

        /// <summary>
        /// Inicjuje okno gry i wewnętrzne struktury danych.
        /// </summary>
        public MainWindow()
        {
            board = new Board();
            gameStarted = false;

            InitializeComponent();
        }

        /// <summary>
        /// Obsługuje zdarzenie ruchu gracza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmd_PlayerMove(object sender, RoutedEventArgs e)
        {
            // Nie rób nic, jeśli gra nie jest uruchomiona lub nikt nie może się już poruszyć.
            if (!gameStarted || !CanMove())
                return;

            // Kliknięty przycisk wyodrębnia odpowiednią współrzędną.
            Button cmd = (Button)e.OriginalSource;
            string content = cmd.Tag.ToString();
            string[] position = content.Split(',');
            int playerRowIndex = int.Parse(position[0]);
            int playerColumnIndex = int.Parse(position[1]);

            // Spróbuj dodać figurę w pozycji gracza.
            if (board.AddChessPiece(playerRowIndex, playerColumnIndex, PlayerMark))
            {
                // Zaktualizuj interfejs UI przycisku klikniętego przez gracza.
                Image playerMarkImage = new Image();
                playerMarkImage.Source = PlayerMarkImage;
                cmd.Content = playerMarkImage;

                // Sprawdź, czy gracz wygrywa.
                if (PlayerWin())
                {
                    MessageBox.Show("Wygrałeś z komputerem");
                    return;
                }

                // AI porusza się, jeśli może.
                if (CanMove())
                {
                    AiMove();

                    // Sprawdź, czy AI wygrywa.
                    if (AiWin())
                        MessageBox.Show("Komputer wygrał z Tobą!");
                }
            }
        }

        /// <summary>
        /// Uruchamia nową grę, gdy gracz kliknie ten przycisk.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStartGame_Click(object sender, RoutedEventArgs e)
        {
            // Rozpocznij grę, jeśli gracz określi, kto pierwszy poruszy.
            if ((rbAiFirst.IsChecked != true) && (rbPlayerFirst.IsChecked != true))
            {
                MessageBox.Show("Kto zaczyna grę jako pierwszy?");
                return;
            }

            // Rozpocznij nową grę, czyszcząc planszę.
            ClearChessboard();
            gameStarted = true;

            // Pozwól AI poruszyć się, jeśli zaczyna rozgrywkę.
            if (rbAiFirst.IsChecked == true)
                AiMove();
        }

        /// <summary>
        /// Pobiera odniesienia do przycisków odpowiadających każdej pozycji szachowej.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <reamrks>
        /// Ta metoda musi zostać wywołana po załadowaniu tego okna, ponieważ odwołania do przycisków są dostępne dopiero po załadowaniu tego okna.
        /// Wystąpi wyjątek, jeśli wywołasz metodę przed załadowaniem okna.
        /// </reamrks>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chessPositions = new Button[Board.Size, Board.Size];
            chessPositions[0, 0] = chessPosition00;
            chessPositions[0, 1] = chessPosition01;
            chessPositions[0, 2] = chessPosition02;
            chessPositions[0, 3] = chessPosition03;
            chessPositions[0, 4] = chessPosition04;

            chessPositions[1, 0] = chessPosition10;
            chessPositions[1, 1] = chessPosition11;
            chessPositions[1, 2] = chessPosition12;
            chessPositions[1, 3] = chessPosition13;
            chessPositions[1, 4] = chessPosition14;

            chessPositions[2, 0] = chessPosition20;
            chessPositions[2, 1] = chessPosition21;
            chessPositions[2, 2] = chessPosition22;
            chessPositions[2, 3] = chessPosition23;
            chessPositions[2, 4] = chessPosition24;

            chessPositions[3, 0] = chessPosition30;
            chessPositions[3, 1] = chessPosition31;
            chessPositions[3, 2] = chessPosition32;
            chessPositions[3, 3] = chessPosition33;
            chessPositions[3, 4] = chessPosition34;

            chessPositions[4, 0] = chessPosition40;
            chessPositions[4, 1] = chessPosition41;
            chessPositions[4, 2] = chessPosition42;
            chessPositions[4, 3] = chessPosition43;
            chessPositions[4, 4] = chessPosition44;

        }

        /// <summary>
        /// Wyczyść planszę.
        /// </summary>
        private void ClearChessboard()
        {
            // Wyczyść tło.
            board.Clear();

            // Wyczyść planszę UI.
            for (int i = 0; i < Board.Size; i++)
            {
                for (int j = 0; j < Board.Size; j++)
                {
                    Image transparentImage = new Image();
                    transparentImage.Source = TransparentImage;
                    chessPositions[i, j].Content = transparentImage;
                }
            }
        }

        /// <summary>
        /// AI wykonuje ruch.
        /// </summary>
        private void AiMove()
        {
            // Znajdź najlepszą pozycję dla AI, aby przejść dalej za pomocą algorytmu Min Max Pruning.
            minMax.GetBestPosition(board, AiMark, out int aiRowIndex, out int aiColumnIndex);

            // Dodaj pionek szachowy w obliczonej pozycji.
            board.AddChessPiece(aiRowIndex, aiColumnIndex, AiMark);
            Image aiMarkImage = new Image();
            aiMarkImage.Source = AiMarkImage;
            chessPositions[aiRowIndex, aiColumnIndex].Content = aiMarkImage;
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy AI lub gracz może nadal się poruszać.
        /// </summary>
        /// <returns>Prawda, jeśli AI lub gracz może nadal się poruszać, w przeciwnym razie false.</returns>
        /// <remarks>
        /// Chociaż ta metoda ma tylko jedną linię, jest to realizowane ze względu na czytelność.
        /// </remarks>
        private bool CanMove()
        {
            return !(board.IsFull || PlayerWin() || AiWin());
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy gracz wygrywa.
        /// </summary>
        /// <returns>Prawda, jeśli gracz wygrywa, w przeciwnym razie false.</returns>
        /// <remarks>
        /// Chociaż ta metoda ma tylko jedną linię, jest to realizowane ze względu na czytelność.
        /// </remarks>
        private bool PlayerWin()
        {
            return Win(PlayerMark);
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy AI wygrywa.
        /// </summary>
        /// <returns>Prawda, jeśli AI wygrywa, w przeciwnym razie false.</returns>
        /// <remarks>
        /// Chociaż ta metoda ma tylko jedną linię, jest to realizowane ze względu na czytelność.
        /// </remarks>
        private bool AiWin()
        {
            return Win(AiMark);
        }

        /// <summary>
        /// Pobiera wartość wskazującą, czy gracz z danym znakiem wygrywa.
        /// </summary>
        /// <param name="mark">The mark of the player.</param>
        /// <returns>Prawda, jeśli wygrywa gracz z danym znacznikiem, w przeciwnym razie false.</returns>
        /// <remarks>
        /// "Gracz" tutaj nie oznacza człowieka grającego w tę grę, oznacza to tylko AI lub gracza tej gry.
        /// </remarks>
        private bool Win(ChessPiece mark)
        {
            // Znacznik boolowski wskazujący, czy użytkownik wygrywa.
            bool win;

            // Sprawdź wiersze.
            for (int i = 0; i < Board.Size; i++)
            {
                win = true;
                for (int j = 0; j < Board.Size; j++)
                {
                    if (board[i, j] != mark)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return true;
            }

            // Sprawdź kolumny.
            for (int j = 0; j < Board.Size; j++)
            {
                win = true;
                for (int i = 0; i < Board.Size; i++)
                {
                    if (board[i, j] != mark)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                    return true;
            }

            // Sprawdź główną przekątną.
            win = true;
            for (int i = 0; i < Board.Size; i++)
            {
                if (board[i, i] != mark)
                {
                    win = false;
                    break;
                }
            }

            if (win)
                return true;

            // Sprawdź przekątną imadła.
            win = true;
            for (int i = 0; i < Board.Size; i++)
            {
                if (board[i, Board.Size - 1 - i] != mark)
                {
                    win = false;
                    break;
                }
            }

            if (win)
                return true;

            return false;
        }
    }
}