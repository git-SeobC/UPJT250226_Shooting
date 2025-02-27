using System.Collections;
using UnityEngine;

public class D_Zone : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name.Contains("Bullet") || collision.gameObject.name.Contains("Enemy"))
        {
            collision.gameObject.SetActive(false);
        }
        //Destroy(collision.gameObject);
    }
}
