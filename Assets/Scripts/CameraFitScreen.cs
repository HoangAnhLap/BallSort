using System;
using UnityEngine;

public class CameraFitScreen : MonoBehaviour
{
    private void Start()
    {
        // Lấy tỷ lệ màn hình dọc (aspect ratio)
        float screenAspect = (float)Screen.width / (float)Screen.height;

        // Điều chỉnh chiều cao camera để phù hợp với tỷ lệ màn hình dọc
        float cameraHeight = Camera.main.orthographicSize * 2;

        // Tính chiều rộng camera
        float cameraWidth = cameraHeight * screenAspect;

        // Điều chỉnh camera sao cho chiều rộng của nó vừa với màn hình dọc
        Camera.main.orthographicSize = cameraWidth / (2f * screenAspect);
        
        // Hiển thị thông tin về camera
        Debug.Log("Camera Width: " + cameraWidth);
        Debug.Log("Camera Height: " + cameraHeight);


    }
}
