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
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Audio_Delay : MonoBehaviour
{
    AudioSource myAudio;
    public float delaySeconds;
    
    [Header("Repeat")]
    public bool repeat;
    public float repeatDelaySecondsMinimum;
    public float repeatDelaySecondsMaximum;

    //Model clonned each time a sound is played
    private GameObject audioModel;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        SaveAudioModel();
        Invoke("PlayAudio", delaySeconds);
    }

    //If repeat is selected before delay, this method will be called recursively
    void PlayAudio()
    {
        //Clone the model
        CloneAudio();
        if (!repeat) return;
        Invoke("PlayAudio", Random.Range(repeatDelaySecondsMinimum, repeatDelaySecondsMaximum));
    }

    //Make a modelbased on the audio source
    void SaveAudioModel()
    {
        audioModel = Instantiate(myAudio.gameObject, transform.position, transform.rotation, transform);
        Destroy(audioModel.GetComponent<Audio_Delay>());
        audioModel.GetComponent<AudioSource>().playOnAwake = true;
        audioModel.gameObject.SetActive(false);
    }

    //Clone the model then destroy it after the audioclip has been played
    void CloneAudio()
    {
        GameObject audioClone = Instantiate(audioModel,transform);
        audioClone.SetActive(true);
        StartCoroutine(WaitAndDestroy(audioClone, myAudio.clip.length));
    }

    private IEnumerator WaitAndDestroy(GameObject toDestroy, float timeSeconde)
    {
        yield return new WaitForSeconds(timeSeconde);
        Destroy(toDestroy);
    }

}