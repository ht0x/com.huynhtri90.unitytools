using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace ObjectPooling
{
    public class PoolManager : MonoBehaviour
    {
        #region ----- Variables -----

        private static PoolManager s_Instance;
        private Dictionary<string, PoolAssetBase> poolDic;

        #endregion

        #region ----- Properties -----

        public static PoolManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var go = new GameObject()
                    {
                        name = nameof(PoolManager)
                    };
                    s_Instance = go.AddComponent<PoolManager>();
                }

                return s_Instance;
            }
        }

        #endregion

        #region ----- Unity Methods -----

        private void Awake()
        {
            if (!ReferenceEquals(s_Instance, null))
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            poolDic = new Dictionary<string, PoolAssetBase>();
        }

        private void OnDestroy()
        {
            poolDic?.Clear();
            poolDic = null;
            s_Instance = null;
        }

        #endregion

        #region ----- Methods -----

        private GameObject Use(string id, PoolType type, bool isForceInit = false)
        {
            if (poolDic.TryGetValue(id, out var poolBase))
                return UseExistingPool(poolBase, isForceInit);

            return CreateNewPool(id, type);
        }

        GameObject UseExistingPool(PoolAssetBase poolAssetBase, bool isForceInit)
        {
            GameObject pool = null;
            pool = isForceInit ? poolAssetBase.Create() : poolAssetBase.Use();
            return pool;
        }

        GameObject CreateNewPool(string id, PoolType type)
        {
            PoolAssetBase newPoolAssetBase = null;
            newPoolAssetBase = type switch
            {
                PoolType.Item => new PoolAssetItem(id, type).Setup(),
                PoolType.Particle => new PoolAssetParticle(id, type).Setup(),
                PoolType.Audio => new PoolAssetAudio(id, type).Setup(),
                _ => null
            };

            if (ReferenceEquals(newPoolAssetBase, null))
            {
                CLog.LogErrorLoopEditor($"[{nameof(PoolManager)}-{nameof(CreateNewPool)}] " +
                                        $"Error! Can't create a new pool with specific type and id:\n" +
                                        $"Type: {type}\n " +
                                        $"Id: {id}");
                return null;
            }

            poolDic.Add(id, newPoolAssetBase);
            return newPoolAssetBase.Use();
        }

        private bool Return(GameObject go)
        {
            if (go == null)
            {
                CLog.LogErrorLoopEditor($"[{nameof(PoolManager)}-{nameof(Return)}] Error! Can't return null object!");
                return false;
            }

            var poolComponent = go.GetComponent<PoolComponentBase>();
            if (poolComponent == null)
            {
                CLog.LogErrorLoopEditor($"[{nameof(PoolManager)}-{nameof(Return)}] Error! No pool component found!");
                return false;
            }

            if (poolDic.TryGetValue(poolComponent.PoolId, out var poolAssetBase))
                return poolAssetBase.Return(go);

            return false;
        }

        #endregion

        #region ----- Item Methods -----

        public GameObject UseItem(string id, bool isForceInit = false)
        {
            var go = Use(id, PoolType.Item, isForceInit);
            var itemComponent = go.GetComponent<PoolComponentItem>();

            if (go != null) go.SetActive(true);
            return go;
        }

        public bool ReturnItem(GameObject go)
        {
            return Return(go);
        }

        #endregion

        #region ----- Audio Methods -----

        public GameObject UseAudio(string id, bool isForceInit = false)
        {
            var go = Use(id, PoolType.Audio, isForceInit);
            var audioComponent = go.GetComponent<PoolComponentAudio>();

            if (go != null) go.SetActive(true);
            return go;
        }

        public bool ReturnAudio(GameObject go)
        {
            return Return(go);
        }

        #endregion

        #region ----- Particle Methods -----

        public GameObject UseParticle(string id, bool isForceInit = false)
        {
            var go = Use(id, PoolType.Particle, isForceInit);
            var particleComponent = go.GetComponent<PoolComponentParticle>();

            if (go != null) go.SetActive(true);
            return go;
        }

        public bool ReturnParticle(GameObject go)
        {
            return Return(go);
        }

        #endregion
    }
}