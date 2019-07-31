namespace GoogleARCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class EnvironmentController : MonoBehaviour
    {
        private Button createNewGameButton;
        private ColorBlock colour;

        private bool isPlaneValid;
        private Pose poser;
        private Image readifyLogo, kloudLogo;
        public GameObject indicator;

        private Anchor anchor;

        public GameEngine controller;

        public GameObject fireworks;

        private Vector3 _position;
        private Quaternion _rotation;
        private Trackable _hit;
        private Pose _pose;

        void Start()
        {
            createNewGameButton = GameObject.FindGameObjectWithTag("GameEnabler").GetComponent<Button>();
            colour = createNewGameButton.colors;
            readifyLogo = GameObject.FindGameObjectWithTag("ReadifyLogo").GetComponent<Image>();
            kloudLogo = GameObject.FindGameObjectWithTag("KloudLogo").GetComponent<Image>();
            readifyLogo.enabled = true;
            kloudLogo.enabled = false;
            fireworks.SetActive(false);
        }

        void Update()
        {
            fireworks.SetActive(controller.winner);
            if (!controller.isGameStarted)
            {
                indicator.SetActive(false);
                colour.normalColor = Color.grey;
                createNewGameButton.colors = colour;
            }
            
            
            if (controller.isGameStarted)
            {
                readifyLogo.enabled = controller.nextPlayer;
                kloudLogo.enabled = !controller.nextPlayer;
                
                //return;
            }

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(transform.position.x, transform.position.y, raycastFilter, out hit) && !controller.isGameStarted)
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
                        colour.normalColor = Color.green;
                        createNewGameButton.colors = colour;
                    }
                    //Horizontal Plane
                    else
                    {
                        UpdateObjectPosition(hit.Pose.position, hit.Pose.rotation);
                        colour.normalColor = Color.red;
                        createNewGameButton.colors = colour;

                        _hit = hit.Trackable;
                        _pose = hit.Pose;
                        _position = hit.Pose.position;
                        _rotation = hit.Pose.rotation;
                    }
                }
            }
        }

        public void CreateAnchor()
        {
            controller.isGameStarted = true;
            anchor = _hit.CreateAnchor(_pose);

            fireworks.transform.position = _position;
            fireworks.transform.rotation = _rotation;
            indicator.transform.SetParent(anchor.transform);
            indicator.SetActive(true);

            createNewGameButton.enabled = false;
        }

        private void UpdateObjectPosition(Vector3 position, Quaternion rotation)
        {
            indicator.SetActive(true);
            indicator.transform.position = position;
            indicator.transform.rotation = rotation;
            
        }
    }
}
