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

        [NullWarning("This component is required for calculating paths and cannot be left null.")]
        public CalculatePathComponent calculatePathComponent;

        [NullWarning("This field is used for moving a cursor transform for debugging purposes. This field CAN be left null if desired")]
        public Transform cursorTransform;

        public float maxSpeed = 4f;
        public float acceleration = 50f;
        [ValueName("Range")]
        public UseFloat useSmoothStopping = new UseFloat(true, 0.75f);

        private Vector3 _groundVelocity;
        private Vector3 _verticalVelocity;

        public float TargetSpeed
        {
            get { return CurrentState.overrideSpeed.DetermineWhichValue(maxSpeed); }
        }
        public float TargetAcceleration
        {
            get { return CurrentState.overrideAcceleration.DetermineWhichValue(acceleration); }
        }

        [NullWarning("This string must contain the name of a state. Additionally, the state must also be located in Resources/States folder")]
        public string defaultState = "Idle";

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

            // Get the state's input
            Vector3 input = CurrentState.Input;

            // calculate the target velocity
            Vector3 targetVelocity = input * TargetSpeed;

            // set the ground velocity
            _groundVelocity = Vector3.MoveTowards(_groundVelocity, targetVelocity, TargetAcceleration * Time.deltaTime);
            
            // Apply gravity
            if (CharacterController.isGrounded)
                _verticalVelocity = Vector3.zero;

            _verticalVelocity += Physics.gravity * Time.deltaTime;

            CharacterController.Move((_groundVelocity + _verticalVelocity) * Time.deltaTime);
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
            instance.name = stateName;
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
