using System;
using UnityEngine;

public class InspectorAttributes : MonoBehaviour
{

}

[AttributeUsage(AttributeTargets.Method)]
public class MethodButtonAttribute : Attribute
{
    public string Name { get; private set; }

    public MethodButtonAttribute(string name)
    {
        Name = name;
    }
}
public class HelpBoxAttribute : Attribute
{
    public string Text { get; private set; }

    public HelpBoxAttribute(string text)
    {
        Text = text;
    }
}
public class RequiredFieldAttribute : Attribute
{
    public string Message { get; private set; }
    public RequiredFieldAttribute(string message = "This reference is required.")
    {
        Message = message;
    }
}
