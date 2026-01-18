using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniMapController : MonoBehaviour {
    [SerializeField] private GameObject flowerMarker;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private TilemapRenderer tmr;
    [SerializeField] private TextMeshProUGUI discoveryRateText;
    [SerializeField] private GameObject playerMarker;
    [SerializeField] private GameObject campMarker;

    private Tilemap minimap;
    private int floorCount;
    private HashSet<Index2D> exploredAreaSet;
    private Index2D campPosition;

    private Action minimapAction;

    public void InitMinimapController() {
        minimap = GetComponent<Tilemap>();
        TileGenerator.GenerateMinimap(minimap, floorTile, wallTile);
        exploredAreaSet = new HashSet<Index2D>();
        floorCount = StageService.Instance.FloorPositionList.Count;
        campPosition = StageService.Instance.CampPosition;
        SetCampMarker(campPosition);

        Bounds bounds = tmr.bounds;
        float aspect = (float)minimapCamera.targetTexture.width / minimapCamera.targetTexture.height;
        minimapCamera.orthographicSize = Mathf.Max(bounds.extents.y, bounds.extents.x / aspect);
        Vector3 camPos = minimapCamera.transform.position;
        camPos.x = bounds.center.x;
        camPos.y = bounds.center.y;
        minimapCamera.transform.position = camPos;

        Dao.StageInstance.Player.OnMove += UpdateMiniMap;
        UpdateMiniMap();
        minimapAction += CompleteMiniMap;
        flowerMarker.SetActive(false);
    }

    /// <summary>
    /// プレイヤーの位置から視界の範囲を掛け合わせて見えている範囲を明るく表示
    /// </summary>
    private void UpdateMiniMap() {
        var playerPos = Dao.StageInstance.Player.StagePosition;
        BoundsInt bounds = minimap.cellBounds;
        //プレイヤーの視野を取得、周辺を明るく表示
        var viewArea = new ViewAreaInStage(playerPos);
        for (int v = viewArea.TopLeft.v - 1; v <= viewArea.BottomRight.v + 1; v++) {
            for (int h = viewArea.TopLeft.h - 1; h <= viewArea.BottomRight.h + 1; h++) {
                Index2D stagePos = new(v, h);
                var tileType = StageService.Instance.GetTile(v, h);
                if (tileType == TileType.Wall || exploredAreaSet.Contains(stagePos)) continue;

                Vector3Int pos = new Vector3Int(h, -v, 0);
                foreach (var direction in Direction.FourDirections) {
                    var searchPos = (stagePos + direction);
                    if (StageService.Instance.GetTile(searchPos) == TileType.Wall) {
                        minimap.SetColor(new(searchPos.h, -searchPos.v), Color.white);
                    }
                }
                Color color = new Color(0, 0, 0, 0.85f);
                exploredAreaSet.Add(stagePos);
                minimap.SetColor(pos, color);
            }
        }

        if (!GameSessionService.Instance.HasFlower) {
            var flowerPos = StageService.Instance._flower.StagePosition;
            if (viewArea.Contains(flowerPos)) {
                flowerMarker.SetActive(true);
                Vector3Int pos = new Vector3Int(flowerPos.h, -flowerPos.v, 0);
                flowerMarker.transform.position = minimap.CellToWorld(pos);
            }
        }
        else {
            flowerMarker.SetActive(false);
        }
        UpdateProgressRateText();
        UpdatePlayerMarker(playerPos);
    }

    private void SetCampMarker(Index2D campPos) {
        Vector3Int pos = new Vector3Int(campPos.h, -campPos.v, 0);
        Vector3 destination = minimap.CellToWorld(pos);
        campMarker.transform.position = destination;
    }

    private void OnDestroy() {
        if (Dao.StageInstance.Player is not null) {
            Dao.StageInstance.Player.OnMove -= UpdateMiniMap;
        }
        minimapAction -= CompleteMiniMap;
    }


    /// <summary>
    /// 探索率を割り出して表示
    /// </summary>
    private void UpdateProgressRateText() {
        int discoverryRate = (int)(100f * exploredAreaSet.Count / floorCount);
        discoveryRateText.SetText(discoverryRate + "%");
        if (discoverryRate == 100) {
            minimapAction?.Invoke();
        }
    }

    /// <summary>
    /// プレイヤーの位置を移動する度に都度取得して適用
    /// </summary>
    /// <param name="playerPos"></param>
    private void UpdatePlayerMarker(Index2D playerPos) {
        Vector3Int pos = new Vector3Int(playerPos.h, -playerPos.v, 0);
        Vector3 destination = minimap.CellToWorld(pos);
        playerMarker.transform.position = destination;
    }

    /// <summary>
    /// ミニマップの探索率が100%になったら演出
    /// </summary>
    private void CompleteMiniMap() {
        discoveryRateText.fontStyle = FontStyles.Bold;
        discoveryRateText.DOColor(Color.yellow, 2f);
        discoveryRateText.rectTransform.DOShakePosition(2f, 20f);
        minimapAction -= CompleteMiniMap;
    }

}
