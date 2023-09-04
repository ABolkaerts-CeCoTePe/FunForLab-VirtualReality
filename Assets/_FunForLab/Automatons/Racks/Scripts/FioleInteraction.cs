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
using UnityEngine.Localization.Tables;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe qui va g�rer l'interaction de l'emplacement d'une fiole dans le rack, notamment avec la pince du FFL 1000.
/// </summary>
public class FioleInteraction : MonoBehaviour {
	private const string ScenarioCodeCBCRack = "CBC_IN_RACK";
	private const string ScenarioCodeQCRack = "QC_IN_RACK";

	private XRSocketInteractorExtension xRSocketInteractor;
	private XRGrabInteractable lastInteractable;
	private Rigidbody interactableRigidbody;
	private bool rotation360;
	private bool pinceGrab;
	private int angle;

	public Transform pincePosition; // Position de la pince
	public TableReference stringTable;

	#region Messages Unity
	private void Awake() {
		xRSocketInteractor = GetComponent<XRSocketInteractorExtension>();
		rotation360 = false;
		pinceGrab = false;
		angle = 0;
	}

	private void Update() {
		if (rotation360) { // Faire tourner la fiole pour faire un 360�
			Transform transform = xRSocketInteractor.attachTransform;
			transform.Rotate(0, 5, 0);
			angle += 5;

			if (angle == 360) { // Oui pourrait �tre bien mieux cod�, mais c'�tait le d�but du stage et �a marche donc voil�
				angle = 0;
				rotation360 = false;
				ActiverPince(); // Activer l'animation de la pince
				EventPinceMonte();
			}

			xRSocketInteractor.attachTransform = transform;
		}
		else if (pinceGrab) { // Faire monter l'emplacement pour qu'il corresponde � celui de la pince
			Transform transform = xRSocketInteractor.attachTransform;
			Vector3 pos = new Vector3(transform.localPosition.x, pincePosition.localPosition.y * 0.33f, transform.localPosition.z);
			transform.localPosition = pos;
		}
	}
	#endregion

	#region M�thodes priv�es
	// Va activer l'animation de la pince du FFL 1000 qui monte.
	private void ActiverPince() {
		XRGrabInteractable rackInteractable = xRSocketInteractor.gameObject.GetComponentInParent<XRGrabInteractable>();
		XRSocketInteractorExtension machineInteractor = (XRSocketInteractorExtension) rackInteractable.firstInteractorSelecting;
		Animator machineAnimator = machineInteractor.GetComponentInParent<Animator>();
		Animator[] animators = machineAnimator.GetComponentsInChildren<Animator>();

		foreach (Animator anims in animators) {
			if (anims.gameObject.CompareTag("Pince")) {
				anims.enabled = true;
				break;
			}
		}
	}
	#endregion

	#region M�thodes publiques
	// Va activer ou d�sactiver la rotation � 360� de l'emplacement.
	public void SwitchRotation360() {
		rotation360 = !rotation360;
	}

	// Va activer le d�placement de l'emplacement pour correspondre � la pince.
	public void EventPinceMonte() {
		pinceGrab = true;
	}

	// Va reset la position de l'emplacement.
	public void EventPinceReset() {
		pinceGrab = false;

		// Remettre l'attachTransform d'origine
		xRSocketInteractor.attachTransform = transform.GetChild(0);
	}

	// Va envoyer un event avec code pour le Sc�nario.
	public void ScenarioSendEvent(string code, bool statut) {
		ScenarioCodeEvent.SendCodeEvent(code, statut);
	}

	// Va appeler ScenarioSendEvent avec le code CBC_IN_RACK.
	public void ScenarioCBCRackEvent(bool statut) {
		ScenarioSendEvent(ScenarioCodeCBCRack, statut);
	}

	// Va appeler ScenarioSendEvent avec le code QC_IN_RACK.
	public void ScenarioQCRackEvent(bool statut) {
		ScenarioSendEvent(ScenarioCodeQCRack, statut);
	}
	#endregion

	#region Action listeners
	// Action lorsqu'une fiole est ins�r�e dans un emplacement. Va r�activer la gravit� et d�sactiver kinematic de l'objet ins�r� dans l'emplacement du rack, sinon la physique fait des choses bizarres.
	public void ChangeGravity() {
		interactableRigidbody = xRSocketInteractor.GetOldestInteractableSelected().transform.GetComponent<Rigidbody>();
		interactableRigidbody.useGravity = true;
		interactableRigidbody.isKinematic = false;
		lastInteractable = (XRGrabInteractable) xRSocketInteractor.GetOldestInteractableSelected();
	}

	// Action lorsqu'une fiole est ins�r�e ou retir�e d'un emplacement. Va v�rifier le type de l'�chantillon et envoyer le message correspondant pour le sc�nario.
	public void CheckSampleType(bool statut) {
		SampleController.SampleType type = lastInteractable.GetComponent<SampleController>().sampleData.sampleType;

		// Envoyer un event pour dire si c'est un �chantillon CBC ou de QC
		if (type == SampleController.SampleType.CBC) {
			//ScenarioCBCRackEvent(statut);
			if (!DialogueLua.GetVariable("AnalysisDone").asBool) {
				if (statut) {
					QuestLog.SetQuestEntryState("QuestFFL1000", 2, QuestState.Success); // Voir s'il n'y a pas moyen d'avoir un composant � part pour faire cela
					//DialogueManager.ShowAlert("�chantillon CBC ins�r� dans le rack !");
				}
				else {
					QuestLog.SetQuestEntryState("QuestFFL1000", 2, QuestState.Active);
					//DialogueManager.ShowAlert("�chantillon CBC retir� du rack !");
				}
			}
		}
		else if (type == SampleController.SampleType.QC) {
			//ScenarioQCRackEvent(statut);
			if (!DialogueLua.GetVariable("AnalysisDone").asBool) {
				if (statut) {
					QuestLog.SetQuestEntryState("QuestFFL1000", 1, QuestState.Success);
					//DialogueManager.ShowAlert("�chantillon QC ins�r� dans le rack !");
				}
				else {
					QuestLog.SetQuestEntryState("QuestFFL1000", 1, QuestState.Active);
					//DialogueManager.ShowAlert("�chantillon QC retir� du rack !");
				}
			}
		}
	}
	#endregion
}
