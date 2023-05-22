using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBlock : MonoBehaviour
{

    public enum Types
    {
        PowShroom,
        Coin,
        Empty
    }
    public SpriteRenderer sprite { get; private set; }
    public Sprite emptyBlock;
    public GameObject PowShroom;
    public Types selectedOption;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void SmallHit() { }
    public void Hit()
    {
        if(selectedOption == Types.PowShroom)
        {
            StartCoroutine(Bounce());
            ShroomSpawn();
            ChangeToEmptyBlock();
        }
    }

    private IEnumerator Bounce()
    {

        Vector3 bouncedPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.25f,this.transform.position.z),
            originalPosition = this.transform.position;
        
        float duration = 0.1f;
        float elapsed = 0;
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(originalPosition, bouncedPosition, elapsed / duration);
            newPosition.z = originalPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(bouncedPosition, originalPosition, elapsed / duration);
            newPosition.z = originalPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void ChangeToEmptyBlock()
    {
        this.gameObject.tag = "EmptyBlock";
        this.sprite.sprite = emptyBlock;
    }

    private void ShroomSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        Instantiate(PowShroom, spawnPosition, Quaternion.identity);
    }
}
