using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;
    private GameObject nearestEnemy;
    //private float gravity = -9.8f;
    //private float verticalSpeed = 0f;
    //private GameObject firePosition;
    void Update()
    {
        //if (nearestEnemy != null)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, speed * Time.deltaTime);
        //}
        //else
        //{
            transform.position += Vector3.right * speed * Time.deltaTime;
        //}
    }

    //private void OnEnable()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject closest = null;
    //    float closestDistance = Mathf.Infinity;
    //    Vector3 position = transform.position;

    //    foreach (GameObject enemy in enemies)
    //    {
    //        Vector3 diff = enemy.transform.position - position;
    //        float curDistance = diff.sqrMagnitude;
    //        if (curDistance < closestDistance)
    //        {
    //            closest = enemy;
    //            closestDistance = curDistance;
    //        }
    //    }
    //    nearestEnemy = closest;
    //}
}

