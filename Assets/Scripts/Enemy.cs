using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.5f;
    Vector3 dir;

    public GameObject explosionFactory;

    private void Start()
    {

        int rand = Random.Range(0, 10);
        // 10개 중에서 3개 약 30% 확률
        if (rand < 3)
        {
            var target = GameObject.FindGameObjectWithTag("Player");

            dir = target.transform.position - transform.position;

            dir.Normalize(); // 방향의 크기를 1로 설정
            // 방향 벡터(Vector3.up, Vector3.down, Vector3.left ...)
        }
        else
        {
            dir = Vector3.left;
        }
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = transform.position;

        Destroy(gameObject);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            ScoreManager.Instance().AddScore(10);
        }
    }
}
