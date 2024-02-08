using System.Collections.Generic;
using EnumCollection;
using UnityEngine;

namespace ObjectPooling
{
    public class PoolAssetBase
    {
        #region ----- Variables -----

        protected string poolId;
        protected PoolType poolType;
        protected GameObject rootPoolOccupied;
        protected GameObject rootPoolFree;
        protected List<GameObject> poolOccupied;
        protected List<GameObject> poolFree;

        #endregion
        
        #region ----- Constructor -----

        internal PoolAssetBase(string id, PoolType type)
        {
            this.poolId = id;
            this.poolType = type;
            poolOccupied = new List<GameObject>();
            poolFree = new List<GameObject>();
        }
        
        #endregion

        #region ----- Methods -----

        internal virtual PoolAssetBase Setup()
        {
            if (!SetupPoolRoot()) return null;
            SetupExistingPools();
            
            return this;
        }

        bool SetupPoolRoot()
        {
            var managerRoot = GameObject.Find(nameof(PoolManager));
            if (managerRoot == null)
            {
                CLog.LogErrorLoopEditor($"[{nameof(PoolAssetBase)}-{nameof(Setup)}] Error! Can't find RKPoolManager root!");
                return false;
            }

            var poolTypeRootName = "Pool" + poolType;
            var poolTypeRoot = MyUtilities.FindObject(managerRoot, poolTypeRootName);
            if (poolTypeRoot == null)
            {
                poolTypeRoot = new GameObject(poolTypeRootName);
                poolTypeRoot.transform.SetParent(managerRoot.transform);
            }

            var rootPoolFreeName = poolId + "_Free";
            rootPoolFree = MyUtilities.FindObject(poolTypeRoot, rootPoolFreeName);
            if (rootPoolFree == null)
            {
                rootPoolFree = new GameObject(rootPoolFreeName);
                rootPoolFree.transform.SetParent(poolTypeRoot.transform);
            }

            var rootPoolOccupiedName = poolId + "_Occupied";
            rootPoolOccupied = MyUtilities.FindObject(poolTypeRoot, rootPoolOccupiedName);
            if (rootPoolOccupied == null)
            {
                rootPoolOccupied = new GameObject(rootPoolOccupiedName);
                rootPoolOccupied.transform.SetParent(poolTypeRoot.transform);
            }

            return true;
        }

        void SetupExistingPools()
        {
            poolFree ??= new List<GameObject>();
            var poolFreeChildCount = rootPoolFree.transform.childCount;
            for (var i = 0; i < poolFreeChildCount; i++)
            {
                var child = rootPoolFree.transform.GetChild(i);
                if (child == null) continue;
                
                poolFree.Add(child.gameObject);
            }

            poolOccupied ??= new List<GameObject>();
            var poolOccupiedChildCount = rootPoolOccupied.transform.childCount;
            for (var i = 0; i < poolOccupiedChildCount; i++)
            {
                var child = rootPoolOccupied.transform.GetChild(i);
                if (child == null) continue;
                
                poolOccupied.Add(child.gameObject);
            }
        }
        
        protected virtual GameObject InitNewPoolObject(string id)
        {
            return null;
        }
        
        internal GameObject Create()
        {
            var newPool = InitNewPoolObject(poolId);
            newPool.transform.SetParent(rootPoolOccupied.transform, false);
            poolOccupied.Add(newPool);
            
            return newPool;
        }

        internal virtual GameObject Use()
        {
            poolFree ??= new List<GameObject>();
            poolOccupied ??= new List<GameObject>();
            for (var i = 0; i < poolFree.Count; i++)
            {
                var pool = poolFree[i];
                if (pool == null) continue;
                if (pool.activeInHierarchy) continue;
                
                poolFree.Remove(pool);
                pool.transform.SetParent(rootPoolOccupied.transform, false);
                if (!poolOccupied.Contains(pool))
                    poolOccupied.Add(pool);

                return pool;
            }

            var newPool = InitNewPoolObject(poolId);
            newPool.transform.SetParent(rootPoolOccupied.transform, false);
            poolOccupied.Add(newPool);
            
            return newPool;
        }

        internal virtual bool Return(GameObject go)
        {
            if (go == null)
            {
                CLog.LogErrorLoopEditor($"[{nameof(PoolAssetBase)}-{nameof(Return)}] Error! Can't return null object!");
                return false;
            }
            
            poolFree ??= new List<GameObject>();
            poolOccupied ??= new List<GameObject>();
            if (poolOccupied.Contains(go))
                poolOccupied.Remove(go);
            
            go.transform.SetParent(rootPoolFree.transform, false);
            go.SetActive(false);
            if (!poolFree.Contains(go))
                poolFree.Add(go);

            return true;
        }
        
        #endregion
    }
}
