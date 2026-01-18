using System;
using Common.Const;

public struct ViewAreaInStage {
    public Index2D TopLeft { get; }
    public Index2D BottomRight { get; }
    public ViewAreaInStage(Index2D centerPosition) {
        int height = PlayerConst.PLAYER_VIEW_SIDE;
        int width = PlayerConst.PLAYER_VIEW_SIDE;
        Index2D offset = new Index2D(height / 2, width / 2);

        Index2D rawTopLeft = centerPosition - offset;
        Index2D rawBottomRight = centerPosition + offset;

        int top = Math.Max(0, rawTopLeft.v);
        int left = Math.Max(0, rawTopLeft.h);
        int bottom = Math.Min(StageService.Instance.StageHeight - 1, rawBottomRight.v);
        int right = Math.Min(StageService.Instance.StageWidth - 1, rawBottomRight.h);

        this.TopLeft = new Index2D(top, left);
        this.BottomRight = new Index2D(bottom, right);
    }

    public bool Contains(Index2D pos) {
        return (TopLeft.v <= pos.v && pos.v <= BottomRight.v) &&
               (TopLeft.h <= pos.h && pos.h <= BottomRight.h);
    }
}
