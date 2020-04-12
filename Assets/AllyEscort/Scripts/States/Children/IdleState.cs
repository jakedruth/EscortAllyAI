using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class IdleState : State
    {
        protected override bool HandleInitialize()
        {
            return true;
        }

        protected override void HandleOnEnter()
        { }

        protected override void HandleUpdate()
        {
            // make sure the Owner does not move
            Owner.MoveToPoint(startingPosition);
        }

        protected override void HandleOnExit()
        { }
    }
}
