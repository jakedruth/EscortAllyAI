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

        public void Initialize(EscortAgent owner, params object[] args)
        {
            //Debug.Log($"Initializing state ({name})");
            Owner = owner;
            Args = args;
            startingPosition = Owner.transform.position;

            if (!HandleInitialize())
            {
                throw new ArgumentException($"Initialize for state {name}");
            }

            _isInitialized = true;
        }

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

        public void Update()
        {
            //Debug.Log($"On Update State ({name})");

            HandleUpdate();
            SetDebugCursorPosition();
        }

        public void OnExit()
        {
            //Debug.Log($"On Exit State ({name})");

            _isInitialized = false;
            HandleOnExit();
        }

        public virtual void SetDebugCursorPosition()
        {
            Owner.cursorTransform.position = startingPosition;
        }

        internal abstract bool HandleInitialize();
        internal abstract void HandleOnEnter();
        internal abstract void HandleUpdate();
        internal abstract void HandleOnExit();
    }
}
