using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Events;

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

        public static bool NullTest(object testObject)
        {
            return testObject == null;
        }

        [NullWarning("This component is required for calculating paths.")]
        public CalculatePathComponent calculatePathComponent;

        [NullWarning]
        public Transform cursorTransform;

        public float maxSpeed;
        public float minSpeed;
        public float acceleration;
        [ValueName("Range")]
        public UseFloat useSmoothStopping;

        public float Speed { get; private set; }

        private Vector3 _verticalVelocity;

        [NullWarning("The state must also be located in Resources/States")]
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

            // initialize the first state
            CurrentState = firstState;
            CurrentState.Initialize(this);
            CurrentPhase = StatePhases.ENTER;
        }

        private void Update()
        {
            // update the current state based on the phase of the state
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

            // Apply gravity
            if (CharacterController.isGrounded)
                _verticalVelocity = Vector3.zero;

            _verticalVelocity += Physics.gravity * Time.deltaTime;

            CharacterController.Move(_verticalVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Used by the states to move the Escort Agent
        /// </summary>
        /// <param name="target">The point in world space to move towards</param>
        /// <param name="distanceToEnd">Override the distance to the target, if necessary. If less than 0,
        ///     the distance will be calculated to the target. It is necessary to override the distance
        ///     if the target being supplied is not the end goal, like a path.
        /// </param>
        /// <returns>return the displacement to the target</returns>
        public Vector3 MoveToPoint(Vector3 target, float distanceToEnd = -1)
        {
            // set up variables
            Vector3 pos = transform.position;
            Vector3 delta = target - pos;
            Vector3 direction = delta.normalized;

            // calculate speed and acceleration. Can be overloaded by a state
            float targetSpeed = CurrentState.overrideSpeed.DetermineWhichValue(maxSpeed);
            float targetAcceleration = CurrentState.overrideAcceleration.DetermineWhichValue(acceleration);
            Speed = Mathf.MoveTowards(Speed, targetSpeed, targetAcceleration * Time.deltaTime);

            // Determine if slowing down needs to apply
            if (useSmoothStopping)
            {
                // get the distance value
                float distance = (distanceToEnd < 0) ? delta.magnitude : distanceToEnd;
                if (distance <= useSmoothStopping.value)
                {
                    // adjust the speed based on the percentage to the target
                    float percentage = distance / useSmoothStopping.value;
                    Speed = percentage * maxSpeed;
                }
            }

            // Make sure the speed never drops below the minimum value
            if (Speed < minSpeed)
                Speed = minSpeed;

            // calcualte velocity
            Vector3 velocity = direction * Speed;
            Vector3 step = velocity * Time.deltaTime;

            // if the step in one frame is larger than the distance to the target,
            // prevent the Escort Agent from over stepping
            if (step.sqrMagnitude > delta.sqrMagnitude)
            {
                step = delta;
            }

            // Move the Escort Agent via the Character Controller
            CharacterController.Move(step);

            // return the displacement to the target
            return delta;
        }

        /// <summary>
        /// Used to transition to another state
        /// </summary>
        /// <param name="stateName">The name of the state to transition too</param>
        /// <param name="args">The arguments that is needed to handle a command, like a point in world space or a transform to follow.</param>
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

        /// <summary>
        /// Get a state
        /// </summary>
        /// <param name="stateName">The name of the state to get</param>
        /// <returns></returns>
        public State GetState(string stateName)
        {
            State state = Resources.Load<State>($"States/{stateName}");
            if (state == null)
                return null;

            State instance = Instantiate(state);

            return instance;
        }

        /// <summary>
        /// Called when a state exits and need to load the next state
        /// </summary>
        private void TransitionToNextState()
        {
            if (NextState == null) 
                return;

            CurrentState = NextState;
            CurrentPhase = StatePhases.ENTER;
            NextState = null;
        }

        /// <summary>
        /// Used for a state debugging. Allows the state to say where the state is moving towards
        /// </summary>
        /// <param name="position">Moves the cursor to the position.</param>
        public void SetCursorPosition(Vector3 position)
        {
            if (cursorTransform != null)
            {
                cursorTransform.position = position;
            }
        }
    }
}
