using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace AllyEscort
{
    public enum StatePhases
    {
        ENTER,
        UPDATE,
        EXIT
    }

    public abstract class State : ScriptableObject
    {
        public EscortAgent Owner { get; private set; }
        internal object[] Args { get; private set; }
        private bool _isInitialized;
        internal Vector3 startingPosition;

        [SerializeField]
        public UseFloat overrideSpeed;
        [SerializeField]
        public UseFloat overrideAcceleration;

        /// <summary>
        /// Used to initialize the current state
        /// </summary>
        /// <param name="owner">Used to keep reference to the escort agent that owns this state</param>
        /// <param name="args">Any arguments that need to be passed on to the state for initializing</param>
        public void Initialize(EscortAgent owner, params object[] args)
        {
            //Debug.Log($"Initializing state ({name})");
            Owner = owner;
            Args = args;
            startingPosition = Owner.transform.position;

            // Check to see if the inheriting class successfully initialized
            if (!HandleInitialize())
            {
                throw new ArgumentException($"Initialize for state {name}");
            }

            _isInitialized = true;
        }

        /// <summary>
        /// Called when entering the state for the first time
        /// </summary>
        public void OnEnter()
        {
            //Debug.Log($"On Enter State ({name})");

            if (!_isInitialized)
            {
                string error = $"State {name} has not been initialized";
                throw new NotImplementedException(error);
            }

            HandleOnEnter();
        }

        /// <summary>
        /// Called to update the state every frame
        /// </summary>
        public void Update()
        {
            //Debug.Log($"On Update State ({name})");

            HandleUpdate();
            SetDebugCursorPosition();
        }


        /// <summary>
        /// Called when exiting the state
        /// </summary>
        public void OnExit()
        {
            //Debug.Log($"On Exit State ({name})");

            _isInitialized = false;
            HandleOnExit();
        }

        /// <summary>
        /// Set the debug cursor's position
        /// </summary>
        public virtual void SetDebugCursorPosition()
        {
            Owner.cursorTransform.position = startingPosition;
        }

        /// <summary>
        /// Initialize function that must be overloaded by inherited classes
        /// </summary>
        /// <returns>Return <code>true</code> if initialized properly</returns>
        protected abstract bool HandleInitialize();

        /// <summary>
        /// On Enter function that must be overloaded by inherited classes
        /// </summary>
        protected abstract void HandleOnEnter();

        /// <summary>
        /// Update function that must be overloaded by inherited classes
        /// </summary>
        protected abstract void HandleUpdate();

        /// <summary>
        /// On Exit function that must be overloaded by inherited classes
        /// </summary>
        protected abstract void HandleOnExit();
    }
}
