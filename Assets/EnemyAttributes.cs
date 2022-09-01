using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttributes : MonoBehaviour
{
    [Header("Objs")]
    public Slider lifeBar;
    public Animator anim;
    AnimationClip[] animationClip;

    [Header("Atributos")]
    int life;
    public int maxLife = 300;
    public int dmg;

    public bool die;
    public float delayDie;

    private void OnEnable()
    {
        life = maxLife;
        lifeBar.maxValue = life;
        lifeBar.value = life;

        animationClip = anim.runtimeAnimatorController.animationClips;

        for(int i = 0; i < animationClip.Length; i++)
        {
            if (animationClip[i].name == "Die") delayDie = animationClip[i].length;
            else if (animationClip[i].name == "Stab Attack") GetComponent<EnemyBehavior>().atkDelay = animationClip[i].length;
            //Debug.Log($"Animation: {animationClip[i]} ----- TimeDuration: {animationClip[i].length} ");
        }
    }

    public void TakeDMG(int dmg)
    {
        if(life > 0 && !die)
        {
            life -= dmg; 
            lifeBar.value = life;
        }
        
        if (life <= 0 && !die)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            die = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        AnimSetBool("isDead", true);

        yield return new WaitForSeconds(delayDie);
        anim.enabled = false;
        Destroy(this.gameObject, 4);
    }
    public void AnimSetBool(string name, bool status)
    {
        anim.SetBool(name, status);
    }
}
