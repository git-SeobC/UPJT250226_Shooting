using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;
    private float gravity = -9.8f;
    private float verticalSpeed = 0f;
    //private GameObject firePosition;
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        verticalSpeed += gravity * Time.deltaTime;
        transform.position += Vector3.down * verticalSpeed * Time.deltaTime;
    }

    private void OnEnable()
    {
        verticalSpeed = 0;
    }
}

