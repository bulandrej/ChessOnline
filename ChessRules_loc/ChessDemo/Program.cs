using System;
using System.Text;
using ChessRules;

namespace ChessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string fen;
            // стандартная начальная позиция
            // fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0"; 
            //fen = "rnbqkbnr/pPppppPp/8/8/8/8/PpPPPPpP/RNBQKBNR w KQkq - 0 1";
            // FEN используемый Евгением:
            fen = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NpPP/RNBQK2R w KQ - 1 8";


            //Client client = new Client();

            //string fen = client.GetFenFromServer();
            //Console.WriteLine(client.GameID);

            //Console.WriteLine(fen);
            //Console.ReadKey();

            Chess chess = new Chess(fen); // начальная позиция

            // позиция для тестирования
            //Chess chess = new Chess("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 0");
            //Console.WriteLine(NextMoves(3, chess));
            //Console.ReadKey();
            //return;

            int i = 0;

            while (true)
            {
                Console.WriteLine(chess.fen);

                Print(ChessToAscii(chess)); // вывод доски на экран

                Console.WriteLine();

                foreach (string moves in chess.YieldValidMoves())

                {
                    if (i > 4)
                    {
                        Console.WriteLine("\n");
                        i = 0;
                    }
                    else
                    {
                        Console.Write(moves + "  ");
                        i++;
                    }
                    
                }
                Console.WriteLine("\n");
                // запрашиваем желаемый ход
                string move = Console.ReadLine();
                if (move == "") break; // выход из игры по нажатию клавиши
                if (move == "s"|| move == "S" || move == "ы" || move == "Ы")
                {
                    //fen = client.GetFenFromServer();
                    chess = new Chess(fen);
                    continue;
                }
                    
                if (!chess.IsValidMove(move))
                    continue;
                //fen = client.SendMove(move);
                //chess = new Chess(fen);
                chess = chess.Move(move);
                
            }
        }

        static int NextMoves(int step, Chess chess)
        {
            if (step == 0) return 1;
            int count = 0;
            foreach (string moves in chess.YieldValidMoves())
                count += NextMoves((step - 1), chess.Move(moves));
            return count;
        }

        static string ChessToAscii(Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            // рисуем рамку вокруг доски:
            sb.AppendLine("  +-----------------+"); // начало доски (горизонталь)
            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" | ");
                for (int x = 0; x < 8; x++)
                    sb.Append(chess.GetFigureAt(x, y) + " ");
                sb.AppendLine("|");
            }
            sb.AppendLine("  +-----------------+"); // конец доски (горизонталь)
            sb.AppendLine("    a b c d e f g h ");

            if (chess.IsCheck) sb.AppendLine("IS CHECK");
            if (chess.IsCheckmate) sb.AppendLine("IS CHECKMATE");
            if (chess.IsStalemate) sb.AppendLine("IS STALEMATE");

            return sb.ToString();
        }

        static void Print(string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.Blue ;
                Console.Write(x);
            }
            Console.ForegroundColor = old;
        }
    }
}
