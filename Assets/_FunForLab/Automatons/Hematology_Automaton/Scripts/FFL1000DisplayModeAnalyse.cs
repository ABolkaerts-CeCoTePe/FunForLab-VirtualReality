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
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HematoAutomatonController;

/// <summary>
/// Classe qui gère spécifiquement l'invite du Mode d'Analyse de l'écran d'affichage du FFL 1000.
/// </summary>
public class FFL1000DisplayModeAnalyse : MonoBehaviour {
	#region Variables UI
	// PanelModeAnalyse
	private Toggle toggleWB;
	private Toggle toggleLW;
	private Toggle togglePD;
	private Toggle toggleBF;
	private Toggle toggleHPC;
	private Toggle toggleHSA;
	private Toggle toggleCBC;
	private Toggle toggleDIFF;
	private Toggle toggleRET;
	private Toggle togglePLTF;
	private Toggle toggleWPC;
	#endregion

	private FFL1000Display scriptDisplay;
	private TypeMesure typeMesureChoix; // Type d'analyse sélectionné (modifier la donnée du FFL 1000 que quand on clique sur OK)
	private TypeTest[] typeTestsChoix;

	#region Messages Unity
	private void Awake() {
		scriptDisplay = gameObject.GetComponent<FFL1000Display>();

		// PanelModeAnalyse
		toggleWB = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleWB").gameObject.GetComponent<Toggle>();
		toggleLW = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleLW").gameObject.GetComponent<Toggle>();
		togglePD = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/TogglePD").gameObject.GetComponent<Toggle>();
		toggleBF = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleBF").gameObject.GetComponent<Toggle>();
		toggleHPC = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleHPC").gameObject.GetComponent<Toggle>();
		toggleHSA = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleHSA").gameObject.GetComponent<Toggle>();
		toggleCBC = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleCBC").gameObject.GetComponent<Toggle>();
		toggleDIFF = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleDIFF").gameObject.GetComponent<Toggle>();
		toggleRET = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleRET").gameObject.GetComponent<Toggle>();
		togglePLTF = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/TogglePLTF").gameObject.GetComponent<Toggle>();
		toggleWPC = transform.Find("PanelBackground/PanelModeAnalyse/PanelBack/PanelContent/ToggleWPC").gameObject.GetComponent<Toggle>();
	}

	private void Start() {
		typeMesureChoix = scriptDisplay.automateFLL1000.typeMesure;
		typeTestsChoix = scriptDisplay.automateFLL1000.typeTests;

		ChangeMesure(typeMesureChoix);
		ChangeTest(typeTestsChoix);
	}
	#endregion

	#region Méthodes publiques
	// Va changer le type de mesure à effectuer.
	public void ChangeMesure(TypeMesure mesure) {
		scriptDisplay.SetTextMesure(mesure switch {
			TypeMesure.LW => "LW",
			TypeMesure.PD => "PD",
			TypeMesure.BF => "BF",
			TypeMesure.HPC => "HPC",
			TypeMesure.hsA => "HSA",
			_ => "WB",
		});
	}

	// Va changer l'affichage du test à effectuer.
	public void ChangeTest(TypeTest[] tests) {
		string msg = string.Empty;

		foreach (TypeTest test in tests) {
			if (test == TypeTest.PLTF) {
				msg += "PLT-F";
			}
			else {
				msg += test.ToString();
			}

			msg += " ";
		}

		scriptDisplay.SetTextTest(msg);
	}

