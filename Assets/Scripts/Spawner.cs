using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Enemy SpawnEnemy(GameObject prefab, Transform location)
    {
        if (prefab)
        {
            GameObject temp = Instantiate(prefab, location);

            temp.transform.parent = null;

            Vector3 previousPosition = temp.transform.position;

            Vector3 newPosition = new Vector3(previousPosition.x, previousPosition.y, 0);

            temp.transform.position = newPosition; // To make the ennemies on the foreg

            return temp.GetComponent<Enemy>();
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetWeightedPrefab(List<EnemyStats> list)
    {
        float weigthSum = 0f;

        foreach (var item in list)
        {
            weigthSum += item.spawnWeigth;
        }

        //Debug.Log("Sum of weigth is " + weigthSum);

        float weigth = Random.Range(0, weigthSum);

        //Debug.Log("Weigth to look for is " + weigth);

        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);

            if (weigth < list[randomIndex].spawnWeigth)
            {
                //Debug.Log("Successully found an enemy to spawn named " + enemies[randomIndex].name);

                return list[randomIndex].prefab;

            }
            else
            {
                weigth -= list[randomIndex].spawnWeigth;

                //Debug.Log("Weigth is insuffisante to spawn this enemy called -" + enemies[randomIndex].name + "- reducing weigth to " + weigth);
            }
        }

        //Debug.Log("Did not found any enemy to spawn");

        return null;
    }

    public static Potion GetWeightedPotion(List<PotionWeighted> list)
    {
        float weigthSum = 0f;

        foreach (var item in list)
        {
            weigthSum += item.weight;
        }


        float weigth = Random.Range(0, weigthSum);


        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);

            if (weigth < list[randomIndex].weight)
            {

                return list[randomIndex].potion;

            }
            else
            {
                weigth -= list[randomIndex].weight;
            }
        }

        return list[0].potion; // return the first potion if the weighted spawn did not work
    }
}
