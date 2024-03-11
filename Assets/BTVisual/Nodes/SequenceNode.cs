﻿namespace BTVisual
{
    public class SequenceNode : CompositeNode
    {
        private int _current;
        
        protected override void OnStart()
        {
            _current = 0; //현재 실행할 차일드번호
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            var child = children[_current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure :
                    return State.Failure;
                case State.Success:
                    _current++; //다음차일드
                    break;
            }

            //모든 차일드가 성공적으로 수행되었다면 Success 아니면 running
            return _current == children.Count ? State.Success : State.Running; 
        }
    }
}