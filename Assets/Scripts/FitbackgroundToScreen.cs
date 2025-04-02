using UnityEngine;

public class FitbackgroundToScreen : MonoBehaviour
{
    void Start()
    {
        FitBackgroundToScreen();
    }

    void FitBackgroundToScreen()
    {
        // Lấy tỷ lệ khung hình màn hình (aspect ratio)
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // Lấy sprite renderer của background
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Lấy kích thước của background
        float spriteWidth = spriteRenderer.bounds.size.x;
        float spriteHeight = spriteRenderer.bounds.size.y;

        // Tính toán chiều rộng và chiều cao của background sao cho phù hợp với màn hình dọc
        float scaleWidth = screenAspect * spriteHeight;
        float scaleHeight = spriteHeight;

        // Điều chỉnh kích thước của background (thay đổi scale của GameObject)
        float scaleFactor = 1.2f; // Tăng kích thước thêm 10%
        transform.localScale = new Vector3((scaleWidth / spriteWidth) * scaleFactor, (scaleHeight / spriteHeight) * scaleFactor, 1);

        // Hiển thị thông tin về kích thước background
        Debug.Log("Background Scale: " + transform.localScale);
    }
}
