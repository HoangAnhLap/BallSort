using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class MoveBallCommand : ICommand
{
    private List<Ball> _balls;//Danh sách bóng dùng để di chuyển(sameBall)
    private Tube _fromTube;
    private Tube _toTube;
    private List<Vector3> _prevPositions;//Vị trí cũ để thực hiện Undo

    public MoveBallCommand(List<Ball> balls, Tube fromTube, Tube toTube)
    {
        this._balls = balls;
        this._fromTube = fromTube;  
        this._toTube = toTube;
        this._prevPositions = new List<Vector3>();
        foreach (var ball in _balls)
        {
            _prevPositions.Add(ball.transform.position);
        }
    }
    //Thực hiện di chuyển bóng
    public void Execute()
    {
        if (_toTube.CanReciveBall(_balls[0]))
        {
            _toTube.StartCoroutine(MoveBalls());
        }
    }
    
    private IEnumerator MoveBalls()
    {
        //Di chuyển từng quả bóng
        foreach (Ball ball in _balls)
        {
            _fromTube.ballInTube.Remove(ball);
            yield return ball.MoveBallCoroutine(_fromTube, _toTube);
            _toTube.ballInTube.Add(ball);
        }
    }

    
    //Quay ngược lại lượt vừa rồi
    public void Undo()
    {
        //Lưu lại từng vị trí đã đi qua của từng quả bóng
            foreach (var ball in _balls)
            {
                _toTube.ballInTube.Remove(ball);
                ball.transform.position = _prevPositions[_balls.IndexOf(ball)];
                _fromTube.ballInTube.Add(ball); 
            }
        
    }
}