using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttributes : MonoBehaviour
{
    [Header("Anim")]
    [SerializeField] Animator anim;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Material[] skin;

    [Header("Colliders")]
    public BoxCollider boxCollider;

    [Header("Atributos")]
    public int life = 300;
    public bool isDead = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        skinnedMeshRenderer.material = skin[Random.Range(0, 3)];
    }

    public void TakeDMG(int dmg)
    {
        if(!isDead && life > 0)
        {
            life -= dmg;
        }
        else if (!isDead && life <= 0)
        {
            isDead = true;
            anim.SetBool("isDead", true);
        }
    }



    #region Sistema de morte

    //Metodo Die é acionado atraves do Event na animação de morte
    public void Dead()
    {
       anim.enabled = false;
       Invoke("DestroyDeadEnemys", 2f);
    }

    //Inimigo desaparece depois de afundar no chao
    void DestroyDeadEnemys(){
        boxCollider.enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(this.gameObject, 1);
    }
    #endregion
}
