using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class EscortAgent : MonoBehaviour
    {
        public CalculatePathComponent calculatePathComponent;

        [Header("Variables")]
        public float maxSpeed;
        public float acceleration;

        private void Awake()
        {
            if (calculatePathComponent == null)
            {
                Debug.LogError($"Error: CalculatePathComponent is null. This class must have one");
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HandleMessage(Message message)
        {
            switch (message.messageType)
            {
                case Message.MessageType.MoveTo:
                    if (message.data is Vector3 point)
                    {
                        List<Vector3> path = calculatePathComponent.GetPath(transform.position, point);
                        if (path != null)
                        {
                            //DrawPathDebug(path);
                            StopAllCoroutines();
                            StartCoroutine(MoveAlongPath(path));
                        }
                    }
                    else
                    {
                        Debug.Log($"Error: message.data is not type Vector3. It is type: {message.data.GetType()}");
                    }
                    break;
                case Message.MessageType.Wait:
                    StopAllCoroutines();
                    break;
                case Message.MessageType.SetSpeed:
                    if (message.data is float speed)
                    {
                        maxSpeed = speed;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator MoveAlongPath(List<Vector3> path)
        {
            Vector3 pos = transform.position;
            float speed = 0;

            DrawPathDebug(path);

            while (path.Count > 0)
            {
                Vector3 delta = path[0] - pos;
                Vector3 dir = delta.normalized;

                speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);
                Vector3 velocity = dir * speed;

                pos += velocity * Time.deltaTime;

                transform.position = pos;

                if (delta.sqrMagnitude <= 0.01f)
                {
                    path.RemoveAt(0);
                }

                yield return null;
            }
        }

        private void DrawPathDebug(IReadOnlyList<Vector3> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.blue, 5.0f, false);
            }
        }
    }



    public struct Message
    {
        public enum MessageType
        {
            MoveTo,
            Wait,
            SetSpeed,
        }

        public MessageType messageType;
        public object data;
    }
}
