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
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// Classe qui g�re l'affichage de la ToDo List du Sc�nario.
/// </summary>
public class ToDoListScenario : MonoBehaviour {
	private static readonly char[] Letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

	private const string Bullet = "�";
	private const string SubBullet = "�";
	private const int Indent = 6; // 6%
	private const int SubIndent = 15; // 15%
	private const int SubSize = 90; // 90%
	private const string ColorCurrent = "#0000FF";
	private const string ColorDone = "#808080";

	#region Variables UI
	private TMP_Text textTitle;
	private TMP_Text textElements;
	#endregion

	public ScenarioManager scenarioManager; // Manager du sc�nario
	public string stringTable; // Table de localisation

	#region Messages Unity
	private void Awake() {
		scenarioManager = GetComponent<ScenarioManager>();

		textTitle = transform.Find("PanelBackground/PanelTitle/TextTitle").gameObject.GetComponent<TMP_Text>();
		textElements = transform.Find("PanelBackground/PanelContent/ScrollViewElements/Viewport/Content").gameObject.GetComponent<TMP_Text>();
	}

	private void Start() {
		scenarioManager.ScenarioChanged += ReceptionEventScenarioChanged;

		SetTextTitle();
		DrawScenarioElements();
	}
	#endregion

	#region M�thodes priv�es
	// Va changer le texte de la barre de titre.
	private void SetTextTitle(bool completed = false) {
		string msg = "Todo � " + LocalizationSettings.StringDatabase.GetLocalizedString(scenarioManager.scenarioInstance.stringTableScenario, "SCENARIO_NAME") + " � " + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "TEXT_DIFFICULTY") + " : ";

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.Secondary) {
			msg += LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "TOGGLE_SECONDARY");
		}
		else {
			msg += LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "TOGGLE_MLT");
		}

		if (completed) {
			msg += " � " + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "DONE") + " !";
		}

		textTitle.text = msg;
	}

	// Va mettre � jour l'affichage de la Todo list.
	private void DrawScenarioElements() {
		string text = string.Empty;
		int current = scenarioManager.ScenarioCurrent;

		for (int i = 0; i < scenarioManager.scenarioInstance.elements.Count; i++) {
			ScenarioElement scenarioElement = scenarioManager.scenarioInstance.elements[i];

			if (scenarioManager.scenarioInstance.ordered) { // Liste tri�e
				text += "<b>" + (i + 1) + ".</b>";
			}
			else { // Liste � puces
				text += "<b>  " + Bullet + "</b>";
			}

			text += "<indent=" + Indent + "%>";

			if (scenarioElement.Done) {
				text += "<s><color=" + ColorDone + ">";
			}
			else if (i == current) {
				text += "<b><color=" + ColorCurrent + ">";
			}

			text += scenarioElement.description.GetLocalizedString();

			if (scenarioElement.Done) {
				text += "</color></s>";
			}
			else if (i == current) {
				text += "</color></b>";
			}

			text += "</indent>\n";

			// S'il y a des sous-�l�ments
			if (scenarioElement.subElements.Count > 0) {
				for (int j = 0; j < scenarioElement.subElements.Count; j++) {
					ScenarioSubElement scenarioSubElement = scenarioElement.subElements[j];

					text += "\t<size=" + SubSize + "%>";

					if (scenarioElement.subOrdered) { // Liste tri�e
						text += "<b>" + Letters[j] + ".</b>";
					}
					else { // Liste � puces
						text += "<b>" + SubBullet + "</b>";
					}

					text += "<indent=" + SubIndent + "%>";

					if (scenarioSubElement.done) {
						text += "<s><color=" + ColorDone + ">";
					}

					text += scenarioSubElement.description.GetLocalizedString();

					if (scenarioSubElement.done) {
						text += "</color></s>";
					}

					text += "</indent></size>\n";
				}
			}
		}

		textElements.text = text;

		if (scenarioManager.ScenarioCompleted) {
			SetTextTitle(true);
		}
		else {
			SetTextTitle();
		}
	}
	#endregion

	#region Action listeners
	// Action lorsqu'un event ScenarioChanged est re�u. Va mettre � jour l'affichage de la Todo list.
	private void ReceptionEventScenarioChanged() {
		DrawScenarioElements();
	}
	#endregion
}
