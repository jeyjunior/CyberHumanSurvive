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
    private int life;
    private int power;
    public bool isDead = false;


    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        skinnedMeshRenderer.material = skin[Random.Range(0, 3)];

        GUIController gameController = GameObject.FindWithTag("GameController").GetComponent<GUIController>();

        switch (gameController.difficulty)
        {
            case 0:
                life = 100;
                power = 10;
                break;
            case 1:
                life = 200;
                power = 50;
                break;
            case 2:
                life = 300;
                power = 100;
                break;
        }
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
            GameObject.FindWithTag("GameController").GetComponent<GameController>().MaxEnemyKill();

        }
    }

    public int EnemyPower()
    {
        return power;
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

        Instantiate(GetComponent<EnemyBehavior>().vfx[1], transform.position, transform.rotation);

        boxCollider.enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(this.gameObject, 1);
    }
    #endregion
}
