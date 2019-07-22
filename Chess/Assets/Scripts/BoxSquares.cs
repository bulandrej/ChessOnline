using ChessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class BoxSquares : Box
    {
        public BoxSquares(ICreatable creator) : base(creator) { }

        public void Init()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    Create(x, y, GetColor(x, y));
                }
        }

        public string GetColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? "BlackSquare" : "WhiteSquare";
        }

        public void ShowSquare(int x, int y)
        {
            SetSpriteAt(x, y, GetColor(x, y));
        }

        public void MarkSquare(int x, int y)
        {
            SetSpriteAt(x, y, GetColor(x, y) + "Marked");
        }

        public void UnmarkSquares()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    ShowSquare(x, y);
        }
        // помечаем, куда можно ходить
        public void MarkSquaresTo(IEnumerable<string> moves, string from) // e2
        {
            UnmarkSquares();
            foreach (string move in moves) // Pe2e4 move[1] move[2]
            { 
                if (from == move.Substring(1, 2))
                    MarkSquare(Coords.GetX(move.Substring(3, 2)), // e4
                               Coords.GetY(move.Substring(3, 2)));
            }
        }
        public void MarkSquaresFrom(IEnumerable<string> moves)
        {
            UnmarkSquares();
            foreach (string move in moves) // Pe2e4 move[1] move[2]
            {
                MarkSquare(Coords.GetX(move.Substring(1, 2)), // e2
                           Coords.GetY(move.Substring(1, 2)));
            }
        }
    }
}
