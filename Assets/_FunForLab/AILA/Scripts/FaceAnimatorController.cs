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
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using UnityEngine;

public class FaceAnimatorController : MonoBehaviour
{
    public AudioSource audioSource;
    public Animator robotAnimator;
    private RobotState robotState;
    private Coroutine talkingCoroutine;

    // Idle
    // Moving
    // Talking
    // Phoning
    // Happy
    // Sad

    public enum RobotState
    {
        Idle,
        Moving,
        Talking,
        Phoning,
        Happy,
        Sad
    }

    private void OnEnable()
    { 
        Lua.RegisterFunction("SetRobotState", this, SymbolExtensions.GetMethodInfo(() => SetRobotState(string.Empty)));
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction("SetRobotState");
    }


    private void Start()
    {
        SetRobotState("Idle");
    }

    private void Update()
    {
        if(audioSource.isPlaying && robotState == RobotState.Idle)
        {
            SetRobotState("Talking");
        }
    }

    public void SetRobotState(string newState)
    {
//        Debug.Log("New robot state " + newState);
        robotState = (RobotState)Enum.Parse(typeof(RobotState),newState);
        switch (robotState)
        {
            case RobotState.Idle:
                robotAnimator.SetTrigger("Smile");
                break;

            case RobotState.Moving:
                robotAnimator.SetTrigger("Smile");
                break;

            case RobotState.Talking:
                StartTalkingCoroutine();
                break;

            case RobotState.Phoning:
                robotAnimator.SetTrigger("Phone");
                break;

            case RobotState.Happy:

                break;

            case RobotState.Sad:

                break;
        }
    }

    private void StartTalkingCoroutine()
    {
        if (robotState == RobotState.Talking && talkingCoroutine == null)
        {
            talkingCoroutine = StartCoroutine(TalkingCoroutine());
        }
    }

    private IEnumerator TalkingCoroutine()
    {
        robotAnimator.SetTrigger("Talks");
        do
        {    
            yield return new WaitForSeconds(0.3f);

        }
        while (audioSource.isPlaying);
        if (robotState == RobotState.Talking) SetRobotState("Idle");
        talkingCoroutine = null;
    }
}
