using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MilitaryAcademy : Building
{
    private int currentTick = 0;

    private List<GameObject> productionQueue;

    private List<int> productionTickTiming;

    public void AddToProductionQueue(GameObject unit, int timing)
    {
        productionQueue.Add(unit);
        productionTickTiming.Add(timing);
    }

    public int GetLastTimingTick()
    {
        if (productionTickTiming.Count == 0)
        {
            return currentTick;
        }
        else
        {
            return productionTickTiming.ElementAt(productionTickTiming.Count - 1);
        }
        
    }

    private void Start()
    {
        base.Start();

        productionQueue = new List<GameObject>();

        productionTickTiming = new List<int>();

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            currentTick = e.tick;
        };
    }

    private void Update()
    {
        base.Update();

        if (productionTickTiming.Count != 0)
        {
            if (currentTick == productionTickTiming.ElementAt(0))
            {
                Instantiate(productionQueue.ElementAt(0), transform.position, Quaternion.identity);

                productionQueue.RemoveAt(0);

                productionTickTiming.RemoveAt(0);
            }
        }
    }
}
