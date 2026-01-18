using System;
using System.Collections.Generic;

public static class ListExtension {
    public static int UpperBound<T>(this IList<T> list, T value, IComparer<T> comparer = null) {
        if (list == null) throw new ArgumentNullException(nameof(list));
        comparer ??= Comparer<T>.Default;

        int left = 0;
        int right = list.Count;

        while (left < right) {
            int mid = left + ((right - left) >> 1);
            if (comparer.Compare(list[mid], value) <= 0)
                left = mid + 1;
            else
                right = mid;
        }

        return left;
    }

}
