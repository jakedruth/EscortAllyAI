using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;

namespace AllyEscort
{
    [Serializable]
    public struct Transition
    {
        public string key;
        public State targetState;
    }

    [RequireComponent(typeof(CharacterController))]
    public class EscortAgent : MonoBehaviour
    {
        public CharacterController CharacterController { get; private set; }
        private Vector3 _verticalVelocity;

        public CalculatePathComponent calculatePathComponent;
        public Transform cursorTransform;

        public string defaultState;
        public List<State> states;

        public State CurrentState { get; private set; }
        public State NextState { get; private set; }
        public StatePhases CurrentPhase { get; private set; }

        private void Awake()
        {
            CharacterController = GetComponent<CharacterController>();

            if (calculatePathComponent == null)
            {
                Debug.LogError($"<b><color=red>Error:</color></b> Calculate Path Component is null. This class must have access to one", this);
                return;
            }

            if (states == null || states.Count == 0)
            {
                Debug.LogError($"<b><color=red>Error:</color></b> There are no states in this state machine", this);
                return;
            }

            State firstState = GetState(defaultState);

            CurrentState = firstState != null ? firstState : states[0];
            CurrentState.Initialize(this);
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
                    TransitionToNextState();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (CharacterController.isGrounded)
                _verticalVelocity = Vector3.zero;

            _verticalVelocity += Physics.gravity * Time.deltaTime;

            CharacterController.Move(_verticalVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Used to receive and handle commands.
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="args">The arguments that is needed to handle a command, like a point in world space or a currentSpeed parameter.</param>
        public bool TransitionToState(string stateName, params object[] args)
        {
            NextState = GetState(stateName);
            if (NextState == null)
            {
                return false;
            }

            // TODO: handle better if the same state. Good for now.
            if (CurrentState == NextState) 
            {
                CurrentState.Initialize(this, args);
                CurrentPhase = StatePhases.ENTER;
            }
            else
            {
                NextState.Initialize(this, args);
                CurrentPhase = StatePhases.EXIT;
            }

            return true;
        }

        public State GetState(string stateName)
        {
            try
            {
                return states.First(s => s.name == stateName);
            }
            catch
            {
                Debug.LogError($"Could not find state {stateName}.");
                return null;
            }

        }

        private void TransitionToNextState()
        {
            if (NextState == null) 
                return;

            CurrentState = NextState;
            CurrentPhase = StatePhases.ENTER;
        }

        public void SetCursorPosition(Vector3 position)
        {
            if (cursorTransform != null)
            {
                cursorTransform.position = position;
            }
        }

        //private IEnumerator MoveAlongPath(List<Vector3> path)
        //{
        //    Vector3 pos = transform.position;
        //    float currentSpeed = 0;

        //    while (path.Count > 0)
        //    {
        //        Vector3 delta = path[0] - pos;
        //        Vector3 dir = delta.normalized;

        //        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        //        // Check to see if should slow down to the target
        //        if (slowDownToTarget && path.Count == 1 && delta.sqrMagnitude < slowDownRange * slowDownRange)
        //        {
        //            float fractionWithinRange = delta.magnitude / slowDownRange;
        //            currentSpeed = fractionWithinRange * maxSpeed;
        //        }

        //        if (currentSpeed < minSpeed)
        //            currentSpeed = minSpeed;

        //        Vector3 velocity = dir * currentSpeed;
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
