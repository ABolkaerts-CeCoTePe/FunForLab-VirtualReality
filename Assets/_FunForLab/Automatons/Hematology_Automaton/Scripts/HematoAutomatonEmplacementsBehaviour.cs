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
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe StateMachineBehaviour pour l'animation du rack et de ses emplacements de fioles dans l'emplacement du FFL 1000.
/// </summary>
public class HematoAutomatonEmplacementsBehaviour : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		int emplacement = animator.GetInteger("RackEmplacement");

		if (emplacement == 11) { // Si on arrive à la sortie
			HematoAutomatonController scriptFFL = animator.gameObject.GetComponent<HematoAutomatonController>();
			scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_ANALYZES_COMPLETED"));
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		int emplacement = animator.GetInteger("RackEmplacement");

		HematoAutomatonController scriptFFL = animator.gameObject.GetComponent<HematoAutomatonController>();
		XRSocketInteractorExtension[] interactors = animator.gameObject.GetComponentsInChildren<XRSocketInteractorExtension>();

		foreach (XRSocketInteractorExtension interactor in interactors) { // Oui pourrait être bien mieux codé, mais c'était le début du stage et ça marche donc voilà
			if (interactor.gameObject.CompareTag("EmplacementRack")) {
				if (emplacement == 11) { // Reset l'interactionLayer afin de pouvoir regrab l'objet quand on arrive à la sortie
					scriptFFL.ResetLayersGrab();
					scriptFFL.DesactiverClignotementLedStatus();
					scriptFFL.display.DesactiverClignotementStatus();
					scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_REMOVE_RACK"));
					return;
				}

				// Si le niveau de réactif est trop bas, arrêter de faire les analyses.
				bool analyseOK = true;

				for (int i = 0; i < scriptFFL.reagentDatas.Length; i++) {
					ReagentData reagentData = scriptFFL.reagentDatas[i];

					if (reagentData.lvlReagent - HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[i] < 0) { // Si lors de la prochaine analyse on a pas assez de réactif
						analyseOK = false;
						scriptFFL.display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(scriptFFL.display.stringTable, "MESSAGE_REAGENT_INSUFFICIENT", arguments: reagentData.reagent));
						scriptFFL.LedStatusRouge();
						break;
					}
				}

				// Récupérer la fiole se trouvant à l'emplacement actuel
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
					else { // Il ne reste plus assez de réactif pour faire des analyses, on annule tout
						animator.SetInteger("RackEmplacement", emplacement + 1);
					}
				}

				break;
			}
		}
	}
}
