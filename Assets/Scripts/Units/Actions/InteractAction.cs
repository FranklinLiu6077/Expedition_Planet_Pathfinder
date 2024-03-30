using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    [SerializeField] private GameObject interactionUI;
    public override string GetActionName() => "Interact";

    public override void TakeAction(GridPosition gridPosition, System.Action onActionComplete)
    {
        if (IsValidActionGridPosition(gridPosition))
        {
            // ��ʾ���� UI
            interactionUI.SetActive(true);

            // �������������� UI Ԫ�صľ������ݣ����ݽ�������Ĳ�ͬ��ʾ��ͬ����Ϣ��ѡ��

            // ��������һ�������������û��� UI �ϵ�ѡ��һ��ѡ����ɣ����� onActionComplete
            // ���磬����Զ��İ�ť�ĵ���¼��������¼���������е��� onActionComplete
        }
        else
        {
            onActionComplete();  // ���λ����Ч���������ûص�
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPositions = new List<GridPosition>();
        GridPosition playerPosition = unit.GetGridPosition();

        // ��������Χ�İ˸�����
        List<GridPosition> potentialPositions = new List<GridPosition>
    {
        new GridPosition(playerPosition.x - 1, playerPosition.z + 1),  // ����
        new GridPosition(playerPosition.x, playerPosition.z + 1),      // ��
        new GridPosition(playerPosition.x + 1, playerPosition.z + 1),  // ����
        new GridPosition(playerPosition.x - 1, playerPosition.z),      // ��
        new GridPosition(playerPosition.x + 1, playerPosition.z),      // ��
        new GridPosition(playerPosition.x - 1, playerPosition.z - 1),  // ����
        new GridPosition(playerPosition.x, playerPosition.z - 1),      // ��
        new GridPosition(playerPosition.x + 1, playerPosition.z - 1),  // ����
    };

        foreach (var pos in potentialPositions)
        {
            if (LevelGrid.Instance.IsValidGridPosition(pos))
            {
                GameObject nonUnitObject = LevelGrid.Instance.GetNonUnitObjectAtGridPosition(pos);
                if (nonUnitObject != null && nonUnitObject.GetComponent<BasePlanet>() != null)
                {
                    validPositions.Add(pos);
                }
            }
        }

        return validPositions;
    }
}
