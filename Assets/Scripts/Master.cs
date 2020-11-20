using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Static
{
    
    public class Master : MonoBehaviour
    {
        // Single Instance
        public static Master instance;

        public PBS.Networking.Manager networkManager;
        public PBS.Battle.View.UI.Canvas ui;
        public PBS.Battle.View.Scene.Canvas scene;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Screen.SetResolution(640, 360, false);
        }

    }
}
