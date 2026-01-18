using System;
using System.Collections.Generic;

namespace Common {
    public static class RandomHelper {
        private static readonly Random random = new Random();

        public static void Shuffle<T>(T[] array) {
            for (int i = array.Length - 1; i > 0; i--) {
                int j = random.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        public static void Shuffle<T>(List<T> list) {
            for (int i = list.Count - 1; i > 0; i--) {
                int j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// numerator / denominator の確率で true を返す。
        /// numerator の既定値は 1。
        /// </summary>
        public static bool Chance(int denominator, int numerator = 1) {
            if (denominator <= 0) throw new ArgumentOutOfRangeException(nameof(denominator), "denominator は正の値である必要があります。");
            if (numerator <= 0) return false;
            if (numerator >= denominator) return true;

            return new Random().Next(denominator) < numerator;
        }

        /// <summary>
        /// 32ビットのシード値を生成
        /// </summary>
        public static int GenerateNewSeed() {
            long ticks = DateTime.Now.Ticks;
            return (int)(ticks ^ (ticks >> 32));
        }

        public static T GetRandomEnumValue<T>() where T : Enum {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        public static (int, int, int) SplitIntoThree(int total) {
            int r1 = UnityEngine.Random.Range(0, total + 1);
            int r2 = UnityEngine.Random.Range(0, total + 1);

            if (r1 > r2) {
                (r1, r2) = (r2, r1);
            }

            int a = r1;
            int b = r2 - r1;
            int c = total - r2;

            return (a, b, c);
        }
    }
}

