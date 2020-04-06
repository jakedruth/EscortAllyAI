using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class EscortMessenger : MonoBehaviour
    {
        public EscortAgent escortAgent;

        //private bool _isRunning = true;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            { 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 20f))
                {
                    InteractableObject interactableObject = hit.transform.GetComponent<InteractableObject>();
                    if (interactableObject != null)
                    {
                        escortAgent.TransitionToState("InteractWithObject", interactableObject);
                    }
                    else
                    {
                        escortAgent.TransitionToState("MoveToPoint", hit.point);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                escortAgent.TransitionToState("Idle");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                escortAgent.TransitionToState("FollowPlayer", transform);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                escortAgent.TransitionToState("MoveAimlessly");
            }
        }
    }
}