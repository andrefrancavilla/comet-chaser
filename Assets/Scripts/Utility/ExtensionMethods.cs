using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class ExtensionMethods
    {
        public static T GetRandom<T>(this List<T> list)
        {
            int rngIndex = Random.Range(0, list.Count);
            return list[rngIndex];
        }
    }
}