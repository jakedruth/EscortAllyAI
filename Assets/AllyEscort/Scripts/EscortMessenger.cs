﻿using System.Collections;
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
                    escortAgent.TransitionToState("ToMoveToPoint", hit.point);
                    if (cursorTransform != null)
                        cursorTransform.position = hit.point;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                escortAgent.TransitionToState("ToIdle");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                escortAgent.TransitionToState("ToFollowPlayer", transform);
            }
        }
    }
}