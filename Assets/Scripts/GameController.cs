
namespace GoogleARCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    public class GameController : MonoBehaviour
    {
        [SerializeField]
        GameObject playerX, playerO;

        [SerializeField] GameController game;

        private GameObject fireWorks;

        public bool nextPlayer;

        private readonly int[,] lines = { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, { 0, 4, 8 }, { 2, 4, 6 } };
        private string[] squares = new string[9];

        private Image readifyLogo, kloudLogo;
        private Text winnerMessage;
        private int moveCounter;

        void Awake()
        {
            winnerMessage = GameObject.FindGameObjectWithTag("WinnerMessage").GetComponent<Text>();
            readifyLogo = GameObject.FindGameObjectWithTag("ReadifyLogo").GetComponent<Image>();
            kloudLogo = GameObject.FindGameObjectWithTag("KloudLogo").GetComponent<Image>();
            fireWorks = GameObject.FindGameObjectWithTag("Fireworks");
            nextPlayer = false;
        }

        void Update()
        {
            readifyLogo.enabled = !nextPlayer;
            kloudLogo.enabled = nextPlayer;

            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit raycastHit = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit);

                if (hit)
                {
                    if (raycastHit.transform.gameObject.CompareTag("IndividualCell"))
                    {
                        moveCounter++;
                        nextPlayer = !nextPlayer;

                        if (moveCounter == 9)
                        {
                            winnerMessage.text = "Draw";
                            ResetGame();

                        }
                        GameObject temp = raycastHit.transform.gameObject;
                        var indexToBeAccessed = System.Convert.ToInt32(temp.name);
                        Destroy(temp);

                        //take turns
                        //first player
                        if (nextPlayer)
                        {
                            playerX.name = "X";
                            squares.SetValue("X", indexToBeAccessed);
                            Instantiate(playerX, temp.transform.position, temp.transform.rotation, game.transform);
                            if (CalculateWinner(squares) != null)
                            {
                                winnerMessage.text = "Readifarian Wins";
                                fireWorks.SetActive(true);
                                ResetGame();

                            }
                        }
                        //second player
                        else
                        {
                            playerO.name = "O";
                            squares.SetValue("O", indexToBeAccessed);
                            Instantiate(playerO, temp.transform.position, temp.transform.rotation, game.transform);
                            if (CalculateWinner(squares) != null)
                            {
                                winnerMessage.text = "Kloudie Wins";
                                fireWorks.SetActive(true);
                                ResetGame();

                            }
                        }

                    }
                }
            }
        }

        private string CalculateWinner(string[] squaresToBePassed)
        {
            for (int i = 0; i < squares.Length - 1; i++)
            {
                var a = lines[i, 0];
                var b = lines[i, 1];
                var c = lines[i, 2];

                if ((squares[a] != null) && (squaresToBePassed[a] == squaresToBePassed[b] && squaresToBePassed[a] == squaresToBePassed[c]))
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
            Button createNewGameButton = GameObject.FindGameObjectWithTag("NewGameButton").GetComponent<Button>();
            createNewGameButton.enabled = true;
        }
    }
}