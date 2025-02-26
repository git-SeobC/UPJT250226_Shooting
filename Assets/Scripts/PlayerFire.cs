using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private GameObject bulletFactory;    // 총알 프리팹
    [SerializeField] private GameObject firePosition;     // 총 발사 위치

    private void Start()
    {
        bulletFactory = Resources.Load<GameObject>("Prefabs/Bullet");
        firePosition = transform.Find("FirePosition").gameObject;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Fire1 = L Ctrl , L Button
        {
            GameObject bullet = Instantiate(bulletFactory); // 총알 생성
            bullet.transform.position = firePosition.transform.position; // 총알 위치 변경
        }
    }
}
