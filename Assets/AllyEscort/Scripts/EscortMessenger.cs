using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class EscortMessenger : MonoBehaviour
    {
        public EscortAgent escortAgent;

        private bool isRunning = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            { 
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 20f))
                {
                    escortAgent.HandleCommand(CommandType.MOVE_TO, hit.point);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                escortAgent.HandleCommand(CommandType.WAIT);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                escortAgent.HandleCommand(CommandType.SET_MAX_SPEED, isRunning ? 2.0f : 10.0f);
                isRunning = !isRunning;
            }
        }
    }
}