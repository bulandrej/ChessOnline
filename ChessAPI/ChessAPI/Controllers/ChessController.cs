using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChessAPI.Models;

namespace ChessAPI.Controllers
{
    public class ChessController : ApiController
    {

        // GET: api/Chess
        public Game GetCurrentGame()
        {
            Logic logic = new Logic();
            return logic.GetCurrentGame();
        }

        public Game GetGameById(int id)
        {
            Logic logic = new Logic();
            return logic.GetGame(id);
        }
        public Game GetMoves(int id, string move) // string move - например, Pe2e4
        {
            Logic logic = new Logic();
            if (move == "resign")
                return logic.ResignGame(id);
            else
                return logic.MakeMove(id, move);
        }

    }
}