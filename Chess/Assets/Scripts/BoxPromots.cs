using UnityEngine;

namespace Assets.Scripts
{
    class BoxPromots : Box
    {
        const string proFigures = "QRBN";
        const int minx = 2;
        const int whitey = 8;
        const int blacky = -1;
        public BoxPromots(ICreatable creator) : base(creator) { }

        public void Init()
        {
            for (int x = minx; x < minx + proFigures.Length; x++)
                Create(x, whitey, GetWhiteProFigure(x));

            for (int x = minx; x < minx + proFigures.Length; x++)
                Create(x, blacky, GetBlackProFigure(x));
        }
         private string GetWhiteProFigure(int x)
        {
            if (x >= minx && x < minx + proFigures.Length)
                return proFigures.Substring(x - minx, 1);
            return "";
        }
        // спрятать все фигуры
        public void HidePromotionFigures()
        {
            foreach (GameObject pro in list.Values)
                SetSpriteFor(pro, "."); 
        }
        // процесс, когда задана фигура     
        public void ShowPromotionFigures(string pawn)
        {
            if (pawn == "P")
                for (int x = minx; x < minx + proFigures.Length; x++)
                    SetSpriteAt(x, whitey, GetWhiteProFigure(x));
            if (pawn == "p")
                for (int x = minx; x < minx + proFigures.Length; x++)
                    SetSpriteAt(x, blacky, GetBlackProFigure(x));
        }
        private string GetBlackProFigure(int x)
        {
            return GetWhiteProFigure(x).ToLower();
        }

        public string GetPromotionFigure(int x, int y)
        {
            if (y == whitey) return GetWhiteProFigure(x);
            if (y == blacky) return GetBlackProFigure(x);
            return "";
        }
    }
}
