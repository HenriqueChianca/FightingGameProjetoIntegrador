using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public bool playerUm;

    Rigidbody fisica;

    //ComabteParte

    public Transform pontoDeAtaque;

    public float alcanceDoAtaque = 1f;

    public LayerMask LayerDoOponente;

    public bool Particulas;

    public ParticleSystem ParticulaDeAtaque;
    //Vida Parte
    public int vidaMaxima = 100;

    public int vidaAtual;

    public BarraDeVidaCodigo barraVida;


    //Movimento Parte
    public float velocidade = 300;

    public float ForcaPulo = 700;

    public float puloDiagonal = 30;

    bool estaNoChao;

    public float pushback = 200;

    public Transform outroPlayer;

    public float distanciaParaGirar = 1f;

    // Start is called before the first frame update
    void Start()
    {
        fisica = GetComponent<Rigidbody>();

        vidaAtual = vidaMaxima;

        barraVida.SetarVidaMaxima(vidaMaxima);

        Particulas = GetComponent<ParticleSystem>();


    }

    // Update is called once per frame
    void Update()
    {
        //Girar
        if(outroPlayer.position.x < transform.position.x - distanciaParaGirar)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(outroPlayer.position.x > transform.position.x + distanciaParaGirar)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }



        //método ataque

        if (Input.GetKeyDown(KeyCode.C) && playerUm)
        {
            Ataque();
            Particulas = true;
            ParticulaDeAtaque.Play();

        }
        else
        {
            Particulas = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && playerUm == false)
        {
            Ataque();
            Particulas = true;
            ParticulaDeAtaque.Play();

        }
        else
        {
            Particulas = false;
        }

        //Debug.Log(Particulas);



        //Movimento

        float movimentoX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.UpArrow);

        if (playerUm)
        {
            movimentoX = Input.GetAxisRaw("Horizontal2");
            apertouPulo = Input.GetKeyDown(KeyCode.W);
        }

        if (estaNoChao == true)
        {

            fisica.velocity = new Vector3(movimentoX * velocidade, fisica.velocity.y, 0);


        }



        if (apertouPulo && estaNoChao)
        {
            fisica.velocity = Vector3.zero;
            fisica.AddForce(puloDiagonal * movimentoX, ForcaPulo, 0, ForceMode.Impulse);

        }

        /* if (Input.GetKeyDown(KeyCode.Space))
         {

             TomarDano(10);
         }*/

    }

    /* public void TomarDano(int dano)
     {


         vidaAtual -= dano;

         barraVida.SetarVida(vidaAtual);
     }*/



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            estaNoChao = true;
        }

       /* if (collision.gameObject.tag == "ParedeDireita")
        {
           

            fisica.AddForce(transform.right);

        }*/

        if (transform.position.y > collision.transform.position.y && estaNoChao == false && 
            collision.gameObject.tag == "Player")
        {
            fisica.velocity = Vector3.zero;
            fisica.AddForce(-transform.right * pushback);

            Debug.Log("Pushback: Eu,"+gameObject+", bati em: "+collision.gameObject);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            estaNoChao = false;
        }
    }

    //Ataque
    void Ataque()
    {
        Collider[] hitPlayer2 = Physics.OverlapSphere(pontoDeAtaque.position, alcanceDoAtaque, LayerDoOponente);

        foreach (Collider oponente in hitPlayer2)
        {

            TomarDano(10);

            Debug.Log("Acertou" + oponente.name);
        }
    }

    public void TomarDano(int dano)
    {


        vidaAtual -= dano;

        barraVida.SetarVida(vidaAtual);

        if (vidaAtual == 0)
        {
            SceneManager.LoadScene("MenuGameOver");
        }
    }

}