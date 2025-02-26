using System.Collections;
using UnityEngine;

public class D_Zone : MonoBehaviour
{
    private bool isPlayerOutOfBounds = false;
    private Coroutine outOfBoundsCoroutine;

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(collision.gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isPlayerOutOfBounds)
            {
                // 플레이어가 다시 D_Zone 안으로 들어왔을 때
                isPlayerOutOfBounds = false;
                if (outOfBoundsCoroutine != null)
                {
                    StopCoroutine(outOfBoundsCoroutine);
                }
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어가 D_Zone을 벗어났을 때
            isPlayerOutOfBounds = true;
            outOfBoundsCoroutine = StartCoroutine(OutOfRangePlayer(collision.gameObject));
        }
    }

    IEnumerator OutOfRangePlayer(GameObject player)
    {
        yield return new WaitForSeconds(5);

        if (isPlayerOutOfBounds)
        {
            Destroy(player);
        }
    }
}
