using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject hud;
    public GameObject GameOverScreen;
    public GameObject MobileControls;



    public Health_Display Health_Display;

    [SerializeField] public ProgressUI progressTracker;
    [SerializeField] public Progress_Display progress_display;

    [SerializeField] Slider staminaBar;
    [SerializeField] Staminabar Staminabar;

    public static UI_Manager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        GameOverScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        hud.SetActive(false);
        MobileControls.SetActive(false);
    }
    void Start()
    {
        

    }

    public void StartGame()
    {
        mainMenuScreen.SetActive(false);
  

        SceneManager.LoadScene("Level_1");
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
        MobileControls.SetActive(false);

    }

    public void ShowHud()
    {
        hud.SetActive(true);
        mainMenuScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        MobileControls.SetActive(true);
    }

    public void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);
        MobileControls.SetActive(false);

        ClearCameraTarget();
    }

    public void SetStaminaFillAmount(float value)
    {
       Staminabar.SetValue(value);
    }

    public void ClearCameraTarget()
    {
        var cam = (CinemachineCamera)CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera;

        cam.Target.TrackingTarget = null;

    }


}
