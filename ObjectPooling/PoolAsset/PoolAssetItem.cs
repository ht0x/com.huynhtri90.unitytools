using System.IO;
using EnumCollection;
using UnityEngine;

namespace ObjectPooling
{
    public class PoolAssetItem : PoolAssetBase
    {
        #region ----- Constructor -----

        public PoolAssetItem(string id, PoolType type) : base(id, type)
        {

        }

        #endregion

        #region ----- Methods -----

        internal override PoolAssetBase Setup()
        {
            var poolBase = base.Setup();
            return poolBase;
        }

        protected override GameObject InitNewPoolObject(string id)
        {
            var prefab = Resources.Load<GameObject>(id);
            var go = Object.Instantiate(prefab);
            go.SetActive(false);
            go.name = id;
            var component = go.GetComponent<PoolComponentItem>();
            if (component == null) component = go.AddComponent<PoolComponentItem>();
            component.PoolId = id;

            return go;
        }

        internal override GameObject Use()
        {
            return base.Use();
        }

        internal override bool Return(GameObject go)
        {
            return base.Return(go);
        }

        #endregion
    }
}