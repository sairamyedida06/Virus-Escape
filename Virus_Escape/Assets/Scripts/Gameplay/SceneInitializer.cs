using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
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
