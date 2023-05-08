using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnap : MonoBehaviour
{
    private Grid grid;
    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }
    void Update()
    {
        Vector3Int cp = grid.LocalToCell(transform.localPosition);
        transform.localPosition = grid.GetCellCenterLocal(cp);
    }
}
