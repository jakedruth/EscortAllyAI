using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Initialize(EscortAgent owner, params object[] args)
        {
            Debug.Log($"Initializing state ({name})");
            Owner = owner;
            Args = args;
            _isInitialized = true;
            HandleInitialize();
        }

        public void OnEnter()
        {
            Debug.Log($"On Enter State ({name})");

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
        }

        public void OnExit()
        {
            Debug.Log($"On Exit State ({name})");

            _isInitialized = false;
            HandleOnExit();
        }

        internal abstract void HandleInitialize();
        internal abstract void HandleOnEnter();
        internal abstract void HandleUpdate();
        internal abstract void HandleOnExit();
    }
}
