using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Tube selectedTube = null;
    private List<Ball> sameBall;
    private ControllState currentControllState;
    
    
    //TestComand
    public CommandInvoker commandInvoker;
    private ICommand selectedCommand;

    private void Start()
    {
        commandInvoker = new CommandInvoker();
        SetControllState(ControllState.Start);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentControllState == ControllState.Processing)return;
            if(Camera.main == null ) return;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if(hit.collider == null ) return;
            if (hit.collider.CompareTag("Tube"))
            {
                Tube clickedTube = hit.collider.GetComponent<Tube>();
                if (clickedTube != null)
                {
                    HandleTubeClick(clickedTube);
                }
            }
        }
        
        
    }

    //Xử kí khi ấn chọn vào ống nghiệm.
    private void HandleTubeClick(Tube clickedTube)
    {
        //
        if (selectedTube == null)
        {
            if (clickedTube.ballInTube.Count== 0) return;
            selectedTube = clickedTube;
            sameBall = selectedTube.GetSameBallInTube();
            Debug.Log("Created SelectedCommand for: " + selectedTube.name);
            selectedCommand = new SelectedCommand(sameBall, selectedTube);
            commandInvoker.ExecuteSelectedCommand(selectedCommand);
            
            
        }else
        {
            if (selectedTube != clickedTube)
            {
                SetControllState(ControllState.Processing);
                //StartCoroutine(MoveToTube(selectedTube,clickedTube,sameBall));
                MoveBalls(selectedTube, clickedTube);
            }
            else
            {
                commandInvoker.UndoLastSelectedCommand();
                SetControllState(ControllState.Finished);
            }
        }
    }

    
 //Comman di chuyển bóng
    public void MoveBalls(Tube fromTube, Tube toTube)
    {
        if (sameBall.Count > 0 && toTube.CanReciveBall(sameBall[0]))
        {
            ICommand movedCommand = new MoveBallCommand(sameBall, fromTube, toTube);
            commandInvoker.ExecuteMoveCommand(movedCommand);
        }
        else
        {
            //Trường hợp đang thêm bóng mà tube đầy chưa được xử lí
            commandInvoker.UndoLastSelectedCommand();
        }
        SetControllState(ControllState.Finished);
        GameManager.instance.CheckWinCodition();
    }

    //Hàm undo
    public void UndoMove()
    {
        commandInvoker.UndoLastMoveCommand();
    }
    
    //Quản lí trạng thái control
      private void SetControllState(ControllState newState)
    {
        if(currentControllState == newState) return;
        currentControllState = newState;
        switch (currentControllState)
        {
            case ControllState.Processing:
                break;
            case ControllState.Finished:
                selectedTube = null;
                break;
            case ControllState.Start:
                selectedTube = null;
                break;
        }
    }
    
}
//Enum quản lí trạng thái của controller
//Dùng để tránh việc người chơi nhấn chuột khi game dang thực hiện
public enum ControllState
{
    Processing,
    Finished,
    Start
        
}


