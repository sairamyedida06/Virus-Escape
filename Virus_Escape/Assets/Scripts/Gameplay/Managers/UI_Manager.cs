using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject settingsPanel;
    public GameObject pauseMenuPanel;
    public GameObject hud;
    public GameObject GameOverScreen;
    public GameObject MobileControls;

    [SerializeField] Image musicIcon;
    [SerializeField] Sprite musicOnSprite;
    [SerializeField] Sprite musicOffSprite;

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
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        mainMenuScreen.SetActive(true);
        hud.SetActive(false);
        GameOverScreen.SetActive(false);
        MobileControls.SetActive(false);
        pauseMenuPanel.SetActive(false);
        staminaBar.enabled = false;
        Staminabar.enabled = false;

        UpdateMusicIcon();
    }

    public void ShowHud()
    {
        hud.SetActive(true);
        mainMenuScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        MobileControls.SetActive(true);
        staminaBar.enabled = true;
    }

    public void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);
        MobileControls.SetActive(false);
        GameManager.Instance.ClearCameraTarget();
    }

    public void StartGameButton()
    {
        mainMenuScreen.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void QuitButton()
    {
        GameManager.Instance.QuitGame();
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
        mainMenuScreen.SetActive(false);
    }
    public void MusicToggleButton()
    {
        AudioManager.Instance.ToggleMusic();
        UpdateMusicIcon();
    }
    public void UpdateMusicIcon()
    {
        if (AudioManager.Instance.IsMusicPlaying())
            musicIcon.sprite = musicOnSprite;
        else
            musicIcon.sprite = musicOffSprite;
    }

    public void SettingsBackButton()
    {
        mainMenuScreen.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void PauseButton()
    {
        pauseMenuPanel.SetActive(true);
        MobileControls.SetActive(false);
        GameManager.Instance.PauseGame();
    }

    public void ResumeButton()
    {
        pauseMenuPanel.SetActive(false);
        MobileControls.SetActive(true);
        GameManager.Instance.ResumeGame();
    }

    public void RestartButton()
    {
        pauseMenuPanel.SetActive(false);
        GameManager.Instance.Restart();
    }

    public void MainMenuButton()
    {
        ShowMainMenu();
        GameManager.Instance.LoadMainMenu();
    }

    public void PauseMenuBackButton()
    {
        pauseMenuPanel.SetActive(false);
        MobileControls.SetActive(true);
        GameManager.Instance.ResumeGame();
    }

    public void SetStaminaFillAmount(float value)
    {
        Staminabar.SetValue(value);
    }
}