// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;

// public class ObjectPoolManager : Singleton<ObjectPoolManager> {
//     public List<PoolObjectInfo> ObjectInfos = new List<PoolObjectInfo>();
//     public List<GameObject> ObjectActiveInfos = new List<GameObject>();
//     public GameObject SpawnObject(GameObject objectToSpwan, Vector3 spawPosition, Quaternion spawQuaternion, Transform parent){
//         PoolObjectInfo pool = ObjectInfos.Find(p => p.LookupString == objectToSpwan.name);
//         if(pool == null){
//             pool = new PoolObjectInfo(){
//                 LookupString = objectToSpwan.name
//             };
//         }
//         ObjectInfos.Add(pool);
//         GameObject spawableObj;
//         if(pool.InactiveObjects.Count < 1){
//             spawableObj = Instantiate(objectToSpwan, spawPosition, spawQuaternion, parent);
//         }
//         else{
//             spawableObj = pool.InactiveObjects.Dequeue();
//             spawableObj.transform.position = spawPosition;
//             spawableObj.transform.rotation = spawQuaternion;
//             spawableObj.transform.parent = parent;
//             spawableObj.SetActive(true);
//         }
//         return spawableObj;
//     }

//     public GameObject SpawnObject(GameObject objectToSpwan, Vector3 spawPosition, Quaternion spawQuaternion){
//         GameObject gObject = SpawnObject(objectToSpwan, spawPosition, spawQuaternion, transform);
//         ObjectActiveInfos.Add(gObject);
//         return gObject;
//     }

//     public void ReturnObjectToPool(GameObject obj, float time){
//         StartCoroutine(DestroyAfterDelay(obj, time));
//     }

//     private IEnumerator<WaitForSeconds> DestroyAfterDelay(GameObject obj, float delay) {
//         yield return new WaitForSeconds(delay);
//         ReturnObjectToPool(obj);
//     }
//     public void ClearObjectActiveInPool(){
//         foreach(var i in ObjectActiveInfos){
//             if(i.gameObject.activeSelf){
//                 ReturnObjectToPool(i);
//             }
//         }
//     }
//     public void ReturnObjectToPool(GameObject obj){
//         string goName = obj.name.Substring(0, obj.name.Length - 7);
//         PoolObjectInfo pool = ObjectInfos.Find(p => p.LookupString == goName);
//         if(pool == null){
//             Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
//         }
//         else{
//             obj.SetActive(false);
//             pool.InactiveObjects.Enqueue(obj);
//         }
//     }
// }
// public class PoolObjectInfo{
//     public string LookupString;
//     public Queue<GameObject> InactiveObjects = new Queue<GameObject>();
// }