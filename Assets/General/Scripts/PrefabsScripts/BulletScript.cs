using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
   public float velocityBullet = 5f;
   public float timeLife = 2f;
   public Vector3 direction;

   public override void OnStartServer()
   {
      base.OnStartServer();

     Invoke("DestroyBullet", timeLife);
      GetComponent<Rigidbody>().velocity = direction * velocityBullet ;
   }

   [ServerRpc]
   void DestroyBullet()
   {
      base.Despawn(gameObject);
   }
   void OnTriggerEnter( Collider collider)
   {
      if(collider.CompareTag("Player"))
      {
        collider.GetComponent<PlayerStatus>().TakeDamage(collider.GetComponent<PlayerStatus>().Owner,2); 
      }
      DestroyBullet();
   }

}
