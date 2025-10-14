using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;

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
        bubble.stack = this;
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
        if (bubbles.Count == 0) return;
        poppedStack = this;
        poppedBubble = bubbles[bubbles.Count - 1];
        bubbles.Remove(poppedBubble);
        MoveToPosLocal(poppedBubble, topPos.localPosition);
        SoundManager.instance.SoundPlay(SoundManager.instance.ballSelected);
    }
    private void MoveToPosLocal(Bubble bubble,Vector3 target,System.Action callback = null)
    {
        bubble.transform.DOLocalMove(target, 0.15f).SetEase(Ease.InOutSine).OnComplete(() => callback?.Invoke());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameController.instance.isCompleted) return;
        //Kiểm tra xem có bóng nào được lấy chưa
        if (poppedBubble != null)
        {
            //Kiểm tra xem có double click hoặc có thể đẩy bóng sang không 
            
            if (poppedBubble.stack == this || CanPush(poppedBubble))
            {
                List<Bubble> sames = new List<Bubble>();
                //Trường hợp có thể đẩy bóng sang một stack khác 
                //Đẩy bóng
                if (poppedBubble.stack != this)
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
                lastPop.stack.Push(lastPop);
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
        Vector3 posInStack = GetPos();

        if (bubble.stack == this)
        {
            // Trả lại bóng vào cùng ống
            Vector3 target = GetPos();
            bubble.transform.DOLocalMove(target, 0.25f).SetEase(Ease.InOutSine);
            SoundManager.instance.SoundPlay(SoundManager.instance.ballPut);
        }
        else
        {
            // Di chuyển từ ống khác đến
            Vector3 targetStack = topPos.localPosition;
            bubble.transform.DOLocalMove(targetStack, 0.25f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    bubble.transform.DOLocalMove(posInStack, 0.25f).SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            if (IsSameFullColor())
                            {
                                SoundManager.instance.SoundPlay(SoundManager.instance.completeStack);
                                fullEffect.gameObject.SetActive(true);
                                fullEffect.Play();
                                Invoke(nameof(HideFullEffect), 5f);
                                GameController.instance.CheckComplete();
                            }
                            else
                            {
                                SoundManager.instance.SoundPlay(SoundManager.instance.merge);
                            }
                        });
                });

            var moveState = new MoveState()
            {
                bubble = bubble,
                from = bubble.stack,
                to = this
            };
            GameController.instance.undoMove.Add(moveState);
        }

        bubble.stack = this;
    }

    IEnumerator IEPushAllSame(List<Bubble> sames)
    {
        if (sames == null || sames.Count == 0) yield break;

        while (sames.Count > 0)
        {
            Bubble b = sames[0];
            MoveToPosLocal(b, poppedStack.topPos.localPosition, () =>
            {
                Push(b);
            });

            sames.RemoveAt(0);
            yield return new WaitForSeconds(0.1f);
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
        bubble.stack = this;
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
