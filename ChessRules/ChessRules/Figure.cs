﻿using System.Collections.Generic;

namespace ChessRules
{
    enum Figure
    {
        // обозначение отсутствия фигур (фигуры нет):
        none,
        // используются принятые международные обозначения шахматных фигур:
        whiteKing = 'K',
        whiteQueen = 'Q',
        whiteRook = 'R',
        whiteBishop = 'B',
        whiteKnight = 'N',
        whitePawn = 'P',
        // чёрным фигурам соответствуют маленькие буквы:
        blackKing = 'k',
        blackQueen = 'q',
        blackRook = 'r',
        blackBishop = 'b',
        blackKnight = 'n',
        blackPawn = 'p'
    }

    static class FigureMethods
    {
        public static Color GetColor(this Figure figure)
        {
            if (figure == Figure.none)
                return Color.none;

            switch (figure)
            {
                case Figure.whiteKing:
                case Figure.whiteQueen:
                case Figure.whiteRook:
                case Figure.whiteBishop:
                case Figure.whiteKnight:
                case Figure.whitePawn:
                    return Color.white;
                case Figure.blackKing:
                case Figure.blackQueen:
                case Figure.blackRook:
                case Figure.blackBishop:
                case Figure.blackKnight:
                case Figure.blackPawn:
                    return Color.black;
                // case Figure.none:
                default:
                    return Color.none;
            }
        }

        public static IEnumerable<Figure> YieldPromotions(this Figure figure, Square to)
        {
            if (figure == Figure.whitePawn && to.y == 7)
            {
                yield return Figure.whiteQueen;
                yield return Figure.whiteRook;
                yield return Figure.whiteBishop;
                yield return Figure.whiteKnight;
            }
            else
            if (figure == Figure.blackPawn && to.y == 0)
            {
                yield return Figure.blackQueen;
                yield return Figure.blackRook;
                yield return Figure.blackBishop;
                yield return Figure.blackKnight;
            }
            else
                yield return Figure.none;
        }

    }
}
