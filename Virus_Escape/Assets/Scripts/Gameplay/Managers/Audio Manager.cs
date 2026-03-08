using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;

    public static AudioManager Instance;

    bool musicEnabled = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
                musicSource = GetComponent<AudioSource>();

            // Load saved music setting
            musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            if (musicEnabled)
                musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            musicSource.PlayOneShot(clip);
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;

        if (musicEnabled)
        {
            musicSource.Play();
            Debug.Log("Music On");
        }
        else
        {
            musicSource.Stop();
            Debug.Log("Music Off");
        }

       
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
    }

    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}