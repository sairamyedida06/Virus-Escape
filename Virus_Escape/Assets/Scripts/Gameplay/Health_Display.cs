using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Health_Display : MonoBehaviour
{
    [SerializeField] GameObject HeartIcon;

    [SerializeField] List<GameObject> Icons;

    


    int maxHealthPoints = 0;

    public int CurrentHealthPoints 
    {
        get
        {
            return maxHealthPoints;
        }
        set
        {
            int oldValue = maxHealthPoints;

            maxHealthPoints = value;

            ManageIcons(maxHealthPoints - oldValue);
        }
    }


    public void ManageIcons(int deltaPoints)
    {
        if(deltaPoints == 0)
        {
            return;
        }

        if (deltaPoints > 0) 
        {
            for (int i = 0; i < deltaPoints; i++)
            {
                var icons = Instantiate(HeartIcon, transform);

                Icons.Add(icons);
            }
        }
        else
        {
            for (int i = 0; i < -deltaPoints; i++)
            {
                var icons = Icons[Icons.Count - 1];

                Icons.Remove(icons);

                Destroy(icons);
            }
        }
    }
}
