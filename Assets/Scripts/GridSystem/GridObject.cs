using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private Unit unit;
    private GameObject nonUnitObject;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString() + "\n" + unit;
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public void SetNonUnitObject(GameObject obj)
    {
        nonUnitObject = obj;
    }

    public GameObject GetNonUnitObject()
    {
        return nonUnitObject;
    }
}
