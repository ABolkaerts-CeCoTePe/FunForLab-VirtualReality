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
using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using static PlayerPrefsManager;

/// <summary>
/// Classe qui g�re l'UI du SkinPod.
/// </summary>
public class SkinPodDisplay : MonoBehaviour {

	[Header("Animation Parameters")]
	public float WaitBeforeSmoke = 3f;
	public float SmokeDuration = 3f;
	public float WaitAfterSmoke = 3f;


	public Door Door;
	public ParticleSystem smoke;
	public AudioSource audioSource;
	public UnityEngine.UI.Toggle toggleSpaceSuit;
	public UnityEngine.UI.Toggle toggleMedicalSuit;

	public TableReference stringTable;

	public UnityEvent onSkinChangedToDummy;
	public UnityEvent onSkinChangedToAstra;

	private bool start = true;

	private void Start() {
		switch (PlayerModelController.Instance.PlayerModel) {
			case PlayerModels.SpaceSuit:
				toggleSpaceSuit.isOn = true;
				break;
			case PlayerModels.MedicalSuit:
                toggleMedicalSuit.isOn = true;
				break;
		}

		start = false;
	}

	private IEnumerator ChangeSuit(PlayerModels model)
	{
        Door.OpenDoor(false);
		yield return new WaitForSeconds(WaitBeforeSmoke);
        audioSource.Play();
		smoke.Play();
		yield return new WaitForSeconds(SmokeDuration);
        PlayerModelController.Instance.ChangeSkinModel(model);
        yield return new WaitForSeconds(SmokeDuration);
		audioSource.Stop();
		smoke.Stop();
		yield return new WaitForSeconds(WaitAfterSmoke);
		Door.OpenDoor(true);

    }

    // Action lorsque l'on clique sur le skin Dummy. Va changer le skin actuel par Dummy, lancer un son et des particules.
    public void ToggleSkinSpaceSuit() {
		Debug.Log("Skin Space choisi");
        onSkinChangedToDummy.Invoke();
        if (PlayerModelController.Instance.PlayerModel != PlayerModels.SpaceSuit && !start) {
            //			Door.OpenDoor(false);
            //			audioSource.Play();
            //			smoke.Emit(50); // Faire un peu de fum�e et attendre un peu avant de faire le changement de mod�le
            //			StartCoroutine(ChangeSkinCoroutine(PlayerModels.Dummy));

            StartCoroutine(ChangeSuit(PlayerModels.SpaceSuit));

            //QuestLog.SetQuestEntryState("QuestTutorial", 2, QuestState.Active); // Voir s'il n'y a pas moyen d'avoir un composant � part pour faire cela
			//DialogueManager.ShowAlert(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "SPACE_SUIT_ON"));

			
		}
	}

	// Action lorsque l'on clique sur le skin Astra. Va changer le skin actuel par Astra, lancer un son et des particules.
	public void ToggleSkinMedicalSuit() {
		Debug.Log("Skin m�dical choisi");
        onSkinChangedToAstra.Invoke();
        if (PlayerModelController.Instance.PlayerModel != PlayerModels.MedicalSuit && !start) {
            
            //           Door.OpenDoor(false);
            //          audioSource.Play();
            //			smoke.Emit(50);
            //			StartCoroutine(ChangeSkinCoroutine(PlayerModels.Astra));
            StartCoroutine(ChangeSuit(PlayerModels.MedicalSuit));

            //QuestLog.SetQuestEntryState("QuestTutorial", 2, QuestState.Success); // Voir s'il n'y a pas moyen d'avoir un composant � part pour faire cela
			//DialogueManager.ShowAlert(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "SPACE_SUIT_REMOVED"));

			
		}
	}

	// Coroutine qui va changer le skin du joueur.
	private IEnumerator ChangeSkinCoroutine(PlayerModels model) {
		yield return new WaitForSeconds(WaitBeforeSmoke);
		PlayerModelController.Instance.ChangeSkinModel(model);
		audioSource.Stop();
		Door.OpenDoor(true);
	}
}
