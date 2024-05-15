using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet;
using UnityEngine;
using FishNet.Connection;

public class PlayerController : NetworkBehaviour
{
    //Instacia prefab balas
    public Transform bulletPrefab;
    public Transform bulletPoint;

    //Movimento player
   public float velocityMove = 5f;
   public float velocityRotation = 5f;
   public float jumpForce = 5f;
    public float directionX; 
    public float directionZ;
    public float directionY;

    //limites do player
    float rotatioX = 0;
    float limitVisonY = 45f;
    float gravity = 9.8f;

    //Tiro do Jogador 
    public float frequenciaTiro = 0.5f;
    bool canShoot = true;

   CharacterController cc;
   Vector3 directionMove = Vector3.zero;

   void Start()
   {
    //Cursor.lockState = CursorLockMode.Locked;
    //Cursor.visible = false;
    
   }
   override public void OnStartClient()
   {
    base.OnStartClient();
    if(base.IsOwner == false)
    {
        return;
    }
    cc = GetComponent<CharacterController>();
    transform.Find("Main Camera").gameObject.SetActive(true);
   }

   void Update()
{
    if(base.IsOwner == false)
    {
        return;
    }
    //Obtém os inputs direcionais com base no inputManager da Unity
     directionX = Input.GetAxis("Horizontal");
     directionZ = Input.GetAxis("Vertical");
     directionY = directionMove.y;

    if(Input.GetAxisRaw("Fire1") == 1 && canShoot == true)
    {
        canShoot = false;
        Server_Atirar(base.Owner,transform.Find("Main Camera").transform.forward);  
    }
     //orientação do jogador em primeira pessoa
    Vector3 front = transform.TransformDirection(Vector3.forward);
    Vector3 right = transform.TransformDirection(Vector3.right);
    
    //Movimento do Jogador
    directionMove = (front * directionZ) + (right * directionX);
    directionMove *= velocityMove;

    //Pular
    if(Input.GetAxisRaw("Jump") == 1 && cc.isGrounded)
    {
        directionMove.y = jumpForce;
    }
    else
    {
        directionMove.y = directionY;
    }
    //Adiciona a Gravidade
    directionMove.y -= gravity *Time.deltaTime;
    //Move o Jogador
    cc.Move(directionMove * Time.deltaTime);

    //Rotação do jogador 
    rotatioX -= Input.GetAxis("Mouse Y") * velocityRotation;
    rotatioX = Mathf.Clamp(rotatioX, -limitVisonY,limitVisonY);
    //Rotaciona a Camera para o ponto do mouse
    Camera.main.transform.localRotation = Quaternion.Euler(rotatioX, 0 ,0);
    transform.rotation *= Quaternion.Euler(0 , Input.GetAxis("Mouse X") * velocityRotation,0);
     

   }

   //chamada pelo update ao clicar com botão direito
   [ServerRpc]
   void Server_Atirar(NetworkConnection coon , Vector3 direcao)
   {
    Transform instacia = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.Euler(Camera.main.transform.forward));
    instacia.GetComponent<BulletScript>().direction = direcao;

    base.Spawn(instacia.gameObject);

    StartCoroutine(ResetBullet());
    IEnumerator ResetBullet()
    {
        yield return new WaitForSeconds(frequenciaTiro);
        Target_ResetBullet(coon);
    }
   }
   [TargetRpc]
   void Target_ResetBullet(NetworkConnection coon)
   {
    canShoot = true;
   }
 
}
