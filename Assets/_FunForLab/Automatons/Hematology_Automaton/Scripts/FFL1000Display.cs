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
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static HematoAutomatonController;
using static ReagentData;

/// <summary>
/// Classe qui gère l'écran d'affichage du FFL 1000 et sa partie principale.
/// </summary>
public class FFL1000Display : MonoBehaviour {
	private const float clignotementDuration = 1f;

	#region Variables UI
	// PanelTitre
	private TMP_Text textTitle;
	private TMP_Text textClock;

	// PanelMenu
	private Button boutonMenu;
	private Button boutonQC;
	private Button boutonWorklist;
	private Button boutonRule;
	private Button boutonExplorer;
	private Button boutonBrowser;
	private Button boutonSettings;

	// Panels principaux
	private Image panelContentMenu;
	private Image panelContentExplorer;
	private Image panelContentBrowser;
	private Image panelModeAnalyse;

	// PanelInfo
	private Image iconStatus;
	private TMP_Text textStatusAnalyse;
	private TMP_Text textMesure;
	private TMP_Text textTest;
	private TMP_Text textLvlWPC;
	private TMP_Text textLvlPLT;
	private TMP_Text textLvlRET;
	private TMP_Text textLvlWDF;
	private TMP_Text textLvlWNR;
	private TMP_Text textInfo;
	private Button boutonMode;
	private Button boutonLancer;
	#endregion

	private FFL1000DisplayExplorer scriptDisplayExplorer;
	private FFL1000DisplayBrowser scriptDisplayBrowser;
	private FFL1000DisplayModeAnalyse scriptModeAnalyse;
	private Coroutine iconStatusCoroutine;

	[HideInInspector]
	public List<SampleData> lastAnalysisSamplesData;

	[HideInInspector]
	public List<SampleData> samplesData;

	[HideInInspector]
	public int currentData;

	// Liste des différents menus de l'UI
	public enum DisplayMenu {
		Menu,
		QCFile,
		WorkList,
		Rule,
		SampleExplorer,
		DataBrowser,
		PatientList
	}

	public string stringTable; // Table de traduction
	public HematoAutomatonController automateFLL1000; // Référence de l'automate FFL 1000
	public DisplayMenu displayMenu; // Menu affiché au lancement

	public SampleRanges[] sampleRanges; // Ranges de valeurs qui seront utilisés

