using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemThrow : MonoBehaviour
{
    [SerializeField] float dashDelay = 0.8f;
    [SerializeField] float dashForce = 300f;
    [SerializeField] float triggerDelayMin = 0f;
    [SerializeField] float triggerDelayMax = 2f;
    void Start()
    {
        StartCoroutine(StartAttack(Random.Range(triggerDelayMax, triggerDelayMin)));
        Destroy(gameObject, 10f);
    }

    IEnumerator StartAttack(float delay)
    {
        Vector2 dir = GameManager.instance.player.transform.position - transform.position;
        if (dir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            dir = new Vector2(-1f, 0f);
        }
        else if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            dir = new Vector2(1f, 0f);
        }
        yield return new WaitForSeconds(delay);

        GetComponent<Animator>().SetTrigger("Attack");

        yield return new WaitForSeconds(dashDelay);

        GetComponent<Rigidbody2D>().AddForce(dir * dashForce * 10f);
    }
}
