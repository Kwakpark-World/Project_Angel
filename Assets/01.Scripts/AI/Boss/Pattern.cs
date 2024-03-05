using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BTVisual.Node;

namespace BTVisual
{
    public abstract class Pattern : MonoBehaviour
    {
        public Node node = null;

        public abstract void OnStart();

        public abstract void OnStop();

        public abstract State OnUpdate();
    }
}
