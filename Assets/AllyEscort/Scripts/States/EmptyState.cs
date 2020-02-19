using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : ScriptableObject
{
    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void OnExit() { }
}
