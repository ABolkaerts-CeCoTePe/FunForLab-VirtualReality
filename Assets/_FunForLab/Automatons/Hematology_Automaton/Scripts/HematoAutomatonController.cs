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
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.XR.Interaction.Toolkit;
using static ReagentData;

/// <summary>
/// Classe qui va g�rer l'automate d'hemato et ses animations.
/// </summary>
public class HematoAutomatonController : MonoBehaviour {
	private static Color32 ledStatusEteinte = new Color32(12, 70, 19, 255); // #0C4613
	private static Color32 ledStatusVerte = new Color32(0, 255, 0, 255); // #00FF00
	private static Color32 ledStatusOrange = new Color32(255, 127, 0, 255); // #FFFF00
	private static Color32 ledStatusRouge = new Color32(255, 0, 0, 255); // #FF0000


	public static int[] QUANTITE_REACTIFS_ANALYSE = new int[] { 10, 10, 10, 10, 10 }; // Quantit� de r�actifs utilis�s pour une analyse (WPC, PLT, RET, WDF, WNR)

	[SerializeField]
	private HematoAutomatonData hematoAutomatonData; // Donn�es sur les niveaux des r�agents et des tests par d�faut

	private Animator animator;
	private XRSocketInteractorExtension emplacementRackInteractor;
	private Material ledStatus;
	private Coroutine ledCoroutine;
	private Color lastLedColor;
	//private bool analyseActive;
	private bool rackInserted;
	private InteractionLayerMask interactionLayerDefault; // Value = 1 pour Layer Default

	// Si on fait une analyse Automatique ou Manuelle
	public enum ModeFonctionnement {
		Automatique,
		Manuel
	}

	// Pour quel type de mesure � faire
	public enum TypeMesure {
		[Description("Whole Blood")]
		WB,
		[Description("Low WBC")]
		LW,
		[Description("Pre-Dilution")]
		PD,
		[Description("Body Fluid")]
		BF,
		[Description("H�matopoietic Progenitor Cells")]
		HPC,
		[Description("High-Sensitive Analysis")]
		hsA
	}

	// Pour quel test � effectuer
	public enum TypeTest {
		// WB, LW, PD
		CBC,
		DIFF,
		RET,
		PLTF,
		WPC,

		// BF
		WBC,
		RBC,
		WBCDiff
	}

	public InteractionLayerMask initialLayersMachineSlot; // Value = 4 pour Layer MachineSlot
	public InteractionLayerMask initialLayersFioles; // Value = 2 pour Layer FioleGrab
	public InteractionLayerMask initialLayerSlotWPC; // Value = 64 pour Layer ReagentWPC
	public InteractionLayerMask initialLayerSlotPLT; // Value = 32 pour Layer ReagentPLT
	public InteractionLayerMask initialLayerSlotRET; // Value = 128 pour Layer ReagentRET
	public InteractionLayerMask initialLayerSlotWDF; // Value = 256 pour Layer ReagentWDF
	public InteractionLayerMask initialLayerSlotWNR; // Value = 512 pour Layer ReagentWNR

	public FFL1000Display display; // Script de l'UI qui va servir � afficher et contr�ler l'automate

	public ModeFonctionnement modeFonctionnement; // Mode de fonctionnement actuel
	public TypeMesure typeMesure; // Type d'analyse s�lectionn� actuellement
	public TypeTest[] typeTests; // Tests discrets s�lectionn�s actuellement

	[HideInInspector]
	public ReagentData[] reagentDatas;

	// Si l'analyse est possible ou non (par exemple si tous les r�actifs ne sont pas pr�sents sera faux).
	public bool AnalysePossible {
		get {
			for (int i = 0; i < reagentDatas.Length; i++) {
				ReagentData reagentData = reagentDatas[i];

				if (reagentData == null || reagentData.lvlReagent < QUANTITE_REACTIFS_ANALYSE[i]) {
					return false;
				}
			}

			return true;
		}
	}

