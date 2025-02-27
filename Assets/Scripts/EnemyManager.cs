using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 현재 시간
    public float currentTime;

    // 일정 시간
    public float createTime = 0.5f;

    // 생성할 적
    public GameObject enemyFactory;

    #region ObjectPool

    // 오브젝트 풀 설정
    public int poolSize = 100;
    GameObject[] enemyObjectPool;

    // 생성 위치(배열)
    public Transform[] spawnPoints;

    private void Start()
    {
        //createTime = Random.Range(min, max);

        enemyObjectPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            var enemy = Instantiate(enemyFactory);
            enemyObjectPool[i] = enemy;
            enemy.SetActive(false);
        }
    }
    #endregion

    private void Update()
    {
        if (ScoreManager.Instance().gameOver) return;

        // 1. 시간이 흐른다
        currentTime += Time.deltaTime;
        // 2. 현재 시간이 일정 시간에 도달하면 적을 생성한다.
        if (currentTime >= createTime)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var enemy = enemyObjectPool[i];
                if (enemy.activeSelf == false)
                {
                    int index = Random.Range(0, spawnPoints.Length);
                    enemy.transform.position = spawnPoints[index].position;
                    enemy.SetActive(true);
                    break;
                }
            }
            // 3. 소환 후 시간을 0으로 리셋
            currentTime = 0f;
            createTime = ScoreManager.Instance().difficultyLevel;
        }
    }
}
