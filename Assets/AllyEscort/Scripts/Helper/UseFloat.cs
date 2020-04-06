using System.Globalization;
using UnityEngine;

[System.Serializable]
public struct UseFloat
{
    public bool use;
    public float value;

    public UseFloat(bool use, float value)
    {
        this.use = use;
        this.value = value;
    }

    public static implicit operator bool(UseFloat uf) => uf.use;

    public float DetermineWhichValue(float other)
    {
        return use ? value : other;
    }
}

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ValueName : PropertyAttribute
{
    public string label;
    public ValueName(string name)
    {
        // Convert the name to title case
        label = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}:";
    }
}
