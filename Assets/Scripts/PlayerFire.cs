using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private GameObject bulletFactory;    // 총알 프리팹
    private GameObject firePosition;     // 총 발사 위치
    private GameObject unit1;
    private GameObject unit2;

    #region ObjectPool
    public int poolSize = 50;
    GameObject[] bulletObjectPool;

    public float fireRate = 0.2f;
    private float nextFireTime = 0f;


    private void Start()
    {
        bulletFactory = Resources.Load<GameObject>("Prefabs/Bullet");
        bulletFactory = Resources.Load<GameObject>("Prefabs/Bullet");
        firePosition = transform.Find("FirePosition").gameObject;
        unit1 = GameObject.Find("Player/Unit1").gameObject;
        unit2 = GameObject.Find("Player/Unit2").gameObject;

        //1.설정된 크기만큼 풀에 오브젝트 생성
        bulletObjectPool = new GameObject[poolSize];

        //2.수만큼 반복해 총알 생성
        for (int i = 0; i < poolSize; i++)
        {
            var bullet = Instantiate(bulletFactory); // 총알 생성
            bulletObjectPool[i] = bullet; // 풀에 등록
            bullet.SetActive(false); // 비활성화
        }
    }
    #endregion

    void Update()
    {
        if (ScoreManager.Instance().gameOver) return;
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime) // Fire1 = L Ctrl , L Button
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var bullet = bulletObjectPool[i];
            if (bullet.activeSelf == false)
            {
                bullet.transform.position = firePosition.transform.position;
                bullet.SetActive(true);
                if (unit1.activeSelf)
                {
                    for (int j = i + 1; j < poolSize; j++)
                    {
                        var bullet2 = bulletObjectPool[j];
                        if (bullet2.activeSelf == false)
                        {
                            bullet2.transform.position = unit1.transform.position;
                            bullet2.SetActive(true);
                            if (unit2.activeSelf)
                            {
                                for (int k = j + 1; k < poolSize; k++)
                                {
                                    var bullet3 = bulletObjectPool[k];
                                    if (bullet3.activeSelf == false)
                                    {
                                        bullet3.transform.position = unit2.transform.position;
                                        bullet3.SetActive(true);
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                break;
            }
        }
        //GameObject bullet = Instantiate(bulletFactory); // 총알 생성
        //bullet.transform.position = firePosition.transform.position; // 총알 위치 변경
    }
}
