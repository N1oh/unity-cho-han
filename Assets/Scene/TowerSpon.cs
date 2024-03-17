using System.Collections;
using UnityEngine;

// [System.Serializable] 어트리뷰트는 해당 클래스의 인스턴스를 Inspector에서 직렬화할 수 있도록 해줍니다.
[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab; // 적 캐릭터 프리팹을 저장하는 변수
    public int spawnCount; // 해당 종류의 적을 소환할 횟수를 저장하는 변수
}

public class TowerSpon : MonoBehaviour
{
    public EnemySpawnData[] enemySpawnData; // 여러 종류의 적 캐릭터 소환 데이터를 담은 배열
    public Transform spawnPoint; // 소환 위치를 나타내는 변수
    public float spawnInterval = 5f; // 각 적을 소환하는 간격을 정의하는 변수

    private int currentSpawnIndex = 0; // 현재 소환할 적의 인덱스를 저장하는 변수

    void Start()
    {
        // 시작할 때 SpawnEnemies 코루틴 시작
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        // 모든 적 종류에 대해 반복
        while (currentSpawnIndex < enemySpawnData.Length)
        {
            // 현재 적 종류의 프리팹과 소환 횟수를 가져옴
            GameObject selectedEnemyPrefab = enemySpawnData[currentSpawnIndex].enemyPrefab;
            int spawnCount = enemySpawnData[currentSpawnIndex].spawnCount;

            // 해당 종류의 적을 지정된 횟수만큼 소환
            for (int i = 0; i < spawnCount; i++)
            {
                // SpawnEnemy 함수 호출하여 적 캐릭터 소환
                SpawnEnemy(selectedEnemyPrefab);

                // 일정 시간 동안 대기
                yield return new WaitForSeconds(spawnInterval);
            }

            // 다음 종류의 적으로 이동
            currentSpawnIndex++;
        }
    }

    // 몬스터를 소환하는 함수
    void SpawnEnemy(GameObject enemyPrefab)
    {
        // 소환 위치에 enemyPrefab을 생성
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
