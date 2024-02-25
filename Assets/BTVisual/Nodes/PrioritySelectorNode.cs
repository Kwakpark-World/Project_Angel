using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public class PrioritySelectorNode : CompositeNode
    {
        private int _beforeIndex = 0;
        protected override void OnStart()
        {
            //_current = 0;
            _beforeIndex = 0;
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
                        if (_beforeIndex != i)
                        {
                            children[_beforeIndex].Break();
                        }
                        Debug.Log($"{i}, {_beforeIndex}");
                        _beforeIndex = i;
                        return State.Running;
                    case State.Failure:
                        //실패하면 아무것도 안함. 다음노드로 바로 넘어간다.
                        break;
                    case State.Success:
                        if (_beforeIndex != i)
                        {
                            children[_beforeIndex].Break();
                        }
                        _beforeIndex = i;
                        return State.Success;
                }
            }

            return State.Failure; //다 돌아도 실패면 실패
        }
    }
}