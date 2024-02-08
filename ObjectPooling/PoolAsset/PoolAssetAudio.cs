using UnityEngine;
using EnumCollection;

namespace ObjectPooling
{
    public class PoolAssetAudio : PoolAssetBase
    {
        #region ----- Constructor -----

        public PoolAssetAudio(string id, PoolType type) : base(id, type)
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
            /*
             * TriHD's notes
             * PIC: KhangPS
             * TODO: Implement loading asset here.
             */
            var prefab = Resources.Load<GameObject>("Prefabs/" + id);
            var go = Object.Instantiate(prefab);
            go.SetActive(false);
            go.name = id;

            var component = go.GetComponent<PoolComponentAudio>();
            if (component == null) component = go.AddComponent<PoolComponentAudio>();
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