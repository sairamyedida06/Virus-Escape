using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject hud;
     public GameObject GameOverScreen;

    public Health_Display Health_Display;

    [SerializeField] public ProgressUI progressTracker;
    [SerializeField] public Progress_Display progress_display;

    public static UI_Manager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        GameOverScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        hud.SetActive(false);
    }
    void Start()
    {
        

    }

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);

        SceneManager.LoadScene("Test Scene");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowMainMenu()
    {
        mainMenuScreen.SetActive(true);

        hud.SetActive(false);
        GameOverScreen.SetActive(false );

    }

    public void ShowHud()
    {
        hud.SetActive(true);
        mainMenuScreen.SetActive(false);
        GameOverScreen.SetActive(false);
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
