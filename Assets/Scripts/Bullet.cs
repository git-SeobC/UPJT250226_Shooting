using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        //StartCoroutine(BulletDestroy());
    }

    IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
