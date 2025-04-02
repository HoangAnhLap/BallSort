using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Animator anim;
    private float speed;
    public int type;
    private void Start()
    {
        anim = GetComponent<Animator>();
        speed = 30f;
    }
    //Gọi hàm di chuyển đến từng đích
    public IEnumerator MoveBallCoroutine(Tube fromTube,Tube toTube)
    {
        yield return MovePos(fromTube.topPosition.position);
        yield return MovePos(toTube.topPosition.position);
        yield return MovePos(toTube.GetTopPosition());
        anim.SetBool("Selected",false);

    }
    //Di chuyển bóng tới điểm đích
    public IEnumerator MovePos(Vector3 targetPosition)
    {
        //Biến targetPos sẽ lưu lại vị trí để tránh TH targetPos có thể bị null
        Vector3 targetPos = targetPosition;
        while (Vector3.Distance(transform.position,targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos,speed *Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
    }
    public void Selected()
    {
        anim.SetBool("Selected",true);
    }

    public void Deselected()
    {
        anim.SetBool("Selected",false);
    }

    public void SetType(int type)
    {
        
    }
    
}
