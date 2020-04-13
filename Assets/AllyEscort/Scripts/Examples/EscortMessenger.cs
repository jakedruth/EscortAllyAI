using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AllyEscort.Example
{
    /// <summary>
    /// This is an example class on how to send messages to an EscortAgent
    /// </summary>
    public class EscortMessenger : MonoBehaviour
    {
        public EscortAgent escortAgent;

        void Update()
        {
            // How to get a point in space from the mouse
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
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            escortAgent.TransitionToState("SprintToPoint", hit.point);
                        }
                        else if (Input.GetKey(KeyCode.LeftControl))
                        {
                            escortAgent.TransitionToState("SneakToPoint", hit.point);
                        }
                        else
                        {
                            escortAgent.TransitionToState("WalkToPoint", hit.point);
                        }
                    }
                }
            }

            // how to send transitions via key presses or mouse presses

            if (Input.GetMouseButtonDown(1))
            {
                escortAgent.TransitionToState("Idle");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                escortAgent.TransitionToState("FollowTransform", transform);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                escortAgent.TransitionToState("MoveAimlessly");
            }
        }
    }
}