using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] UI_Manager uiManager;

    private void Awake()
    {
        if(UI_Manager.Instance == null)
        {
            Instantiate(uiManager);

            DontDestroyOnLoad(UI_Manager.Instance);

        }
    }
    public enum Scenetype
    {
        Gameplay,
        MainMenu
    }

    [SerializeField] Scenetype scenetype = Scenetype.Gameplay;

    private void Start()
    {
        if(scenetype == Scenetype.Gameplay)
        {
            UI_Manager.Instance.ShowHud();
        }
        else if( scenetype == Scenetype.MainMenu)
        {
            UI_Manager.Instance.ShowMainMenu();
           
        }
    }
}
