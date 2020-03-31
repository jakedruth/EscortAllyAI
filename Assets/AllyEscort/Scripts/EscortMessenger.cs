using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class EscortMessenger : MonoBehaviour
    {
        public EscortAgent escortAgent;
        public Transform cursorTransform;

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
                        if (cursorTransform != null)
                        {
                            cursorTransform.SetParent(null);
                            cursorTransform.position = interactableObject.transform.position;
                        }
                    }
                    else
                    {
                        escortAgent.TransitionToState("MoveToPoint", hit.point);
                        if (cursorTransform != null)
                        {
                            cursorTransform.SetParent(null);
                            cursorTransform.position = hit.point;
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                escortAgent.TransitionToState("Idle");
                if (cursorTransform != null)
                {
                    cursorTransform.SetParent(null);
                    cursorTransform.position = escortAgent.transform.position;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                escortAgent.TransitionToState("FollowPlayer", transform);
                if (cursorTransform != null)
                {
                    cursorTransform.SetParent(transform);
                    cursorTransform.position = transform.position;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                escortAgent.TransitionToState("WalkAimlessly");
            }
        }
    }
}