	#region Messages Unity
	private void Awake() {
		// Récupérer les autres scripts de display
		scriptDisplayExplorer = gameObject.GetComponent<FFL1000DisplayExplorer>();
		scriptDisplayBrowser = gameObject.GetComponent<FFL1000DisplayBrowser>();
		scriptModeAnalyse = gameObject.GetComponent<FFL1000DisplayModeAnalyse>();

		// PanelTitre
		textTitle = transform.Find("PanelBackground/PanelTitle/TextTitle").gameObject.GetComponent<TMP_Text>();
		textClock = transform.Find("PanelBackground/PanelTitle/TextClock").gameObject.GetComponent<TMP_Text>();

		// PanelMenu
		boutonMenu = transform.Find("PanelBackground/PanelMenu/ButtonMenu").gameObject.GetComponent<Button>();
		boutonQC = transform.Find("PanelBackground/PanelMenu/ButtonQC").gameObject.GetComponent<Button>();
		boutonWorklist = transform.Find("PanelBackground/PanelMenu/ButtonWorklist").gameObject.GetComponent<Button>();
		boutonRule = transform.Find("PanelBackground/PanelMenu/ButtonRule").gameObject.GetComponent<Button>();
		boutonExplorer = transform.Find("PanelBackground/PanelMenu/ButtonExplorer").gameObject.GetComponent<Button>();
		boutonBrowser = transform.Find("PanelBackground/PanelMenu/ButtonBrowser").gameObject.GetComponent<Button>();
		boutonSettings = transform.Find("PanelBackground/PanelMenu/ButtonSettings").gameObject.GetComponent<Button>();

		// Panels principaux
		panelContentMenu = transform.Find("PanelBackground/PanelContentMenu").gameObject.GetComponent<Image>();
		panelContentExplorer = transform.Find("PanelBackground/PanelContentExplorer").gameObject.GetComponent<Image>();
		panelContentBrowser = transform.Find("PanelBackground/PanelContentBrowser").gameObject.GetComponent<Image>();
		panelModeAnalyse = transform.Find("PanelBackground/PanelModeAnalyse").gameObject.GetComponent<Image>();

		// PanelInfo
		iconStatus = transform.Find("PanelBackground/PanelInfo/IconStatus").gameObject.GetComponent<Image>();
		textStatusAnalyse = transform.Find("PanelBackground/PanelInfo/IconStatus/TextStatusAnalyse").gameObject.GetComponent<TMP_Text>();
		textMesure = transform.Find("PanelBackground/PanelInfo/TextMesure").gameObject.GetComponent<TMP_Text>();
		textTest = transform.Find("PanelBackground/PanelInfo/TextTest").gameObject.GetComponent<TMP_Text>();
		textLvlWPC = transform.Find("PanelBackground/PanelInfo/TextLvlWPC").gameObject.GetComponent<TMP_Text>();
		textLvlPLT = transform.Find("PanelBackground/PanelInfo/TextLvlPLT").gameObject.GetComponent<TMP_Text>();
		textLvlRET = transform.Find("PanelBackground/PanelInfo/TextLvlRET").gameObject.GetComponent<TMP_Text>();
		textLvlWDF = transform.Find("PanelBackground/PanelInfo/TextLvlWDF").gameObject.GetComponent<TMP_Text>();
		textLvlWNR = transform.Find("PanelBackground/PanelInfo/TextLvlWNR").gameObject.GetComponent<TMP_Text>();
		textInfo = transform.Find("PanelBackground/PanelInfo/TextInfo").gameObject.GetComponent<TMP_Text>();
		boutonMode = transform.Find("PanelBackground/PanelInfo/ButtonMode").gameObject.GetComponent<Button>();
		boutonLancer = transform.Find("PanelBackground/PanelInfo/ButtonLancer").gameObject.GetComponent<Button>();

		// Mettre les niveaux des réactifs à vide
		NiveauxReagentEmpty(Reagent.WPC);
		NiveauxReagentEmpty(Reagent.PLT);
		NiveauxReagentEmpty(Reagent.RET);
		NiveauxReagentEmpty(Reagent.WDF);
		NiveauxReagentEmpty(Reagent.WNR);
	}

	private void Start() {
		StartCoroutine(ClockCoroutine());

		displayMenu = DisplayMenu.Menu;
		ChangeMenuActif((int) displayMenu);

		currentData = -1;
		scriptDisplayBrowser.ChangeBrowserTab(0);
		scriptDisplayBrowser.EffaceBrowser();
	}

	private void OnDisable() {
		StopAllCoroutines();
	}
	#endregion

	#region Méthodes privées
	// Va changer le message dans la barre de titre.
	private void MessageTitle(string msg) {
		textTitle.text = "FFL 1000 – " + msg;
	}
	#endregion

	#region Méthodes publiques
	// Va activer le bouton pour lancer l'analyse.
	public void ActiverBoutonLancer() {
		boutonLancer.interactable = true;
	}

	// Va désactiver le bouton pour lancer l'analyse.
	public void DesactiverBoutonLancer() {
		boutonLancer.interactable = false;
	}

	// Va activer le bouton pour changer le mode d'analyse.
	public void ActiverBoutonMode() {
		boutonMode.interactable = true;
	}

	// Va désactiver le bouton pour changer le mode d'analyse.
	public void DesactiverBoutonMode() {
		boutonMode.interactable = false;
	}

	// Va écrire un message dans la zone d'info.
	public void MessageInfo(string message) {
		textInfo.text = message;
	}

	public string GetMessageInfo() {
		return textInfo.text;
	}

