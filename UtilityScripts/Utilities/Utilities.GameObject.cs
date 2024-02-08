using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public partial class MyUtilities
{
    #region ----- Static Methods -----

    public static GameObject FindObjectChild(GameObject root, string name, bool ignoreCase = false)
    {
        if (root)
        {
            Transform rootTrans = root.transform;
            var count = rootTrans.childCount;
            if (ignoreCase) name = name.ToLower();
            for (var i = 0; i < count; i++)
            {
                Transform childTrans = rootTrans.GetChild(i);
                var obj = childTrans.gameObject;            
                string objName = ignoreCase ? obj.name.ToLower() : obj.name;

                if (objName.Equals(name))
                    return obj;
            }
        }

        return null;
    }

    public static GameObject FindObjectChild(GameObject root, string contain, StringComparison comparison)
    {
        if (root)
        {
            Transform rootTrans = root.transform;
            var count = rootTrans.childCount;

            for (var i = 0; i < count; i++)
            {
                Transform childTrans = rootTrans.GetChild(i);
                var obj = childTrans.gameObject;
                String objName = obj.name;

                if (objName.StartsWith(contain, comparison))
                    return obj;
            }
        }

        return null;
    }

    public static GameObject FindObject(string name, bool includeInactive = true)
    {
        Scene curScene = SceneManager.GetActiveScene();
        GameObject result = null;
        if (curScene.isLoaded)
        {
            List<GameObject> allRootGoList = curScene.GetRootGameObjects().ToList();
            allRootGoList.ForEach(g => {

                if (g.name.Equals(name))
                {
                    result = g;
                    return;
                }

                Transform[] transArr = g.GetComponentsInChildren<Transform>(includeInactive);
                if (transArr != null)
                {
                    for (int k = 0; k < transArr.Length; k++)
                    {
                        GameObject go = transArr[k].gameObject;
                        if (go.name.Equals(name))
                        {
                            result = go;
                            break;
                        }
                    }
                }
            });
        }

        return result;
    }


    public static GameObject FindObject(GameObject go, string name)
    {
        if (!go) return null;
        Transform goTrans = go.transform;

        for (var i = 0; i < goTrans.childCount; i++)
        {
            Transform childTrans = goTrans.GetChild(i);
            var child = childTrans.gameObject;
            String childName = child.name;

            if (childName.Equals(name))
                return child;

            var tmp = FindObject(child, name);

            if (tmp)
                return tmp;
        }

        return null;
    }
    

    public static GameObject FindObjectStartWith(GameObject go, string name)
    {
        if (!go)
            return null;

        Transform goTrans = go.transform;

        for (int i = 0; i < go.transform.childCount; i++)
        {
            Transform childTrans = goTrans.GetChild(i);
            var child = childTrans.gameObject;
            string childName = child.name;
            if (childName.StartsWith(name))
                return child;

            var tmp = FindObject(child, name);
            if (tmp)
                return tmp;
        }

        return null;
    }
    
    public static List<GameObject> FindObjectsStartWith(GameObject go, string name)
    {
        if (!go)
            return null;

        List<GameObject> result = new List<GameObject>();
        Transform goTrans = go.transform;
        for (var i = 0; i < go.transform.childCount; i++)
        {
            Transform childTrans = goTrans.GetChild(i);
            var child = childTrans.gameObject;
            string childName = child.name;
            if (childName.StartsWith(name))
                result.Add(child);

            var tmp = FindObject(child, name);
            if (tmp)
                result.Add(child);
        }

        return result;
    }
    
    public static List<GameObject> FindObjectsWithExactName(GameObject go, string exactName, bool ignoreCase = false)
    {
        if (!go)
            return null;
    
        List<GameObject> result = new List<GameObject>();
        Transform goTrans = go.transform;
        exactName = ignoreCase ? exactName.ToLower() : exactName;
        for (var i = 0; i < go.transform.childCount; i++)
        {
            Transform childTrans = goTrans.GetChild(i);
            var child = childTrans.gameObject;
            string childName = ignoreCase ? child.name.ToLower() : child.name;
            if (childName.Equals(exactName))
                result.Add(child);
    
            var tmp = FindObject(child, exactName);
            if (tmp)
                result.Add(child);
        }
    
        return result;
    }

    public static GameObject[] FindAllGameObjectsInScenes()
    {
        List<GameObject> results = new List<GameObject>();
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene curScene = SceneManager.GetSceneAt(i);
            if (curScene.isLoaded)
            {
                List<GameObject> allRootGoList = curScene.GetRootGameObjects().ToList();
                allRootGoList.ForEach(g => {
                    Transform[] transArr = g.GetComponentsInChildren<Transform>(true);
                    if (transArr != null)
                    {
                        for (int k = 0; k < transArr.Length; k++)
                        {
                            GameObject go = transArr[k].gameObject;
                            results.Add(go);
                        }
                    }
                });
            }
        }

        return results.ToArray();
    }

    public static GameObject[] FindAllGameObjectsInScene(string sceneName)
    {
        List<GameObject> results = new List<GameObject>();
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene curScene = SceneManager.GetSceneAt(i);
            if (curScene.name.Equals(sceneName) && curScene.isLoaded)
            {
                List<GameObject> allRootGoList = curScene.GetRootGameObjects().ToList();
                allRootGoList.ForEach(g => {
                    Transform[] transArr = g.GetComponentsInChildren<Transform>(true);
                    if (transArr != null)
                    {
                        for (int k = 0; k < transArr.Length; k++)
                        {
                            GameObject go = transArr[k].gameObject;
                            results.Add(go);
                        }
                    }
                });
               break;
            }
        }

        return results.ToArray();
    }
    
    public static GameObject FindRootGameObjectInScenes(string goName)
    {
        GameObject result = null;
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene curScene = SceneManager.GetSceneAt(i);
            if (!curScene.isLoaded) 
                continue;
            
            List<GameObject> allRootGoList = curScene.GetRootGameObjects().ToList();
            for (var k = 0; k < allRootGoList.Count; k++)
                if (allRootGoList[k].name.Equals(goName))
                {
                    result = allRootGoList[k];
                    return result;
                }
        }

        return null;
    }

    public static GameObject[] FindAllGameObjectsInScenes(string exactName, string containName)
    {
        List<GameObject> results = new List<GameObject>();
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene curScene = SceneManager.GetSceneAt(i);
            if (curScene.isLoaded)
            {
                List<GameObject> allRootGoList = curScene.GetRootGameObjects().ToList();
                allRootGoList.ForEach(g => {
                    Transform[] transArr = g.GetComponentsInChildren<Transform>(true);
                    if (transArr != null)
                    {
                        for (int k = 0; k < transArr.Length; k++)
                        {
                            GameObject go = transArr[k].gameObject;
                            if (!string.IsNullOrEmpty(exactName) && go.name.Equals(exactName))
                                results.Add(go);

                            if (!string.IsNullOrEmpty(containName) && go.name.Contains(containName))
                                results.Add(go);
                        }
                    }
                });
            }
        }

        return results.ToArray();
    } 
    
    public static T[] FindAllComponentsInScenes<T>(bool includeInactive = true)
    { 
        List<T> results = new List<T>();
        Scene curScene = SceneManager.GetActiveScene();
        if (curScene.isLoaded)
        {
            List<GameObject> allRootGoList = curScene.GetRootGameObjects().ToList();
            allRootGoList.ForEach(g => {
                T[] componentArr = g.GetComponentsInChildren<T>(includeInactive);
                if (componentArr != null)
                {
                    for (var k = 0; k < componentArr.Length; k++)
                        results.Add(componentArr[k]);
                }
            });
        }
      
        return results.ToArray();
    }

    public static Vector4 GetWorldBoundaryWith2DBoxCollider(BoxCollider2D col2D)
    {
        if (!col2D)
            return Vector4.zero;
        
        Vector4 worldBoundary = Vector4.zero;
        Bounds colBounds = col2D.bounds;
        Vector3 center = colBounds.center;
        Vector3 extent = colBounds.extents;
        worldBoundary.x = center.x - extent.x;
        worldBoundary.y = center.x + extent.x;
        worldBoundary.z = center.y - extent.y;
        worldBoundary.w = center.y + extent.y;

        return worldBoundary;
    }


    private static Transform FindWithHierarchyPath_Recursive(Transform root, string hierarchyPath, bool keepFindingFirstMatch = true)
    {
        if (string.IsNullOrEmpty(hierarchyPath)) return root;
        var names = hierarchyPath.Split('/');
        if (names.Length == 0) return root;

        var name = names[0];
        var subPath = string.Join("/", names[1..]);

        GameObject foundObjectWithName = null;
        for (var i = 0; i < root.childCount; i++)
        {
            Transform childTrans = root.GetChild(i);
            if (childTrans.name.Equals(name))
            {
                foundObjectWithName = childTrans.gameObject;
                break;
            }
        }

        if (foundObjectWithName == null)
        {
            if (keepFindingFirstMatch) foundObjectWithName = FindObject(root.gameObject, name);
            if (foundObjectWithName == null) return null;
        }

        keepFindingFirstMatch = false;
        return FindWithHierarchyPath_Recursive(foundObjectWithName.transform, subPath, keepFindingFirstMatch);
    }

    /// <summary>
    /// name: can be object's name or object's hierachy path
    /// root: where searcher start from
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="root"></param>
    /// <param name="name"></param>
    /// <param name="includeInactive"></param>
    /// <returns></returns>
    public static T FindObject<T>(Transform root, string name, bool includeInactive = true) where T: UnityEngine.Object
    {
        if (root == null || string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            return null;

        Transform tf = root.Find(name);
        if (tf == null)
        {
            if (includeInactive) tf = FindWithHierarchyPath_Recursive(root, name);
        }

        if (tf == null) 
        {
            var go = FindObject(root.gameObject, name);
            tf = go ? go.transform : null;
        }
        if (tf == null) return null;

        if (typeof(GameObject).IsAssignableFrom(typeof(T)))
            return tf.gameObject as T;

        if (tf.gameObject.TryGetComponent<T>(out var t))
            return t;
        return null;
    }

    public static Transform[] FindAllChildTransform(GameObject go, bool includeInactive = true)
    {
        if (!go) return null;
        var result = new List<Transform>();
        var foundChildTransforms = go.GetComponentsInChildren<Transform>(includeInactive);
        
        if (foundChildTransforms == null) return null;
        for (var i = 0; i < foundChildTransforms.Length; i++)
        {
            if (foundChildTransforms[i] == null) continue;
            result.Add(foundChildTransforms[i]);
        }

        return result.ToArray();
    }

    #endregion
}
