using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessRules;
using System.Data.Entity;

namespace ChessAPI.Models
{
    public class Logic
    {
        private ShessModel db;

        public Logic()
        {
            db = new ShessModel();
        }

        public Game GetCurrentGame()
        {
            Game game;
            var currentGames = db.Games.Where(g => g.Status == "play");
            if (currentGames.Count() > 0)
                game = currentGames.First();
            else
                game = NewGame();
            return game;
        }

        private Game NewGame()
        {
            Chess chess = new Chess();

            Game game = new Game();
            game.FEN = chess.fen; // берём текущий fen
            game.Status = "play";

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }

        public Game ResignGame(int id)
        {
            Game game = GetGame(id);
            if (game == null) return game;
            if (game.Status != "play") return game;

            game.Status = "done";

            db.Entry(game).State = EntityState.Modified;
            db.SaveChanges();

            return game;
        }

        public Game GetGame(int id)
        {
            Game game = db.Games.Find(id);
            return game;
        }

        public Game MakeMove(int id, string move)
        {
            Game game = GetGame(id);
            if (game == null) return game;
            if (game.Status != "play") return game; // если игра завершена - в ней нельзя играть

            Chess chess = new Chess(game.FEN);
            if (!chess.IsValidMove(move))
                return game;
            chess = chess.Move(move);
            game.FEN = chess.fen; // перезаписываем fen

            if (chess.IsCheckmate ||
                chess.IsStalemate)
                game.Status = "done";

            db.Entry(game).State = EntityState.Modified;
            db.SaveChanges();

            return game;
        }
    }
}