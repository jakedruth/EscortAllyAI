using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AllyEscort
{
    public enum CommandType
    {
        TRIGGER_STATE
    }

    [Serializable]
    public struct Transition
    {
        public string key;
        public BaseState targetState;
    }

    public class EscortAgent : MonoBehaviour
    {
        public CalculatePathComponent calculatePathComponent;
        public List<Transition> transitions;

        public BaseState CurrentState { get; private set; }
        public StatePhases CurrentPhase { get; private set; }

        [Header("Variables")]
        public float maxSpeed;
        public float minSpeed;
        public float acceleration;
        public bool slowDownToTarget;
        public float slowDownRange;

        private void Awake()
        {
            if (calculatePathComponent == null)
            {
                Debug.LogError($"Error: CalculatePathComponent is null. This class must have one");
            }

            CurrentState = transitions[0].targetState;
            CurrentPhase = StatePhases.ENTER;
        }

        void Update()
        {
            switch (CurrentPhase)
            {
                case StatePhases.ENTER:
                    CurrentState.OnEnter();
                    CurrentPhase = StatePhases.UPDATE;
                    break;
                case StatePhases.UPDATE:
                    CurrentState.Update();
                    break;
                case StatePhases.EXIT:
                    CurrentState.OnExit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Used to receive and handle commands.
        /// </summary>
        /// <param name="commandType">Used to determine what type of command is sent.</param>
        /// <param name="args">The arguments that is needed to handle a command, like a point in world space or a speed parameter.</param>
        public void HandleCommand(CommandType commandType, params object[] args)
        {
            switch (commandType)
            {
                case CommandType.TRIGGER_STATE:
                {
                    if (args[0] is string transitionKey)
                    {
                        BaseState nextState = TransitionToState(transitionKey);
                        object[] arguments = new object[args.Length - 1];
                        Array.Copy(args, 1, arguments, 0, args.Length);

                    }
                    break;
                }

                //case CommandType.MOVE_TO:
                //{
                //    if (args is Vector3 point)
                //    {
                //        List<Vector3> path = calculatePathComponent.GetPath(transform.position, point);
                //        if (path != null)
                //        {
                //            //StopAllCoroutines();
                //            //StartCoroutine(MoveAlongPath(path));
                //        }
                //    }
                //    else
                //    {
                //        Debug.Log($"Error: command is not type Vector3. It is type: {args.GetType()}");
                //    }

                //    break;
                //}
                //case CommandType.WAIT:
                //{
                //    //StopAllCoroutines();
                //    break;
                //}
                //case CommandType.SET_MAX_SPEED:
                //{
                //    if (args is float speed)
                //    {
                //        maxSpeed = speed;
                //    }
                //    else
                //    {
                //        Debug.Log($"Error: command is not type Float. It is type: {args.GetType()}");
                //    }

                //    break;
                //}
                //case CommandType.SET_MIN_SPEED:
                //{
                //    if (args is float speed)
                //    {
                //        minSpeed = speed;
                //    }
                //    else
                //    {
                //        Debug.Log($"Error: command is not type Float. It is type: {args.GetType()}");
                //    }

                //    break;
                //}
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public BaseState TransitionToState(string trigger)
        {
            return transitions.First(t => t.key == trigger).targetState;
        }

        //private IEnumerator MoveAlongPath(List<Vector3> path)
        //{
        //    Vector3 pos = transform.position;
        //    float speed = 0;

        //    while (path.Count > 0)
        //    {
        //        Vector3 delta = path[0] - pos;
        //        Vector3 dir = delta.normalized;

        //        speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);

        //        // Check to see if should slow down to the target
        //        if (slowDownToTarget && path.Count == 1 && delta.sqrMagnitude < slowDownRange * slowDownRange)
        //        {
        //            float fractionWithinRange = delta.magnitude / slowDownRange;
        //            speed = fractionWithinRange * maxSpeed;
        //        }

        //        if (speed < minSpeed)
        //            speed = minSpeed;

        //        Vector3 velocity = dir * speed;
        //        pos += velocity * Time.deltaTime;

        //        transform.position = pos;

        //        if (delta.sqrMagnitude <= 0.01f)
        //        {
        //            path.RemoveAt(0);
        //        }

        //        yield return null;
        //    }
        //}

        //private void DrawPathDebug(IReadOnlyList<Vector3> path)
        //{
        //    for (int i = 0; i < path.Count - 1; i++)
        //    {
        //        Debug.DrawLine(path[i], path[i + 1], Color.blue, 5.0f, false);
        //    }
        //}
    }
}
