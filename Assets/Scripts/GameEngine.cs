using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{

    [SerializeField] GameEngine game;

    public bool nextPlayer, winner, isGameStarted;

    private readonly int[,] lines = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
    private string[] squares = new string[9];
    private int moveCounter;
    //internal TicTacToeValue currentTurn;

    internal void NotifyCellChange(string str)
    {
        if (winner)
        {
            return;
        }

        isGameStarted = true;
        if (moveCounter == 9)
        {
            Debug.Log("Draw");
            ResetGame();

        }
 
        var indexToBeAccessed = System.Convert.ToInt32(str) - 1;

        if (!nextPlayer)
        {
            squares.SetValue("X", indexToBeAccessed);

            if (CalculateWinner() == "X")
            {
                winner = true;
                Debug.Log("Readifarian Wins");
            }
        }
        //second player
        else
        {
            squares.SetValue("O", indexToBeAccessed);

            if (CalculateWinner() == "O")
            {
                winner = true;
                Debug.Log("Kloudie Wins");
            }
        }
        moveCounter++;
        nextPlayer = !nextPlayer;
    }

    private string CalculateWinner()
    {
        for (int i = 0; i < squares.Length - 1; i++)
        {
            var a = lines[i, 0];
            var b = lines[i, 1];
            var c = lines[i, 2];

            if ((squares[a] != null) && (squares[a] == squares[b] && squares[a] == squares[c]))
            {
                return squares[a];
            }
        }
        return null;
    }

    public void ResetGame()
    {
        squares = new string[9];
        moveCounter = 0;
        nextPlayer = false;
        isGameStarted = false;
    }
}
