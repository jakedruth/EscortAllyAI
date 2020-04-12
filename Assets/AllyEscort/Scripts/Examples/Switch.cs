using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort.Example
{
    public class Switch : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public Material switchOffMaterial;
        public Material switchOnMaterial;

        public bool IsOn { get; private set; }

        /// <summary>
        /// Public toggle to the switch
        /// </summary>
        [ContextMenu("Toggle the switch")]
        public void ToggleSwitch()
        {
            SetSwitch(!IsOn);
        }

        /// <summary>
        /// Set the switch to on or off
        /// </summary>
        /// <param name="value">Set the switch</param>
        public void SetSwitch(bool value)
        {
            if(IsOn == value)
                return;
            IsOn = value;

            // Change a material based on if the switch is on or off
            meshRenderer.material = IsOn ? switchOnMaterial : switchOffMaterial;

            // This class could be used to do a multitude of other things like open a door, lower a bridge, turn off a light, ect.
        }
    }
}
