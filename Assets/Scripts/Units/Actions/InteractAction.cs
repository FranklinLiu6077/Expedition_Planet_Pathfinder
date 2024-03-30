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
            // 显示交互 UI
            interactionUI.SetActive(true);

            // 可以在这里设置 UI 元素的具体内容，根据交互对象的不同显示不同的信息或选项

            // 假设你有一个方法来处理用户在 UI 上的选择，一旦选择完成，调用 onActionComplete
            // 例如，你可以订阅按钮的点击事件，并在事件处理程序中调用 onActionComplete
        }
        else
        {
            onActionComplete();  // 如果位置无效，立即调用回调
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validPositions = new List<GridPosition>();
        GridPosition playerPosition = unit.GetGridPosition();

        // 检查玩家周围的八个格子
        List<GridPosition> potentialPositions = new List<GridPosition>
    {
        new GridPosition(playerPosition.x - 1, playerPosition.z + 1),  // 左上
        new GridPosition(playerPosition.x, playerPosition.z + 1),      // 上
        new GridPosition(playerPosition.x + 1, playerPosition.z + 1),  // 右上
        new GridPosition(playerPosition.x - 1, playerPosition.z),      // 左
        new GridPosition(playerPosition.x + 1, playerPosition.z),      // 右
        new GridPosition(playerPosition.x - 1, playerPosition.z - 1),  // 左下
        new GridPosition(playerPosition.x, playerPosition.z - 1),      // 下
        new GridPosition(playerPosition.x + 1, playerPosition.z - 1),  // 右下
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
