using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class InteractWithObjectState : MoveToPointState
    {
        private InteractableObject _interactableObject;
        [SerializeField]
        public float interactWaitTimer;
        [SerializeField]
        private float _timer;

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

        internal override void HandleOnEnter()
        {
            base.HandleOnEnter();

            _timer = 0;
        }

        internal override void HandleEmptyPath()
        {
            _timer += Time.deltaTime;
            if (_timer >= interactWaitTimer)
            {
                base.HandleEmptyPath();
                _interactableObject.Interact();
            }
        }
    }
}