using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Assets.Scripts
{
    class Client
    {

        public const string address = "http://asp741368-001-site1.itempurl.com/api/Chess/";

        WebClient web;

        public int GameID { get; private set; }

        public Client()
        {
            web = new WebClient();
        }

        public string GetFenFromServer()
        {
            string json = web.DownloadString(address);
            GameID = GetIdFromJSON(json);
            string fen = GetFenFromJSON(json);
            return fen;
        }

        public string SendMove(string move)
        {
            string json = web.DownloadString(address + GameID.ToString() + "/" + move); // здесь искать проблему
            string fen = GetFenFromJSON(json);
            return fen;
        }

        int GetIdFromJSON(string json)
        {
            // {"ID":8,"FEN":"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1","Status":"play"}
            //  ^x   ^^
            //       yz
            int x = json.IndexOf("\"ID\"");
            int y = json.IndexOf(":", x) + 1;
            // находим место, где это всё заканчивается, где у нас закрывается кавычка после y
            int z = json.IndexOf(",", y);
            string id = json.Substring(y, z - y);
            return Convert.ToInt32(id);
        }

        string GetFenFromJSON(string json)
        {
            // {"ID":8,"FEN":"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1","Status":"play"}
            //         ^x   ^ ^y                                                      ^z
            int x = json.IndexOf("\"FEN\""); // ищем позицию начала
            int y = json.IndexOf(":\"", x) + 2; // ищем начиная с позиции x, + добавляем 2 позиции
            // находим место, где это всё заканчивается - где у нас закрывается кавычка после y
            int z = json.IndexOf("\"", y);
            string fen = json.Substring(y, z - y);
            return fen;
        }

        string GetFenFromJSON2(string json)
        {
            string str = json.Split(',')[1];
            string s = str.Split(':')[1];
            string fen = s.Trim('"');
            return fen;
        }


    }
}
