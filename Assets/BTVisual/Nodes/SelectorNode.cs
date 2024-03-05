using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class SelectorNode : CompositeNode
    {
        
        protected override void OnStart()
        {
            //_current = 0;
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            for (int i = 0; i < children.Count; ++i)
            {
                var child = children[i];
                
                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        //실패하면 아무것도 안함. 다음노드로 바로 넘어간다.
                        break;
                    case State.Success:
                        return State.Success;
                }
            }
            
            return State.Failure; //다 돌아도 실패면 실패
        }
    }
}