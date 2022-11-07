using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVidaCodigo : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image preencher;

    public void SetarVidaMaxima(int vida)
    {
        slider.maxValue = vida;
        slider.value = vida;

        preencher.color = gradient.Evaluate(1f);

    }

 public void SetarVida(int vida)
    {
        slider.value = vida;

        preencher.color = gradient.Evaluate(slider.normalizedValue);
    }
}
