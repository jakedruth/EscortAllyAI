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

    public abstract class BaseState : ScriptableObject
    {
        public abstract void Initialize(params object[] args);

        public abstract void OnEnter();

        public abstract void Update();

        public abstract void OnExit();
    }
}
