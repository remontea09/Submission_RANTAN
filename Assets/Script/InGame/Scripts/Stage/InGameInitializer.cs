
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour {
    [SerializeField] private InGameManager gameManager;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private MiasmaController miasmaController;
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private MiniMapController miniMapController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Camp camp;
    [SerializeField] private Flower _flower;
    [SerializeField] private SceneEffectManager sceneEffectManager;
    [SerializeField] private LogService _logService;
    [SerializeField] private TutorialManager _tutorialManagerPrefab;

    // ゲームの起点
    private void Awake() {
        switch (Dao.GameSettings.GameType) {
            case GameType.Normal: InitializeNormal(); break;
            case GameType.Tutorial: InitializeTutorial(); break;
        }
    }

    private void InitializeNormal() {
        GameSessionService.Instance.Load();
        AudioService.Instance.InitInGameAudio();

        InitializeStage();
    }

    private void InitializeTutorial() {
        GameSessionService.Instance.LoadTutorial();
        AudioService.Instance.InitInGameAudio();

        InitializeStage();
        Instantiate(_tutorialManagerPrefab).InitTutorialManager();
    }

    private void InitializeStage() {
        var dungeonGenerator = new DungeonGenerator();
        var terrainMap = dungeonGenerator.GenerateDungeon();
        var campPosition = dungeonGenerator.GenerateCamp(ref terrainMap);

        camp.InitCamp(campPosition);

        StageService.Instance.InitStageServise(terrainMap, enemyPrefab, campPosition, _flower);

        var playerPosition = campPosition + Direction.Down;
        playerController.InitPlayerController(playerPosition);

        TileGenerator.GenerateStageTile(tilemap);
        miniMapController.InitMinimapController();
        miasmaController.InitMiasmaController();

        StageService.Instance.SpawnEnemyRandomPosition(GameSessionService.Instance.EnemyCount);

        sceneEffectManager.InitSceneEffectManager();

        gameManager.InitInGameManager(miasmaController, playerController);
        _logService.Initialize();
    }
}
