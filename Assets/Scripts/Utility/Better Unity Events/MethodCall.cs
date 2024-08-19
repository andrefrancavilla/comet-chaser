using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class BetterEvent
{
    public MethodCall[] methodCalls;

    public void Invoke()
    {
        foreach (MethodCall call in methodCalls)
        {
            call?.Invoke();
        }
    }
}

[System.Serializable]
public class MethodCall
{
    public UnityEngine.Object target; // Changed to UnityEngine.Object for general use
    public string methodName;    // Name of the method to call

    public int selectedEnumIndex;
    public float floatValue;
    public int intValue;
    public string stringValue;
    public bool boolValue;

    // To be used in the custom property drawer
    [NonSerialized]
    public object[] parameters;

    public void AssignParameters(params object[] parameters)
    {
        this.parameters = parameters;
    }

    public void Invoke()
    {
        if (target == null) return;

        MethodInfo method = target.GetType().GetMethod(methodName);
        if (method == null) return;

        var parameterInfos = method.GetParameters();
        object[] parameterValues = new object[parameterInfos.Length];

        for (int i = 0; i < parameterInfos.Length; i++)
        {
            var paramType = parameterInfos[i].ParameterType;

            if (paramType.IsEnum)
            {
                var enumValues = Enum.GetValues(paramType);
                parameterValues[i] = enumValues.GetValue(selectedEnumIndex);
            }
            else if (paramType == typeof(float))
            {
                parameterValues[i] = floatValue;
            }
            else if (paramType == typeof(int))
            {
                parameterValues[i] = intValue;
            }
            else if (paramType == typeof(string))
            {
                parameterValues[i] = stringValue;
            }
            else if (paramType == typeof(bool))
            {
                parameterValues[i] = boolValue;
            }
            else
            {
                Debug.LogWarning($"Unsupported parameter type: {paramType.Name}");
                return;
            }
        }

        method.Invoke(target, parameterValues);
    }
}