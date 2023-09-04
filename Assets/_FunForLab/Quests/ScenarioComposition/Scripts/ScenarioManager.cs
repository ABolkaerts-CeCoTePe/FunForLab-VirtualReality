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
using UnityEngine;

/// <summary>
/// Classe qui va gérer le scénario.
/// </summary>
public class ScenarioManager : MonoBehaviour {
	[SerializeField]
	private ScenarioData scenarioData;

	[HideInInspector]
	public ScenarioData scenarioInstance; // Pour ne pas modifier le ScriptableObject original du scénario

	public delegate void ScenarioChangedDelegate();
	public event ScenarioChangedDelegate ScenarioChanged;

	public bool ScenarioCompleted {
		get {
			if (scenarioInstance.scenarioDone) {
				return true;
			}

			foreach (ScenarioElement scenario in scenarioInstance.elements) {
				if (!scenario.done) {
					return false;
				}
			}

			scenarioInstance.scenarioDone = true;
			return true;
		}
	}

	public int ScenarioCurrent {
		get {
			if (scenarioInstance.ordered) {
				for (int i = 0; i < scenarioInstance.elements.Count; i++) {
					if (!scenarioInstance.elements[i].done) {
						return i;
					}
				}
			}

			return -1;
		}
	}

	public int ScenarioLastStep {
		get {
			if (scenarioInstance.ordered) {
				for (int i = 0; i < scenarioInstance.elements.Count; i++) {
					if (scenarioInstance.elements[i].step && ScenarioCurrent > i) {
						return i;
					}
				}
			}

			return -1;
		}
	}

	#region Messages Unity
	private void Awake() {
		scenarioInstance = Instantiate(scenarioData); // Faire une copie du ScriptableObject

		scenarioInstance.elements.Sort();

		foreach (ScenarioElement scenario in scenarioInstance.elements) {
			scenario.subElements.Sort();
		}
	}

	private void OnEnable() {
		ScenarioCodeEvent.ScenarioCode += ScenarioUpdate;
	}

	private void OnDisable() {
		ScenarioCodeEvent.ScenarioCode -= ScenarioUpdate;
	}
	#endregion

	#region Action listeners
	// Action lorsque qu'un event ScenarioCodeEvent est reçu. Va mettre à jour le scénario.
	public void ScenarioUpdate(string code, bool statut) {
		int current = ScenarioCurrent;
		int lastStep = ScenarioLastStep;

		if (ScenarioCompleted) {
			Debug.Log("Scénario déjà complété ! Plus de modifications");
			return;
		}

		for (int i = 0; i < scenarioInstance.elements.Count; i++) {
			ScenarioElement scenario = scenarioInstance.elements[i];

			if (scenario.code == code) {
				if (scenario.done || current == -1 || current == i) { // On peut le modifier que s'il est déjà marqué comme accompli, ou s'il s'agit d'une liste à puces, ou s'il s'agit de l'élément actuel

					if (lastStep == -1 || (current == i && current >= lastStep)) { // Ne mettre à jour un scénario précédent que si on n'a pas franchi une étape
						scenario.done = statut;

						ScenarioChanged?.Invoke(); // Envoyer un event pour mettre à jour l'UI Todo list
					}

					break;
				}
				else {
					Debug.Log("ScenarioManager - Event annulé car pas dans l'ordre !");
				}
			}
			else if (scenario.subElements.Count > 0) {
				int subCurrent = scenario.SubScenarioCurrent;

				for (int j = 0; j < scenario.subElements.Count; j++) {
					ScenarioSubElement subElement = scenario.subElements[j];

					if (subElement.code == code) {
						if (subElement.done || (subCurrent == -1 && current == i) || (subCurrent == j && current == i)) { // On peut modifier un sous-élément que s'il est déjà marqué comme accompli, ou s'il s'agit d'une liste à puces et que l'élément principal est l'actuel, ou s'il s'agit du sous-élément actuel avec l'élément principal actuel

							if (lastStep == -1 || (current == i && current >= lastStep)) { // Ne mettre à jour un scénario précédent que si on n'a pas franchi une étape
								subElement.done = statut;

								// Si tous les sous-scénarios sont complétés, on complète le parent aussi
								if (scenario.SubElementsCompleted) {
									scenario.done = true;
								}
								else {
									scenario.done = false;
								}

								ScenarioChanged?.Invoke(); // Envoyer un event pour mettre à jour l'UI Todo list
							}

							break;
						}
						else {
							Debug.Log("ScenarioManager - Sub Event annulé car pas dans l'ordre !");
						}
					}
				}
			}
		}
	}
	#endregion
}

/// <summary>
/// Classe qui va permettre de faire l'event avec code pour le scénario.
/// </summary>
public static class ScenarioCodeEvent {
	public delegate void ScenarioCodeEventDelegate(string code, bool statut);
	public static event ScenarioCodeEventDelegate ScenarioCode;

	// Va invoquer l'event.
	public static void SendCodeEvent(string code, bool statut) {
		ScenarioCode?.Invoke(code, statut);
	}
}
