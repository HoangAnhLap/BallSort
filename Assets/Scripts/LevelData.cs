using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class LevelData
{
    public int numStack;
    public List<int> bubbleTypes;
}


[System.Serializable]
public class BubbleData
{
    public int bubbleType;
    public bool isHidden;
}