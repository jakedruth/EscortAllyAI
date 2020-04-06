﻿using System;
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

        public CalculatePathComponent calculatePathComponent;
        public Transform cursorTransform;

        public float maxSpeed;
        public float minSpeed;
        public float acceleration;
        [ValueName("Range")]
        public UseFloat useSmoothStopping;

        public float Speed { get; private set; }

        private Vector3 _verticalVelocity;

        public string defaultState;

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

            State firstState = GetState(defaultState);
            if (firstState == null)
            {
                Debug.LogError($"<b><color=red>Error:</color></b> Could not find the default state, {defaultState}", this);
                return;
            }

            CurrentState = firstState;
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

        public Vector3 MoveToPoint(Vector3 target, float distanceToEnd = -1)
        {
            Vector3 pos = transform.position;
            Vector3 delta = target - pos;
            Vector3 direction = delta.normalized;

            float targetSpeed = CurrentState.overrideSpeed.DetermineWhichValue(maxSpeed);
            float targetAcceleration = CurrentState.overrideAcceleration.DetermineWhichValue(acceleration);

            Speed = Mathf.MoveTowards(Speed, targetSpeed, targetAcceleration * Time.deltaTime);

            if (useSmoothStopping)
            {
                float distance = (distanceToEnd < 0) ? delta.magnitude : distanceToEnd;
                if (distance <= useSmoothStopping.value)
                {
                    float percentage = distance / useSmoothStopping.value;
                    Speed = percentage * maxSpeed;
                }
            }

            if (Speed < minSpeed)
                Speed = minSpeed;

            Vector3 velocity = direction * Speed;
            Vector3 step = velocity * Time.deltaTime;

            if (step.sqrMagnitude > delta.sqrMagnitude)
            {
                step = delta;
            }

            CharacterController.Move(step);

            return delta;
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
            State state = Resources.Load<State>($"States/{stateName}");
            State instance = Instantiate(state);

            return instance;
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
        //        if (useSmoothStop && path.Count == 1 && delta.sqrMagnitude < slowDownRange * slowDownRange)
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
