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
                        break;

                    case State.Success:
                        return State.Success;
                }
            }
            
            return State.Failure;
        }
    }
}