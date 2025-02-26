using UnityEngine;

// 배경 스크롤 기능
// Material 의 Offset을 건드렸더니 이미지가 밀리더라

// 필요한 것 : 머티리얼, 스크롤링 속도
// 어떻게 진행할 것인가 : 위로? 아래로? 옆으로?

public class Background : MonoBehaviour
{
    public Material backgroundMaterial;
    public float scrollSpeed = 0.2f;

    void Update()
    {
        backgroundMaterial.mainTextureOffset += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}
