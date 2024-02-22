using UnityEngine;

namespace Game.Extensions {
    public static class ArrayExtensions {
        public static T RandomElement<T>(this T[] array) {
            if (array.Length == 0) return default;
            return array[Random.Range(0, array.Length)];
        }
    }
}