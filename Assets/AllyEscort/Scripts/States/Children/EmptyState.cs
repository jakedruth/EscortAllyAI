using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    /// <summary>
    /// Used for an empty state. This does nothing at all.
    /// </summary>
    [CreateAssetMenu(fileName = "New State", menuName = "Ally Escort/Create New State", order = 2)]
    public class EmptyState : State
    {
        protected override bool HandleInitialize()
        {
            return true;
        }

        protected override void HandleOnEnter()
        { }

        protected override void HandleUpdate()
        { }

        protected override void HandleOnExit()
        { }
    }
}
