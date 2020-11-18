using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene
{
    public class BattleCamera : MonoBehaviour
    {
        public UnityEngine.Camera mainCamera;
        public Vector3 defaultPosition;

        private void Awake()
        {
            transform.position = defaultPosition;
        }
    }
}