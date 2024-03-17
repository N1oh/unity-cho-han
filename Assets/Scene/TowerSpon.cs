using System.Collections;
using UnityEngine;

// [System.Serializable] ��Ʈ����Ʈ�� �ش� Ŭ������ �ν��Ͻ��� Inspector���� ����ȭ�� �� �ֵ��� ���ݴϴ�.
[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab; // �� ĳ���� �������� �����ϴ� ����
    public int spawnCount; // �ش� ������ ���� ��ȯ�� Ƚ���� �����ϴ� ����
}

public class TowerSpon : MonoBehaviour
{
    public EnemySpawnData[] enemySpawnData; // ���� ������ �� ĳ���� ��ȯ �����͸� ���� �迭
    public Transform spawnPoint; // ��ȯ ��ġ�� ��Ÿ���� ����
    public float spawnInterval = 5f; // �� ���� ��ȯ�ϴ� ������ �����ϴ� ����

    private int currentSpawnIndex = 0; // ���� ��ȯ�� ���� �ε����� �����ϴ� ����

    void Start()
    {
        // ������ �� SpawnEnemies �ڷ�ƾ ����
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // ��� �� ������ ���� �ݺ�
        while (currentSpawnIndex < enemySpawnData.Length)
        {
            // ���� �� ������ �����հ� ��ȯ Ƚ���� ������
            GameObject selectedEnemyPrefab = enemySpawnData[currentSpawnIndex].enemyPrefab;
            int spawnCount = enemySpawnData[currentSpawnIndex].spawnCount;

            // �ش� ������ ���� ������ Ƚ����ŭ ��ȯ
            for (int i = 0; i < spawnCount; i++)
            {
                // SpawnEnemy �Լ� ȣ���Ͽ� �� ĳ���� ��ȯ
                SpawnEnemy(selectedEnemyPrefab);

                // ���� �ð� ���� ���
                yield return new WaitForSeconds(spawnInterval);
            }

            // ���� ������ ������ �̵�
            currentSpawnIndex++;
        }
    }

    // ���͸� ��ȯ�ϴ� �Լ�
    void SpawnEnemy(GameObject enemyPrefab)
    {
        // ��ȯ ��ġ�� enemyPrefab�� ����
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