	#region Messages Unity
	private void Awake() {
		if (hematoAutomatonData != null) {
			modeFonctionnement = hematoAutomatonData.modeFonctionnement;
			typeMesure = hematoAutomatonData.typeMesure;
			typeTests = hematoAutomatonData.typeTests;
		}
		
		reagentDatas = new ReagentData[5];
		rackInserted = false;

		interactionLayerDefault = new InteractionLayerMask { value = 1 };

		animator = GetComponent<Animator>();

		XRSocketInteractorExtension[] components = GetComponentsInChildren<XRSocketInteractorExtension>();

		foreach (XRSocketInteractorExtension component in components) {
			if (component.CompareTag("EmplacementRack")) {
				emplacementRackInteractor = component;
				break;
			}
		}

		ledStatus = gameObject.transform.Find("modelisation/boutons/boutton_vert1").GetComponent<Renderer>().material;
		lastLedColor = ledStatusEteinte;
	}

	private void Start() {
		if (display != null) {
			display.ChangerModeFonctionnement(modeFonctionnement);
		}

		//analyseActive = false;
	}

	private void OnEnable() {
		ReagentSlotEvent.ReagentSlot += DetectReagent;
	}

	private void OnDisable() {
		ReagentSlotEvent.ReagentSlot -= DetectReagent;
		StopAllCoroutines();
	}
	#endregion

	#region M�thodes priv�es
	// Va d�sactiver tous les layers d'interaction du rack, de ses emplacements de fioles ainsi que des fioles qui y sont attach�s, afin de ne plus pouvoir les prendres une fois que l'animation � commenc�e.
	private void DisableLayersGrab() {
		// Le d�sactiver pour le slot et le rack
		emplacementRackInteractor.interactionLayers = interactionLayerDefault;
		emplacementRackInteractor.showInteractableHoverMeshes = false; // D�sactiver les meshs d'hover sinon elles apparaissent une fois tout d�sactiv�

		XRGrabInteractable interactable = (XRGrabInteractable) emplacementRackInteractor.firstInteractableSelected;
		interactable.interactionLayers = interactionLayerDefault;

		// Ainsi que pour les emplacements de fiole et les fioles
		XRSocketInteractorExtension[] emplacements = interactable.GetComponentsInChildren<XRSocketInteractorExtension>();

		for (int i = 0; i < emplacements.Length; i++) {
			emplacements[i].interactionLayers = interactionLayerDefault;

			XRGrabInteractable fiole = (XRGrabInteractable) emplacements[i].firstInteractableSelected;

			if (fiole != null) {
				fiole.interactionLayers = interactionLayerDefault;
			}
		}

		// Et pour les emplacements de r�actifs avec les cartouches de r�actifs
		ReagentSlotController[] reagentSlotControllers = GetComponentsInChildren<ReagentSlotController>();

		foreach (ReagentSlotController reagentSlotController in reagentSlotControllers) {
			XRSocketInteractorExtension xRSocketInteractorExtension = reagentSlotController.GetComponent<XRSocketInteractorExtension>();
			xRSocketInteractorExtension.interactionLayers = interactionLayerDefault;

			((XRGrabInteractable) xRSocketInteractorExtension.firstInteractableSelected).interactionLayers = interactionLayerDefault;
		}
	}

	// Va v�rifier tous les r�agents, activer ou non l'analyse et changer la couleur de la LED en correspondance.
	private void CheckReagents() {
		bool ledNotGreen = false;

		for (int i = 0; i < reagentDatas.Length; i++) {
			ReagentData reagentData = reagentDatas[i];

			if (reagentData != null) { // L'emplacement contient un r�actif
				if (reagentData.lvlReagent <= 0) { // Il est vide
					reagentData.lvlReagent = 0;
					LedStatusRouge();
					ledNotGreen = true;

					if (display != null) {
						display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_REAGENT_EMPTY", arguments: reagentData.reagent));
					}
				}
				else if (reagentData.lvlReagent < QUANTITE_REACTIFS_ANALYSE[i]) { // Le niveau est insuffisant pour faire une analyse
					LedStatusRouge();
					ledNotGreen = true;

					if (display != null) {
						display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_REAGENT_INSUFFICIENT", arguments: reagentData.reagent));
					}
				}
				else if (reagentData.lvlReagent <= 25) { // Le niveau est bas mais suffisant
					LedStatusOrange();
					ledNotGreen = true;
				}
			}
			else { // L'emplacement de r�actif est vide, la cartouche n'est pas pr�sente
				LedStatusRouge();
				ledNotGreen = true;

				if (display != null) {
					display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_SLOT_REAGENT_EMPTY", arguments: (Reagent) i));
				}
			}
		}

		if (!ledNotGreen) { // Le niveau est correct
			if (rackInserted) {
				LedStatusVert();
			}
			else {
				EteindreLedStatus();

				if (display != null) {
					display.CouleurStatus(ledStatusVerte);
				}
			}
		}

		if (display != null) {
			if (AnalysePossible) {
				if (rackInserted) {
					display.ActiverBoutonLancer();
					display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_READY_TO_START"));
				}
				else {
					display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_FFL1000_READY"));
				}
			}
			else {
				display.DesactiverBoutonLancer();
			}
		}
	}
	#endregion

