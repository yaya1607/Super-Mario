using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBlock : Block
{

    public enum Types
    {
        PowShroom,
        Coin,
        Empty
    }
    public SpriteRenderer sprite { get; private set; }
    public Sprite emptyBlock;
    public GameObject coin;
    public GameObject PowShroom;
    public Types selectedOption;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void SmallHit()
    {
        base.SmallHit();
        if (selectedOption == Types.PowShroom)
        {
            ShroomSpawn();
        } 
        else if (selectedOption == Types.Coin) 
        {
            CoinSpawn();
        }
        ChangeToEmptyBlock();
    }
    public override void Hit()
    {
        //Big or small Mario is able to hit the mystery block.
        base.Hit();
        SmallHit();
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
    private void CoinSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        Instantiate(coin, spawnPosition, Quaternion.identity);
    }

    
}
