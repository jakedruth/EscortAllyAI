using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class EmptyState : State
    {
        internal override bool HandleInitialize()
        {
            return true;
        }

        internal override void HandleOnEnter()
        { }

        internal override void HandleUpdate()
        { }

        internal override void HandleOnExit()
        { }
    }
}
