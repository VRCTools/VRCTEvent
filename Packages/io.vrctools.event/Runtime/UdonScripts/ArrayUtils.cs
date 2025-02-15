// Copyright 2025 .start <https://dotstart.tv>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;

namespace VRCTools.Event {
  
  /// <summary>
  /// Provides a small set of utility functions for interactions with arrays.
  /// </summary>
  internal static class ArrayUtils {

    /// <summary>
    /// Locates an array element within the given array.
    ///
    /// When no matching entry exists within the array, -1 is returned.
    /// </summary>
    /// <param name="source">a source array</param>
    /// <param name="element">a target element</param>
    /// <param name="offset">a search offset within the array</param>
    /// <typeparam name="T">an array type</typeparam>
    /// <returns>the element's index within the given array or, if no matching element is found, -1</returns>
    public static int FindElement<T>(T[] source, T element, int offset = 0) {
      for (var i = offset; i < source.Length; i++) {
        var existing = source[i];
        if ((element == null) != (existing == null) || existing == null || !existing.Equals(element)) {
          continue;
        }

        return i;
      }
      return -1;
    }

    /// <summary>
    /// Creates a copy of the given array with the specified element appended at its end.
    /// </summary>
    /// <param name="source">a source array</param>
    /// <param name="element">an element</param>
    /// <typeparam name="T">an array type</typeparam>
    /// <returns>a new mutated array</returns>
    public static T[] AddElement<T>(T[] source, T element) {
      var newArray = new T[source.Length + 1];
      Array.Copy(source, newArray, source.Length);
      newArray[source.Length] = element;
      return newArray;
    }

    /// <summary>
    /// Creates a copy of the given array with the element at the given index removed.
    /// </summary>
    /// <param name="source">a source array</param>
    /// <param name="i">an element index</param>
    /// <typeparam name="T">an array type</typeparam>
    /// <returns>a mutated array</returns>
    public static T[] RemoveElement<T>(T[] source, int i) {
      var newArray = new T[source.Length - 1];

      for (var j = 0; j < newArray.Length; j++) {
        if (j == i) {
          continue;
        }
        
        var existing = source[i];
        if (j < i) {
          newArray[j] = existing;
        } else {
          newArray[j - 1] = existing;
        }
      }

      return newArray;
    }
  }
}
