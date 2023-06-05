using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public PlayerMovement movement { get; private set; }
    public CapsuleCollider2D collider { get; private set; }
    public AnimationScript animation { get; private set; }
    public SpriteRenderer sprite { get; private set; }

    public Sprite smallSprite;
    public Sprite bigSprite;

    public bool big { get; private set; }
    public bool dead { get; private set; }
    public bool immune { get; private set; }
    public bool star => gameObject.tag == "StarPlayer";


    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        collider = GetComponent<CapsuleCollider2D>();
        animation = GetComponent<AnimationScript>();
        sprite = GetComponent<SpriteRenderer>();
        SetDead(false);
        SetBig(false);
    }
    private void OnEnable()
    {
        SetDead(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Eat Shroom
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collectable"))
        {
            collision.gameObject.SetActive(false);
            if (collision.gameObject.tag == "PowShroom" )
            {
                if (!big)
                {
                    StartCoroutine(PowerUpCoroutine());
                }
            } 
            else if (collision.gameObject.tag == "OneUpShroom")
            {
                GameManager.Instance.AddLive();
            }
            else if (collision.gameObject.tag == "Star")
            {
                StarOn(8f);
            }
        } 
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("SpinningShell"))
        {
            if (!immune && !star)
            {                
                if (transform.DotTest(collision.transform, Vector2.down, 65f))
                {
                    //Give Koopa the direction that Mario has trampled to make a spin in the opposite direction.
                    float trampledDirection = transform.position.x - collision.transform.position.x >= 0 ? 1 : -1;
                    collision.gameObject.GetComponent<Enemy>().Trampled(trampledDirection);
                }
                else
                {
                    if (big)
                    {
                        ImmuneOn(2f);
                    }
                    else
                    {
                        SetDead(true);
                        movement.FallOut();
                        GameManager.Instance.ResetLevelWithDelay(3f);
                    }
                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        //Function to handle hitting the mystery block
        if (collision.gameObject.tag != "EmptyBlock" && movement.jumping)
        {
            if (transform.DotTest(collision.transform, Vector2.up, big ? 25f : 45f))
            {
                if (collision.gameObject.TryGetComponent<Block>(out Block block))
                {
                    if (big)
                    {
                        block.Hit();
                    }
                    else
                    {
                        block.SmallHit();
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "DeathBarrier")
            GameManager.Instance.ResetLevelWithDelay(3f);
        if (collision.gameObject.name == "FlagPole")
        {
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            PullFlagDown(Find(collision.GetComponentsInChildren<Transform>(),"Flag"),Find(collision.GetComponentsInChildren<Transform>(),"Base"));
        }
        if (collision.gameObject.name == "Castle")
        {
            GameManager.Instance.NextLevel();
        }
            


    }
    //Find the children of FlagPole
    private Transform Find(Transform[] gameObjects , string nameObject)
    {
        foreach (Transform gameObject in gameObjects)
        {
            if (gameObject.name == nameObject)
                return gameObject;
        }
        return null;
    }

    private void PullFlagDown(Transform flag, Transform basePosition)
    {
        StartCoroutine(PullFlagCoroutine(flag, basePosition));
    }

    private IEnumerator PullFlagCoroutine(Transform flag, Transform basePosition)
    {
        float durationPerUnit = 0.15f, elapsed = 0f, duration;
        Time.timeScale = 0;
        Vector3 currentPosition = new Vector3(flag.position.x, transform.position.y, transform.position.z);
        duration = durationPerUnit * Mathf.Abs(basePosition.position.y - currentPosition.y);
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, basePosition.position, elapsed / duration);
            newPosition.z = basePosition.position.z;
            this.transform.position = newPosition;
            flag.position = newPosition;
            elapsed += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = 1f;
    }

    private void StarOn(float duration)
    {
        gameObject.tag = "StarPlayer";
        Star(duration);
    }

    private void Star(float duration)
    {
        StartCoroutine(StarCoroutine(duration));
    }

    private IEnumerator StarCoroutine(float duration)
    {
        float elapsed = 0f;
        int index =0;
        Color pink = new Color(1f, 0.5f, 0.67f, 1f),
            green = new Color(0.5f, 1f, 0.5f, 1f),
            blue = new Color(0.33f, 1f, 1f, 1f);
        while (elapsed <= duration)
        {
            index = Convert.ToInt32(elapsed *10)% 3;
            sprite.color = index == 0 ? pink : index == 1 ? green : blue;
            elapsed += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        StarOff();
    }

    private void StarOff()
    {
        gameObject.tag = "Player";
        sprite.color = Color.white;
    }
    private void ImmuneOn(float duration)
    {
        SetImmune(true);
        SetBig(false);
        collider.size = new Vector2(0.75f, 1f);
        collider.offset = new Vector2(0f, 0f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("SpinningShell"), true);
        Flash(duration);
    }
    private void ImmuneOff()
    {
        SetImmune(false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("SpinningShell"), false);
    }

    private void Flash(float duration)
    {
        StartCoroutine(FlashCoroutine(duration));
    }

    private IEnumerator FlashCoroutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed <= duration)
        {
            if ((elapsed / 0.125f) % 2 == 0) sprite.enabled = true;
            else sprite.enabled = false;
            elapsed += 0.125f;
            yield return new WaitForSeconds(0.125f);
        }
        ImmuneOff();
    }
    private IEnumerator PowerUpCoroutine()
    {
        float duration = 1f, elapsed = 0f;
        SetBig(true);
        collider.size = new Vector2(0.75f, 2f);
        collider.offset = new Vector2(0f, 0.5f);
        animation.enabled = false;
        Time.timeScale = 0.0f;
        while (elapsed <= duration)
        {
            if ((elapsed / 0.125f) % 2 == 0) sprite.sprite = smallSprite;
            else sprite.sprite = bigSprite;
            elapsed += 0.125f;
            if (elapsed != duration)
                yield return new WaitForSecondsRealtime(0.125f);
        }
        Time.timeScale = 1.0f;
        animation.enabled = true;
    }
    private void SetDead(bool isDead)
    {
        dead = isDead;
        movement.SetDead(isDead);
    }
    private void SetBig(bool isBig)
    {
        big = isBig;
        movement.SetBig(isBig);
    }
    private void SetImmune(bool isImmune)
    {
        immune = isImmune;
        movement.SetImmune(isImmune);
    }
}
