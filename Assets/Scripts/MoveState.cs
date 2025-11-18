using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MoveState
{
    public TheStack from;
    public TheStack to;
    public List<Bubble> bubble;
}
