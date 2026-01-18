using UnityEngine;
using Utilities;

public abstract class StageEntity : MonoBehaviour {
    public EntityType EntityType { get; protected set; }

    public Index2D StagePosition { get; protected set; }

    protected void InitStageEntity(EntityType entityType, Index2D stagePosition) {
        this.EntityType = entityType;
        this.StagePosition = stagePosition;

        transform.position = CoordinateConvertUtil.StageToWorldPosition(stagePosition);
    }
}
