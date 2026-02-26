using System.Collections.Generic;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
    public int RemainingCoins => Coins.Count;

    [SerializeField] List<GameObject> Coins;

    [SerializeField] LevelExit LevelExit;
    

    void Start()
    {
        Coins = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coin"));
    }

    // Update is called once per frame
    void Update()
    {
        int previousRemaingCoins = RemainingCoins;

        for (int i = Coins.Count - 1; i >= 0; i--) 
        {
            var coins = Coins[i];

            if(coins == null)
            {
                Coins.RemoveAt(i);
            }
        }

        UI_Manager.Instance.progress_display.SetCoins(RemainingCoins);

        if (previousRemaingCoins != RemainingCoins && RemainingCoins == 0) 
        {
            LevelExit.OpenGate();
        }
    }


}

