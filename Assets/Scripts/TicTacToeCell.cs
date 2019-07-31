
namespace GoogleARCore
{
    using UnityEngine;

    public class TicTacToeCell : MonoBehaviour
    {
        public TicTacToeValue value = TicTacToeValue.Empty;

        public GameEngine controller;

        public GameObject emptyDisplay;
        public GameObject naughtDisplay;
        public GameObject crossDisplay;


        public void OnClick()
        {
            //if (value != TicTacToeValue.Empty) { return; }

            //value = controller.currentTurn;

            //emptyDisplay.SetActive(value == TicTacToeValue.Empty);
            if (controller.winner) return;
            
            if (!controller.nextPlayer)
            {
                emptyDisplay.SetActive(false);
                naughtDisplay.SetActive(true);
                var str = naughtDisplay.tag;
                controller.NotifyCellChange(str);

            } else
            {
                emptyDisplay.SetActive(false);
                crossDisplay.SetActive(true);
                var str = crossDisplay.tag;
                controller.NotifyCellChange(str);

            }
        }
    }
}
