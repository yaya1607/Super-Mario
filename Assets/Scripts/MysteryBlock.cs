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
        OneUpShroom,
        Star,
        Empty
    }
    public SpriteRenderer sprite { get; private set; }
    public Sprite emptyBlock;
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
            PowShroomSpawn();
        } 
        else if (selectedOption == Types.Coin) 
        {
            CoinSpawn();
        }
        else if(selectedOption == Types.OneUpShroom)
        {
            OneUpShroomSpawn();
        }
        else if (selectedOption == Types.Star)
        {
            StarSpawn();
        }

        ChangeToEmptyBlock();
    }

    private void StarSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GameObject star = GameManager.Instance.GetStar(spawnPosition);
        star.SetActive(true);
    }

    private void OneUpShroomSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GameObject oneUpShroom = GameManager.Instance.GetOneUpShroom(spawnPosition);
        oneUpShroom.SetActive(true);
    }

    public override void Hit()
    {
        //Big and small Mario are able to hit the mystery block.
        base.Hit();
        SmallHit();
    }
    private void ChangeToEmptyBlock()
    {
        this.gameObject.tag = "EmptyBlock";
        this.sprite.sprite = emptyBlock;
    }

    private void PowShroomSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GameObject powShroom = GameManager.Instance.GetPowShroom(spawnPosition);
        powShroom.SetActive(true);
    }
    private void CoinSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GameObject coin = GameManager.Instance.GetCoin(spawnPosition);
        coin.SetActive(true);
    }

    
}
