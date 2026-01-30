using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
[CustomEditor(typeof(InspectorAttributes), true)]
public class InspectorAttributesEditor : Editor
{

    private List<MethodInfo> _buttonMethods;
    private List<FieldInfo> _helpBoxFields;
    private List<FieldInfo> _defaultFields;
    private void OnEnable()
    {
        CacheButtonMethods();
        CacheHelpBoxFields();
        CacheAllFields();
    }
    public override void OnInspectorGUI()
    {
        if (_buttonMethods == null)
        {
            CacheButtonMethods();
        }

        if (_buttonMethods != null && _buttonMethods.Count > 0)
        {
            foreach (var method in _buttonMethods)
            {
                var attr = (MethodButtonAttribute)method.GetCustomAttribute(typeof(MethodButtonAttribute));
                if (GUILayout.Button(attr.Name))
                {
                    method.Invoke(target, null);
                }
            }
        }
        if (_helpBoxFields != null && _helpBoxFields.Count > 0)
        {
            foreach (var field in _helpBoxFields)
            {
                var attr = (HelpBoxAttribute)field.GetCustomAttribute(typeof(HelpBoxAttribute));
                EditorGUILayout.HelpBox(attr.Text, MessageType.Info);
            }
        }
        var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var value = field.GetValue(target);
            var attr = field.GetCustomAttribute<RequiredFieldAttribute>();
            if (attr != null && (value == null || (value is UnityEngine.Object uo && uo == null)))
            {
                EditorGUILayout.HelpBox(attr.Message, MessageType.Error);
            }
        }
        base.OnInspectorGUI();
    }

    private void CacheButtonMethods()
    {
        List<MethodInfo> buttons = new List<MethodInfo>();
        var methods = target.GetType().GetMethods();
        foreach (var method in methods)
        {
            if (method.GetCustomAttribute(typeof(MethodButtonAttribute)) != null)
                buttons.Add(method);
        }
        _buttonMethods = buttons;
    }
    private void CacheHelpBoxFields()
    {
        List<FieldInfo> helpBoxes = new List<FieldInfo>();
        var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute(typeof(HelpBoxAttribute)) != null)
                helpBoxes.Add(field);
        }
        _helpBoxFields = helpBoxes;
    }
    private void CacheAllFields()
    {
        List<FieldInfo> defaultFields = new List<FieldInfo>();
        var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute(typeof(RequiredFieldAttribute)) == null)
                defaultFields.Add(field);
        }
        _defaultFields = defaultFields;
    }
}