	#region M�thodes publiques
	// Va activer l'animation pour l'analyse de l'emplacement du rack.
	public void LancementAnimationAnalyse() {
		Debug.Log("Lancement animation rack hemato automaton");

		if (display != null) {
			display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_LAUNCH_ANALYSIS"));
			display.DesactiverBoutonLancer();
		}

		StartCoroutine(DelayAnimationCoroutine("RackEmplacement", 2)); // Attendre un d�lai de 2 secondes avant de continuer
	}

	// Va r�sactiver tous les layers d'interaction du rack, de ses emplacements de fioles ainsi que des fioles qui y sont attach�s.
	public void ResetLayersGrab() {
		// Le r�activer pour le slot et le rack
		emplacementRackInteractor.interactionLayers = initialLayersMachineSlot;
		emplacementRackInteractor.showInteractableHoverMeshes = true;

		XRGrabInteractable interactable = (XRGrabInteractable) emplacementRackInteractor.firstInteractableSelected;
		interactable.interactionLayers = initialLayersMachineSlot;

		// Ainsi que pour les emplacements de fiole et les fioles
		XRSocketInteractorExtension[] emplacements = interactable.GetComponentsInChildren<XRSocketInteractorExtension>();

		for (int i = 0; i < emplacements.Length; i++) {
			emplacements[i].interactionLayers = initialLayersFioles;

			XRGrabInteractable fiole = (XRGrabInteractable) emplacements[i].firstInteractableSelected;

			if (fiole != null) {
				fiole.interactionLayers = initialLayersFioles;
			}
		}

		// Et pour les emplacements de r�actifs avec les cartouches de r�actifs
		ReagentSlotController[] reagentSlotControllers = GetComponentsInChildren<ReagentSlotController>();

		foreach (ReagentSlotController reagentSlotController in reagentSlotControllers) {
			XRSocketInteractorExtension xRSocketInteractorExtension = reagentSlotController.GetComponent<XRSocketInteractorExtension>();

			switch (reagentSlotController.reagentType) {
				case Reagent.WPC:
					xRSocketInteractorExtension.interactionLayers = initialLayerSlotWPC;
					((XRGrabInteractable) xRSocketInteractorExtension.firstInteractableSelected).interactionLayers = initialLayerSlotWPC;
					break;
				case Reagent.RET:
					xRSocketInteractorExtension.interactionLayers = initialLayerSlotRET;
					((XRGrabInteractable) xRSocketInteractorExtension.firstInteractableSelected).interactionLayers = initialLayerSlotRET;
					break;
				case Reagent.PLT:
					xRSocketInteractorExtension.interactionLayers = initialLayerSlotPLT;
					((XRGrabInteractable) xRSocketInteractorExtension.firstInteractableSelected).interactionLayers = initialLayerSlotPLT;
					break;
				case Reagent.WDF:
					xRSocketInteractorExtension.interactionLayers = initialLayerSlotWDF;
					((XRGrabInteractable) xRSocketInteractorExtension.firstInteractableSelected).interactionLayers = initialLayerSlotWDF;
					break;
				case Reagent.WNR:
					xRSocketInteractorExtension.interactionLayers = initialLayerSlotWNR;
					((XRGrabInteractable) xRSocketInteractorExtension.firstInteractableSelected).interactionLayers = initialLayerSlotWNR;
					break;
			}
		}

		if (QuestLog.GetQuestEntryState("QuestFFL1000", 1) == QuestState.Success && QuestLog.GetQuestEntryState("QuestFFL1000", 2) == QuestState.Success) {
			DialogueLua.SetVariable("AnalysisDone", true);
			QuestLog.SetQuestEntryState("QuestFFL1000", 4, QuestState.Success);
		}

		DialogueLua.SetVariable("AutomatonBusy", false);
	}

