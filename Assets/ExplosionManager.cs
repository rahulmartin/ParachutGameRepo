using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab;
    List<GameObject> objects = new List<GameObject>();
    public void ClearExplosions()
    {
        foreach(GameObject item in objects)
        {
            if(item != null)
                GameObject.Destroy(item);
        }

        objects.Clear();
    }

    public void CreateGameOverEffect()
    {
        int range = Random.Range(5, 11);
        for(int i=0; i<range; i++)
        {
            CreateARandomExplosion();
        }
    }

    public void CreateExplosionAt(Vector3 position)
    {
        GameObject item = GameObject.Instantiate(explosionPrefab, position, Quaternion.identity);
        objects.Add(item);
        StartCoroutine(DelayAndCallback(item));
    }

    private IEnumerator DelayAndCallback(GameObject item)
    {
        yield return new WaitForSeconds(0.5f);
        if(objects.Contains(item))
            objects.Remove(item);
        GameObject.Destroy(item);
    }

    private void CreateARandomExplosion()
    {
        Vector3 position = Random.insideUnitCircle * 6;
        position.z = 0;
        float scale = Random.insideUnitCircle.x;
        Vector3 rotation = Random.insideUnitSphere * 2;
        GameObject item = GameObject.Instantiate(explosionPrefab, position, Quaternion.Euler(rotation));
        objects.Add(item);
    }

}
