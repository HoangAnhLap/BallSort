using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Experimental;
using UnityEngine.EventSystems;

public class TheStack : MonoBehaviour,IPointerDownHandler
{
    public Bubble bubblePrefab;
    public List<Bubble> bubbles = new List<Bubble>();
    public Transform topPos;
    public ParticleSystem fullEffect;
    
    
    
    public static Bubble poppedBubble;
    public static TheStack poppedStack;
    
    //
   

    public Bubble InstantiateBubble(int type)
    {
        var bubble = Instantiate(bubblePrefab, transform);
        bubble.SetType(type);
        bubble.thisStack = this;
        return bubble;
    }
    //Đẩy bóng ống
    public void ForcePush(Bubble bubble)
    {
        if(bubbles.Count == 4)return;
        bubbles.Add(bubble);
        bubble.transform.localPosition = GetPos();
    }
    //Lấy quả bóng đầu tiên
    public void Pop()
    {
        if(bubbles.Count == 0) return;
        poppedStack = this;
        poppedBubble = bubbles[bubbles.Count - 1];
        bubbles.Remove(poppedBubble);
        StartCoroutine(MoveToPosLocal(poppedBubble, topPos.localPosition));
    }
    IEnumerator MoveToPosLocal(Bubble bubble,Vector3 target,System.Action callback = null)
    {
        while (Vector3.Distance(bubble.transform.localPosition, target) > 0.1f)
        {
            bubble.transform.localPosition = Vector3.Lerp(bubble.transform.localPosition, target,25f * Time.deltaTime);
            yield return null;
        }
        bubble.transform.localPosition = target;
        callback?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameController.instance.isCompleted) return;
        //Kiểm tra xem có bóng nào được lấy chưa
        if (poppedBubble != null)
        {
            //Kiểm tra xem có double click hoặc có thể đẩy bóng sang không 
            
            if (poppedBubble.thisStack == this || CanPush(poppedBubble))
            {
                List<Bubble> sames = new List<Bubble>();
                //Trường hợp có thể đẩy bóng sang một stack khác 
                //Đẩy bóng
                if (poppedBubble.thisStack != this)
                {
                    sames = poppedStack.GetSameBubbles(4 - this.bubbles.Count - 1 , poppedBubble.type);
                }
                
                Push(poppedBubble);
                
                if (sames != null && sames.Count > 0) StartCoroutine(IEPushAllSame(sames));
                
                poppedBubble = null;
            }
            //Trường hợp chọn stack khác nhưng không thể đẩy bóng sang
            else
            {
                var lastPop = poppedBubble;
                Pop();
                lastPop.thisStack.Push(lastPop);
            }
        }
        //Nếu chưa lấy ra một quả từ stack này
        else
        {
            Pop();
        }
        
    }
    public void Push(Bubble bubble)
    {
        bubbles.Add(bubble);
        bubble.transform.SetParent(transform);
        Vector3 posInStack = this.GetPos();
        //Trường hợp bóng của ống này thì đẩy lại vào ống
        if (bubble.thisStack == this)
        {
            Vector3 target = bubble.thisStack.GetPos();
            StartCoroutine(MoveToPosLocal(bubble, target));
        }
        //Trường hợp bóng của ống khác thì di chuyển bóng tới ống chọn
        else
        {
            Vector3 targetStack =  this.topPos.localPosition;
            //Di chuyển tới ống chọn
            StartCoroutine(MoveToPosLocal(bubble, targetStack,() => StartCoroutine(MoveToPosLocal(bubble,posInStack,
                () =>
                {
                    if (IsSameFullColor())
                    {
                        fullEffect.gameObject.SetActive(true);
                        fullEffect.Play();
                        Invoke(nameof(HideFullEffect),5f);
                        GameController.instance.CheckComplete();
                    }

                    
                }))));
            var moveState = new MoveState()
            {
                bubble = bubble,
                from = bubble.thisStack,
                to = this
            };
            GameController.instance.undoMove.Add(moveState);
            

        }
        bubble.thisStack = this;
    }
    IEnumerator IEPushAllSame(List<Bubble> sames)
    {
        if(sames == null || sames.Count == 0) yield break;
        bool wait = false;
        while(sames.Count > 0)
        {
            if (!wait)
            {
                wait = true;
             yield return  poppedStack.StartCoroutine(MoveToPosLocal(sames[0], poppedStack.topPos.localPosition,
                    () => Push(sames[0])));
                sames.RemoveAt(0);
                wait = false;
            }
            yield return null;
        }

        poppedStack = null;
    }
    
    //Kiểm tra xem có đẩy bóng vào được không
    public bool CanPush(Bubble bubble)
    {
        return (bubbles.Count == 0 || bubble.type == bubbles[bubbles.Count - 1].type && bubbles.Count != 4);
    }
    public Vector3 GetPos()
    {
        
        return transform.position +  new Vector3(0,Bubble.size * (bubbles.Count - 0.45f), 0);
    }

    
    //Đẩy bóng đang chọn vào lại
    public void PushPopBack()
    {
        if (poppedBubble != null && poppedStack == this)
        {
            Push(poppedBubble);
            poppedBubble = null;
        }
    }
    
    public void PushBack(Bubble bubble)
    {
        bubbles.Add(bubble);
        bubble.transform.SetParent(transform);
        Vector3 target = this.GetPos();
        bubble.transform.localPosition = target;
        bubble.thisStack = this;
    }

    public void RemoveLastBubble()
    {
        Debug.Log("Removing last bubble");
        bubbles.RemoveAt(bubbles.Count - 1);
    }
    public void HideFullEffect()
    {
        fullEffect.gameObject.SetActive(false);
    }
    //Lấy danh sách bóng tương tự có thể được di chuyển
    public List<Bubble> GetSameBubbles(int maxBonus, int type)
    {
        if (maxBonus <= 0) return null;
        List<Bubble> list = new List<Bubble>();
        for (int i = poppedStack.bubbles.Count - 1; i >= 0 && list.Count < maxBonus;)
        {
            if (poppedStack.bubbles[i].type == type)
            {
                list.Add(poppedStack.bubbles[i]);
                poppedStack.bubbles.RemoveAt(i);
                --i;
            }
            else
            {
                return list;
            }
        }
        return list;
    }

    public bool IsSameFullColor()
    {
        if (bubbles.Count == 0) return true;
        if (bubbles.Count != 4) return false;
        int type  = bubbles[0].type;
        for (int i = 0; i < 4; i++)
        {
            if (bubbles[i].type != type)
            {
                return false;
            }
        }

        return true;
    }
}
