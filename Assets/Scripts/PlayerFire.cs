using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private GameObject bulletFactory;    // 총알 프리팹
    private GameObject firePosition;     // 총 발사 위치
    private GameObject unit1;
    private GameObject unit2;
    private GameObject unit3;
    private GameObject unit4;

    #region ObjectPool
    public int poolSize = 50;
    GameObject[] bulletObjectPool;

    public float fireRate;
    private float normalFireRate;
    private float nextFireTime = 0f;

    private void Start()
    {
        bulletFactory = Resources.Load<GameObject>("Prefabs/Bullet");
        bulletFactory = Resources.Load<GameObject>("Prefabs/Bullet");
        firePosition = transform.Find("FirePosition").gameObject;
        unit1 = GameObject.Find("Player/Unit1").gameObject;
        unit2 = GameObject.Find("Player/Unit2").gameObject;
        unit3 = GameObject.Find("Player/Unit3").gameObject;
        unit4 = GameObject.Find("Player/Unit4").gameObject;
        normalFireRate = fireRate;
        unit1.gameObject.SetActive(ScoreManager.Instance().unitActivated[0]);
        unit2.gameObject.SetActive(ScoreManager.Instance().unitActivated[1]);
        unit3.gameObject.SetActive(ScoreManager.Instance().unitActivated[2]);
        unit4.gameObject.SetActive(ScoreManager.Instance().unitActivated[3]);

        ScoreManager.Instance().UnitActivationChanged += OnUnitActivationChanged;

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

    private void OnUnitActivationChanged(object sender, UnitActivationEvent e)
    {
        if (e.UnitActivated[3])
        {
            fireRate = normalFireRate * 0.75f;
            unit4.gameObject.SetActive(true);
        }
        else unit4.gameObject.SetActive(false);
        if (e.UnitActivated[2])
        {
            unit3.gameObject.SetActive(true);
        }
        else unit3.gameObject.SetActive(false);
        if (e.UnitActivated[1])
        {
            unit2.gameObject.SetActive(true);
        }
        else unit2.gameObject.SetActive(false);
        if (e.UnitActivated[0])
        {
            unit1.gameObject.SetActive(true);
        }
        else
        {
            fireRate = normalFireRate;
            unit1.gameObject.SetActive(false);
        }
    }

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
        for (int i = 0; i < poolSize * 0.2f; i++)
        {
            var bullet = bulletObjectPool[i];
            if (bullet.activeSelf == false)
            {
                bullet.transform.position = firePosition.transform.position;
                bullet.SetActive(true);
                break;
            }
        }
        if (unit1.activeSelf)
        {
            for (int i = (int)(poolSize * 0.2f); i < poolSize * 0.4f; i++)
            {
                var bullet = bulletObjectPool[i];
                if (bullet.activeSelf == false)
                {
                    bullet.transform.position = unit1.transform.position;
                    bullet.SetActive(true);
                    break;
                }
            }
        }
        if (unit2.activeSelf)
        {
            for (int i = (int)(poolSize * 0.4f); i < poolSize * 0.6f; i++)
            {
                var bullet = bulletObjectPool[i];
                if (bullet.activeSelf == false)
                {
                    bullet.transform.position = unit2.transform.position;
                    bullet.SetActive(true);
                    break;
                }
            }
        }
        if (unit3.activeSelf)
        {
            for (int i = (int)(poolSize * 0.6f); i < poolSize * 0.8f; i++)
            {
                var bullet = bulletObjectPool[i];
                if (bullet.activeSelf == false)
                {
                    bullet.transform.position = unit3.transform.position;
                    bullet.SetActive(true);
                    break;
                }
            }
        }
        if (unit4.activeSelf)
        {
            for (int i = (int)(poolSize * 0.8f); i < poolSize; i++)
            {
                var bullet = bulletObjectPool[i];
                if (bullet.activeSelf == false)
                {
                    bullet.transform.position = unit4.transform.position;
                    bullet.SetActive(true);
                    break;
                }
            }
        }
    }
}
