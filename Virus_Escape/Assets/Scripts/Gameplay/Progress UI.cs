using System.Collections.Generic;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    public int RemainingCoins => Coins.Count;

    [SerializeField] List<GameObject> Coins;
    

    void Start()
    {
        Coins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = Coins.Count - 1; i >= 0; i--) 
        {
            var coins = Coins[i];

            if(coins == null)
            {
                Coins.RemoveAt(i);
            }
        }

        UI_Manager.Instance.progress_display.SetCoins(RemainingCoins);
    }


}