	// Va allumer la led de status ainsi que l'ic�ne de status de l'UI en vert.
	public void LedStatusVert() {
		ledStatus.color = ledStatusVerte;
		lastLedColor = ledStatusVerte;

		if (display != null) {
			display.CouleurStatus(ledStatusVerte);
		}
	}

	// Va allumer la led de status ainsi que l'ic�ne de status de l'UI en orange.
	public void LedStatusOrange() {
		ledStatus.color = ledStatusOrange;
		lastLedColor = ledStatusOrange;

		if (display != null) {
			display.CouleurStatus(ledStatusOrange);
		}
	}

	// Va allumer la led de status ainsi que l'ic�ne de status de l'UI en rouge.
	public void LedStatusRouge() {
		ledStatus.color = ledStatusRouge;
		lastLedColor = ledStatusRouge;

		if (display != null) {
			display.CouleurStatus(ledStatusRouge);
		}
	}

	// Va �teindre la led de status.
	public void EteindreLedStatus() {
		ledStatus.color = ledStatusEteinte;
	}

	// Va activer le clignotement de la led de status.
	public void ActiverClignotementLedStatus() {
		ledCoroutine = StartCoroutine(ClignotementLedStatusCoroutine());
	}

	// Va d�sactiver le clignotement de la led de status.
	public void DesactiverClignotementLedStatus() {
		StopCoroutine(ledCoroutine);
		ledStatus.color = lastLedColor; // Pour rallumer la led une fois fini
	}

	public Color ActualLedColor() {
		return ledStatus.color;
	}

	// Va diminuer les niveaux des r�actifs.
	public void ConsumeReagents(int[] levels) {
		if (levels.Length != 5) {
			return;
		}

		if (reagentDatas[0] != null) {
			reagentDatas[0].lvlReagent -= levels[0]; // Comme on a les r�f�rences des ReagentData, en le modifiant ici on le modifie aussi dans ReagentController !

			if (reagentDatas[0].lvlReagent < 0) {
				reagentDatas[0].lvlReagent = 0;
			}
		}

		if (reagentDatas[1] != null) {
			reagentDatas[1].lvlReagent -= levels[1];

			if (reagentDatas[1].lvlReagent < 0) {
				reagentDatas[1].lvlReagent = 0;
			}
		}

		if (reagentDatas[2] != null) {
			reagentDatas[2].lvlReagent -= levels[2];

			if (reagentDatas[2].lvlReagent < 0) {
				reagentDatas[2].lvlReagent = 0;
			}
		}

		if (reagentDatas[3] != null) {
			reagentDatas[3].lvlReagent -= levels[3];

			if (reagentDatas[3].lvlReagent < 0) {
				reagentDatas[3].lvlReagent = 0;
			}
		}

		if (reagentDatas[4] != null) {
			reagentDatas[4].lvlReagent -= levels[4];

			if (reagentDatas[4].lvlReagent < 0) {
				reagentDatas[4].lvlReagent = 0;
			}
		}
	}

	// Va envoyer un event avec code pour le Sc�nario.
	/*public void ScenarioSendEvent(string code, bool statut) {
		//ScenarioCodeEvent.SendCodeEvent(code, statut);
	}*/

	// Va appeler ScenarioSendEvent avec le code FFL1000_ANALYSIS_START.
	/*public void ScenarioAnalysisStartEvent(bool statut) {
		//ScenarioSendEvent(ScenarioCodeAnalysisStart, statut);
	}*/
	#endregion

	#region Action listeners
	// Action lorsqu'un rack est ins�r� dans la machine. Va activer le bouton pour lancer l'analyse dans l'UI.
	public void ActiverBoutonLancer() {
		rackInserted = true;
		CheckReagents();

		if (AnalysePossible && display != null) {
			display.ActiverBoutonLancer();
			display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_READY_TO_START"));
		}
	}

