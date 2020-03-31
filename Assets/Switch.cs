using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material switchOffMaterial;
    public Material switchOnMaterial;

    private bool _isOn;
    public bool IsOn
    {
        get { return _isOn; }
        set
        {
            if (_isOn == value)
                return;
            _isOn = value;

            meshRenderer.material = _isOn ? switchOnMaterial : switchOffMaterial;
        }
    }

    [ContextMenu("Toggle the switch")]
    public void ToggleSwitch()
    {
        IsOn = !IsOn;
    }
}
