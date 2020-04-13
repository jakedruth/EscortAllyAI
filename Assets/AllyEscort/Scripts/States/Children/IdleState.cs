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
        { Input = Vector3.zero; }

        protected override void HandleUpdate()
        { }

        protected override void HandleOnExit()
        { }

        public override void HandleDebugCursorPosition()
        {
            Owner.cursorTransform.position = Position;
        }
    }
}
