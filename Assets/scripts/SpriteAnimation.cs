using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation
{
    private float animationTime;
    private int animationStage;
    private int maxAnimationStage;
    
    public Sprite[] rightAnimation;
    public Sprite[] leftAnimation;
    public float[] animationDurations;
    
    private Sprite[] currentAnimationPtr;

    public SpriteAnimation(Sprite[] animationRight, Sprite[] animationLeft, float[] durations)
    {
        this.rightAnimation = animationRight;
        this.leftAnimation = animationLeft;
        currentAnimationPtr = rightAnimation;

        this.animationDurations = durations;

        animationStage = 0;
        maxAnimationStage = animationRight.Length;
        animationTime = 0;
    }

    public void turn()
    {
        if (currentAnimationPtr == rightAnimation)
        {
            currentAnimationPtr = leftAnimation;
        }
        else
        {
            currentAnimationPtr = rightAnimation;
        }
    }

    public void turnRight()
    {
        currentAnimationPtr = rightAnimation;
    }

    public void turnLeft()
    {
        currentAnimationPtr = leftAnimation;
    }

    /**
     * Progresses the animation and returns the sprite
     * that the aniimation is currently on
     */
    public Sprite progressAnimation()
    {
        animationTime += Time.deltaTime;
        if (animationTime > animationDurations[animationStage])
        {
            animationStage++;
            animationTime = 0;
        }

        if (animationStage == maxAnimationStage)
        {
            animationStage = 0;
            animationTime = 0;
        }

        return currentAnimationPtr[animationStage];
    }
}