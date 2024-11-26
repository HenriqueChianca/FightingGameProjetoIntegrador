using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Controller : MonoBehaviour
{
    public Rigidbody fisica;

    // Comabte Parte
    public Transform pontoDeAtaque;
    public float alcanceDoAtaque = 1f;
    public LayerMask LayerDoOponente;
    public float poriximityBlockP2 = 1f;
    private AudioSource AudioSource;
    public float pushbackStrike = 10;
    public ParticleSystem ParticulaDeAtaque;

    // Vida Parte
    public int vidaMaxima = 100;
    public int vidaAtual;
    public BarraDeVidaCodigo barraVida;

    // Movimento Parte
    public float velocidade = 300;
    public float ForcaPulo = 700;
    public float puloDiagonal = 30;
    bool estaNoChao;
    public float pushback = 200;
    public Transform outroPlayer;
    public float distanciaParaGirar = 1f;
    public Animator animator;

    private float previousXPosition;
    private bool wasGroundedLastFrame;
    private bool hasJumped;
    private bool olhandoParaDireita = true;

    public float flipThreshold = 0.1f;

    // Start é chamado antes do primeiro frame
    void Start()
    {
        fisica = GetComponent<Rigidbody>();
        vidaAtual = vidaMaxima;
        barraVida.SetarVidaMaxima(vidaMaxima);
        previousXPosition = transform.position.x;
        wasGroundedLastFrame = false;
        hasJumped = false;
        estaNoChao = true;
        AudioSource = GetComponent<AudioSource>();
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            return; // Interrompe o Update, impedindo movimentação
        }

        // Mantém o Collider na posição do jogador
        BoxCollider collider = GetComponent<BoxCollider>();

        if (collider != null)
        {
            // Alinha o collider à posição do jogador em tempo real
            collider.center = transform.position;
        }


        // Método de ataque
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Ataque();
            animator.SetBool("Apertou",true);
           
            //ParticulaDeAtaque.Play();
        }
        else
        {
            animator.SetBool("Apertou", false);
        }

        // Movimentação e Flip
        float movimentoX = Input.GetAxisRaw("Horizontal");
        bool apertouPulo = Input.GetKeyDown(KeyCode.UpArrow);

        AtualizarMoveDirection(movimentoX);
        AtualizarAnimacoes(movimentoX);

        if (outroPlayer.position.x > transform.position.x - distanciaParaGirar && olhandoParaDireita)
        {
            Flip();
        }
        else if (outroPlayer.position.x < transform.position.x + distanciaParaGirar && !olhandoParaDireita)
        {
            Flip();
        }

        // Movimentação no chão
        if (estaNoChao)
        {
            Vector3 targetVelocity = new Vector3(movimentoX * velocidade, fisica.velocity.y, 0);
            fisica.velocity = targetVelocity;
        }

        AtualizarAnimacoes(movimentoX);

        // Pulo
        if (apertouPulo && estaNoChao && !hasJumped)
        {
            fisica.velocity = Vector3.zero;
            fisica.AddForce(new Vector3(puloDiagonal * movimentoX, ForcaPulo, 0), ForceMode.Impulse);
            hasJumped = true;
            animator.SetTrigger("Jump");
        }

        if (!estaNoChao)
        {
            animator.SetBool("Pulou", true);
        }
        else
        {
            animator.SetBool("Pulou", false);
            hasJumped = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            estaNoChao = true;
        }

        if (transform.position.y > collision.transform.position.y && estaNoChao == false && collision.gameObject.tag == "Player")
        {
            fisica.velocity = Vector3.zero;
            fisica.AddForce(-transform.right * pushback);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            estaNoChao = false;
        }
    }

    // Função que atualiza a direção de movimento no Animator
    private void AtualizarMoveDirection(float movimentoX)
    {
        if (olhandoParaDireita)
        {
            animator.SetFloat("MoveDirection", movimentoX > 0 ? 1 : (movimentoX < 0 ? -1 : 0));
        }
        else
        {
            animator.SetFloat("MoveDirection", movimentoX < 0 ? 1 : (movimentoX > 0 ? -1 : 0));
        }
    }

    // Função para controlar as animações do Player 2
    private void AtualizarAnimacoes(float movimentoX)
    {
        if (olhandoParaDireita)
        {
            if (movimentoX < 0)
            {
                animator.SetBool("AndouPraFrente", true);
                animator.SetBool("AndouPraTras", false);
            }
            else if (movimentoX > 0)
            {
                animator.SetBool("AndouPraFrente", false);
                animator.SetBool("AndouPraTras", true);
            }
            else
            {
                animator.SetBool("AndouPraFrente", false);
                animator.SetBool("AndouPraTras", false);
            }
        }
        else
        {
            if (movimentoX < 0)
            {
                animator.SetBool("AndouPraFrente", false);
                animator.SetBool("AndouPraTras", true);
            }
            else if (movimentoX > 0)
            {
                animator.SetBool("AndouPraFrente", true);
                animator.SetBool("AndouPraTras", false);
            }
            else
            {
                animator.SetBool("AndouPraFrente", false);
                animator.SetBool("AndouPraTras", false);
            }
        }
    }

    // Flip para inverter o Player 2
    private void Flip()
    {
        olhandoParaDireita = !olhandoParaDireita;

        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Função de ataque
    void Ataque()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Apertou"))
        {
            Debug.Log("Evento de ataque disparado.");
            // return; // Sai do método se a animação não estiver ativa.
        }

        Collider[] hitPlayer1 = Physics.OverlapSphere(pontoDeAtaque.position, alcanceDoAtaque, LayerDoOponente);

        foreach (Collider oponente in hitPlayer1)
        {
            if (oponente.gameObject != gameObject)
            {
                oponente.GetComponent<PlayerController>().TomarDano(10);
                AudioSource.Play();
                Vector3 direcaoEmpurrao = (transform.position - oponente.transform.position).normalized;
                direcaoEmpurrao.y = 0; // Zerar o empurrão vertical

                // Adicionar força de empurrão ao atacante
                fisica.AddForce(direcaoEmpurrao * pushbackStrike, ForceMode.Impulse);
            }
        }

        if (ParticulaDeAtaque != null)
        {
            ParticulaDeAtaque.transform.position = pontoDeAtaque.position; // Posiciona a partícula no ponto do ataque
            ParticulaDeAtaque.Play(); // Toca o sistema de partículas
        }
    }

    // Função de tomar dano
    public void TomarDano(int dano)
    {
        vidaAtual -= dano;
        barraVida.SetarVida(vidaAtual);

        if (vidaAtual <= 0)
        {
            SceneManager.LoadScene("MenuGameOver");
        }
    }
}