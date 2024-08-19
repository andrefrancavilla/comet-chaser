using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

[CustomPropertyDrawer(typeof(MethodCall))]
public class MethodCallPropertyDrawer : PropertyDrawer
{
    private SerializedProperty targetProperty;
    private SerializedProperty methodNameProperty;
    private SerializedProperty selectedEnumIndexProperty;
    private SerializedProperty floatValueProperty;
    private SerializedProperty intValueProperty;
    private SerializedProperty stringValueProperty;
    private SerializedProperty boolValueProperty;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Initialize properties
        targetProperty = property.FindPropertyRelative("target");
        methodNameProperty = property.FindPropertyRelative("methodName");
        selectedEnumIndexProperty = property.FindPropertyRelative("selectedEnumIndex");
        floatValueProperty = property.FindPropertyRelative("floatValue");
        intValueProperty = property.FindPropertyRelative("intValue");
        stringValueProperty = property.FindPropertyRelative("stringValue");
        boolValueProperty = property.FindPropertyRelative("boolValue");

        var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        // Draw target
        EditorGUI.PropertyField(rect, targetProperty, new GUIContent("Target"));
        rect.y += EditorGUIUtility.singleLineHeight;

        if (targetProperty.objectReferenceValue != null)
        {
            var targetObject = targetProperty.objectReferenceValue as MonoBehaviour;
            if (targetObject != null)
            {
                var methods = targetObject.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => m.GetParameters().Length > 0)
                    .ToArray();

                var methodNames = methods.Select(m => m.Name).ToArray();
                int selectedMethodIndex = Array.IndexOf(methodNames, methodNameProperty.stringValue);

                // Draw method selection
                selectedMethodIndex = EditorGUI.Popup(rect, "Method", Mathf.Clamp(selectedMethodIndex, 0, methodNames.Length - 1), methodNames);
                methodNameProperty.stringValue = methodNames[selectedMethodIndex];
                rect.y += EditorGUIUtility.singleLineHeight;

                if (methodNames.Length > 0 && selectedMethodIndex >= 0)
                {
                    var selectedMethod = methods[selectedMethodIndex];
                    var parameterTypes = selectedMethod.GetParameters().Select(p => p.ParameterType).ToArray();

                    // Draw parameters
                    for (int i = 0; i < parameterTypes.Length; i++)
                    {
                        var paramType = parameterTypes[i];
                        var paramRect = new Rect(position.x, rect.y, position.width, EditorGUIUtility.singleLineHeight);

                        if (paramType.IsEnum)
                        {
                            var enumValues = Enum.GetValues(paramType);
                            var enumNames = Enum.GetNames(paramType);
                            selectedEnumIndexProperty.intValue = EditorGUI.Popup(paramRect, $"Parameter {i} ({paramType.Name})", selectedEnumIndexProperty.intValue, enumNames);
                            rect.y += EditorGUIUtility.singleLineHeight;
                        }
                        else if (paramType == typeof(float))
                        {
                            floatValueProperty.floatValue = EditorGUI.FloatField(paramRect, $"Parameter {i} ({paramType.Name})", floatValueProperty.floatValue);
                            rect.y += EditorGUIUtility.singleLineHeight;
                        }
                        else if (paramType == typeof(int))
                        {
                            intValueProperty.intValue = EditorGUI.IntField(paramRect, $"Parameter {i} ({paramType.Name})", intValueProperty.intValue);
                            rect.y += EditorGUIUtility.singleLineHeight;
                        }
                        else if (paramType == typeof(string))
                        {
                            stringValueProperty.stringValue = EditorGUI.TextField(paramRect, $"Parameter {i} ({paramType.Name})", stringValueProperty.stringValue);
                            rect.y += EditorGUIUtility.singleLineHeight;
                        }
                        else if (paramType == typeof(bool))
                        {
                            boolValueProperty.boolValue = EditorGUI.Toggle(paramRect, $"Parameter {i} ({paramType.Name})", boolValueProperty.boolValue);
                            rect.y += EditorGUIUtility.singleLineHeight;
                        }
                        else
                        {
                            EditorGUI.LabelField(paramRect, $"Unsupported type: {paramType.Name}");
                            rect.y += EditorGUIUtility.singleLineHeight;
                        }
                    }
                }
                else
                {
                    EditorGUI.LabelField(rect, "No methods available.");
                    rect.y += EditorGUIUtility.singleLineHeight;
                }
            }
        }
        else
        {
            EditorGUI.LabelField(rect, "Select a target object.");
            rect.y += EditorGUIUtility.singleLineHeight;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight; // For target field
        if (targetProperty == null) return height;

        try
        {
            if (targetProperty.objectReferenceValue != null)
            {
                var targetObject = targetProperty.objectReferenceValue as MonoBehaviour;
                if (targetObject != null)
                {
                    var methods = targetObject.GetType()
                        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(m => m.GetParameters().Length > 0)
                        .ToArray();

                    var methodNames = methods.Select(m => m.Name).ToArray();
                    height += EditorGUIUtility.singleLineHeight; // For method dropdown

                    if (methodNames.Length > 0)
                    {
                        var selectedMethodIndex = Array.IndexOf(methodNames, methodNameProperty.stringValue);
                        if (selectedMethodIndex >= 0 && selectedMethodIndex < methods.Length)
                        {
                            var selectedMethod = methods[selectedMethodIndex];
                            var parameterTypes = selectedMethod.GetParameters().Select(p => p.ParameterType).ToArray();
                            height += parameterTypes.Length * EditorGUIUtility.singleLineHeight; // Estimate height for parameters
                        }
                    }
                    else
                    {
                        height += EditorGUIUtility.singleLineHeight; // For "No methods available" label
                    }
                }
            }
            else
            {
                height += EditorGUIUtility.singleLineHeight; // For "Select a target object" label
            }
        }
        catch (Exception)
        {
            return height;
        }
        

        return height;
    }
}