	// Va changer la couleur de l'icône de status.
	public void CouleurStatus(Color32 color) {
		iconStatus.color = color;
	}

	// Va activer le clignotement de l'icône de status.
	public void ActiverClignotementStatus() {
		iconStatusCoroutine = StartCoroutine(ClignotementStatusCoroutine());
	}

	// Va désactiver le clignotement de l'icône de status.
	public void DesactiverClignotementStatus() {
		StopCoroutine(iconStatusCoroutine);
		Color c = iconStatus.color;
		c.a = 1; // Pour remettre l'alpha à 1
		iconStatus.color = c;
	}

	// Va effacer toutes les donnes se trouvant dans le menu Navigateur de Données (dans FFL1000DisplayBrowser).
	public void EffaceBrowser() {
		scriptDisplayBrowser.EffaceBrowser();
	}

	// Va ajouter les données d'un échantillon analysé.
	public void AddSampleList(SampleData data) {
		scriptDisplayExplorer.AddSampleList(data);
		currentData = samplesData.Count - 1;

		// Mettre à jour directement le Navigateur de Données quand un échantillon se fait analyser
		EffaceBrowser();
		scriptDisplayBrowser.ChangeMesure(data.modeAnalyse);
		AffichageDonneesSample();
	}

	// Va afficher toutes les données d'un échantillon dans le menu Navigateur de Données (dans FFL1000DisplayBrowser).
	public void AffichageDonneesSample() {
		scriptDisplayBrowser.AffichageDonneesSample();
	}

	// Va changer le texte du type de Mesure.
	public void SetTextMesure(string mesure) {
		textMesure.text = mesure;
	}

	// Va changer le type de mesure à effectuer (dans FFL1000DisplayBrowser).
	public void ChangeMesure(TypeMesure mesure) {
		scriptDisplayBrowser.ChangeMesure(mesure);
	}

	// Va changer le texte des tests discrets.
	public void SetTextTest(string test) {
		textTest.text = test;
	}

	// Va mettre à jour les niveaux de tous les réactifs.
	public void NiveauxReagents(ReagentData[] datas) {
		for (int i = 0; i < datas.Length; i++) {
			ReagentData reagentData = datas[i];

			if (reagentData != null) {
				NiveauReagent(reagentData);
			}
			else {
				NiveauxReagentEmpty((Reagent) i);
			}
		}
	}

	// Va mettre à jour le niveau du réactif passé.
	public void NiveauReagent(ReagentData data) {
		switch (data.reagent) {
			case Reagent.WPC:
				if (data.lvlReagent <= QUANTITE_REACTIFS_ANALYSE[0]) {
					textLvlWPC.text = "<color=#FF0000>" + data.lvlReagent + "%</color>";
				}
				else if (data.lvlReagent <= 25) {
					textLvlWPC.text = "<color=#FFFF00>" + data.lvlReagent + "%</color>";
				}
				else {
					textLvlWPC.text = "<color=#FFFFFF>" + data.lvlReagent + "%</color>";
				}

				break;
			case Reagent.PLT:
				if (data.lvlReagent <= QUANTITE_REACTIFS_ANALYSE[1]) {
					textLvlPLT.text = "<color=#FF0000>" + data.lvlReagent + "%</color>";
				}
				else if (data.lvlReagent <= 25) {
					textLvlPLT.text = "<color=#FFFF00>" + data.lvlReagent + "%</color>";
				}
				else {
					textLvlPLT.text = "<color=#FFFFFF>" + data.lvlReagent + "%</color>";
				}

				break;
			case Reagent.RET:
				if (data.lvlReagent <= QUANTITE_REACTIFS_ANALYSE[2]) {
					textLvlRET.text = "<color=#FF0000>" + data.lvlReagent + "%</color>";
				}
				else if (data.lvlReagent <= 25) {
					textLvlRET.text = "<color=#FFFF00>" + data.lvlReagent + "%</color>";
				}
				else {
					textLvlRET.text = "<color=#FFFFFF>" + data.lvlReagent + "%</color>";
				}

				break;
			case Reagent.WDF:
				if (data.lvlReagent <= QUANTITE_REACTIFS_ANALYSE[3]) {
					textLvlWDF.text = "<color=#FF0000>" + data.lvlReagent + "%</color>";
				}
				else if (data.lvlReagent <= 25) {
					textLvlWDF.text = "<color=#FFFF00>" + data.lvlReagent + "%</color>";
				}
				else {
					textLvlWDF.text = "<color=#FFFFFF>" + data.lvlReagent + "%</color>";
				}

				break;
			case Reagent.WNR:
			default:
				if (data.lvlReagent <= QUANTITE_REACTIFS_ANALYSE[4]) {
					textLvlWNR.text = "<color=#FF0000>" + data.lvlReagent + "%</color>";
				}
				else if (data.lvlReagent <= 25) {
					textLvlWNR.text = "<color=#FFFF00>" + data.lvlReagent + "%</color>";
				}
				else {
					textLvlWNR.text = "<color=#FFFFFF>" + data.lvlReagent + "%</color>";
				}

				break;
		}
	}

