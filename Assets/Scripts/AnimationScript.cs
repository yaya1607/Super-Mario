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
    [System.Serializable]
    public class Animations
    {
        public string name;
        public Sprite[] animationSprites;
        [SerializeField] public bool isLoop;
        [SerializeField] public float timePerAnimate = 0f;
        public Animations(Animations animations)
        {
            this.name = animations.name;
            this.animationSprites = animations.animationSprites;
            this.isLoop = animations.isLoop;
            this.timePerAnimate = animations.timePerAnimate;
        }
    }
    public void StartAnimation()
    {
        
        if (currentAnimation.isLoop)
        {
            timer += Time.deltaTime;
            if (timer > currentAnimation.timePerAnimate)
            {
                if (index < currentAnimation.animationSprites.Length)
                {
                    sprite.sprite = currentAnimation.animationSprites[index];
                    timer = 0;
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
        }
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
}
