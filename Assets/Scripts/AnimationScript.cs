using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public SpriteRenderer sprite { get; private set; }
    [SerializeField] public Animations[] animations;
    private Animations currentAnimation;
    private float timer;
    private int index;
    public string initialAnimationName;
    private string animationName;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        index = 0;
        animationName = initialAnimationName;
        currentAnimation = new Animations(GetAnimByName(initialAnimationName));
    }

    private void Update()
    {
        if (animationName != currentAnimation.name)
        {
            currentAnimation = GetAnimByName(animationName);
            index = 0;
        }
        StartAnimation();
    }
   
    public void StartAnimation()
    {
        
        if (!currentAnimation.isSingle)
        {
            //Update animation follow the timePerAnimate each movement.
            //If timer reach the time per animate it will call the function update the sprite.
            timer += Time.deltaTime;
            if (timer > currentAnimation.timePerAnimate)
            {
                //Update the spriterederer using the current sprite in the chain.
                if (index < currentAnimation.animationSprites.Length)
                {
                    sprite.sprite = currentAnimation.animationSprites[index];
                    timer = 0;
                    index++;
                }
                //If run out of the sprites in the chain index will be set as 0 to loop the movement.
                else if (currentAnimation.isLoop)
                {
                    index = 0;
                }
            }
        }
        //It's for movement just have 1 sprite such as idle... 
        else
        {
            sprite.sprite = currentAnimation.animationSprites[0];
        }
        
    }
    private Animations GetAnimByName(string name)
    {
        for(int i=0; i< animations.Length; i++)
        {
            if (animations[i].name == name) 
                return animations[i];
        }
        return null;
    }
    public void ChangeAnimation(string name)
    {
        animationName = name;
    }

    [System.Serializable]
    public class Animations
    {
        public string name;
        public Sprite[] animationSprites;
        [SerializeField] public bool isLoop;
        [SerializeField] public bool isSingle;
        [SerializeField] public float timePerAnimate = 0f;
        public Animations(Animations animations)
        {
            this.name = animations.name;
            this.animationSprites = animations.animationSprites;
            this.isLoop = animations.isLoop;
            this.timePerAnimate = animations.timePerAnimate;
        }
    }
}
