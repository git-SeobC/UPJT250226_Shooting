using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float[] Layer_Speed = new float[5];
    public GameObject[] Layer_Objects = new GameObject[5];
    public float boundSizeX;
    public float sizeX;

    private Vector2[] startPos = new Vector2[5];
    private float[] currentPos = new float[5];

    void Start()
    {
        // 각 레이어의 초기 위치 저장
        for (int i = 0; i < 5; i++)
        {
            startPos[i] = Layer_Objects[i].transform.position;
            currentPos[i] = startPos[i].x;
        }
    }

    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            // 레이어를 왼쪽으로 이동
            currentPos[i] -= Layer_Speed[i] * Time.deltaTime;

            // 레이어가 화면 밖으로 나가면 다시 오른쪽으로 재배치
            if (currentPos[i] < startPos[i].x - boundSizeX * sizeX)
            {
                currentPos[i] += boundSizeX * sizeX;
            }

            // 레이어 위치 업데이트
            Layer_Objects[i].transform.position = new Vector2(currentPos[i], startPos[i].y);
        }
    }
}