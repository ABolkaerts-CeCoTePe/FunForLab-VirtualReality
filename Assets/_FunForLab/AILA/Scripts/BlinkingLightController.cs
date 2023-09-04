/*
 * Copyright 2023 Interreg V EMR 145 - FunForLab
 *
 * Licensed under the EUPL, Version 1.2 or – as soon they will be approved by the European Commission - subsequent versions of the EUPL (the "Licence");
 * You may not use this work except in compliance with the Licence.
 * You may obtain a copy of the Licence at:
 *
 * https://joinup.ec.europa.eu/software/page/eupl
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the Licence is distributed on an "AS IS" basis,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Licence for the specific language governing permissions and limitations under the Licence.
 * 
 *  Author: Aurélien Bolkaerts
 *  
 *  Project: Interreg V EMR 145 - FunForLab
 *  Website: http://funforlab.eu
 *  Project Date: March 2021 - August 2023
 *  Contributors:
 *      Centre de Recherche des Instituts Groupés de la Haute Ecole Libre Mosane (CRIG):
 *          - Isabelle Bragard
 *          - Birgit Quinting 
 *          - Sonia El Guendi
 *          - Annabelle Lejeune
 *          - Ingrid Hamer
 *          - Mélanie Zenner
 *          - Jérome Foguenne
 *      Centre de recherche et de formation continue de la Haute Ecole Namur Liège Luxembourg (FoRS):
 *          - Julien Lecointre
 *          - Simon Daniau
 *          - Laura Ramonfosse
 *          - Christophe Clément
 *          - Amandine Schreiber
 *      Ausbildungsakademie für Gesundheitsberufe, Uniklinik RWTH Aachen (UKAachen):
 *          - Eva Schönen
 *          - Giannina Lindt
 *          - Patricia Büts
 *          - Silvia Schneiders
 *          - Miriam Scheld
 *          - Monika Krichel-Frings
 *          - Christiane  Stickelmann
 *      Centre de Coopération Technique et Pédagogique (CeCoTePe):
 *          - Frédéric Kotnik
 *          - Brian Deschamps
 *          - Mélanie Zenner
 *          - Aurélien Bolkaerts
 *          - Guillaume Vilvorder
 *          - Maxime Palmisano
 *      Zuyd Hogeschool:
 *          - Olivier Segers
 *          - Olaf Brouwers
 *          - Jeroen Heijdeman
 *          - Marliene Bos
 *          - Ron Reuleaux 
 *      UC Leuven & Limburg (UCLL):
 *          - Eveline Strackx
 *          - Karolien Decamps
 *          - Evi Lemmens
 *          - Laura De Bock
 *          - Raf Donders
 *          - Evelyn Jans
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlinkingLightController : MonoBehaviour
{
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Color emissionColor;

    [SerializeField] private float _maxIntensity;
    [SerializeField] private float _minIntensity;

    [SerializeField] private float _lightUpTime;
    [SerializeField] private float _delayBetweenBlink;
    [SerializeField] private float _delayBetweenPattern;

    public void SetColor(string colorName)
    {
        Color newColor;
        switch (colorName)
        {
            case "Blue":
                newColor = Color.blue;
                break;

            case "Red":
                newColor = Color.red;
                break;

            case "Green":
                newColor = Color.green;
                break;

            case "Yellow":
                newColor = Color.yellow;
                break;

            default:
                newColor = Color.red;
                break;
        }
        SetColor(newColor);
    }

    private IEnumerator InfiniteBlink()
    {
        WaitForSeconds waitTime = new WaitForSeconds(2);
        while (true)
        {
            StartCoroutine("Blink");
            yield return new WaitForSeconds(_delayBetweenBlink);
            StartCoroutine("Blink");
            yield return new WaitForSeconds(_delayBetweenPattern);
        }
    }
    private IEnumerator Blink()
    {
        SetIntensity(_maxIntensity);
        yield return new WaitForSeconds(_lightUpTime);
        SetIntensity(_minIntensity);
    }

    private void Start()
    {
        SetColor(lightMaterial.color);
        StartCoroutine("InfiniteBlink");
    }

    private void SetColor(Color color)
    {
        lightMaterial.color = color;
        float hue, saturation, value;
        Color.RGBToHSV(color, out hue, out saturation, out value);
        emissionColor = Color.HSVToRGB(hue, 1.0f, 1.0f);
    }


    public void SetIntensity(float intensity)
    {
        Color newEmissionColor = emissionColor * intensity;
        lightMaterial.SetColor("_EmissionColor", newEmissionColor);
    }
}
