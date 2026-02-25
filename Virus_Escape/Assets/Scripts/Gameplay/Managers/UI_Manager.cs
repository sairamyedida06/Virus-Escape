using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen;

    public static UI_Manager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GameOverScreen.SetActive(false);

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);

        ClearCameraTarget();
    }

    public void ClearCameraTarget()
    {
        var cam = (CinemachineCamera)CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera;

        cam.Target.TrackingTarget = null;

    }


}
