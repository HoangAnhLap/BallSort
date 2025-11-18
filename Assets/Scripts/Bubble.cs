using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Bubble : MonoBehaviour
{
    public int type;
    public Sprite[] sprites;
    public Sprite HideSprite;
    public TheStack stack;
    public static float size = 90;
    public bool isHidden = false;


    public void Appear()
    {
        isHidden = false;
        GetComponent<Image>().sprite = sprites[type];
    }

    public void SetType(int type, bool hidden)
    {
        this.type = type;
        this.isHidden = hidden;
        if (isHidden)
        {
            GetComponent<Image>().sprite = HideSprite;
        }
        else
        {
            GetComponent<Image>().sprite = sprites[type];
        }

        transform.Rotate(Vector3.forward, Random.Range(0, 360));
    }
}