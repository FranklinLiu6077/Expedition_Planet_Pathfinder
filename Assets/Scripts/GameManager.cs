using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Unit playerUnitPrefab; // ��ҵ�λ��Ԥ����
    [SerializeField] private GameObject obstaclePrefab2x2; // 2x2�ϰ����Ԥ����
    [SerializeField] private GameObject[] obstaclePrefabs1x1; // 1x1�ϰ����Ԥ��������

    [SerializeField] private int width = 30; // ��ͼ���
    [SerializeField] private int height = 30; // ��ͼ�߶�
    [SerializeField] private float cellSize = 2f; // ��Ԫ���С

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
        PlacePlayer(); // �������
        GenerateObstacles(); // �����ϰ���
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

        // �ڵ�ͼ��������2x2���ϰ���
        Vector3 centerPosition = LevelGrid.Instance.GetWorldPosition(new GridPosition(width / 2, height / 2));
        centerPosition += new Vector3(cellSize / 2, 0, cellSize / 2); // ����λ�õ�2x2���������
        GameObject centerObstacle = Instantiate(obstaclePrefab2x2, centerPosition, Quaternion.identity); // ʵ����������
        centerObstacle.transform.localScale = new Vector3(cellSize * 2, centerObstacle.transform.localScale.y, cellSize * 2); // �����������С�Ը���2x2����

        // ����·���ڵ�Ŀ�������
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
        // �洢�������ϰ���λ�õ��б�
        List<GridPosition> usedPositions = new List<GridPosition>();

        // �����������1x1�ϰ���
        int maxObstacles = 12; // �����������
        for (int i = 0; i < Math.Min(maxObstacles, width * height / 10); i++)
        {
            GridPosition randomPosition;
            do
            {
                randomPosition = new GridPosition(UnityEngine.Random.Range(0, width), UnityEngine.Random.Range(0, height));
            } while (!CanPlaceObstacle(randomPosition, usedPositions) || IsCentralObstacle(randomPosition));

            // ����λ�ü�����ΧһȦ��λ����ӵ���ʹ���б�
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
        // ����Ƿ�Ϊ�����ϰ���
        int centerX = width / 2;
        int centerZ = height / 2;
        return position.x >= centerX - 1 && position.x <= centerX && position.z >= centerZ - 1 && position.z <= centerZ;
    }

    private bool CanPlaceObstacle(GridPosition position, List<GridPosition> usedPositions)
    {
        // ����Ƿ��ڽ�ֹ����������
        if (position.x >= 0 && position.x <= 3 && position.z >= 0 && position.z <= 3)
        {
            return false;
        }

        // �����Χ�������Ƿ���������
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
