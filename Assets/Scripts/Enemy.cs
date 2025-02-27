using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.5f;
    Vector3 dir;

    public GameObject explosionFactory;

    // OnEnable은 유니티에서 제공해주는 활성화 단계에 호출되는 함수
    // 껐다가 킬 때마다가 작동

    private void OnEnable()
    {
        int rand = Random.Range(0, 10);
        // 10개 중에서 3개 약 30% 확률
        if (rand < 7)
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            if (target == null) dir = Vector3.left;
            else dir = target.transform.position - transform.position;

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
        if (ScoreManager.Instance().gameOver) gameObject.SetActive(false);

        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject explosion = Instantiate(explosionFactory);
        explosion.transform.position = transform.position;

        // 물체의 이름에 Bullet이 포함되어 있다면
        if (collision.gameObject.name.Contains("Bullet"))
        {
            collision.gameObject.SetActive(false);
        }
        else
        {
            //if (true /*UI 무적이 켜져있으면 off 되도록 기능 추가*/)
            //{
            //    Destroy(collision.gameObject);
            //}
        }

        gameObject.SetActive(false);
        //Destroy(gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            ScoreManager.Instance().AddScore(10);
        }
    }
}
