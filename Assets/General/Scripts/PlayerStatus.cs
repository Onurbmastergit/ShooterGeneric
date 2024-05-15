using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Connection;
using FishNet.Managing;

public class PlayerStatus : NetworkBehaviour
{
    public int vidaAtual;
    public int vidaTotal;

    Color32 corVermelho = new Color32(249, 6, 0, 255); // Red = F90600
    Color32 corVerde = new Color32(0, 249, 22, 255);   // Green = 00F916
    Color32 corAmarelo = new Color32(249, 192, 0, 255);// Yellow = F9C000
    void Start()
    {
        vidaAtual = vidaTotal;
        float fillAmount = (float)vidaAtual/vidaTotal; 
        ChangeColor();
        GameObject.FindWithTag("Life").GetComponent<Image>().fillAmount = fillAmount;
        GameObject.FindWithTag("GameOver").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("GameHud").GetComponent<Canvas>().enabled = true;
    }
    [TargetRpc]
     public void TakeDamage(NetworkConnection conn, int damage)
   {
    vidaAtual -= damage;
    float fillAmount = (float)vidaAtual/vidaTotal; 
    GameObject.FindWithTag("Life").GetComponent<Image>().fillAmount = fillAmount;
    ChangeColor();
    VerificarMorte();
   }
   void VerificarMorte()
   {
    if(vidaAtual <= 0)
    {
        GameObject.FindWithTag("GameOver").GetComponent<Canvas>().enabled = true;
        GameObject.FindWithTag("GameHud").GetComponent<Canvas>().enabled = false;
        transform.gameObject.GetComponent<PlayerController>().enabled = false;
        transform.gameObject.GetComponent<CharacterController>().enabled = false;
        transform.gameObject.AddComponent<Rigidbody>();
        transform.gameObject.GetComponent<Rigidbody>().AddForce(50,0,0, ForceMode.Force);
    }
   }
    public void ChangeColor()
    {
     if (vidaAtual <= vidaTotal * 0.2f) // Menos de 20% de vida (Vermelho)
        {
            HudController.instacia.lifebar.color = corVermelho;
        }
        else if (vidaAtual >= vidaTotal * 0.8f) // Mais de 80% de vida (Verde)
        {
            HudController.instacia.lifebar.color = corVerde;
        }
        else // Entre 20% e 80% de vida (Amarelo)
        {
            HudController.instacia.lifebar.color = corAmarelo;
        }
    }
 
}
