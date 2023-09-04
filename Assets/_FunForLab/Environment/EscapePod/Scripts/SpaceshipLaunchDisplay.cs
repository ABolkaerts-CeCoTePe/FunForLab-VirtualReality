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
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Classe qui gère l'écran permettant de faire décoller le vaisseau.
/// </summary>
public class SpaceshipLaunchDisplay : MonoBehaviour
{
    [Header("Animation Parameters")]
    public float WaitDoorDuration = 5f;
    public float WaitForAnouncement = 5f;
    public float FadeOutDuration = 5f;

    public Button buttonLaunch;
    public TableReference stringTable;
    public FadeScreen fadeScreen;
    public Animator doorAnimator;

    public AudioTrigger audioTrigger;

    


    // Action lorsque le bouton Décoller est cliqué.
    public void ButtonLaunch()
    {
        StartCoroutine("Launch");
    }

    private IEnumerator Launch()
    {
        Debug.Log("Décollage du vaisseau !");
        CloseDoor();
        yield return new WaitForSeconds(WaitDoorDuration);
        TakingOff();
        yield return new WaitForSeconds(WaitForAnouncement);
        FadeIn();
        yield return new WaitForSeconds(FadeOutDuration);

        //       QuestLog.SetQuestEntryState("QuestTutorial", 3, QuestState.Success);
        //       DialogueManager.ShowAlert(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "SHIP_TAKE_OFF"));


        EndOfTutorial();
    }

    
    private void CloseDoor()
    {
        buttonLaunch.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "DOOR_CLOSE");
        doorAnimator.enabled = true;
        buttonLaunch.interactable = false;
    }

    private void TakingOff()
    {
        buttonLaunch.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "SHIP_TAKE_OFF");
        audioTrigger.PlayNextAudioClip();
    }

    private void FadeIn()
    {
        fadeScreen.fadeDuration = FadeOutDuration;
        fadeScreen.FadeIn();

    }
    private void EndOfTutorial()
    {
        PlayerPrefs.SetString("Level", "Sylvia");
        SceneManager.LoadScene("Level1-game");

    }
}
