using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<bool> isObserved;

    private bool hasChanged;

    private void Start()
    {
        isObserved = new List<bool>();

        hasChanged = false;
    }

    private void Update()
    {
        if (hasChanged)
        {
            if (isObserved.Count() == 0)
            {
                transform.Find("Shadow").gameObject.GetComponent<Renderer>().enabled = true;
            }

            if (isObserved.Count() == 1)
            {
                transform.Find("Shadow").gameObject.GetComponent<Renderer>().enabled = false;

                if (gameObject.GetComponent<Renderer>().enabled == false)
                {
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
            }

            hasChanged = false;
        }
    }

    public void AddObserver()
    {
        isObserved.Add(true);

        hasChanged = true;
    }

    public void RemoveObserver()
    {
        isObserved.RemoveAt(0);

        hasChanged = true;
    }
}
