using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class Tube : MonoBehaviour
{
   public List<Ball> ballInTube = new List<Ball>();
   private int maxBallInTube;
   public Transform topPosition;
   private Vector3 pivot = new Vector3(0,0.5f,0);
   

   private void Awake()
   {
      pivot = transform.position + pivot;
      maxBallInTube = 4;
   }

 //Hàm kiểm tra xem có thể nhận bóng hay không
   public bool CanReciveBall(Ball ball)
   {
      if(ballInTube.Count >= maxBallInTube) return false;
      return ballInTube.Count == 0 || ballInTube[ballInTube.Count - 1].type == ball.type;
   }
   //Lấy bóng có màu tương tự với bóng ở đỉnh liên tiếp
   public List<Ball> GetSameBallInTube()
   {
      if(ballInTube.Count == 0) return new List<Ball>();
      List<Ball> sameBallInTube = new List<Ball>();
      Ball topBall = ballInTube[ballInTube.Count - 1];
      for (int i = ballInTube.Count - 1; i >= 0; i--)
      {
         if (ballInTube[i].type == topBall.type)
         {
            sameBallInTube.Add(ballInTube[i]);
         }
         else
         {
            break;
         }
      }
      return sameBallInTube;
   }
   //Lấy vị trí top của bóng trong ống
   public Vector3 GetTopPosition()
   {
      return pivot + new Vector3(0,ballInTube.Count * 0.85f, 0);
   }
   public bool CheckComplete()
   {
      if (ballInTube.Count == 0) return true;
      Ball ballCheck = ballInTube[0];
      foreach (var ball in ballInTube)
      {
         if (ball.type != ballCheck.type)
         {
            return false;
         }
      }
      return true;
   }
}