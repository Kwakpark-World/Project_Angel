using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public enum ENodeState
    {
        ENS_Running,
        ENS_Success,
        ENS_Failure,
        ENS_IDLE
    }

    public ENodeState Evaluate();
}

public class BehaviorTreeRunner : MonoBehaviour
{
    INode _rootNode;
    public BehaviorTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}

