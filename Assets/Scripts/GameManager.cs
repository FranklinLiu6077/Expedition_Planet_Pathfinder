using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Unit playerUnitPrefab; // 玩家单位的预制体
    [SerializeField] private GameObject obstaclePrefab2x2; // 2x2障碍物的预制体
    [SerializeField] private GameObject[] obstaclePrefabs1x1; // 1x1障碍物的预制体数组

    [SerializeField] private int width = 30; // 地图宽度
    [SerializeField] private int height = 30; // 地图高度
    [SerializeField] private float cellSize = 2f; // 单元格大小

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
        PlacePlayer(); // 放置玩家
        GenerateObstacles(); // 生成障碍物
    }

    private void PlacePlayer()
    {
        Vector3 playerStartPosition = LevelGrid.Instance.GetWorldPosition(new GridPosition(0, 0));
        Instantiate(playerUnitPrefab, playerStartPosition, Quaternion.identity);
        Unit playerUnit = playerUnitPrefab.GetComponent<Unit>();

        if (playerUnit != null)
        {
            UnitActionSystem.Instance.SetSelectedUnit(playerUnit);
        }
    }

    private void GenerateObstacles()
    {
        SpawnStellar();
        SpawnPlanetary();
    }

    private void SpawnStellar()
    {
        int centerX = Mathf.Clamp(width / 2, 1, width - 2);
        int centerZ = Mathf.Clamp(height / 2, 1, height - 2);

        // 在地图中心生成2x2的障碍物
        Vector3 centerPosition = LevelGrid.Instance.GetWorldPosition(new GridPosition(width / 2, height / 2));
        centerPosition += new Vector3(cellSize / 2, 0, cellSize / 2); // 调整位置到2x2区域的中心
        GameObject centerObstacle = Instantiate(obstaclePrefab2x2, centerPosition, Quaternion.identity); // 实例化大球体
        centerObstacle.transform.localScale = new Vector3(cellSize * 2, centerObstacle.transform.localScale.y, cellSize * 2); // 调整大球体大小以覆盖2x2区域

        // 更新路径节点的可行走性
        for (int x = -1; x <= 0; x++)
        {
            for (int z = -1; z <= 0; z++)
            {
                int gridX = centerX + x;
                int gridZ = centerZ + z;

                if (gridX >= 0 && gridX < width && gridZ >= 0 && gridZ < height)
                {
                    Pathfinding.Instance.GetNode(gridX, gridZ).SetIsWalkable(false);
                }
            }
        }
    }

    private void SpawnPlanetary()
    {
        // 存储已生成障碍物位置的列表
        List<GridPosition> usedPositions = new List<GridPosition>();

        // 随机生成其他1x1障碍物
        int maxObstacles = 12; // 最大星球数量
        for (int i = 0; i < Math.Min(maxObstacles, width * height / 10); i++)
        {
            GridPosition randomPosition;
            do
            {
                randomPosition = new GridPosition(UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height));
            } while (!CanPlaceObstacle(randomPosition, usedPositions) || IsCentralObstacle(randomPosition));

            // 将新位置及其周围一圈的位置添加到已使用列表
            usedPositions.Add(randomPosition);
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    GridPosition adjacentPosition = new GridPosition(randomPosition.x + x, randomPosition.z + z);
                    if (!usedPositions.Contains(adjacentPosition))
                    {
                        usedPositions.Add(adjacentPosition);
                    }
                }
            }

            Vector3 obstaclePosition = LevelGrid.Instance.GetWorldPosition(randomPosition);
            GameObject randomObstaclePrefab = obstaclePrefabs1x1[UnityEngine.Random.Range(0, obstaclePrefabs1x1.Length)];
            Instantiate(randomObstaclePrefab, obstaclePosition, Quaternion.identity);
            Pathfinding.Instance.GetNode(randomPosition.x, randomPosition.z).SetIsWalkable(false);
        }
    }

    private bool IsCentralObstacle(GridPosition position)
    {
        // 检查是否为中心障碍物
        int centerX = width / 2;
        int centerZ = height / 2;
        return position.x >= centerX - 1 && position.x <= centerX && position.z >= centerZ - 1 && position.z <= centerZ;
    }

    private bool CanPlaceObstacle(GridPosition position, List<GridPosition> usedPositions)
    {
        // 检查是否在禁止生成区域内
        if (position.x >= 0 && position.x <= 3 && position.z >= 0 && position.z <= 3)
        {
            return false;
        }

        // 检查周围两格内是否已有星球
        for (int x = -2; x <= 2; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                GridPosition checkPosition = new GridPosition(position.x + x, position.z + z);
                if (usedPositions.Contains(checkPosition))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
