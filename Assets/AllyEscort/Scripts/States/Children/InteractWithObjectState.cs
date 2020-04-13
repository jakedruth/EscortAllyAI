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

        protected override bool HandleInitialize()
        {
            if (Args[0] is InteractableObject interactable)
            {
                _interactableObject = interactable;
                CalculatePath(_interactableObject.transform.position);
                return true;
            }

            return false;
        }

        protected override void HandleOnEnter()
        {
            base.HandleOnEnter();

            _timer = 0;
        }

        /// <summary>
        /// While the path is empty, update a timer to when the interaction with the object occurs.
        /// </summary>
        protected override void HandleEmptyPath()
        {
            Input = Vector3.zero;
            _timer += Time.deltaTime;
            if (_timer >= interactWaitTimer)
            {
                base.HandleEmptyPath();
                _interactableObject.Interact();
            }
        }
    }
}