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
                    Message message = new Message
                    {
                        messageType = Message.MessageType.MoveTo,
                        data = hit.point
                    };

                    escortAgent.HandleMessage(message);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Message message = new Message
                {
                    messageType = Message.MessageType.Wait,
                    data = null
                };

                escortAgent.HandleMessage(message);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Message message = new Message
                {
                    messageType = Message.MessageType.SetSpeed,
                    data = (isRunning) ? 2.0f : 10.0f
                };

                escortAgent.HandleMessage(message);
                isRunning = !isRunning;
            }
        }
    }
}