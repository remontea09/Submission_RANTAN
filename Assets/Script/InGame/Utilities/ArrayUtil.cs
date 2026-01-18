using System;

namespace Utilities {
    public static class ArrayUtil {
        public static T[,] Skew45<T>(T[,] map, Direction direction) {
            if (direction == Direction.None) return map;

            T[,] skewMap = (T[,])map.Clone();
            int skewCount = Array.IndexOf(Direction.EightDirections, direction);
            int n = skewMap.GetLength(0);
            int rings = n / 2;
            for (int k = 0; k < rings; k++) {
                int top = k;
                int left = k;
                int bottom = n - 1 - k;
                int right = n - 1 - k;

                int perimeter = (right - left + bottom - top) * 2;
                if (perimeter == 0) continue;

                int shift = (rings - k) * skewCount;

                T[] buffer = new T[perimeter];
                int idx = 0;

                for (int j = left; j < right; j++)
                    buffer[idx++] = skewMap[top, j];
                for (int i = top; i < bottom; i++)
                    buffer[idx++] = skewMap[i, right];
                for (int j = right; j > left; j--)
                    buffer[idx++] = skewMap[bottom, j];
                for (int i = bottom; i > top; i--)
                    buffer[idx++] = skewMap[i, left];

                T[] skewed = new T[perimeter];
                for (int i = 0; i < perimeter; i++)
                    skewed[(i + shift) % perimeter] = buffer[i];

                idx = 0;
                for (int j = left; j < right; j++)
                    skewMap[top, j] = skewed[idx++];
                for (int i = top; i < bottom; i++)
                    skewMap[i, right] = skewed[idx++];
                for (int j = right; j > left; j--)
                    skewMap[bottom, j] = skewed[idx++];
                for (int i = bottom; i > top; i--)
                    skewMap[i, left] = skewed[idx++];
            }

            return skewMap;
        }
    }
}
