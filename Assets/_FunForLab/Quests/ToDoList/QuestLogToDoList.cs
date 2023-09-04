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
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using static PixelCrushers.DialogueSystem.QuestLogWindow;

/// <summary>
/// Classe qui gère l'affichage de la ToDo List de Quest Log du Dialogue System.
/// </summary>
public class QuestLogToDoList : MonoBehaviour {
	private const int Indent = 6; // 6%
	private const string ColorCurrent = "#0000FF";
	private const string ColorDone = "#808080";

	#region Variables UI
	private TMP_Text textTitle;
	private TMP_Text textElements;
	#endregion

	public TableReference stringTable; // Table de localisation
	public string quest;

	private QuestInfo questInfo;

	private void Awake() {
		textTitle = transform.Find("PanelBackground/PanelTitle/TextTitle").gameObject.GetComponent<TMP_Text>();
		textElements = transform.Find("PanelBackground/PanelContent/ScrollViewElements/Viewport/Content").gameObject.GetComponent<TMP_Text>();
	}

	private void Start() {
		DrawScenarioElements();
	}

	#region Méthodes privées
	// Va changer le texte de la barre de titre.
	private void SetTextTitle(bool completed = false) {
		string msg = "Quest Log Todo – " + questInfo.Heading.text + " – " + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "TEXT_DIFFICULTY") + " : ";

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.Secondary) {
			msg += LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "TOGGLE_SECONDARY");
		}
		else {
			msg += LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "TOGGLE_MLT");
		}

		if (completed) {
			msg += " – " + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "DONE") + " !";
		}

		textTitle.text = msg;
	}

	// Va récupérer les informations d'une quête.
	private QuestInfo GetQuestInfo(string title) {
		DialogueSystemController.ChangeDialogueLanguage(); // Rafraichir la langue du Dialogue System

		string group = string.Empty;

		FormattedText localizedTitle = FormattedText.Parse(QuestLog.GetQuestTitle(title), DialogueManager.masterDatabase.emphasisSettings);
		FormattedText heading = localizedTitle;
		FormattedText description = FormattedText.Parse(QuestLog.GetQuestDescription(title), DialogueManager.masterDatabase.emphasisSettings);

		int entryCount = QuestLog.GetQuestEntryCount(title);
		FormattedText[] entries = new FormattedText[entryCount];
		QuestState[] entryStates = new QuestState[entryCount];

		for (int i = 0; i < entryCount; i++) {
			entries[i] = FormattedText.Parse(QuestLog.GetQuestEntry(title, i + 1), DialogueManager.masterDatabase.emphasisSettings);
			entryStates[i] = QuestLog.GetQuestEntryState(title, i + 1);
		}

		bool trackable = false;
		bool track = QuestLog.IsQuestTrackingEnabled(title);
		bool abandonable = false;

		return new QuestInfo(group, title, heading, description, entries, entryStates, trackable, track, abandonable);
	}

	// Va mettre à jour l'affichage de la Todo list.
	public void DrawScenarioElements() {
		Debug.Log("DrawScenarioElements");

		questInfo = GetQuestInfo(quest);
		string text = string.Empty;

		if (QuestLog.GetQuestState(quest) == QuestState.Unassigned) {
			Debug.Log("Quest " + quest + " unassigned !");
			text += LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "NO_CURRENT_TASKS");
		}
		else {
			for (int i = 0; i < questInfo.Entries.Length; i++) {
				text += "<b>" + (i + 1) + ".</b>";
				text += "<indent=" + Indent + "%>";

				if (questInfo.EntryStates[i] == QuestState.Success) {
					text += "<s><color=" + ColorDone + ">";
				}
				else if (questInfo.EntryStates[i] == QuestState.Active) {
					text += "<b><color=" + ColorCurrent + ">";
				}

				text += questInfo.Entries[i].text;

				if (questInfo.EntryStates[i] == QuestState.Success) {
					text += "</color></s>";
				}
				else if (questInfo.EntryStates[i] == QuestState.Active) {
					text += "</color></b>";
				}

				text += "</indent>\n";
			}
		}

		textElements.text = text;

		if (QuestLog.GetQuestState(quest) == QuestState.Success) {
			SetTextTitle(true);
		}
		else {
			SetTextTitle();
		}
	}
	#endregion
}
