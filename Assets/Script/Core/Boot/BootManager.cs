using UnityEngine;
using UnityEngine.SceneManagement;

public class BootManager : MonoBehaviour {

    private void Awake() {
        Application.targetFrameRate = 60;

        Dao.Master.InitMaster();
    }

    private void Start() {
        MetaSessionService.Instance.Load();

        SceneManager.LoadScene("Title");
    }

}
