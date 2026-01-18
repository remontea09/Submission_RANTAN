using System.Collections.Generic;
using Common.Const;
using UnityEngine;

public class InGameManager : MonoBehaviour {
    [SerializeField] private PlayerUI playerUI;

    private const int MIN_ACTION_DISTANCE_SQUARE = 40;

    private MiasmaController miasmaController;


    public void InitInGameManager(MiasmaController miasmaController, PlayerController player) {
        this.miasmaController = miasmaController;
        playerUI.InitPlayerUI();
        player.OnDecideAction += PlayerTurnEnd; ;
    }


    private void PlayerTurnEnd() {
        ExecuteCharacterTurn();
        TurnEnd();
    }

    private void ExecuteCharacterTurn() {
        var playerPosition = Dao.StageInstance.Player.StagePosition;
        List<StageCharacter> tmpCharacterList = new(StageService.Instance.StageCharacterMap.Values);
        tmpCharacterList.RemoveAll((character) => {
            // 離れたキャラは処理しない
            return Index2D.DistanceSquare(playerPosition, character.StagePosition) > MIN_ACTION_DISTANCE_SQUARE;
        });
        // 距離準に行動決定&移動実行
        tmpCharacterList.Sort((a, b) => {
            int distA = Index2D.DistanceSquare(playerPosition, a.StagePosition);
            int distB = Index2D.DistanceSquare(playerPosition, b.StagePosition);
            return distA.CompareTo(distB);
        });
        foreach (var character in tmpCharacterList) {
            if (character.IsDeath) continue;
            character.DecideAndMove();
        }
        tmpCharacterList.Sort((a, b) => a.Speed.CompareTo(b.Speed));
        foreach (var character in tmpCharacterList) {
            if (character.IsDeath) continue;
            character.ExecuteTurn();
        }
    }

    private void TurnEnd() {
        GameSessionService.Instance.TurnCount++;
        if (GameSessionService.Instance.TurnCount % DungeonConst.ENEMY_SPAWN_INTERVAL == 0 && GameSessionService.Instance.EnemyCount > StageService.Instance.StageCharacterMap.Count) {
            StageService.Instance.SpawnEnemyRandomPosition();
        }
        if (GameSessionService.Instance.TurnCount % DungeonConst.ENEMY_ENERGY_CHANGE_INTERVAL == 0) {
            foreach (var charcter in StageService.Instance.StageCharacterMap.Values) {
                if (charcter is EnemyController) {
                    (charcter as EnemyController).ChangeEnergy();
                }
            }
        }
        miasmaController.UpdateMiasma();

        EventService.Instance.DispatchEvent(EventType.TurnEndBuff);
        EventService.Instance.DispatchEvent(EventType.TurnEnd);
        AnimationService.Instance.Play();
    }

    private void OnDestroy() {
        EventService.Instance.RemoveAllListners();
        GameSessionService.Instance.Reset();
    }
}
