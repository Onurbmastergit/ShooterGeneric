using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   public GameObject targetPlayer;
   public float velocityMove;
   private CharacterController cc;
    void Start()
    {
        InvokeRepeating("SelectionTarget",0, 1 );
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(targetPlayer == null)
        {
            return;
        }
        Vector3 moviment = ( targetPlayer.transform.position - transform.position).normalized;
        moviment *= velocityMove;
        cc.Move(moviment * Time.deltaTime);
    }
   void SelectionTarget()
   {
     GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
      if(targets == null)
    {
        targetPlayer = null;
        return;
    }
     targetPlayer = targets[0];
   
     foreach( GameObject target in targets)
     {
        //O jOGADOR QUE O INIMIGO ESTA SEGUINDO ESTÁ MAIS LONGE DO QUE O ALVO DO LOOP
        if( Vector3.Distance(transform.position, targetPlayer.transform.position)       
            >           
            Vector3.Distance(transform.position, target.transform.position)
          )
        {
            //SE ESTIVER TROCA O JOGADOR QUE O INIMIGO ESTÁ SEGUINDO PELO NOVO ALVO
            targetPlayer = target;

        }
     }
   }
}
