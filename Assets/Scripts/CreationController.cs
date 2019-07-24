
namespace GoogleARCore
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    public class CreationController : MonoBehaviour
    {
        [SerializeField]
        GameObject verticalCell, horizontalCell;

        private GameObject objectToSpawn;

        [SerializeField] GameController game;

        private GameObject[,] grid = new GameObject[3, 3];
        //below array is used for re-arranging each individual cell to match the game logic
        private string[] names = { "6", "3", "0", "7", "4", "1", "8", "5", "2" };

        private Button createNewGameButton;
        private GameObject[] takenCells;
        private GameObject[] emptyCells;
        private GameObject fireWorks;
        private float x, y, z;
        private ColorBlock colour;

        private void Start()
        {
            createNewGameButton = GameObject.FindGameObjectWithTag("NewGameButton").GetComponent<Button>();
            createNewGameButton.enabled = false;
            fireWorks = GameObject.FindGameObjectWithTag("Fireworks");
            fireWorks.SetActive(false);
            colour = createNewGameButton.colors;
        }

        public void Update()
        {
            createNewGameButton.enabled = false;
            Text winnerMessage = GameObject.FindGameObjectWithTag("WinnerMessage").GetComponent<Text>();
            //winnerMessage.text = "Nothing";
           
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(transform.position.x, transform.position.y, raycastFilter, out hit))
            {
                if (hit.Trackable is FeaturePoint)
                {

                    //winnerMessage.text = "Feature Point Detected";
                }
                else if (hit.Trackable is DetectedPlane)
                {
                    DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                    //check type of the detected plane
                    //vertical plane
                    if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                    {
                        Pose pose = detectedPlane.CenterPose;

                        x = pose.position.x;
                        y = pose.position.y;
                        z = pose.position.z;

                        objectToSpawn = verticalCell;
                        //winnerMessage.text = "X: " + x.ToString("F2") + " Y: " + y.ToString("F2") + " Z: " + z.ToString("F2");
                        createNewGameButton.enabled = true;
                        //change button colour
                        colour.normalColor = Color.red;
                        createNewGameButton.colors = colour;

                    }
                    //Horizontal Plane
                    else
                    {
                        Pose pose = detectedPlane.CenterPose;

                        x = pose.position.x;
                        y = pose.position.y;
                        z = pose.position.z;

                        objectToSpawn = horizontalCell;
                        //winnerMessage.text = "X: " + x.ToString("F2") + " Y: " + y.ToString("F2") + " Z: " + z.ToString("F2");
                        createNewGameButton.enabled = true;
                        //change button colour
                        colour.normalColor = Color.green;
                        createNewGameButton.colors = colour;

                        //adjust fireworks' position acccordingly
                        fireWorks.transform.SetPositionAndRotation(new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }

        }

        public void CreateGame()
        {
            while (game.transform.childCount > 0)
            {
                DestroyImmediate(game.transform.GetChild(0).gameObject);
            }

            var counter = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector3 dynamicPos = new Vector3((((float)(x + (i * .1)))), ((float)(y + (j * .1))), z);
                    GameObject temp = (GameObject)Instantiate(objectToSpawn, dynamicPos, Quaternion.identity, game.transform);
                    temp.name = names[counter];
                    grid[i, j] = temp;
                    counter++;
                }
            }
            createNewGameButton.enabled = false;
            fireWorks.SetActive(false);
            Text winnerMessage = GameObject.FindGameObjectWithTag("WinnerMessage").GetComponent<Text>();
            winnerMessage.text = "";
        }
    }
}
