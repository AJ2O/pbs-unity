using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene.Entities
{
    public class BaseEntity : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer shadowRenderer;

        private MaterialPropertyBlock _propBlock;
        void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        public void Scale(
            float scaleAll = 1f,
            float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f, 
            bool absolute = true)
        {
            float newX = (absolute ? transform.localScale.x : 1f) * scaleAll * scaleX;
            float newY = (absolute ? transform.localScale.y : 1f) * scaleAll * scaleY;
            float newZ = (absolute ? transform.localScale.z : 1f) * scaleAll * scaleZ;
            transform.localScale = new Vector3(newX, newY, newZ);
        }
    }
}

