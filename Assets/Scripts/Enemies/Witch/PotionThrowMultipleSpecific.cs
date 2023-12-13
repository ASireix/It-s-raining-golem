using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Multiple Throw Specific", menuName = "Witch/Potion Throw/Multiple Specific")]
public class PotionThrowMultipleSpecific : PotionThrow
{
    public PotionBelt poisons;
    public PotionBelt heals;

    public override void Throw(Vector2 launchPosition, Vector2[] targetPositions)
    {
        List<Potion> potionList = new List<Potion>();

        for (int i = 0; i < targetPositions.Length; i++)
        {
            potionList.Add(Spawner.GetWeightedPotion(poisons.potions));
        }

        int rndIndex = Random.Range(0, potionList.Count);

        potionList[rndIndex] = Spawner.GetWeightedPotion(heals.potions);

        for (int i = 0; i < targetPositions.Length; i++)
        {
            Rigidbody2D potionRb = Instantiate(potionList[i], launchPosition, Quaternion.identity).
            GetComponent<Rigidbody2D>();

            potionRb.velocity = CalculateVelocity(targetPositions[i], launchPosition, speed);
            potionRb.AddTorque(Random.Range(minTorque, maxTorque));
        }
    }

    Vector2 CalculateVelocity(Vector2 target, Vector2 origin, float time)
    {
        //define the distance x and y first
        Vector2 distance = target - origin;
        Vector2 distance_x_z = distance;
        distance_x_z.Normalize();
        distance_x_z.y = 0;

        //creating a float that represents our distance 
        float sy = distance.y;
        float sxz = distance.magnitude;

        //calculating initial x velocity
        //Vx = x / t
        float Vxz = sxz / time;

        ////calculating initial y velocity
        //Vy0 = y/t + 1/2 * g * t
        float Vy = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector2 result = distance_x_z * Vxz;
        result.y = Vy;

        return result;
    }
}
