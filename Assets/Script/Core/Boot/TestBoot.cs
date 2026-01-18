using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBoot : MonoBehaviour {

    private void Awake() {
        Application.targetFrameRate = 60;

        Dao.Master.InitMaster();

        SceneManager.LoadScene("EnergyTest");
    }

}
