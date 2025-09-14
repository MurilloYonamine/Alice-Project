using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour {
    public static EnemyPoolManager Instance { get; private set; }

    public GameObject pooledObjectsContainer;
    public GameObject objectToPool;
    [HideInInspector] public List<GameObject> pooledObjects;

    public int amountToPool;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Novo método chamado pelo GameManager
    public void Initialize() {
        pooledObjects = new List<GameObject>();
        GameObject tilePool = new GameObject("Enemy Pool");
        tilePool.transform.SetParent(pooledObjectsContainer.transform);

        int parentCount = Mathf.CeilToInt(amountToPool / 10f);
        GameObject[] subContainers = new GameObject[parentCount];
        for (int i = 0; i < parentCount; i++) {
            subContainers[i] = new GameObject($"Enemy Pool {i * 10}-{(i + 1) * 10 - 1}");
            subContainers[i].transform.SetParent(tilePool.transform);
        }

        for (int i = 0; i < amountToPool; i++) {
            GameObject temp = Instantiate(objectToPool);
            temp.SetActive(false);
            pooledObjects.Add(temp);
            int parentIndex = i / 10;
            temp.transform.SetParent(subContainers[parentIndex].transform);
        }
    }

    public GameObject GetPooledObject() {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy) return pooledObjects[i];
        }
        return null;
    }
    public GameObject ReturnToPool(GameObject obj) {
        obj.transform.SetParent(pooledObjectsContainer.transform);
        obj.SetActive(false);
        return obj;
    }
}
