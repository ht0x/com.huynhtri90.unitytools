using EnumCollection;
using UnityEngine;

namespace ObjectPooling
{
    public class PoolAssetParticle : PoolAssetBase
    {
        #region ----- Constructor -----

        public PoolAssetParticle(string id, PoolType type) : base(id, type)
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
            var prefab = Resources.Load<GameObject>("Prefabs/" + id);
            var go = Object.Instantiate(prefab);
            go.SetActive(false);
            go.name = id;

            var component = go.GetComponent<PoolComponentParticle>();
            if (component == null) component = go.AddComponent<PoolComponentParticle>();
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
