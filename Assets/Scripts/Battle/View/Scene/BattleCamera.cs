using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene
{
    /// <summary>
    /// A camera meant to be used for viewing the <seealso cref="Canvas"/>.
    /// </summary>
    public class BattleCamera : MonoBehaviour
    {
        #region Attributes
        /// <summary>
        /// The Unity Camera to use.
        /// </summary>
        public UnityEngine.Camera mainCamera;
        /// <summary>
        /// The default/resting position of the camera.
        /// </summary>
        public Vector3 defaultPosition;
        #endregion

        private void Awake()
        {
            transform.position = defaultPosition;
        }
    }
}