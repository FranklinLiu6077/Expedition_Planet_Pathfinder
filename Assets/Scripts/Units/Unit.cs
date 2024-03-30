using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;

    private MoveAction moveAction;
    public event Action<int> OnStaminaChanged;

    [SerializeField]
    private int stamina = 100;
    public Stamina staminaComponent;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(gridPosition, this);
        staminaComponent = FindObjectOfType<Stamina>();
    }

    private void Update()
    {
        
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            // �������λ�øı�
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void ReduceStamina(int amount)
    {
        stamina -= amount;
        if (stamina < 0) stamina = 0;
        OnStaminaChanged?.Invoke(stamina);
        // ����������һЩ�߼������統�����ľ�ʱִ�еĲ���
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
