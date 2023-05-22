using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public CapsuleCollider2D collider { get; private set; }
    public AnimationScript animation { get; private set;}
    public Sprite smallSprite;
    public Sprite bigSprite;
    public SpriteRenderer sprite { get; private set; }
    public bool big;

    private void Awake()
    {
        animation = GetComponent<AnimationScript>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        big = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Shroom"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(PowerUpRoutine());
            big = true;
            collider.size = new Vector2(0.75f, 2f);
            collider.offset = new Vector2(0f, 0.5f);
        }
    }
    private IEnumerator PowerUpRoutine()
    {
        float duration = 1f,elapsed = 0f;
        animation.enabled = false;
        Time.timeScale = 0.0f;
        while (elapsed <= duration)
        {
            if ((elapsed / 0.125f) % 2 == 0) sprite.sprite = smallSprite;
            else sprite.sprite = bigSprite;
            elapsed += 0.125f;
            if(elapsed != duration)
                yield return new WaitForSecondsRealtime(0.125f);
        }
        Time.timeScale = 1.0f;
        animation.enabled = true;
    }

}