	// Va cocher directement les boutons selon ceux mémorisés (appelé depuis FFL1000Display).
	public void AfficherPanelMode() {
		// Cocher les bons boutons radios et cases à cocher
		switch (typeMesureChoix) {
			case TypeMesure.WB:
				toggleWB.isOn = true;
				break;
			case TypeMesure.LW:
				toggleLW.isOn = true;
				break;
			case TypeMesure.PD:
				togglePD.isOn = true;
				break;
			case TypeMesure.BF:
				toggleBF.isOn = true;
				break;
			case TypeMesure.HPC:
				toggleHPC.isOn = true;
				break;
			case TypeMesure.hsA:
				toggleHSA.isOn = true;
				break;
		}

		foreach (TypeTest test in typeTestsChoix) {
			switch (test) {
				case TypeTest.CBC:
					toggleCBC.isOn = true;
					Image imgCBC = toggleCBC.transform.Find("Background/Checkback").gameObject.GetComponent<Image>();
					Color c = imgCBC.color;
					c.a = 0.5f;
					imgCBC.color = c; // Rendre la checkbox aussi désactivée en la rendant transparente

					break;
				case TypeTest.DIFF:
					toggleDIFF.isOn = true;
					break;
				case TypeTest.RET:
					toggleRET.isOn = true;
					break;
				case TypeTest.PLTF:
					togglePLTF.isOn = true;
					break;
				case TypeTest.WPC:
					toggleWPC.isOn = true;
					break;
			}
		}
	}

	// Va reset les choix cochés (appelé depuis FFL1000Display).
	public void AnnulerPanelMode() {
		// Reset les choix de mesure et de tests
		typeMesureChoix = scriptDisplay.automateFLL1000.typeMesure;
		typeTestsChoix = scriptDisplay.automateFLL1000.typeTests;
	}

	// Va enregistrer les choix cochés (appelé depuis FFL1000Display).
	public void OKPanelMode() {
		List<TypeTest> tmp = new List<TypeTest>();

		if (toggleCBC.isOn) {
			tmp.Add(TypeTest.CBC);
		}

		if (toggleDIFF.isOn) {
			tmp.Add(TypeTest.DIFF);
		}

		if (toggleRET.isOn) {
			tmp.Add(TypeTest.RET);
		}

		if (togglePLTF.isOn) {
			tmp.Add(TypeTest.PLTF);
		}

		if (toggleWPC.isOn) {
			tmp.Add(TypeTest.WPC);
		}

		typeTestsChoix = tmp.ToArray();

		// Enregistrer les choix de mesure et de tests
		scriptDisplay.automateFLL1000.typeMesure = typeMesureChoix;
		scriptDisplay.automateFLL1000.typeTests = typeTestsChoix;
		ChangeMesure(typeMesureChoix);
		ChangeTest(typeTestsChoix);
	}
	#endregion

