using Common.Const;
using UnityEngine;

namespace Utilities {
    public static class CoordinateConvertUtil {

        public static Vector2 StageToWorldPosition(Index2D stagePosition) {
            return new Vector2(stagePosition.h * GrobalConst.CELL_SIZE, -stagePosition.v * GrobalConst.CELL_SIZE);
        }

        public static Index2D WorldToStagePosition(Vector2 worldPosition) {
            return new Index2D(-Mathf.FloorToInt((worldPosition.y) / GrobalConst.CELL_SIZE), Mathf.FloorToInt((worldPosition.x) / GrobalConst.CELL_SIZE));
        }
    }
}
