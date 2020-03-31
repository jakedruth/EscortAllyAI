using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class InteractWithObjectState : MoveToPointState
    {
        private InteractableObject _interactableObject;

        internal override bool HandleInitialize()
        {
            if (Args[0] is InteractableObject interactable)
            {
                _interactableObject = interactable;
                CalculatePath(_interactableObject.transform.position);
                return true;
            }

            return false;
        }

        internal override void HandleOnExit()
        {
            base.HandleOnExit();
            _interactableObject.Interact();
        }
    }
}