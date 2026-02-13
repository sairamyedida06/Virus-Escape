using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RefreshRate refreshRate;
    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;

        DontDestroyOnLoad(gameObject);
    }
}
