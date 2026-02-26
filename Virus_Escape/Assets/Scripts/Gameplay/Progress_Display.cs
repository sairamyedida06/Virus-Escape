using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Progress_Display : MonoBehaviour
{
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject SuccessPanel;
    [SerializeField] TextMeshProUGUI remaingCoins;
    void Start()
    {
        MainPanel.SetActive(true);
        SuccessPanel.SetActive(false);
    }

    public void SetCoins(int amount)
    {
        remaingCoins.text = amount.ToString();

        if (amount > 0)
        {
            MainPanel.SetActive(true);
            SuccessPanel.SetActive(false);
        }
        else
        {
            MainPanel.SetActive(false);
            SuccessPanel.SetActive(true);
        }

    }
}


