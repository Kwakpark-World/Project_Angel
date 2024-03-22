using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public abstract class Pattern : MonoBehaviour
    {
        public Node OwnerNode { get; set; }

        public abstract void OnStart();

        public abstract void OnStop();

        public abstract Node.State OnUpdate();
    }
}
