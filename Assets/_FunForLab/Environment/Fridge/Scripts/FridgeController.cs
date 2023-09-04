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
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe qui va g�rer un frigo.
/// </summary>
public class FridgeController : MonoBehaviour {
	public UnityEvent correctContentEnter;
	/*private const string ScenarioCodeCBCInArchiveFridge = "CBC_IN_ARCHIVE_FRIDGE";
	private const string ScenarioCodeQCInArchiveFridge = "QC_IN_ARCHIVE_FRIDGE";
	private const string ScenarioCodeCBCInQCFridge = "CBC_IN_QC_FRIDGE";
	private const string ScenarioCodeQCInQCFridge = "QC_IN_QC_FRIDGE";*/

	private List<GameObject> contents;

	// Type de frigo
	public enum FridgeType {
		QC,
		Archive
	}

	public TableReference stringTable; // Table de traduction
	public TMP_Text textLabel; // Texte pour le label du frigo
	public TMP_Text textTemperature; // Texte pour la temp�rature du frigo
	public FridgeType fridgeType; // Type de frigo
	public int temperature = -10; // Temp�rature du frigo

	#region Messages Unity
	private void Start() {
		contents = new List<GameObject>();

		SetTemperature(temperature);
		textLabel.text = fridgeType == FridgeType.QC ? LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "QC") : LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "ARCHIVING");
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<XRGrabInteractable>() != null && !contents.Contains(other.gameObject)) { // Ne prendre en compte que les objets qui poss�dent un XR Grab Interactable (que les objets grabbables)
			if (other.gameObject.CompareTag("Fiole")) { // Si c'est une fiole, on envoie l'event
				SampleController.SampleType type = other.gameObject.GetComponent<SampleController>().sampleData.sampleType;

				if (type == SampleController.SampleType.CBC) {
					if (fridgeType == FridgeType.Archive) {
						correctContentEnter.Invoke();

                    }
					else {
						//ScenarioSendEvent(ScenarioCodeCBCInQCFridge, true);
					}
				}
				else if (type == SampleController.SampleType.QC) {
					if (fridgeType == FridgeType.Archive) {
						//ScenarioSendEvent(ScenarioCodeQCInArchiveFridge, true);
					}
					else {
                        correctContentEnter.Invoke();
                    }
				}
			}

			contents.Add(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.GetComponent<XRGrabInteractable>() != null) {
			if (other.gameObject.CompareTag("Fiole")) { // Si c'est une fiole, on envoie l'event
				SampleController.SampleType type = other.gameObject.GetComponent<SampleController>().sampleData.sampleType;

				if (type == SampleController.SampleType.CBC) {
					if (fridgeType == FridgeType.Archive) {
						//ScenarioSendEvent(ScenarioCodeCBCInArchiveFridge, false);
						QuestLog.SetQuestEntryState("QuestFFL1000", 6, QuestState.Active);
					}
					else {
						//ScenarioSendEvent(ScenarioCodeCBCInQCFridge, false);
					}
				}
				else if (type == SampleController.SampleType.QC) {
					if (fridgeType == FridgeType.Archive) {
						//ScenarioSendEvent(ScenarioCodeQCInArchiveFridge, false);
					}
					else {
						//ScenarioSendEvent(ScenarioCodeQCInQCFridge, false);
						QuestLog.SetQuestEntryState("QuestFFL1000", 5, QuestState.Active);
					}
				}
			}

			contents.Remove(other.gameObject);
		}
	}
	#endregion

	#region M�thodes publiques
	// Va changer la temp�rature affich�e.
	public void SetTemperature(int t) {
		temperature = t;
		textTemperature.text = temperature + "�C";
	}

	// Va envoyer un event avec code pour le Sc�nario.
	public void ScenarioSendEvent(string code, bool statut) {
		ScenarioCodeEvent.SendCodeEvent(code, statut);
	}
	#endregion
}
