using UnityEngine;

public class MiniMapCameraController : MonoBehaviour {

    private Camera minimapCamera;


    public void InitMinimapCameraController(Vector3Int cellCenter) {
        minimapCamera = GetComponent<Camera>();
        minimapCamera.transform.position = new Vector3(
            cellCenter.x,
            cellCenter.y,
            minimapCamera.transform.position.z
            );

    }
}
