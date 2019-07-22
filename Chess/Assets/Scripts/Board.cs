using System;
using System.Collections.Generic;
using UnityEngine;
using ChessRules;
using Assets.Scripts;

public class Board : MonoBehaviour, ICreatable
{
    BoxSquares squares;
    BoxFigures figures;
    BoxPromots promots;

    Client client;

    DragAndDrop dad;
	Chess chess;
	string onPromotionMove;

    public Board()
    {
        squares = new BoxSquares(this);
        figures = new BoxFigures(this);
        promots = new BoxPromots(this);

        client = new Client();
        chess = new Chess();
        dad = new DragAndDrop(PickObject, DropObject);
		onPromotionMove = "";
    }

    public GameObject menu;
    public void Menu()
    {
        if (menu.active == true)
            menu.SetActive(false);
        else
        menu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    void Start()
    {
        //Screen.fullScreen = !Screen.fullScreen;
        // Для PC:
        //Screen.SetResolution(800, 600, false);
        // Для android телефона (так выходит за края экрана):
        //Screen.SetResolution(720, 1280, false);
        InitGameObjects();
        chess = new Chess(client.GetFenFromServer());
		ShowFigures();
        squares.MarkSquaresFrom(chess.YieldValidMoves());
        promots.HidePromotionFigures();
        InvokeRepeating("Refresh", 2, 2);
    }

    void Refresh()
    {
        string fen = client.GetFenFromServer();
        if (chess.fen == fen) return;
        chess = new Chess(fen);
        ShowFigures();
        squares.MarkSquaresFrom(chess.YieldValidMoves());
        promots.HidePromotionFigures();
    }
    private void InitGameObjects()
    {
        squares.Init();
        figures.Init();
        promots.Init();
    }
    public GameObject CreateGameObject(int x, int y, string pattern)
    {
        GameObject go = Instantiate(GameObject.Find(pattern));
        go.transform.position = Coords.GetVector(x, y);
        go.name = pattern;
        return go;
    }

    public void SetSprite(GameObject go, string source)
    {
        go.GetComponent<SpriteRenderer>().sprite =
             GameObject.Find(source).GetComponent<SpriteRenderer>().sprite;
    }

        void ShowFigures()
	    {
		    for (int y = 0; y < 8; y++)
			    for (int x = 0; x < 8; x++) 
			    {
				    string figure = chess.GetFigureAt(x, y).ToString();

                    figures.SetPosition(x, y, squares);
                    figures.SetSpriteAt(x, y, figure);
			    }
	    }

    void Update () {
		dad.Action();
	}

	void DropObject(Vector2 from, Vector2 to)
	{
        string e2 = Coords.GetSquare(from);
        string e4 = Coords.GetSquare(to);
        string figure = chess.GetFigureAt(e2).ToString();
        string move = figure + e2 + e4;
        if (move.Length != 5) return;
        Debug.Log(move);
		if (figure == "P" && e4[1] == '8' || // e4 может быть равно, например, "d8"
		   figure == "p" && e4[1] == '1')
            if (chess.Move(move) != chess)
		{
			onPromotionMove = move;
                promots.ShowPromotionFigures(figure);
                return;
		}
        MakeMove(move); // 1-е место, где выполняется ход
        ShowFigures();
        squares.MarkSquaresFrom(chess.YieldValidMoves());
    }

    public void NewGame()
    {
        menu.SetActive(false);
        MakeMove("resign");
    }

    void MakeMove(string move)
    {
        string fen;
        // new Game:
        // простейшая (неполная) проверка ввода слова resign
        if (move == "resign" || move == "RESIGN" || move == "куышпт" || move == "RESIGN")
        {
            client.SendMove(move); // отсылаем на сервер
            fen = client.SendMove(move);
            chess = new Chess(fen);
            return;
        }

        if (!chess.IsValidMove(move)) return;

            fen = client.SendMove(move);

        if (fen == chess.fen) return;
        chess = new Chess(move);
    }

    void PickObject(Vector2 from)
    {
        if(onPromotionMove != "") // здесь ещё только фигура с её ходом 
        {
            int x = Coords.GetX(from);
            int y = Coords.GetY(from);

            onPromotionMove += promots.GetPromotionFigure(x, y);

            //  здесь уже фигура с ходом и promotion
            MakeMove(onPromotionMove);
            onPromotionMove = "";
            ShowFigures();
            squares.MarkSquaresFrom(chess.YieldValidMoves());
            promots.HidePromotionFigures();
            return;
        }
        squares.MarkSquaresTo(chess.YieldValidMoves(), Coords.GetSquare(from));
    }

}


