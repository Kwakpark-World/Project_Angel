﻿using UnityEngine;

namespace BTVisual
{
    public class DebugNode : ActionNode
    {
        public string message;
        protected override void OnStart()
        {
            Debug.Log($"OnStart : {message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop : {message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate : {message}");
            return State.Success;
        }
    }
}