/*
 * Copyright 2023 Interreg V EMR 145 - FunForLab
 *
 * Licensed under the EUPL, Version 1.2 or � as soon they will be approved by the European Commission - subsequent versions of the EUPL (the "Licence");
 * You may not use this work except in compliance with the Licence.
 * You may obtain a copy of the Licence at:
 *
 * https://joinup.ec.europa.eu/software/page/eupl
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the Licence is distributed on an "AS IS" basis,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Licence for the specific language governing permissions and limitations under the Licence.
 * 
 *  Author: Aur�lien Bolkaerts
 *  
 *  Project: Interreg V EMR 145 - FunForLab
 *  Website: http://funforlab.eu
 *  Project Date: March 2021 - August 2023
 *  Contributors:
 *      Centre de Recherche des Instituts Group�s de la Haute Ecole Libre Mosane (CRIG):
 *          - Isabelle Bragard
 *          - Birgit Quinting 
 *          - Sonia El Guendi
 *          - Annabelle Lejeune
 *          - Ingrid Hamer
 *          - M�lanie Zenner
 *          - J�rome Foguenne
 *      Centre de recherche et de formation continue de la Haute Ecole Namur Li�ge Luxembourg (FoRS):
 *          - Julien Lecointre
 *          - Simon Daniau
 *          - Laura Ramonfosse
 *          - Christophe Cl�ment
 *          - Amandine Schreiber
 *      Ausbildungsakademie f�r Gesundheitsberufe, Uniklinik RWTH Aachen (UKAachen):
 *          - Eva Sch�nen
 *          - Giannina Lindt
 *          - Patricia B�ts
 *          - Silvia Schneiders
 *          - Miriam Scheld
 *          - Monika Krichel-Frings
 *          - Christiane  Stickelmann
 *      Centre de Coop�ration Technique et P�dagogique (CeCoTePe):
 *          - Fr�d�ric Kotnik
 *          - Brian Deschamps
 *          - M�lanie Zenner
 *          - Aur�lien Bolkaerts
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
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> paragrapheGO = new List<GameObject>();
    [SerializeField] private float waitTimeBeforeStart = 0;
    [SerializeField] private List<float> waitTimeAfterParagraphe = new List<float>();
    [SerializeField] private string mainMenuSceneName = "GameUI";

    private int _index = 0;

    private void Awake()
    {
        foreach(GameObject paragraphe in paragrapheGO)
            EnableParagraphe(paragraphe, false);
    }
    private void Start()
    {
        //PlayParagraphe();
        Invoke("PlayParagraphe", waitTimeBeforeStart);
        DisableAllParagraphes();

    }

    private IEnumerator WaitAndPlayNextParagraphe()
    {
        DisableAllParagraphes();
        EnableParagraphe(paragrapheGO[_index], true);

        float waitTime = waitTimeAfterParagraphe[_index];
        waitTime += paragrapheGO[_index].GetComponent<AudioSource>().clip.length;

        Debug.Log("Wait time [" + _index + "] = " + waitTime);
        yield return new WaitForSeconds(waitTime);

        NextParagraphe();
    }

    private void PlayParagraphe()
    {
        StartCoroutine(WaitAndPlayNextParagraphe());
    }

    private void EnableParagraphe(GameObject paragraphe, bool state)
    {
        paragraphe.SetActive(state);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void DisableAllParagraphes()
    {
        foreach (GameObject paragraphe in paragrapheGO)
        {
            EnableParagraphe(paragraphe, false);
        }
    }

    private void NextParagraphe()
    {
        _index++;
        if (_index >= paragrapheGO.Count)
        {
            ChangeScene();
        }
        else
        {
            StartCoroutine(WaitAndPlayNextParagraphe());
        }
    }

    public void Next()
    {
        StopCoroutine("WaitAndPlayNextParagraphe");
        NextParagraphe();
    }

    public void SkipAll()
    {
        ChangeScene();
    }

}