	// Va enlever le niveau du réagent passé.
	public void NiveauxReagentEmpty(Reagent reagent) {
		switch (reagent) {
			case Reagent.WPC:
				textLvlWPC.text = "<color=#FF0000>--%</color>";
				break;
			case Reagent.PLT:
				textLvlPLT.text = "<color=#FF0000>--%</color>";
				break;
			case Reagent.RET:
				textLvlRET.text = "<color=#FF0000>--%</color>";
				break;
			case Reagent.WDF:
				textLvlWDF.text = "<color=#FF0000>--%</color>";
				break;
			case Reagent.WNR:
				textLvlWNR.text = "<color=#FF0000>--%</color>";
				break;
		}
	}

	// Va changer le mode de fonctionnement dans l'affichage.
	public void ChangerModeFonctionnement(ModeFonctionnement mode) {
		textStatusAnalyse.text = mode switch {
			ModeFonctionnement.Manuel => "MAN.",
			_ => "AUTO",
		};
	}

	// Va rafraichir l'échantillon courant de la liste des échantillons de l'Explorateur.
	public void RefreshCurrentExplorerList() {
		scriptDisplayExplorer.RefreshRowData(currentData);
	}
	#endregion

	#region Action listeners
	// Action lorsque l'on clique sur le bouton Lancer l'analyse. Va lancer l'analyse dans l'automate FFL 1000.
	public void LancerAnalyse() {
		Debug.Log("Lancer analyse FFL 1000");

		scriptDisplayExplorer.ResetLastAnalysisSamples();

		// Désactiver le bouton Mode
		DesactiverBoutonMode();

		// Lancer l'animation du FFL 1000
		automateFLL1000.LancementAnimationAnalyse();

		// Envoyer l'event du Scénario
		//automateFLL1000.ScenarioAnalysisStartEvent(true);

		// TODO Pas très générique tout ça ...
		if (QuestLog.GetQuestState("QuestFFL1000") == QuestState.Unassigned) {
			QuestLog.SetQuestState("QuestFFL1000", QuestState.Active);
		}

		if (QuestLog.GetQuestEntryState("QuestFFL1000", 4) != QuestState.Active) {
			QuestLog.SetQuestEntryState("QuestFFL1000", 4, QuestState.Active);
		}

		Lua.Run("DisableDirectionGuide();");
		DialogueLua.SetVariable("AutomatonBusy", true);
		DialogueManager.ShowAlert("FFL 1000 : " + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "MESSAGE_LAUNCH_ANALYSIS"));
	}

	// Action lorsque l'on clique sur un bouton pour changer le menu actif. Va changer le menu actif.
	public void ChangeMenuActif(int menu) {
		boutonMenu.interactable = true;
		boutonQC.interactable = false;
		boutonWorklist.interactable = false;
		boutonRule.interactable = false;
		boutonExplorer.interactable = true;
		boutonBrowser.interactable = true;
		boutonSettings.interactable = false;

		panelContentMenu.gameObject.SetActive(false);
		panelContentExplorer.gameObject.SetActive(false);
		panelContentBrowser.gameObject.SetActive(false);

		switch ((DisplayMenu) menu) { // TODO Rajouter des menus différents, voir ce qu'il y a pour chaque menu !
			case DisplayMenu.QCFile:
				MessageTitle(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BUTTON_QC")); // TODO Le texte ici ne se met pas automatiquement à jour quand la langue change, il faut s'enregistrer à un event de Localization !
				boutonQC.interactable = false;
				break;
			case DisplayMenu.WorkList:
				MessageTitle(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BUTTON_WORKLIST"));
				boutonWorklist.interactable = false;
				break;
			case DisplayMenu.Rule:
				MessageTitle(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BUTTON_RULE"));
				boutonRule.interactable = false;
				break;
			case DisplayMenu.SampleExplorer:
				MessageTitle(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BUTTON_EXPLORER"));
				boutonExplorer.interactable = false;
				panelContentExplorer.gameObject.SetActive(true);
				break;
			case DisplayMenu.DataBrowser:
				MessageTitle(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BUTTON_BROWSER"));
				boutonBrowser.interactable = false;
				panelContentBrowser.gameObject.SetActive(true);
				break;
			case DisplayMenu.Menu:
			default:
				MessageTitle(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BUTTON_MENU"));
				boutonMenu.interactable = false;
				panelContentMenu.gameObject.SetActive(true);
				break;
		}
	}

	// Action lorsque l'on clique sur le bouton Mode (dans FFL1000DisplayModeAnalyse). Va afficher la fenêtre du choix du mode.
	public void AfficherPanelMode() {
		panelModeAnalyse.gameObject.SetActive(true);
		scriptModeAnalyse.AfficherPanelMode();
	}

	// Action lorsque l'on clique sur le bouton Annuler du panel Mode d'Analyse (dans FFL1000DisplayModeAnalyse). Va fermer la fenêtre du choix du mode sans enregistrer.
	public void AnnulerPanelMode() {
		panelModeAnalyse.gameObject.SetActive(false);
		scriptModeAnalyse.AnnulerPanelMode();
	}

	// Action lorsque l'on clique sur le bouton OK du panel Mode d'Analyse (dans FFL1000DisplayModeAnalyse). Va fermer la fenêtre du choix du mode en enregistrant les données.
	public void OKPanelMode() {
		panelModeAnalyse.gameObject.SetActive(false);
		scriptModeAnalyse.OKPanelMode();
	}
	#endregion

	#region Coroutines
	// Coroutine qui affiche la date et l'heure dans la barre de titre de l'UI.
	private IEnumerator ClockCoroutine() {
		bool alt = true; // Pour faire clignoter les : de l'heure toute les secondes

		while (enabled) {
			textClock.text = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.DateTimeFormat.GetAbbreviatedDayName(DateTime.Now.DayOfWeek) + DateTime.Now.ToString(" dd/MM/yyyy "); // Pour avoir le jour de la semaine traduit

			textClock.text += alt ? DateTime.Now.ToString("HH:mm") : DateTime.Now.ToString("HH mm");
			alt = !alt;

			yield return new WaitForSeconds(1); // Mettre à jour l'horloge toutes les secondes
		}
	}

	// Coroutine gérant le clignotement de l'icône de status.
	private IEnumerator ClignotementStatusCoroutine() {
		float timer = 0;
		bool reverse = false;

		while (enabled) {
			Color newColor = iconStatus.color;

			if (reverse) {
				newColor.a = Mathf.Lerp(0, 1, timer / clignotementDuration); // Invisible à visible
			}
			else {
				newColor.a = Mathf.Lerp(1, 0, timer / clignotementDuration); // Visible à invisible
			}

			iconStatus.color = newColor;

			timer += Time.deltaTime;

			if (timer >= clignotementDuration) {
				timer = 0;
				reverse = !reverse;
			}

			yield return null;
		}
	}
	#endregion
}
