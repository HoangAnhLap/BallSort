using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker
{
    private ICommand lastCommandMoveHistory;
    private ICommand lastCommandSelectedHistory;
    

    public void ExecuteMoveCommand(ICommand command)
    {
        command.Execute();
        lastCommandMoveHistory = command;
    }
    
    //Return lại lượt di chuyển gần nhất
    public void UndoLastMoveCommand()
    {
        if (lastCommandMoveHistory != null)
        {
            lastCommandMoveHistory.Undo();
            lastCommandMoveHistory = null;
        }
    }
    
    
    public void ExecuteSelectedCommand(ICommand command)
    {
        command.Execute();
        lastCommandSelectedHistory = command;
    }
    
    //Bỏ chọn
    public void UndoLastSelectedCommand()
    {
        if (lastCommandSelectedHistory != null)
        {
            lastCommandSelectedHistory.Undo();
            lastCommandSelectedHistory = null;
        }
    }
    

    
}
