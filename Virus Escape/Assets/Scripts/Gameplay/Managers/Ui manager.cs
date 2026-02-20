using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Uimanager : MonoBehaviour
{
    [SerializeField] CinemachineCamera CinemachineCamera;
    [SerializeField] GameObject GameoverScreen;
    public static Uimanager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
       GameoverScreen.SetActive(false);
        
    }

    public void ShowGameOverMenu()
    {
        GameoverScreen.SetActive(true);
        StopCameraTracking();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Test Scene");
    }

    public void StopCameraTracking()
    {
        var cam = (CinemachineCamera)CinemachineBrain.GetActiveBrain(0).ActiveVirtualCamera;

        cam.Target.TrackingTarget = null;
    }
}
