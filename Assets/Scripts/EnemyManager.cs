using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 현재 시간
    public float currentTime;

    // 일정 시간
    public float createTime = 0.5f;

    // 생성할 적
    public GameObject enemyFactory;
    private void Start()
    {
        createTime = Random.Range(1, 5);
    }

    private void Update()
    {
        // 1. 시간이 흐른다
        currentTime += Time.deltaTime;
        // 2. 현재 시간이 일정 시간에 도달하면 적을 생성한다.
        if (currentTime >= createTime)
        {
            GameObject enemy = Instantiate(enemyFactory);
            enemy.transform.position = transform.position;
            // 3. 소환 후 시간을 0으로 리셋
            currentTime = 0f;
            createTime = Random.Range(1, 5);
        }
    }
}
