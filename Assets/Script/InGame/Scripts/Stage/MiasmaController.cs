using Common.Const;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiasmaController : MonoBehaviour {
    [SerializeField] private Tilemap miasmaTilemap;
    [SerializeField] private TileBase miasmaTile;

    private readonly static float spreadSpeed = 0.125f;
    private readonly Color miasmaColor = new Color(1, 1, 1, 0.5f);

    private float spreadLength;
    private int restCount;

    private int waitCount = 0;

    public void InitMiasmaController() {
        TileGenerator.GenerateMiasmaTile(miasmaTilemap, miasmaTile);
        spreadLength = Mathf.Max(StageService.Instance.StageHeight, StageService.Instance.StageWidth) * 1.5f;
        restCount = (StageService.Instance.StageHeight + GrobalConst.TILEMAP_MARGIN * 2) * (StageService.Instance.StageWidth + GrobalConst.TILEMAP_MARGIN * 2);
    }

    public void UpdateMiasma() {

        if(waitCount < 30) {
            waitCount++;
            return;
        }

        var preLength = spreadLength;
        spreadLength -= spreadSpeed;
        int margin = GrobalConst.TILEMAP_MARGIN;
        var campPosition = StageService.Instance.CampPosition;

        for (int i = -margin; i < StageService.Instance.StageHeight + margin; ++i) {
            int x = i - campPosition.v;
            for (int j = -margin; j < StageService.Instance.StageWidth + margin; ++j) {
                int y = j - campPosition.h;
                var distanceSq = (x * x) + (y * y);
                var tilePos = new Vector3Int(j, -i);
                if (distanceSq > preLength * preLength) {
                    var character = StageService.Instance.GetStageCharacter(new(i, j));
                    if (character != null) {
                        character.TakeMiasmaDamage();
                    }
                }
                else if (distanceSq > spreadLength * spreadLength && restCount > 0) {
                    if (miasmaTilemap.GetColor(tilePos) == miasmaColor) continue;

                    miasmaTilemap.SetColor(tilePos, miasmaColor);
                    restCount--;
                    // キャンプだけ残るの防止
                    if (restCount == 1) {
                        tilePos = new Vector3Int(campPosition.h, -campPosition.v);
                        miasmaTilemap.SetColor(tilePos, miasmaColor);
                        restCount = 0;
                    }
                }
            }
        }
    }
}
