
using System.Collections.Generic;

public enum ActionRange : short {
    None = 0,
    FrontOne = 1,
    FrontTwo = 2,
    FrontSide = 3,
    A3,
    B2,
    B3,
    C1,
    C2,
    C3,
    D1,
    D2,
    D3,
    E1,
    E2,
    E3,
    F1,
    F2,
    F3,
    G1,
    G2,
    G3,
    H1,
    H2,
    H3,
    Mine = 100,
}

namespace EnumUtilities {
    public static class ActionRangeTable {
        public static readonly IReadOnlyDictionary<ActionRange, ActionRange> SearchRangeDictionary = new Dictionary<ActionRange, ActionRange>() {
            [ActionRange.None] = ActionRange.None,
            [ActionRange.Mine] = ActionRange.Mine,
            [ActionRange.FrontOne] = ActionRange.FrontOne,
            [ActionRange.FrontTwo] = ActionRange.FrontTwo,
            [ActionRange.FrontSide] = ActionRange.FrontOne,
            [ActionRange.A3] = ActionRange.FrontTwo,
            [ActionRange.B2] = ActionRange.FrontTwo,
            [ActionRange.B3] = ActionRange.FrontTwo,
            [ActionRange.C1] = ActionRange.FrontOne,
            [ActionRange.C2] = ActionRange.FrontOne,
            [ActionRange.C3] = ActionRange.FrontTwo,
            [ActionRange.D1] = ActionRange.FrontOne,
            [ActionRange.D2] = ActionRange.FrontOne,
            [ActionRange.D3] = ActionRange.FrontOne,
            [ActionRange.E1] = ActionRange.FrontOne,
            [ActionRange.E2] = ActionRange.FrontTwo,
            [ActionRange.E3] = ActionRange.FrontTwo,
            [ActionRange.F1] = ActionRange.FrontTwo,
            [ActionRange.F2] = ActionRange.FrontTwo,
            [ActionRange.F3] = ActionRange.FrontTwo,
            [ActionRange.G1] = ActionRange.FrontOne,
            [ActionRange.G2] = ActionRange.FrontTwo,
            [ActionRange.G3] = ActionRange.FrontTwo,
            [ActionRange.H1] = ActionRange.FrontTwo,
            [ActionRange.H2] = ActionRange.FrontTwo,
            [ActionRange.H3] = ActionRange.FrontTwo,
        };
        public static readonly IReadOnlyDictionary<ActionRange, int[,]> ActionRangeDictionary = new Dictionary<ActionRange, int[,]>() {
            [ActionRange.None] = new int[,] {
                {0}
            },

            [ActionRange.Mine] = new int[,] {
                {1}
            },

            [ActionRange.FrontOne] = new int[,] {
                { 0, 1, 0 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            },

            [ActionRange.FrontTwo] = new int[,] {
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0 } ,
                { 0, 0, 0, 0, 0 } ,
                { 0, 0, 0, 0, 0 }
            },

            [ActionRange.FrontSide] = new int[,] {
                { 1, 1, 1 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            },

            [ActionRange.A3] = new int[,] {
                { 0, 0, 1, 0, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
            },

            [ActionRange.B2] = new int[,] {
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 0, 0, 0 },
            },

            [ActionRange.B3] = new int[,] {
                { 0, 0, 1, 0, 0 },
                { 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0 },
            },

            [ActionRange.C1] = new int[,] {
                {0, 1, 0 },
                {1, 0, 1 },
                {0, 0, 0 }
            },

            [ActionRange.C2] = new int[,] {
                {1, 1, 1 },
                {1, 0, 1 },
                {0, 0, 0 }
            },

            [ActionRange.C3] = new int[,] {
                {1, 1, 1, 1, 1 },
                {1, 1, 1, 1, 1 },
                {1, 1, 0, 1, 1 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
            },

            [ActionRange.D1] = new int[,] {
                {0, 1, 0 },
                {0, 0, 0 },
                {0, 1, 0 }
            },

            [ActionRange.D2] = new int[,] {
                {1, 1, 1 },
                {0, 0, 0 },
                {0, 1, 0 },
            },

            [ActionRange.D3] = new int[,] {
                {0, 0, 0, 0, 0 },
                {1, 1, 1, 1, 1 },
                {0, 0, 0, 0, 0 },
                {0, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.E1] = new int[,] {
                {0, 1, 0 },
                {1, 0, 1 },
                {0, 1, 0 }
            },

            [ActionRange.E2] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 0, 1, 0, 0 },
                {0, 1, 0, 1, 0 },
                {0, 0, 1, 0, 0 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.E3] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 0, 1, 0, 0 },
                {1, 1, 0, 1, 1 },
                {0, 0, 1, 0, 0 },
                {0, 0, 1, 0, 0 }
            },

            [ActionRange.F1] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 0, 1, 0, 0 },
                {0, 1, 0, 1, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.F2] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 0, 1, 0, 0 },
                {0, 1, 0, 1, 0 },
                {1, 0, 1, 0, 1 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.F3] = new int[,] {
                {0, 0, 1, 0, 0, },
                {0, 1, 1, 1, 0, },
                {0, 1, 0, 1, 0, },
                {1, 0, 1, 0, 1, },
                {1, 0, 0, 0, 1, }
            },

            [ActionRange.G1] = new int[,] {
                {1, 1, 1 },
                {1, 0, 1 },
                {0, 1, 0 }
            },

            [ActionRange.G2] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 1, 1, 1, 0 },
                {0, 1, 0, 1, 0 },
                {0, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.G3] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 1, 1, 1, 0 },
                {1, 1, 0, 1, 1 },
                {0, 1, 1, 1, 0 },
                {0, 0, 1, 0, 0 }
            },

            [ActionRange.H1] = new int[,] {
                {0, 0, 1, 0, 0 },
                {0, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 1, 0, 0 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.H2] = new int[,] {
                {1, 0, 1, 0, 1 },
                {0, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 1, 0, 0 },
                {0, 0, 0, 0, 0 }
            },

            [ActionRange.H3] = new int[,] {
                {1, 0, 1, 0, 1 },
                {1, 1, 1, 1, 1 },
                {1, 1, 0, 1, 1 },
                {0, 1, 1, 1, 0 },
                {0, 0, 0, 0, 0 }
            }

        };
        public static int GetMaxHitCount(ActionRange rangeType) {
            int count = 0;
            var range = ActionRangeDictionary[rangeType];
            for (int i = 0; i < range.GetLength(0); ++i) {
                for (int j = 0; j < range.GetLength(1); ++j) {
                    if (range[i, j] == 1) {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
