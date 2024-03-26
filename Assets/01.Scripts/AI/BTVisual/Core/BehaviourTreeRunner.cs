using System;
using UnityEngine;

namespace BTVisual
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;
        public Brain brain;

        private void Start()
        {
            tree = tree.Clone(); //복제해서 시작함.
            tree.Bind(brain); //만약 EnemyBrain과 같은 녀석을 여기서 바인드해서 넣어줘야 한다면 수정
        }

        private void Update()
        {
            if (brain.CurrentHealth <= 0f)
            {
                return;
            }

            tree.Update();
        }
    }
}