	// Action lorsqu'un rack est retir� de la machine. Va reset l'animation de l'analyse du rack, et donc remettre l'emplacement � sa position d'origine.
	public void ResetAnimationAnalyse() {
		Debug.Log("Fin animation rack FFL 1000");

		EteindreLedStatus();

		if (display != null) {
			display.MessageInfo(LocalizationSettings.StringDatabase.GetLocalizedString(display.stringTable, "MESSAGE_RACK_REMOVED"));
			display.ActiverBoutonMode(); // R�activer le bouton Mode
		}

		// Reset le param�tre des animations
		animator.SetInteger("RackEmplacement", 0);

		// Attendre 2 secondes avant de d�sactiver l'animator
		StartCoroutine(DisableAnimatorCoroutine());
	}

	// Action lorsqu'un rack est retir� de la machine. Va d�sactiver le bouton pour lancer l'analyse dans l'UI.
	public void DesactiverBoutonLancer() {
		rackInserted = false;

		if (display != null) {
			display.DesactiverBoutonLancer();
		}
	}

	// Action lorsqu'un rack est soit ins�r� soit r�tir� de la machine. Va appeler ScenarioSendEvent avec le code RACK_IN_FFL1000.
	public void ScenarioRackInEvent(bool statut) {
		/*if (analyseActive) {
			analyseActive = false; // Ne pas envoyer l'event du rack retir� si l'analyse a �t� effectu�e
		}
		else {
			ScenarioSendEvent(ScenarioCodeRackIn, statut);
		}*/
		if (!DialogueLua.GetVariable("AnalysisDone").asBool) {
			if (statut) {
				QuestLog.SetQuestEntryState("QuestFFL1000", 3, QuestState.Success); // Voir s'il n'y a pas moyen d'avoir un composant � part pour faire cela
				//DialogueManager.ShowAlert("Rack positionn� dans l'automate FFL 1000 !");
			}
			else {
				QuestLog.SetQuestEntryState("QuestFFL1000", 3, QuestState.Active);
				//DialogueManager.ShowAlert("Rack retir� de l'automate FFL 1000 !");
			}
		}
	}

	// Action lors de la r�ception d'un event ReagentSlotEvent. Va stocker le r�actif et mettre � jour l'affichage.
	private void DetectReagent(Reagent reagent, ReagentData reagentData) {
		if (reagentData != null) {
			reagentDatas[(int) reagent] = reagentData;

			if (display != null) {
				display.NiveauReagent(reagentData);
			}
		}
		else {
			reagentDatas[(int) reagent] = null;

			if (display != null) {
				display.NiveauxReagentEmpty(reagent);
			}
		}

		CheckReagents();
	}
	#endregion

	#region Coroutines
	// Coroutine qui va attendre un certain nombre de secondes avant d'activer l'animation.
	private IEnumerator DelayAnimationCoroutine(string trigger, float seconds) {
		DisableLayersGrab();

		yield return new WaitForSeconds(seconds);

		animator.enabled = true;
		animator.SetInteger(trigger, 1);
		//analyseActive = true;

		ActiverClignotementLedStatus();

		if (display != null) {
			display.ActiverClignotementStatus();
		}
	}

	// Coroutine qui va attendre 2 secondes avant de d�sactiver l'animator, sinon l'animation de reset n'avait pas le temps de se finir.
	private IEnumerator DisableAnimatorCoroutine() {
		yield return new WaitForSeconds(2);
		CheckReagents();
		animator.enabled = false;
	}

	// Coroutine qui va g�rer le clignotement de la led de status.
	private IEnumerator ClignotementLedStatusCoroutine() {
		bool reverse = false;

		while (enabled) {
			if (reverse) {
				EteindreLedStatus();
			}
			else {
				ledStatus.color = lastLedColor;
			}

			reverse = !reverse;

			yield return new WaitForSeconds(1);
		}
	}
	#endregion
}

/// <summary>
/// Classe qui va permettre de faire l'event pour les r�actifs.
/// </summary>
public static class ReagentSlotEvent {
	public delegate void ReagentSlotEventDelegate(Reagent reagent, ReagentData reagentData);
	public static event ReagentSlotEventDelegate ReagentSlot;

	// Va invoquer l'event.
	public static void SendReagentSlotEvent(Reagent reagent, ReagentData reagentData) {
		ReagentSlot?.Invoke(reagent, reagentData);
	}
}
