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

        
    }
}
