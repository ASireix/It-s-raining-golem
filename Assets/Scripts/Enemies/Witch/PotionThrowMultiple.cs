using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Multiple Throw Random", menuName = "Witch/Potion Throw/Multiple Random")]
public class PotionThrowMultiple : PotionThrow
{
    public override void Throw(Vector2 launchPosition, Vector2[] targetPositions)
    {
        for (int i = 0;  i < targetPositions.Length; i++)
        {
            Rigidbody2D potionRb = Instantiate(Spawner.GetWeightedPotion(potionBelt.potions), launchPosition, Quaternion.identity).
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
