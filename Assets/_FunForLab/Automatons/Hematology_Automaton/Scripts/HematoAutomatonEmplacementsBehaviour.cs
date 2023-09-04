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
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe StateMachineBehaviour pour l'animation du rack et de ses emplacements de fioles dans l'emplacement du FFL 1000.
/// </summary>
public class HematoAutomatonEmplacementsBehaviour : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		int emplacement = animator.GetInteger("RackEmplacement");

		if (emplacement == 11) { // Si on arrive � la sortie
			HematoAutomatonController scriptFFL = animator.gameObject.GetComponent<HematoAutomatonController>();
			scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_ANALYZES_COMPLETED"));
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		int emplacement = animator.GetInteger("RackEmplacement");

		HematoAutomatonController scriptFFL = animator.gameObject.GetComponent<HematoAutomatonController>();
		XRSocketInteractorExtension[] interactors = animator.gameObject.GetComponentsInChildren<XRSocketInteractorExtension>();

		foreach (XRSocketInteractorExtension interactor in interactors) { // Oui pourrait �tre bien mieux cod�, mais c'�tait le d�but du stage et �a marche donc voil�
			if (interactor.gameObject.CompareTag("EmplacementRack")) {
				if (emplacement == 11) { // Reset l'interactionLayer afin de pouvoir regrab l'objet quand on arrive � la sortie
					scriptFFL.ResetLayersGrab();
					scriptFFL.DesactiverClignotementLedStatus();
					scriptFFL.display.DesactiverClignotementStatus();
					scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_REMOVE_RACK"));
					return;
				}

				// Si le niveau de r�actif est trop bas, arr�ter de faire les analyses.
				bool analyseOK = true;

				for (int i = 0; i < scriptFFL.reagentDatas.Length; i++) {
					ReagentData reagentData = scriptFFL.reagentDatas[i];

					if (reagentData.lvlReagent - HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[i] < 0) { // Si lors de la prochaine analyse on a pas assez de r�actif
						analyseOK = false;
						scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_REAGENT_INSUFFICIENT", arguments: reagentData.reagent));
						scriptFFL.LedStatusRouge();
						break;
					}
				}

				// R�cup�rer la fiole se trouvant � l'emplacement actuel
				XRSocketInteractorExtension[] emplacements = ((XRGrabInteractable) interactor.firstInteractableSelected).GetComponentsInChildren<XRSocketInteractorExtension>();

				XRGrabInteractable fiole = (XRGrabInteractable) emplacements[10 - emplacement].firstInteractableSelected;
				if (fiole == null) {
					Debug.Log("Emplacement " + emplacement + " vide !"); // Ne rien faire sur cet emplacement et passer au suivant
					scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_EMPLACEMENT_EMPTY", arguments: emplacement));
					animator.SetInteger("RackEmplacement", emplacement + 1);
				}
				else { // Faire tourner la fiole qui est dans l'emplacement (scan), puis la prendre avec la pince
					Debug.Log("Emplacement " + emplacement + " contient = " + fiole.gameObject.GetComponent<SampleController>().sampleData.sampleID);
					scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_EMPLACEMENT_CONTAINS", arguments: new object[] { emplacement, fiole.gameObject.GetComponent<SampleController>().sampleData.sampleID }));

					if (fiole.gameObject.GetComponent<SampleController>().sampleData.sampleType == SampleController.SampleType.CBC) {
						QuestLog.SetQuestEntryState("QuestFFL1000", 2, QuestState.Success);
					}
					else {
						QuestLog.SetQuestEntryState("QuestFFL1000", 1, QuestState.Success);
					}

					if (analyseOK) {
						// Faire tourner la fiole pour la scanner
						FioleInteraction script = emplacements[10 - emplacement].gameObject.GetComponent<FioleInteraction>();
						script.SwitchRotation360();
					}
					else { // Il ne reste plus assez de r�actif pour faire des analyses, on annule tout
						animator.SetInteger("RackEmplacement", emplacement + 1);
					}
				}

				break;
			}
		}
	}
}
