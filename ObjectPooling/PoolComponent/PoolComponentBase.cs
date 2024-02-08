using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class PoolComponentBase : MonoBehaviour
    {
        #region ----- Variables ----

        private string poolId;

        #endregion

        #region ----- Properties -----

        internal string PoolId
        {
            get => poolId;
            set => poolId = value;
        }

        #endregion
    }
}
