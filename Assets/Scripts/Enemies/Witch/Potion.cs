using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    bool broken = false;
    [SerializeField] GameObject vaporPrefab;
    [SerializeField] SpriteRenderer[] potionContent;
    [SerializeField][ColorUsageAttribute(true, true)] Color hiddenContentColor;
    [SerializeField] float vaporDuration;
    [SerializeField] float revealDelay; // above 0, the potion starts white and become normal after x amount of time
    [SerializeField] AnimationCurve revealAnimationCurve;

    [Header("VFX")]
    [SerializeField] AnimationCurve flashAlpha;
    [SerializeField] AnimationCurve bottleAlpha;
    [SerializeField] float vaporReleaseDelay;
    [SerializeField] float flashSpeed;

    [Header("SFX")]
    [SerializeField] SoundEffectSO sfx_Pop;

    private void Start()
    {
        if (revealDelay > 0)
        {
            StartCoroutine(RevealPotion(revealDelay));
        }
    }

    void BreakPotion()
    {
        broken = true;
        GetComponent<Rigidbody2D>().simulated = false;
        StartCoroutine(FlashSprite());
        StartCoroutine(DelaySpawnVapor());
    }

    public void Init(float duration, float delay = 0f)
    {
        vaporDuration = duration;
        if (delay > 0)
        {
            revealDelay = delay;
            StartCoroutine(RevealPotion(delay));
        }
    }

    IEnumerator RevealPotion(float delay)
    {
        Dictionary<SpriteRenderer,Color> dicoColor = new Dictionary<SpriteRenderer,Color>();
        for (int i = 0; i < potionContent.Length; i++)
        {
            dicoColor.Add(potionContent[i], potionContent[i].color);
            potionContent[i].color = hiddenContentColor;
        }

        for (float t = 0f; t < 1f; t += Time.deltaTime / delay)
        {
            foreach (KeyValuePair<SpriteRenderer,Color> entry in dicoColor)
            {
                entry.Key.color = Color.Lerp(hiddenContentColor, entry.Value, revealAnimationCurve.Evaluate(t));
            }
            yield return new WaitForEndOfFrame();
        }
    }

    Dictionary<SpriteRenderer,Material> GetAllMaterials()
    {
        Dictionary<SpriteRenderer,Material> mat = new Dictionary<SpriteRenderer,Material>();
        foreach (var sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            mat.Add(sprite, sprite.material);
        }
        return mat;
    }

    IEnumerator DelaySpawnVapor()
    {
        yield return new WaitForSeconds(vaporReleaseDelay);

        SoundManager.instance.PlaySFX(sfx_Pop);

        Vapor temp = Instantiate(vaporPrefab).GetComponent<Vapor>();
        temp.transform.position = transform.position;
        temp.SetTimer(vaporDuration);
    }

    IEnumerator FlashSprite()
    {
        Dictionary<SpriteRenderer, Material> mat = GetAllMaterials();
        Color c;
        for (float i = 0f; i < 1f; i += Time.deltaTime / flashSpeed)
        {
            foreach(KeyValuePair<SpriteRenderer,Material> entry in mat)
            {
                c = entry.Key.color;
                c.a = bottleAlpha.Evaluate(i);
                entry.Value.SetFloat("_Flash",flashAlpha.Evaluate(i));
                entry.Key.color = c;
            }
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
        Destroy(gameObject,2f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!broken)
            BreakPotion();
    }
}
