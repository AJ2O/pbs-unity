using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene.Entities
{
    /// <summary>
    /// The base class for objects meant to be drawn on the <seealso cref="Canvas"/>.
    /// </summary>
    public class BaseEntity : MonoBehaviour
    {
        #region Attributes
        /// <summary>
        /// The main sprite renderer.
        /// </summary>
        public SpriteRenderer spriteRenderer;
        /// <summary>
        /// A shadow sprite matching the shape of <seealso cref="spriteRenderer"/>.
        /// </summary>
        public SpriteRenderer shadowRenderer;

        /// <summary>
        /// Used for modifying properties of the sprite drawn in <seealso cref="spriteRenderer"/>.
        /// </summary>
        private MaterialPropertyBlock _propBlock;
        #endregion

        void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        /// <summary>
        /// Scales the size of this object by the specified properties.
        /// </summary>
        /// <param name="scaleAll">The amount to scale all the axes by.</param>
        /// <param name="scaleX">The amount to scale the X-size by.</param>
        /// <param name="scaleY">The amount to scale the Y-size by.</param>
        /// <param name="scaleZ">The amount to scale the Z-size by.</param>
        /// <param name="additive">If false, the object's scale is reset to (1,1,1) before applying scaling.</param>
        public void Scale(
            float scaleAll = 1f,
            float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f, 
            bool additive = true)
        {
            float newX = (additive ? transform.localScale.x : 1f) * scaleAll * scaleX;
            float newY = (additive ? transform.localScale.y : 1f) * scaleAll * scaleY;
            float newZ = (additive ? transform.localScale.z : 1f) * scaleAll * scaleZ;
            transform.localScale = new Vector3(newX, newY, newZ);
        }
    }
}

