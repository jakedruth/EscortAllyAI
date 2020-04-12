using System.Globalization;
using UnityEngine;

[System.Serializable]
public struct UseFloat
{
    public bool use;
    public float value;

    /// <summary>
    /// Custom class that is used to determine if a value should be used
    /// </summary>
    /// <param name="use">A toggle to determine if to use the float {value}</param>
    /// <param name="value">The float value</param>
    public UseFloat(bool use, float value)
    {
        this.use = use;
        this.value = value;
    }

    /// <summary>
    /// Convert UseFloat to a bool
    /// </summary>
    /// <param name="uf">shorthand for UseFloat</param>
    public static implicit operator bool(UseFloat uf) => uf.use;

    /// <summary>
    /// Used to determine which value to use
    /// </summary>
    /// <param name="other">The other value to use if <code>use</code> is set to false</param>
    /// <returns>returns {value} if {use} is <code>true</code>, else returns {other}</returns>
    public float DetermineWhichValue(float other)
    {
        return use ? value : other;
    }
}

/// <summary>
/// Attribute to change the word "Value" in the inspector
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
public class ValueName : PropertyAttribute
{
    public string label;
    public ValueName(string name)
    {
        // Convert the name to "Title Case"
        label = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}:";
    }
}
