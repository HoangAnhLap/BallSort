using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public int type;
    public Sprite[] sprites;
    public TheStack stack;
    public static float size = 90;


    public void SetType(int type)
    {
        this.type = type;
        GetComponent<Image>().sprite = sprites[type];
        transform.Rotate(Vector3.forward,Random.Range(0,360));
    }

    
    
}