	#region Actions listeners
	// Action lorsque l'on clique sur un des boutons radios du mode d'analyse. Va cocher automatiquement les bonnes valeurs prédéfinies.
	public void ModeAnalyseRadio(Toggle sender) {
		if (sender.isOn) { // Ne le prendre en compte que s'il est coché
			TMP_Text label = sender.transform.Find("Label").gameObject.GetComponent<TMP_Text>();
			string choix = label.text.Split(' ')[0];

			Image imgCBC = toggleCBC.transform.Find("Background/Checkback").gameObject.GetComponent<Image>();
			Image imgDIFF = toggleDIFF.transform.Find("Background/Checkback").gameObject.GetComponent<Image>();
			Image imgRET = toggleRET.transform.Find("Background/Checkback").gameObject.GetComponent<Image>();
			Image imgPLTF = togglePLTF.transform.Find("Background/Checkback").gameObject.GetComponent<Image>();
			Image imgWPC = toggleWPC.transform.Find("Background/Checkback").gameObject.GetComponent<Image>();

			Color c;

			toggleDIFF.interactable = true;
			toggleRET.interactable = true;
			togglePLTF.interactable = true;
			toggleWPC.interactable = true;

			c = imgDIFF.color;
			c.a = 1f;
			imgDIFF.color = c;

			c = imgRET.color;
			c.a = 1f;
			imgRET.color = c;

			c = imgPLTF.color;
			c.a = 1f;
			imgPLTF.color = c;

			c = imgWPC.color;
			c.a = 1f;
			imgWPC.color = c;

			switch (choix) {
				case "WB":
					typeMesureChoix = TypeMesure.WB; // CBC coché par défaut et non décochable
					toggleCBC.isOn = true;

					c = imgCBC.color;
					c.a = 0.5f;
					imgCBC.color = c; // Rendre la checkbox aussi désactivée en la rendant transparente

					break;
				case "LW":
					typeMesureChoix = TypeMesure.LW; // DIFF ne peut pas être changé en LW
					toggleCBC.isOn = true;
					toggleDIFF.isOn = false; // TODO Voir si en LW DIFF est coché ou non
					toggleDIFF.interactable = false;

					c = imgDIFF.color;
					c.a = 0.5f;
					imgDIFF.color = c; // Rendre la checkbox aussi désactivée en la rendant transparente

					break;
				case "PD":
					typeMesureChoix = TypeMesure.PD; // CBC+DIFF+RET+PLTF ou CBC+DIFF ou CBC en PD
					toggleCBC.isOn = true;
					toggleWPC.isOn = false;
					toggleWPC.interactable = false;

					c = imgWPC.color;
					c.a = 0.5f;
					imgWPC.color = c;

					if (toggleRET.isOn || togglePLTF.isOn) {
						toggleRET.isOn = true;
						togglePLTF.isOn = true;
					}

					break;
				case "BF":
					typeMesureChoix = TypeMesure.BF; // Pas de tests discrets en BF
					toggleCBC.isOn = false;
					toggleDIFF.isOn = false;
					toggleRET.isOn = false;
					togglePLTF.isOn = false;
					toggleWPC.isOn = false;
					toggleDIFF.interactable = false;
					toggleRET.interactable = false;
					togglePLTF.interactable = false;
					toggleWPC.interactable = false;

					c = imgDIFF.color;
					c.a = 0.5f;
					imgDIFF.color = c;

					c = imgRET.color;
					c.a = 0.5f;
					imgRET.color = c;

					c = imgPLTF.color;
					c.a = 0.5f;
					imgPLTF.color = c;

					c = imgWPC.color;
					c.a = 0.5f;
					imgWPC.color = c;

					break;
				case "HPC":
					typeMesureChoix = TypeMesure.HPC; // Pas à faire pour le moment
					break;
				case "hsA":
					typeMesureChoix = TypeMesure.hsA; // Pas à faire pour le moment
					break;
			}
		}
	}

	// Action lorsque l'on clique sur une des checkboxs des tests discrets (uniquement pour RET et PLT-F). Va cocher ou décocher automatiquement les 2 cases si une est cochée ou non.
	public void TestDiscretRadio(Toggle sender) {
		if (typeMesureChoix == TypeMesure.PD) { // Ne le prendre en compte que si le type d'analyse est PD
			TMP_Text label = sender.transform.Find("Label").gameObject.GetComponent<TMP_Text>();
			string choix = label.text;

			if (choix == "RET") {
				if (sender.isOn && !togglePLTF.isOn) { // Faire en sorte que si on coche un des 2, cocher l'autre avec
					togglePLTF.isOn = true;
				}
				else if (!sender.isOn && togglePLTF.isOn) { // Et si on décoche un, l'autre aussi
					togglePLTF.isOn = false;
				}
			}
			else { // choix == "PLT-F"
				if (sender.isOn && !toggleRET.isOn) { // Faire en sorte que si on coche un des 2, cocher l'autre avec
					toggleRET.isOn = true;
				}
				else if (!sender.isOn && toggleRET.isOn) { // Et si on décoche un, l'autre aussi
					toggleRET.isOn = false;
				}
			}
		}
	}

	// Action lorsqu'un toggle se fait hover. Va changer sa couleur.
	public void ToggleHoverEnter(Toggle sender) {
		if (sender.interactable) { // Ne le prendre en compte que s'il est activé
			sender.gameObject.GetComponent<Image>().color = new Color32(255, 191, 127, 255);
		}
	}

	// Action lorsqu'un toggle ne se fait plus hover. Va reset sa couleur.
	public void ToggleHoverExit(Toggle sender) {
		if (sender.interactable) { // Ne le prendre en compte que s'il est activé
			sender.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
		}
	}
	#endregion
}
