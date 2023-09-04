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
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static HematoAutomatonController;

/// <summary>
/// Classe qui gère spécifiquement le Navigateur de Données de l'écran d'affichage du FFL 1000.
/// </summary>
public class FFL1000DisplayBrowser : MonoBehaviour {
	private const string donneeVide = "----";
	private const int SizeQCPresent = 75;

	#region Variables UI
	// PanelContentBrowser -> PanelTop
	private Button buttonValidate;
	private TMP_Text textModeAnalyse;
	private TMP_Text textNumSample;
	private TMP_Text textDateSample;
	private TMP_Text textEmplacementSample;
	private TMP_Text textCommentSample;
	private TMP_Text textHospitalPatient;
	private TMP_Text textDoctorPatient;
	private TMP_Text textIDPatient;
	private TMP_Text textDateSexeAgePatient;
	private TMP_Text textNamePatient;
	private TMP_Text textCommentPatient;

	// PanelContentBrowser -> PanelTabs
	private Button boutonTabMain;
	private Button boutonTabMessages;
	private Button boutonTabGraph;

	// PanelContentBrowser -> PanelTypes
	private Image panelTypeAnalyse;

	// PanelContentBrowser -> Panels
	private Image panelBrowserData;
	private Image panelBrowserMessages;
	private Image panelBrowserGraphs;

	// PanelContentBrowser -> Panels -> Datas
	private TMP_Text textCBC;
	private TMP_Text textDIFF;
	private TMP_Text textRET;
	private TMP_Text textPLTF;
	private TMP_Text textTitleWBCFlags;
	private TMP_Text textTitleRBCFlags;
	private TMP_Text textTitlePLTFlags;
	private TMP_Text textTitleWBCBF;
	private TMP_Text textTitleRBCBF;
	private TMP_Text textWBCDiff;
	private Image tableCBC;
	private Image tableDIFF;
	private Image tableRET;
	private Image tablePLTF;
	private Image scrollViewTitleWBCFlags;
	private Image scrollViewTitleRBCFlags;
	private Image scrollViewTitlePLTFlags;
	private Image tableWBCBF;
	private Image tableRBCBF;
	private Image tableWBCDiff;

	// PanelContentBrowser -> PanelData -> DataCBC -> PanelCBC1
	private TMP_Text textWBC;
	private TMP_Text textRBC;
	private TMP_Text textHGB;
	private TMP_Text textHCT;
	private TMP_Text textMCV;
	private TMP_Text textMCH;
	private TMP_Text textMCHC;
	private TMP_Text textPLT;
	private TMP_Text textSignWBC;
	private TMP_Text textSignRBC;
	private TMP_Text textSignHGB;
	private TMP_Text textSignHCT;
	private TMP_Text textSignMCV;
	private TMP_Text textSignMCH;
	private TMP_Text textSignMCHC;
	private TMP_Text textSignPLT;

	// PanelContentBrowser -> PanelData -> DataCBC -> PanelCBC2
	private TMP_Text textRDWSD;
	private TMP_Text textRDWCV;
	private TMP_Text textPDW;
	private TMP_Text textMPV;
	private TMP_Text textPLCR;
	private TMP_Text textPCT;
	private TMP_Text textSignRDWSD;
	private TMP_Text textSignRDWCV;
	private TMP_Text textSignPDW;
	private TMP_Text textSignMPV;
	private TMP_Text textSignPLCR;
	private TMP_Text textSignPCT;

	// PanelContentBrowser -> PanelData -> DataCBC -> PanelCBC3
	private TMP_Text textNRBCH;
	private TMP_Text textNRBCP;
	private TMP_Text textSignNRBCH;
	private TMP_Text textSignNRBCP;

	// PanelContentBrowser -> PanelData -> DataDIFF -> PanelDIFF1
	private TMP_Text textNEUTH;
	private TMP_Text textLYMPHH;
	private TMP_Text textMONOH;
	private TMP_Text textEOH;
	private TMP_Text textBASOH;
	private TMP_Text textSignNEUTH;
	private TMP_Text textSignLYMPHH;
	private TMP_Text textSignMONOH;
	private TMP_Text textSignEOH;
	private TMP_Text textSignBASOH;

	// PanelContentBrowser -> PanelData -> DataDIFF -> PanelDIFF2
	private TMP_Text textNEUTP;
	private TMP_Text textLYMPHP;
	private TMP_Text textMONOP;
	private TMP_Text textEOP;
	private TMP_Text textBASOP;
	private TMP_Text textSignNEUTP;
	private TMP_Text textSignLYMPHP;
	private TMP_Text textSignMONOP;
	private TMP_Text textSignEOP;
	private TMP_Text textSignBASOP;

	// PanelContentBrowser -> PanelData -> DataDIFF -> PanelDIFF3
	private TMP_Text textIGH;
	private TMP_Text textIGP;
	private TMP_Text textSignIGH;
	private TMP_Text textSignIGP;

	// PanelContentBrowser -> PanelData -> DataRET -> PanelRET
	private TMP_Text textRETP;
	private TMP_Text textRETH;
	private TMP_Text textIRF;
	private TMP_Text textLFR;
	private TMP_Text textMFR;
	private TMP_Text textHFR;
	private TMP_Text textRETHE;
	private TMP_Text textSignRETP;
	private TMP_Text textSignRETH;
	private TMP_Text textSignIRF;
	private TMP_Text textSignLFR;
	private TMP_Text textSignMFR;
	private TMP_Text textSignHFR;
	private TMP_Text textSignRETHE;

	// PanelContentBrowser -> PanelData -> DataPLTF -> PanelPLTF
	private TMP_Text textIPF;
	private TMP_Text textSignIPF;

	// PanelContentBrowser -> PanelData -> ScrollViews
	private TMP_Text textWBCFlags;
	private TMP_Text textRBCFlags;
	private TMP_Text textPLTFlags;

	// PanelContentBrowser -> PanelData -> DataWBC -> PanelWBC
	private TMP_Text textWBCBF;
	private TMP_Text textSignWBCBF;

	// PanelContentBrowser -> PanelData -> DataRBC -> PanelRBC
	private TMP_Text textRBCBF;
	private TMP_Text textSignRBCBF;

	// PanelContentBrowser -> PanelData -> DataWBCDiff -> PanelWBCDiff1
	private TMP_Text textMNH;
	private TMP_Text textPMNH;
	private TMP_Text textSignMNH;
	private TMP_Text textSignPMNH;

	// PanelContentBrowser -> PanelData -> DataWBCDiff -> PanelWBCDiff2
	private TMP_Text textMNP;
	private TMP_Text textPMNP;
	private TMP_Text textSignMNP;
	private TMP_Text textSignPMNP;

	// PanelContentBrowser -> PanelData -> DataWBCDiff -> PanelWBCDiff3
	private TMP_Text textTCBFH;
	private TMP_Text textSignTCBFH;

	// PanelContentBrowser -> PanelMessages
	private TMP_Text textActions;
	private TMP_Text textErrorsRules;

	// PanelContentBrowser -> PanelGraphs
	private Image panelGraphWDFEXT;
	private Image panelGraphWNR;
	private Image panelGraphWPC;
	private Image panelGraphRET;
	private Image panelGraphPLTF;
	private Image panelGraphPLT;
	#endregion

	private Color32 colorValidated = new Color32(0, 127, 255, 255);
	private Color32 colorNoValidated = new Color32(127, 127, 127, 255);
	private Color32 colorRed = new Color32(255, 0, 0, 255);
	private Color32 colorBlack = new Color32(0, 0, 0, 255);

	private FFL1000Display scriptDisplay;

	private SampleRanges actualSampleRanges;

	#region Messages Unity
	private void Awake() {
		scriptDisplay = gameObject.GetComponent<FFL1000Display>();

		// PanelContentBrowser -> PanelTop
		buttonValidate = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/ButtonValidate").gameObject.GetComponent<Button>();
		textModeAnalyse = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoSample/TextModeAnalyse").gameObject.GetComponent<TMP_Text>();
		textNumSample = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoSample/TextNumSample").gameObject.GetComponent<TMP_Text>();
		textDateSample = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoSample/TextDateSample").gameObject.GetComponent<TMP_Text>();
		textEmplacementSample = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoSample/TextEmplacementSample").gameObject.GetComponent<TMP_Text>();
		textCommentSample = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoSample/TextCommentSample").gameObject.GetComponent<TMP_Text>();
		textHospitalPatient = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoPatient/TextHospitalPatient").gameObject.GetComponent<TMP_Text>();
		textDoctorPatient = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoPatient/TextDoctorPatient").gameObject.GetComponent<TMP_Text>();
		textIDPatient = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoPatient/TextIDPatient").gameObject.GetComponent<TMP_Text>();
		textDateSexeAgePatient = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoPatient/TextDateSexeAgePatient").gameObject.GetComponent<TMP_Text>();
		textNamePatient = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoPatient/TextNamePatient").gameObject.GetComponent<TMP_Text>();
		textCommentPatient = transform.Find("PanelBackground/PanelContentBrowser/PanelTop/PanelInfoPatient/TextCommentPatient").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelTabs
		boutonTabMain = transform.Find("PanelBackground/PanelContentBrowser/PanelTabs/ButtonTabMain").gameObject.GetComponent<Button>();
		boutonTabMessages = transform.Find("PanelBackground/PanelContentBrowser/PanelTabs/ButtonTabMessages").gameObject.GetComponent<Button>();
		boutonTabGraph = transform.Find("PanelBackground/PanelContentBrowser/PanelTabs/ButtonTabGraph").gameObject.GetComponent<Button>();

		// PanelContentBrowser -> PanelTypes
		panelTypeAnalyse = transform.Find("PanelBackground/PanelContentBrowser/PanelTypes/PanelTypeAnalyse").gameObject.GetComponent<Image>();

		// PanelContentBrowser -> Panels
		panelBrowserData = transform.Find("PanelBackground/PanelContentBrowser/PanelData").gameObject.GetComponent<Image>();
		panelBrowserMessages = transform.Find("PanelBackground/PanelContentBrowser/PanelMessages").gameObject.GetComponent<Image>();
		panelBrowserGraphs = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs").gameObject.GetComponent<Image>();

		// PanelContentBrowser -> Panels -> Datas
		textCBC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextCBC").gameObject.GetComponent<TMP_Text>();
		textDIFF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextDIFF").gameObject.GetComponent<TMP_Text>();
		textRET = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextRET").gameObject.GetComponent<TMP_Text>();
		textPLTF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextPLTF").gameObject.GetComponent<TMP_Text>();
		textTitleWBCFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextWBCFlags").gameObject.GetComponent<TMP_Text>();
		textTitleRBCFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextRBCFlags").gameObject.GetComponent<TMP_Text>();
		textTitlePLTFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextPLTFlags").gameObject.GetComponent<TMP_Text>();
		textTitleWBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextWBC").gameObject.GetComponent<TMP_Text>();
		textTitleRBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextRBC").gameObject.GetComponent<TMP_Text>();
		textWBCDiff = transform.Find("PanelBackground/PanelContentBrowser/PanelData/TextWBCDiff").gameObject.GetComponent<TMP_Text>();
		tableCBC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC").gameObject.GetComponent<Image>();
		tableDIFF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF").gameObject.GetComponent<Image>();
		tableRET = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET").gameObject.GetComponent<Image>();
		tablePLTF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataPLTF").gameObject.GetComponent<Image>();
		scrollViewTitleWBCFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/ScrollViewWBCFlags").gameObject.GetComponent<Image>();
		scrollViewTitleRBCFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/ScrollViewRBCFlags").gameObject.GetComponent<Image>();
		scrollViewTitlePLTFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/ScrollViewPLTFlags").gameObject.GetComponent<Image>();
		tableWBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBC").gameObject.GetComponent<Image>();
		tableRBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRBC").gameObject.GetComponent<Image>();
		tableWBCDiff = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff").gameObject.GetComponent<Image>();

		// PanelContentBrowser -> PanelData -> DataCBC -> PanelCBC1
		textWBC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextWBC").gameObject.GetComponent<TMP_Text>();
		textRBC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextRBC").gameObject.GetComponent<TMP_Text>();
		textHGB = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextHGB").gameObject.GetComponent<TMP_Text>();
		textHCT = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextHCT").gameObject.GetComponent<TMP_Text>();
		textMCV = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextMCV").gameObject.GetComponent<TMP_Text>();
		textMCH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextMCH").gameObject.GetComponent<TMP_Text>();
		textMCHC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextMCHC").gameObject.GetComponent<TMP_Text>();
		textPLT = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextPLT").gameObject.GetComponent<TMP_Text>();
		textSignWBC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignWBC").gameObject.GetComponent<TMP_Text>();
		textSignRBC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignRBC").gameObject.GetComponent<TMP_Text>();
		textSignHGB = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignHGB").gameObject.GetComponent<TMP_Text>();
		textSignHCT = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignHCT").gameObject.GetComponent<TMP_Text>();
		textSignMCV = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignMCV").gameObject.GetComponent<TMP_Text>();
		textSignMCH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignMCH").gameObject.GetComponent<TMP_Text>();
		textSignMCHC = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignMCHC").gameObject.GetComponent<TMP_Text>();
		textSignPLT = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC1/GridElementBody/TextSignPLT").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataCBC -> PanelCBC2
		textRDWSD = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextRDWSD").gameObject.GetComponent<TMP_Text>();
		textRDWCV = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextRDWCV").gameObject.GetComponent<TMP_Text>();
		textPDW = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextPDW").gameObject.GetComponent<TMP_Text>();
		textMPV = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextMPV").gameObject.GetComponent<TMP_Text>();
		textPLCR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextPLCR").gameObject.GetComponent<TMP_Text>();
		textPCT = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextPCT").gameObject.GetComponent<TMP_Text>();
		textSignRDWSD = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextSignRDWSD").gameObject.GetComponent<TMP_Text>();
		textSignRDWCV = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextSignRDWCV").gameObject.GetComponent<TMP_Text>();
		textSignPDW = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextSignPDW").gameObject.GetComponent<TMP_Text>();
		textSignMPV = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextSignMPV").gameObject.GetComponent<TMP_Text>();
		textSignPLCR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextSignPLCR").gameObject.GetComponent<TMP_Text>();
		textSignPCT = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC2/GridElementBody/TextSignPCT").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataCBC -> PanelCBC3
		textNRBCH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC3/GridElementBody/TextNRBCH").gameObject.GetComponent<TMP_Text>();
		textNRBCP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC3/GridElementBody/TextNRBCP").gameObject.GetComponent<TMP_Text>();
		textSignNRBCH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC3/GridElementBody/TextSignNRBCH").gameObject.GetComponent<TMP_Text>();
		textSignNRBCP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataCBC/PanelCBC3/GridElementBody/TextSignNRBCP").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataDIFF -> PanelDIFF1
		textNEUTH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextNEUTH").gameObject.GetComponent<TMP_Text>();
		textLYMPHH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextLYMPHH").gameObject.GetComponent<TMP_Text>();
		textMONOH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextMONOH").gameObject.GetComponent<TMP_Text>();
		textEOH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextEOH").gameObject.GetComponent<TMP_Text>();
		textBASOH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextBASOH").gameObject.GetComponent<TMP_Text>();
		textSignNEUTH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextSignNEUTH").gameObject.GetComponent<TMP_Text>();
		textSignLYMPHH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextSignLYMPHH").gameObject.GetComponent<TMP_Text>();
		textSignMONOH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextSignMONOH").gameObject.GetComponent<TMP_Text>();
		textSignEOH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextSignEOH").gameObject.GetComponent<TMP_Text>();
		textSignBASOH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF1/GridElementBody/TextSignBASOH").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataDIFF -> PanelDIFF2
		textNEUTP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextNEUTP").gameObject.GetComponent<TMP_Text>();
		textLYMPHP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextLYMPHP").gameObject.GetComponent<TMP_Text>();
		textMONOP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextMONOP").gameObject.GetComponent<TMP_Text>();
		textEOP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextEOP").gameObject.GetComponent<TMP_Text>();
		textBASOP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextBASOP").gameObject.GetComponent<TMP_Text>();
		textSignNEUTP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextSignNEUTP").gameObject.GetComponent<TMP_Text>();
		textSignLYMPHP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextSignLYMPHP").gameObject.GetComponent<TMP_Text>();
		textSignMONOP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextSignMONOP").gameObject.GetComponent<TMP_Text>();
		textSignEOP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextSignEOP").gameObject.GetComponent<TMP_Text>();
		textSignBASOP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF2/GridElementBody/TextSignBASOP").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataDIFF -> PanelDIFF3
		textIGH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF3/GridElementBody/TextIGH").gameObject.GetComponent<TMP_Text>();
		textIGP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF3/GridElementBody/TextIGP").gameObject.GetComponent<TMP_Text>();
		textSignIGH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF3/GridElementBody/TextSignIGH").gameObject.GetComponent<TMP_Text>();
		textSignIGP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataDIFF/PanelDIFF3/GridElementBody/TextSignIGP").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataRET -> PanelRET
		textRETP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextRETP").gameObject.GetComponent<TMP_Text>();
		textRETH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextRETH").gameObject.GetComponent<TMP_Text>();
		textIRF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextIRF").gameObject.GetComponent<TMP_Text>();
		textLFR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextLFR").gameObject.GetComponent<TMP_Text>();
		textMFR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextMFR").gameObject.GetComponent<TMP_Text>();
		textHFR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextHFR").gameObject.GetComponent<TMP_Text>();
		textRETHE = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextRETHE").gameObject.GetComponent<TMP_Text>();
		textSignRETP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignRETP").gameObject.GetComponent<TMP_Text>();
		textSignRETH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignRETH").gameObject.GetComponent<TMP_Text>();
		textSignIRF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignIRF").gameObject.GetComponent<TMP_Text>();
		textSignLFR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignLFR").gameObject.GetComponent<TMP_Text>();
		textSignMFR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignMFR").gameObject.GetComponent<TMP_Text>();
		textSignHFR = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignHFR").gameObject.GetComponent<TMP_Text>();
		textSignRETHE = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRET/PanelRET/GridElementBody/TextSignRETHE").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataPLTF -> PanelPLTF
		textIPF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataPLTF/PanelPLTF/GridElementBody/TextIPF").gameObject.GetComponent<TMP_Text>();
		textSignIPF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataPLTF/PanelPLTF/GridElementBody/TextSignIPF").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> ScrollViews
		textWBCFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/ScrollViewWBCFlags/Viewport/TextWBCFlags").gameObject.GetComponent<TMP_Text>();
		textRBCFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/ScrollViewRBCFlags/Viewport/TextRBCFlags").gameObject.GetComponent<TMP_Text>();
		textPLTFlags = transform.Find("PanelBackground/PanelContentBrowser/PanelData/ScrollViewPLTFlags/Viewport/TextPLTFlags").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataWBC -> PanelWBC
		textWBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBC/PanelWBC/GridElementBody/TextWBCBF").gameObject.GetComponent<TMP_Text>();
		textSignWBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBC/PanelWBC/GridElementBody/TextSignWBCBF").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataRBC -> PanelRBC
		textRBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRBC/PanelRBC/GridElementBody/TextRBCBF").gameObject.GetComponent<TMP_Text>();
		textSignRBCBF = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataRBC/PanelRBC/GridElementBody/TextSignRBCBF").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataWBCDiff -> PanelWBCDiff1
		textMNH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff1/GridElementBody/TextMNH").gameObject.GetComponent<TMP_Text>();
		textPMNH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff1/GridElementBody/TextPMNH").gameObject.GetComponent<TMP_Text>();
		textSignMNH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff1/GridElementBody/TextSignMNH").gameObject.GetComponent<TMP_Text>();
		textSignPMNH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff1/GridElementBody/TextSignPMNH").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataWBCDiff -> PanelWBCDiff2
		textMNP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff2/GridElementBody/TextMNP").gameObject.GetComponent<TMP_Text>();
		textPMNP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff2/GridElementBody/TextPMNP").gameObject.GetComponent<TMP_Text>();
		textSignMNP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff2/GridElementBody/TextSignMNP").gameObject.GetComponent<TMP_Text>();
		textSignPMNP = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff2/GridElementBody/TextSignPMNP").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelData -> DataWBCDiff -> PanelWBCDiff3
		textTCBFH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff3/GridElementBody/TextTCBFH").gameObject.GetComponent<TMP_Text>();
		textSignTCBFH = transform.Find("PanelBackground/PanelContentBrowser/PanelData/DataWBCDiff/PanelWBCDiff3/GridElementBody/TextSignTCBFH").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelMessages
		textActions = transform.Find("PanelBackground/PanelContentBrowser/PanelMessages/ScrollViewActions/Viewport/TextActions").gameObject.GetComponent<TMP_Text>();
		textErrorsRules = transform.Find("PanelBackground/PanelContentBrowser/PanelMessages/ScrollViewErrorsRules/Viewport/TextErrorsRules").gameObject.GetComponent<TMP_Text>();

		// PanelContentBrowser -> PanelGraphs
		panelGraphWDFEXT = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs/PanelGraphWDFEXT").gameObject.GetComponent<Image>();
		panelGraphWNR = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs/PanelGraphWNR").gameObject.GetComponent<Image>();
		panelGraphWPC = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs/PanelGraphWPC").gameObject.GetComponent<Image>();
		panelGraphRET = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs/PanelGraphRET").gameObject.GetComponent<Image>();
		panelGraphPLTF = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs/PanelGraphPLTF").gameObject.GetComponent<Image>();
		panelGraphPLT = transform.Find("PanelBackground/PanelContentBrowser/PanelGraphs/PanelGraphRBCPLT/PanelGraphPLT").gameObject.GetComponent<Image>();
	}

	private void Start() {
		ChangeBrowserTab(0);
		EffaceBrowser();
		ResetBoutonValider();

		// Afficher les flags uniquement si on est en difficulté TLM
		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT) {
			textTitleWBCFlags.gameObject.SetActive(true);
			textTitleRBCFlags.gameObject.SetActive(true);
			textTitlePLTFlags.gameObject.SetActive(true);
			scrollViewTitleWBCFlags.gameObject.SetActive(true);
			scrollViewTitleRBCFlags.gameObject.SetActive(true);
			scrollViewTitlePLTFlags.gameObject.SetActive(true);
		}
		else {
			textTitleWBCFlags.gameObject.SetActive(false);
			textTitleRBCFlags.gameObject.SetActive(false);
			textTitlePLTFlags.gameObject.SetActive(false);
			scrollViewTitleWBCFlags.gameObject.SetActive(false);
			scrollViewTitleRBCFlags.gameObject.SetActive(false);
			scrollViewTitlePLTFlags.gameObject.SetActive(false);
		}
	}
	#endregion

	#region Méthodes privées
	// Va afficher les données CBC d'un échantillon.
	private void AffichageDonneesSampleCBC(SampleData data) {
		// CBC 1
		textWBC.text = string.Format("{0,7:0.00}", data.wBC); // Format pour avoir 4 emplacements d'unités, 1 point, et 2 décimales
		textRBC.text = string.Format("{0,7:0.00}", data.rBC);
		textHGB.text = string.Format("{0,7:0.0}", data.hGB);
		textHCT.text = string.Format("{0,7:0.0}", data.hCT);
		textMCV.text = string.Format("{0,7:0.0}", data.mCV);
		textMCH.text = string.Format("{0,7:0.0}", data.mCH);
		textMCHC.text = string.Format("{0,7:0.0}", data.mCHC);
		textPLT.text = string.Format("{0,7}", data.pLT);

		textWBC.color = colorBlack;
		textRBC.color = colorBlack;
		textHGB.color = colorBlack;
		textHCT.color = colorBlack;
		textMCV.color = colorBlack;
		textMCH.color = colorBlack;
		textMCHC.color = colorBlack;
		textPLT.color = colorBlack;

		textSignWBC.text = string.Empty;
		textSignRBC.text = string.Empty;
		textSignHGB.text = string.Empty;
		textSignHCT.text = string.Empty;
		textSignMCV.text = string.Empty;
		textSignMCH.text = string.Empty;
		textSignMCHC.text = string.Empty;
		textSignPLT.text = string.Empty;

		textSignWBC.color = colorBlack;
		textSignRBC.color = colorBlack;
		textSignHGB.color = colorBlack;
		textSignHCT.color = colorBlack;
		textSignMCV.color = colorBlack;
		textSignMCH.color = colorBlack;
		textSignMCHC.color = colorBlack;
		textSignPLT.color = colorBlack;

		// Afficher les ranges de valeurs uniquement si on est en difficulté TLM
		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) { // La zone à droite des données avec + ou - indique si la valeur est plus grande ou plus petite que le range normal
			if (actualSampleRanges.wBC_min != -1 && data.wBC < actualSampleRanges.wBC_min) {
				textSignWBC.text = "-";
				textWBC.color = colorRed;
				textSignWBC.color = colorRed;
			}
			else if (actualSampleRanges.wBC_max != -1 && data.wBC > actualSampleRanges.wBC_max) {
				textSignWBC.text = "+";
				textWBC.color = colorRed;
				textSignWBC.color = colorRed;
			}
			else {
				textSignWBC.text = string.Empty;
				textWBC.color = colorBlack;
				textSignWBC.color = colorBlack;
			}

			if (actualSampleRanges.rBC_min != -1 && data.rBC < actualSampleRanges.rBC_min) {
				textSignRBC.text = "-";
				textRBC.color = colorRed;
				textSignRBC.color = colorRed;
			}
			else if (actualSampleRanges.rBC_max != -1 && data.wBC > actualSampleRanges.rBC_max) {
				textSignRBC.text = "+";
				textRBC.color = colorRed;
				textSignRBC.color = colorRed;
			}
			else {
				textSignRBC.text = string.Empty;
				textRBC.color = colorBlack;
				textSignRBC.color = colorBlack;
			}

			if (actualSampleRanges.hGB_min != -1 && data.hGB < actualSampleRanges.hGB_min) {
				textSignHGB.text = "-";
				textHGB.color = colorRed;
				textSignHGB.color = colorRed;
			}
			else if (actualSampleRanges.hGB_max != -1 && data.wBC > actualSampleRanges.hGB_max) {
				textSignHGB.text = "+";
				textHGB.color = colorRed;
				textSignHGB.color = colorRed;
			}
			else {
				textSignHGB.text = string.Empty;
				textHGB.color = colorBlack;
				textSignHGB.color = colorBlack;
			}

			if (actualSampleRanges.hCT_min != -1 && data.hCT < actualSampleRanges.hCT_min) {
				textSignHCT.text = "-";
				textHCT.color = colorRed;
				textSignHCT.color = colorRed;
			}
			else if (actualSampleRanges.hCT_max != -1 && data.wBC > actualSampleRanges.hCT_max) {
				textSignHCT.text = "+";
				textHCT.color = colorRed;
				textSignHCT.color = colorRed;
			}
			else {
				textSignHCT.text = string.Empty;
				textHCT.color = colorBlack;
				textSignHCT.color = colorBlack;
			}

			if (actualSampleRanges.mCV_min != -1 && data.mCV < actualSampleRanges.mCV_min) {
				textSignMCV.text = "-";
				textMCV.color = colorRed;
				textSignMCV.color = colorRed;
			}
			else if (actualSampleRanges.mCV_max != -1 && data.wBC > actualSampleRanges.mCV_max) {
				textSignMCV.text = "+";
				textMCV.color = colorRed;
				textSignMCV.color = colorRed;
			}
			else {
				textSignMCV.text = string.Empty;
				textMCV.color = colorBlack;
				textSignMCV.color = colorBlack;
			}

			if (actualSampleRanges.mCH_min != -1 && data.mCH < actualSampleRanges.mCH_min) {
				textSignMCH.text = "-";
				textMCH.color = colorRed;
				textSignMCH.color = colorRed;
			}
			else if (actualSampleRanges.mCH_max != -1 && data.wBC > actualSampleRanges.mCH_max) {
				textSignMCH.text = "+";
				textMCH.color = colorRed;
				textSignMCH.color = colorRed;
			}
			else {
				textSignMCH.text = string.Empty;
				textMCH.color = colorBlack;
				textSignMCH.color = colorBlack;
			}

			if (actualSampleRanges.mCHC_min != -1 && data.mCHC < actualSampleRanges.mCHC_min) {
				textSignMCHC.text = "-";
				textMCHC.color = colorRed;
				textSignMCHC.color = colorRed;
			}
			else if (actualSampleRanges.mCHC_max != -1 && data.wBC > actualSampleRanges.mCHC_max) {
				textSignMCHC.text = "+";
				textMCHC.color = colorRed;
				textSignMCHC.color = colorRed;
			}
			else {
				textSignMCHC.text = string.Empty;
				textMCHC.color = colorBlack;
				textSignMCHC.color = colorBlack;
			}

			if (actualSampleRanges.pLT_min != -1 && data.pLT < actualSampleRanges.pLT_min) {
				textSignPLT.text = "-";
				textPLT.color = colorRed;
				textSignPLT.color = colorRed;
			}
			else if (actualSampleRanges.pLT_max != -1 && data.wBC > actualSampleRanges.pLT_max) {
				textSignPLT.text = "+";
				textPLT.color = colorRed;
				textSignPLT.color = colorRed;
			}
			else {
				textSignPLT.text = string.Empty;
				textPLT.color = colorBlack;
				textSignPLT.color = colorBlack;
			}
		}

		// CBC 2
		textRDWSD.text = string.Format("{0,7:0.0}", data.rDWSD);
		textRDWCV.text = string.Format("{0,7:0.0}", data.rDWCV);
		textPDW.text = string.Format("{0,7:0.0}", data.pDW);
		textMPV.text = string.Format("{0,7:0.0}", data.mPV);
		textPLCR.text = string.Format("{0,7:0.0}", data.pLCR);
		textPCT.text = string.Format("{0,7:0.00}", data.pCT);

		textRDWSD.color = colorBlack;
		textRDWCV.color = colorBlack;
		textPDW.color = colorBlack;
		textMPV.color = colorBlack;
		textPLCR.color = colorBlack;
		textPCT.color = colorBlack;

		textSignRDWSD.text = string.Empty;
		textSignRDWCV.text = string.Empty;
		textSignPDW.text = string.Empty;
		textSignMPV.text = string.Empty;
		textSignPLCR.text = string.Empty;
		textSignPCT.text = string.Empty;

		textSignRDWSD.color = colorBlack;
		textSignRDWCV.color = colorBlack;
		textSignPDW.color = colorBlack;
		textSignMPV.color = colorBlack;
		textSignPLCR.color = colorBlack;
		textSignPCT.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.rDWSD_min != -1 && data.rDWSD < actualSampleRanges.rDWSD_min) {
				textSignRDWSD.text = "-";
				textRDWSD.color = colorRed;
				textSignRDWSD.color = colorRed;
			}
			else if (actualSampleRanges.rDWSD_max != -1 && data.rDWSD > actualSampleRanges.rDWSD_max) {
				textSignRDWSD.text = "+";
				textRDWSD.color = colorRed;
				textSignRDWSD.color = colorRed;
			}
			else {
				textSignRDWSD.text = string.Empty;
				textRDWSD.color = colorBlack;
				textSignRDWSD.color = colorBlack;
			}

			if (actualSampleRanges.rDWCV_min != -1 && data.rDWCV < actualSampleRanges.rDWCV_min) {
				textSignRDWCV.text = "-";
				textRDWCV.color = colorRed;
				textSignRDWCV.color = colorRed;
			}
			else if (actualSampleRanges.rDWCV_max != -1 && data.rDWCV > actualSampleRanges.rDWCV_max) {
				textSignRDWCV.text = "+";
				textRDWCV.color = colorRed;
				textSignRDWCV.color = colorRed;
			}
			else {
				textSignRDWCV.text = string.Empty;
				textRDWCV.color = colorBlack;
				textSignRDWCV.color = colorBlack;
			}

			if (actualSampleRanges.pDW_min != -1 && data.pDW < actualSampleRanges.pDW_min) {
				textSignPDW.text = "-";
				textPDW.color = colorRed;
				textSignPDW.color = colorRed;
			}
			else if (actualSampleRanges.pDW_max != -1 && data.pDW > actualSampleRanges.pDW_max) {
				textSignPDW.text = "+";
				textPDW.color = colorRed;
				textSignPDW.color = colorRed;
			}
			else {
				textSignPDW.text = string.Empty;
				textPDW.color = colorBlack;
				textSignPDW.color = colorBlack;
			}

			if (actualSampleRanges.mPV_min != -1 && data.mPV < actualSampleRanges.mPV_min) {
				textSignMPV.text = "-";
				textMPV.color = colorRed;
				textSignMPV.color = colorRed;
			}
			else if (actualSampleRanges.mPV_max != -1 && data.mPV > actualSampleRanges.mPV_max) {
				textSignMPV.text = "+";
				textMPV.color = colorRed;
				textSignMPV.color = colorRed;
			}
			else {
				textSignMPV.text = string.Empty;
				textMPV.color = colorBlack;
				textSignMPV.color = colorBlack;
			}

			if (actualSampleRanges.pLCR_min != -1 && data.pLCR < actualSampleRanges.pLCR_min) {
				textSignPLCR.text = "-";
				textPLCR.color = colorRed;
				textSignPLCR.color = colorRed;
			}
			else if (actualSampleRanges.pLCR_max != -1 && data.pLCR > actualSampleRanges.pLCR_max) {
				textSignPLCR.text = "+";
				textPLCR.color = colorRed;
				textSignPLCR.color = colorRed;
			}
			else {
				textSignPLCR.text = string.Empty;
				textPLCR.color = colorBlack;
				textSignPLCR.color = colorBlack;
			}

			if (actualSampleRanges.pCT_min != -1 && data.pCT < actualSampleRanges.pCT_min) {
				textSignPCT.text = "-";
				textPCT.color = colorRed;
				textSignPCT.color = colorRed;
			}
			else if (actualSampleRanges.pCT_max != -1 && data.pCT > actualSampleRanges.pCT_max) {
				textSignPCT.text = "+";
				textPCT.color = colorRed;
				textSignPCT.color = colorRed;
			}
			else {
				textSignPCT.text = string.Empty;
				textPCT.color = colorBlack;
				textSignPCT.color = colorBlack;
			}
		}

		// CBC 3
		textNRBCH.text = string.Format("{0,7:0.00}", data.nRBCH);
		textNRBCP.text = string.Format("{0,7:0.0}", data.nRBCP);

		textNRBCH.color = colorBlack;
		textNRBCP.color = colorBlack;

		textSignNRBCH.text = string.Empty;
		textSignNRBCP.text = string.Empty;

		textSignNRBCH.color = colorBlack;
		textSignNRBCP.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.nRBCH_min != -1 && data.nRBCH < actualSampleRanges.nRBCH_min) {
				textSignNRBCH.text = "-";
				textNRBCH.color = colorRed;
				textSignNRBCH.color = colorRed;
			}
			else if (actualSampleRanges.nRBCH_max != -1 && data.nRBCH > actualSampleRanges.nRBCH_max) {
				textSignNRBCH.text = "+";
				textNRBCH.color = colorRed;
				textSignNRBCH.color = colorRed;
			}
			else {
				textSignNRBCH.text = string.Empty;
				textNRBCH.color = colorBlack;
				textSignNRBCH.color = colorBlack;
			}

			if (actualSampleRanges.nRBCP_min != -1 && data.nRBCP < actualSampleRanges.nRBCP_min) {
				textSignNRBCP.text = "-";
				textNRBCP.color = colorRed;
				textSignNRBCP.color = colorRed;
			}
			else if (actualSampleRanges.nRBCP_max != -1 && data.nRBCP > actualSampleRanges.nRBCP_max) {
				textSignNRBCP.text = "+";
				textNRBCP.color = colorRed;
				textSignNRBCP.color = colorRed;
			}
			else {
				textSignNRBCP.text = string.Empty;
				textNRBCP.color = colorBlack;
				textSignNRBCP.color = colorBlack;
			}
		}
	}

	// Va afficher les données DIFF d'un échantillon.
	private void AffichageDonneesSampleDIFF(SampleData data) {
		// DIFF 1
		textNEUTH.text = string.Format("{0,7:0.00}", data.neutH);
		textLYMPHH.text = string.Format("{0,7:0.00}", data.lymphH);
		textMONOH.text = string.Format("{0,7:0.00}", data.monoH);
		textEOH.text = string.Format("{0,7:0.00}", data.eoH);
		textBASOH.text = string.Format("{0,7:0.00}", data.basoH);

		textNEUTH.color = colorBlack;
		textLYMPHH.color = colorBlack;
		textMONOH.color = colorBlack;
		textEOH.color = colorBlack;
		textBASOH.color = colorBlack;

		textSignNEUTH.text = string.Empty;
		textSignLYMPHH.text = string.Empty;
		textSignMONOH.text = string.Empty;
		textSignEOH.text = string.Empty;
		textSignBASOH.text = string.Empty;

		textSignNEUTH.color = colorBlack;
		textSignLYMPHH.color = colorBlack;
		textSignMONOH.color = colorBlack;
		textSignEOH.color = colorBlack;
		textSignBASOH.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.neutH_min != -1 && data.neutH < actualSampleRanges.neutH_min) {
				textSignNEUTH.text = "-";
				textNEUTH.color = colorRed;
				textSignNEUTH.color = colorRed;
			}
			else if (actualSampleRanges.neutH_max != -1 && data.neutH > actualSampleRanges.neutH_max) {
				textSignNEUTH.text = "+";
				textNEUTH.color = colorRed;
				textSignNEUTH.color = colorRed;
			}
			else {
				textSignNEUTH.text = string.Empty;
				textNEUTH.color = colorBlack;
				textSignNEUTH.color = colorBlack;
			}

			if (actualSampleRanges.lymphH_min != -1 && data.lymphH < actualSampleRanges.lymphH_min) {
				textSignLYMPHH.text = "-";
				textLYMPHH.color = colorRed;
				textSignLYMPHH.color = colorRed;
			}
			else if (actualSampleRanges.lymphH_max != -1 && data.lymphH > actualSampleRanges.lymphH_max) {
				textSignLYMPHH.text = "+";
				textLYMPHH.color = colorRed;
				textSignLYMPHH.color = colorRed;
			}
			else {
				textSignLYMPHH.text = string.Empty;
				textLYMPHH.color = colorBlack;
				textSignLYMPHH.color = colorBlack;
			}

			if (actualSampleRanges.monoH_min != -1 && data.monoH < actualSampleRanges.monoH_min) {
				textSignMONOH.text = "-";
				textMONOH.color = colorRed;
				textSignMONOH.color = colorRed;
			}
			else if (actualSampleRanges.monoH_max != -1 && data.monoH > actualSampleRanges.monoH_max) {
				textSignMONOH.text = "+";
				textMONOH.color = colorRed;
				textSignMONOH.color = colorRed;
			}
			else {
				textSignMONOH.text = string.Empty;
				textMONOH.color = colorBlack;
				textSignMONOH.color = colorBlack;
			}

			if (actualSampleRanges.eoH_min != -1 && data.eoH < actualSampleRanges.eoH_min) {
				textSignEOH.text = "-";
				textEOH.color = colorRed;
				textSignEOH.color = colorRed;
			}
			else if (actualSampleRanges.eoH_max != -1 && data.eoH > actualSampleRanges.eoH_max) {
				textSignEOH.text = "+";
				textEOH.color = colorRed;
				textSignEOH.color = colorRed;
			}
			else {
				textSignEOH.text = string.Empty;
				textEOH.color = colorBlack;
				textSignEOH.color = colorBlack;
			}

			if (actualSampleRanges.basoH_min != -1 && data.basoH < actualSampleRanges.basoH_min) {
				textSignBASOH.text = "-";
				textBASOH.color = colorRed;
				textSignBASOH.color = colorRed;
			}
			else if (actualSampleRanges.basoH_max != -1 && data.basoH > actualSampleRanges.basoH_max) {
				textSignBASOH.text = "+";
				textBASOH.color = colorRed;
				textSignBASOH.color = colorRed;
			}
			else {
				textSignBASOH.text = string.Empty;
				textBASOH.color = colorBlack;
				textSignBASOH.color = colorBlack;
			}
		}

		// DIFF 2
		textNEUTP.text = string.Format("{0,7:0.0}", data.neutP);
		textLYMPHP.text = string.Format("{0,7:0.0}", data.lymphP);
		textMONOP.text = string.Format("{0,7:0.0}", data.monoP);
		textEOP.text = string.Format("{0,7:0.0}", data.eoP);
		textBASOP.text = string.Format("{0,7:0.0}", data.basoP);

		textNEUTP.color = colorBlack;
		textLYMPHP.color = colorBlack;
		textMONOP.color = colorBlack;
		textEOP.color = colorBlack;
		textBASOP.color = colorBlack;

		textSignNEUTP.text = string.Empty;
		textSignLYMPHP.text = string.Empty;
		textSignMONOP.text = string.Empty;
		textSignEOP.text = string.Empty;
		textSignBASOP.text = string.Empty;

		textSignNEUTP.color = colorBlack;
		textSignLYMPHP.color = colorBlack;
		textSignMONOP.color = colorBlack;
		textSignEOP.color = colorBlack;
		textSignBASOP.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.neutP_min != -1 && data.neutP < actualSampleRanges.neutP_min) {
				textSignNEUTP.text = "-";
				textNEUTP.color = colorRed;
				textSignNEUTP.color = colorRed;
			}
			else if (actualSampleRanges.neutP_max != -1 && data.neutP > actualSampleRanges.neutP_max) {
				textSignNEUTP.text = "+";
				textNEUTP.color = colorRed;
				textSignNEUTP.color = colorRed;
			}
			else {
				textSignNEUTP.text = string.Empty;
				textNEUTP.color = colorBlack;
				textSignNEUTP.color = colorBlack;
			}

			if (actualSampleRanges.lymphP_min != -1 && data.lymphP < actualSampleRanges.lymphP_min) {
				textSignLYMPHP.text = "-";
				textLYMPHP.color = colorRed;
				textSignLYMPHP.color = colorRed;
			}
			else if (actualSampleRanges.lymphP_max != -1 && data.lymphP > actualSampleRanges.lymphP_max) {
				textSignLYMPHP.text = "+";
				textLYMPHP.color = colorRed;
				textSignLYMPHP.color = colorRed;
			}
			else {
				textSignLYMPHP.text = string.Empty;
				textLYMPHP.color = colorBlack;
				textSignLYMPHP.color = colorBlack;
			}

			if (actualSampleRanges.monoP_min != -1 && data.monoP < actualSampleRanges.monoP_min) {
				textSignMONOP.text = "-";
				textMONOP.color = colorRed;
				textSignMONOP.color = colorRed;
			}
			else if (actualSampleRanges.monoP_max != -1 && data.monoP > actualSampleRanges.monoP_max) {
				textSignMONOP.text = "+";
				textMONOP.color = colorRed;
				textSignMONOP.color = colorRed;
			}
			else {
				textSignMONOP.text = string.Empty;
				textMONOP.color = colorBlack;
				textSignMONOP.color = colorBlack;
			}

			if (actualSampleRanges.eoP_min != -1 && data.eoP < actualSampleRanges.eoP_min) {
				textSignEOP.text = "-";
				textEOP.color = colorRed;
				textSignEOP.color = colorRed;
			}
			else if (actualSampleRanges.eoP_max != -1 && data.eoP > actualSampleRanges.eoP_max) {
				textSignEOP.text = "+";
				textEOP.color = colorRed;
				textSignEOP.color = colorRed;
			}
			else {
				textSignEOP.text = string.Empty;
				textEOP.color = colorBlack;
				textSignEOP.color = colorBlack;
			}

			if (actualSampleRanges.basoP_min != -1 && data.basoP < actualSampleRanges.basoP_min) {
				textSignBASOP.text = "-";
				textBASOP.color = colorRed;
				textSignBASOP.color = colorRed;
			}
			else if (actualSampleRanges.basoP_max != -1 && data.basoP > actualSampleRanges.basoP_max) {
				textSignBASOP.text = "+";
				textBASOP.color = colorRed;
				textSignBASOP.color = colorRed;
			}
			else {
				textSignBASOP.text = string.Empty;
				textBASOP.color = colorBlack;
				textSignBASOP.color = colorBlack;
			}
		}

		// DIFF 3
		textIGH.text = string.Format("{0,7:0.00}", data.iGH);
		textIGP.text = string.Format("{0,7:0.0}", data.iGP);

		textIGH.color = colorBlack;
		textIGP.color = colorBlack;

		textSignIGH.text = string.Empty;
		textSignIGP.text = string.Empty;

		textSignIGH.color = colorBlack;
		textSignIGP.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.iGH_min != -1 && data.iGH < actualSampleRanges.iGH_min) {
				textSignIGH.text = "-";
				textIGH.color = colorRed;
				textSignIGH.color = colorRed;
			}
			else if (actualSampleRanges.iGH_max != -1 && data.iGH > actualSampleRanges.iGH_max) {
				textSignIGH.text = "+";
				textIGH.color = colorRed;
				textSignIGH.color = colorRed;
			}
			else {
				textSignIGH.text = string.Empty;
				textIGH.color = colorBlack;
				textSignIGH.color = colorBlack;
			}

			if (actualSampleRanges.iGP_min != -1 && data.iGP < actualSampleRanges.iGP_min) {
				textSignIGP.text = "-";
				textIGP.color = colorRed;
				textSignIGP.color = colorRed;
			}
			else if (actualSampleRanges.iGP_max != -1 && data.iGP > actualSampleRanges.iGP_max) {
				textSignIGP.text = "+";
				textIGP.color = colorRed;
				textSignIGP.color = colorRed;
			}
			else {
				textSignIGP.text = string.Empty;
				textIGP.color = colorBlack;
				textSignIGP.color = colorBlack;
			}
		}
	}

	// Va afficher les données RET d'un échantillon.
	private void AffichageDonneesSampleRET(SampleData data) {
		// RET
		textRETP.text = string.Format("{0,7:0.00}", data.rETP);
		textRETH.text = string.Format("{0,7:0.0000}", data.rETH);
		textIRF.text = string.Format("{0,7:0.0}", data.iRF);
		textLFR.text = string.Format("{0,7:0.0}", data.lFR);
		textMFR.text = string.Format("{0,7:0.0}", data.mFR);
		textHFR.text = string.Format("{0,7:0.0}", data.hFR);
		textRETHE.text = string.Format("{0,7:0.0}", data.rETHe);

		textRETP.color = colorBlack;
		textRETH.color = colorBlack;
		textIRF.color = colorBlack;
		textLFR.color = colorBlack;
		textMFR.color = colorBlack;
		textHFR.color = colorBlack;
		textRETHE.color = colorBlack;

		textSignRETP.text = string.Empty;
		textSignRETH.text = string.Empty;
		textSignIRF.text = string.Empty;
		textSignLFR.text = string.Empty;
		textSignMFR.text = string.Empty;
		textSignHFR.text = string.Empty;
		textSignRETHE.text = string.Empty;

		textSignRETP.color = colorBlack;
		textSignRETH.color = colorBlack;
		textSignIRF.color = colorBlack;
		textSignLFR.color = colorBlack;
		textSignMFR.color = colorBlack;
		textSignHFR.color = colorBlack;
		textSignRETHE.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.rETP_min != -1 && data.rETP < actualSampleRanges.rETP_min) {
				textSignRETP.text = "-";
				textRETP.color = colorRed;
				textSignRETP.color = colorRed;
			}
			else if (actualSampleRanges.rETP_max != -1 && data.rETP > actualSampleRanges.rETP_max) {
				textSignRETP.text = "+";
				textRETP.color = colorRed;
				textSignRETP.color = colorRed;
			}
			else {
				textSignRETP.text = string.Empty;
				textRETP.color = colorBlack;
				textSignRETP.color = colorBlack;
			}

			if (actualSampleRanges.rETH_min != -1 && data.rETH < actualSampleRanges.rETH_min) {
				textSignRETH.text = "-";
				textRETH.color = colorRed;
				textSignRETH.color = colorRed;
			}
			else if (actualSampleRanges.rETH_max != -1 && data.rETH > actualSampleRanges.rETH_max) {
				textSignRETH.text = "+";
				textRETH.color = colorRed;
				textSignRETH.color = colorRed;
			}
			else {
				textSignRETH.text = string.Empty;
				textRETH.color = colorBlack;
				textSignRETH.color = colorBlack;
			}

			if (actualSampleRanges.iRF_min != -1 && data.iRF < actualSampleRanges.iRF_min) {
				textSignIRF.text = "-";
				textIRF.color = colorRed;
				textSignIRF.color = colorRed;
			}
			else if (actualSampleRanges.iRF_max != -1 && data.iRF > actualSampleRanges.iRF_max) {
				textSignIRF.text = "+";
				textIRF.color = colorRed;
				textSignIRF.color = colorRed;
			}
			else {
				textSignIRF.text = string.Empty;
				textIRF.color = colorBlack;
				textSignIRF.color = colorBlack;
			}

			if (actualSampleRanges.lFR_min != -1 && data.lFR < actualSampleRanges.lFR_min) {
				textSignLFR.text = "-";
				textLFR.color = colorRed;
				textSignLFR.color = colorRed;
			}
			else if (actualSampleRanges.lFR_max != -1 && data.lFR > actualSampleRanges.lFR_max) {
				textSignLFR.text = "+";
				textLFR.color = colorRed;
				textSignLFR.color = colorRed;
			}
			else {
				textSignLFR.text = string.Empty;
				textLFR.color = colorBlack;
				textSignLFR.color = colorBlack;
			}

			if (actualSampleRanges.mFR_min != -1 && data.mFR < actualSampleRanges.mFR_min) {
				textSignMFR.text = "-";
				textMFR.color = colorRed;
				textSignMFR.color = colorRed;
			}
			else if (actualSampleRanges.mFR_max != -1 && data.mFR > actualSampleRanges.mFR_max) {
				textSignMFR.text = "+";
				textMFR.color = colorRed;
				textSignMFR.color = colorRed;
			}
			else {
				textSignMFR.text = string.Empty;
				textMFR.color = colorBlack;
				textSignMFR.color = colorBlack;
			}

			if (actualSampleRanges.hFR_min != -1 && data.hFR < actualSampleRanges.hFR_min) {
				textSignHFR.text = "-";
				textHFR.color = colorRed;
				textSignHFR.color = colorRed;
			}
			else if (actualSampleRanges.hFR_max != -1 && data.hFR > actualSampleRanges.hFR_max) {
				textSignHFR.text = "+";
				textHFR.color = colorRed;
				textSignHFR.color = colorRed;
			}
			else {
				textSignHFR.text = string.Empty;
				textHFR.color = colorBlack;
				textSignHFR.color = colorBlack;
			}

			if (actualSampleRanges.rETHe_min != -1 && data.rETHe < actualSampleRanges.rETHe_min) {
				textSignRETHE.text = "-";
				textRETHE.color = colorRed;
				textSignRETHE.color = colorRed;
			}
			else if (actualSampleRanges.rETHe_max != -1 && data.rETHe > actualSampleRanges.rETHe_max) {
				textSignRETHE.text = "+";
				textRETHE.color = colorRed;
				textSignRETHE.color = colorRed;
			}
			else {
				textSignRETHE.text = string.Empty;
				textRETHE.color = colorBlack;
				textSignRETHE.color = colorBlack;
			}
		}
	}

	// Va afficher les données PLT-F d'un échantillon.
	private void AffichageDonneesSamplePLTF(SampleData data) {
		// PLTF
		textIPF.text = string.Format("{0,7:0.0}", data.iPF);

		textIPF.color = colorBlack;

		textSignIPF.text = string.Empty;

		textSignIPF.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.iPF_min != -1 && data.iPF < actualSampleRanges.iPF_min) {
				textSignIPF.text = "-";
				textIPF.color = colorRed;
				textSignIPF.color = colorRed;
			}
			else if (actualSampleRanges.iPF_max != -1 && data.iPF > actualSampleRanges.iPF_max) {
				textSignIPF.text = "+";
				textIPF.color = colorRed;
				textSignIPF.color = colorRed;
			}
			else {
				textSignIPF.text = string.Empty;
				textIPF.color = colorBlack;
				textSignIPF.color = colorBlack;
			}
		}
	}

	// Va afficher les données WBC d'un échantillon.
	private void AffichageDonneesSampleWBC(SampleData data) {
		// WBC
		textWBCBF.text = string.Format("{0,7:0.000}", data.wBCBF);

		textWBCBF.color = colorBlack;

		textSignWBCBF.text = string.Empty;

		textSignWBCBF.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.wBCBF_min != -1 && data.wBCBF < actualSampleRanges.wBCBF_min) {
				textSignWBCBF.text = "-";
				textWBCBF.color = colorRed;
				textSignWBCBF.color = colorRed;
			}
			else if (actualSampleRanges.wBCBF_max != -1 && data.wBCBF > actualSampleRanges.wBCBF_max) {
				textSignWBCBF.text = "+";
				textWBCBF.color = colorRed;
				textSignWBCBF.color = colorRed;
			}
			else {
				textSignWBCBF.text = string.Empty;
				textWBCBF.color = colorBlack;
				textSignWBCBF.color = colorBlack;
			}
		}
	}

	// Va afficher les données RBC d'un échantillon.
	private void AffichageDonneesSampleRBC(SampleData data) {
		// RBC
		textRBCBF.text = string.Format("{0,7:0.000}", data.rBCBF);

		textRBCBF.color = colorBlack;

		textSignRBCBF.text = string.Empty;

		textSignRBCBF.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.rBCBF_min != -1 && data.rBCBF < actualSampleRanges.rBCBF_min) {
				textSignRBCBF.text = "-";
				textRBCBF.color = colorRed;
				textSignRBCBF.color = colorRed;
			}
			else if (actualSampleRanges.rBCBF_max != -1 && data.rBCBF > actualSampleRanges.rBCBF_max) {
				textSignRBCBF.text = "+";
				textRBCBF.color = colorRed;
				textSignRBCBF.color = colorRed;
			}
			else {
				textSignRBCBF.text = string.Empty;
				textRBCBF.color = colorBlack;
				textSignRBCBF.color = colorBlack;
			}
		}
	}

	// Va afficher les données WBC Diff d'un échantillon.
	private void AffichageDonneesSampleWBCDiff(SampleData data) {
		// WBC Diff 1
		textMNH.text = string.Format("{0,7:0.000}", data.mNH);
		textPMNH.text = string.Format("{0,7:0.000}", data.pMNH);

		textMNH.color = colorBlack;
		textPMNH.color = colorBlack;

		textSignMNH.text = string.Empty;
		textSignPMNH.text = string.Empty;

		textSignMNH.color = colorBlack;
		textSignPMNH.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.mNH_min != -1 && data.mNH < actualSampleRanges.mNH_min) {
				textSignMNH.text = "-";
				textMNH.color = colorRed;
				textSignMNH.color = colorRed;
			}
			else if (actualSampleRanges.mNH_max != -1 && data.mNH > actualSampleRanges.mNH_max) {
				textSignMNH.text = "+";
				textMNH.color = colorRed;
				textSignMNH.color = colorRed;
			}
			else {
				textSignMNH.text = string.Empty;
				textMNH.color = colorBlack;
				textSignMNH.color = colorBlack;
			}

			if (actualSampleRanges.pMNH_min != -1 && data.pMNH < actualSampleRanges.pMNH_min) {
				textSignPMNH.text = "-";
				textPMNH.color = colorRed;
				textSignPMNH.color = colorRed;
			}
			else if (actualSampleRanges.pMNH_max != -1 && data.pMNH > actualSampleRanges.pMNH_max) {
				textSignPMNH.text = "+";
				textPMNH.color = colorRed;
				textSignPMNH.color = colorRed;
			}
			else {
				textSignPMNH.text = string.Empty;
				textPMNH.color = colorBlack;
				textSignPMNH.color = colorBlack;
			}
		}

		// WBC Diff 2
		textMNP.text = string.Format("{0,7:0.0}", data.mNP);
		textPMNP.text = string.Format("{0,7:0.0}", data.pMNP);

		textMNP.color = colorBlack;
		textPMNP.color = colorBlack;

		textSignMNP.text = string.Empty;
		textSignPMNP.text = string.Empty;

		textSignMNP.color = colorBlack;
		textSignPMNP.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.mNP_min != -1 && data.mNP < actualSampleRanges.mNP_min) {
				textSignMNP.text = "-";
				textMNP.color = colorRed;
				textSignMNP.color = colorRed;
			}
			else if (actualSampleRanges.mNP_max != -1 && data.mNP > actualSampleRanges.mNP_max) {
				textSignMNP.text = "+";
				textMNP.color = colorRed;
				textSignMNP.color = colorRed;
			}
			else {
				textSignMNP.text = string.Empty;
				textMNP.color = colorBlack;
				textSignMNP.color = colorBlack;
			}

			if (actualSampleRanges.pMNP_min != -1 && data.pMNP < actualSampleRanges.pMNP_min) {
				textSignPMNP.text = "-";
				textPMNP.color = colorRed;
				textSignPMNP.color = colorRed;
			}
			else if (actualSampleRanges.pMNP_max != -1 && data.pMNP > actualSampleRanges.pMNP_max) {
				textSignPMNP.text = "+";
				textPMNP.color = colorRed;
				textSignPMNP.color = colorRed;
			}
			else {
				textSignPMNP.text = string.Empty;
				textPMNP.color = colorBlack;
				textSignPMNP.color = colorBlack;
			}
		}

		// WBC Diff 3
		textTCBFH.text = string.Format("{0,7:0.000}", data.tCBFH);

		textTCBFH.color = colorBlack;

		textSignTCBFH.text = string.Empty;

		textSignTCBFH.color = colorBlack;

		if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
			if (actualSampleRanges.tCBFH_min != -1 && data.tCBFH < actualSampleRanges.tCBFH_min) {
				textSignTCBFH.text = "-";
				textTCBFH.color = colorRed;
				textSignTCBFH.color = colorRed;
			}
			else if (actualSampleRanges.tCBFH_max != -1 && data.tCBFH > actualSampleRanges.tCBFH_max) {
				textSignTCBFH.text = "+";
				textTCBFH.color = colorRed;
				textSignTCBFH.color = colorRed;
			}
			else {
				textSignTCBFH.text = string.Empty;
				textTCBFH.color = colorBlack;
				textSignTCBFH.color = colorBlack;
			}
		}
	}

	// Va mettre à jour le texte du bouton valider selon l'échantillon.
	private void ChangeBoutonValider() {
		ColorBlock colors = buttonValidate.colors;

		if (scriptDisplay.samplesData[scriptDisplay.currentData].validated) {
			buttonValidate.interactable = true;

			colors.normalColor = colorValidated;
			buttonValidate.colors = colors;

			buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATED_ANALYSIS");
		}
		else {
			colors.normalColor = colorNoValidated;
			buttonValidate.colors = colors;

			if (scriptDisplay.samplesData[scriptDisplay.currentData].qcPresent) {
				if (scriptDisplay.samplesData[scriptDisplay.currentData].sampleType == SampleController.SampleType.CBC) {
					buttonValidate.interactable = true;
				}
				else {
					buttonValidate.interactable = false; // Ne pas pouvoir valider un échantillon de QC
				}

				buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATE_ANALYSIS") + "\n<size=" + SizeQCPresent + "%>(" + LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "QC_PRESENT") + ")</size>";
			}
			else {
				buttonValidate.interactable = false;
				buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATE_ANALYSIS") + "\n<size=" + SizeQCPresent + "%>(" + LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "NO_QC_PRESENT") + ")</size>";
			}
		}
	}

	// Va réinitialiser l'apparence du bouton Valider.
	private void ResetBoutonValider() {
		buttonValidate.interactable = false;

		ColorBlock colors = buttonValidate.colors;
		colors.normalColor = colorNoValidated;
		buttonValidate.colors = colors;

		buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATE_ANALYSIS") + "\n<size=" + SizeQCPresent + "%>(" + LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "NO_QC_PRESENT") + ")</size>";
	}

	// Va déterminer quel SampleRanges il faut utiliser pour cet échantillon.
	private SampleRanges SelectSampleRanges() {
		foreach (SampleRanges sampleRanges in scriptDisplay.sampleRanges) {
			SampleData sampleData = scriptDisplay.samplesData[scriptDisplay.currentData];
			//Debug.Log("SampleRanges : " + sampleRanges + " | sex : " + sampleData.patientSex + " - " + sampleRanges.patientSex + " | years : " + sampleData.patientYears + "(" + sampleRanges.patientYearsMin + ", " + sampleRanges.patientYearsMax + ")");

			if (sampleData.patientSex == sampleRanges.patientSex && sampleData.patientYears <= sampleRanges.patientYearsMax && sampleData.patientYears >= sampleRanges.patientYearsMin) {
				//Debug.Log("Selected SampleRanges : " + sampleRanges);
				return sampleRanges;
			}
		}

		//Debug.Log("No SampleRanges selected !");
		return null;
	}
	#endregion

	#region Méthodes publiques
	// Va effacer toutes les donnes se trouvant dans le menu Nagivateur de Données.
	public void EffaceBrowser() {
		EffaceDonneesSample();
		EffaceDonneesPatient();
		EffaceTypeAnalyse();
		EffaceDonneesCBC();
		EffaceDonneesDIFF();
		EffaceDonneesRET();
		EffaceDonneesPLTF();
		EffaceWBCFlags();
		EffaceRBCFlags();
		EffacePLTFlags();
		EffaceDonneesWBC();
		EffaceDonneesRBC();
		EffaceDonneesWBCDiff();
		EffaceActions();
		EffaceErrorsRules();
	}

	// Va effacer les données d'un échantillon dans l'interface.
	public void EffaceDonneesSample() {
		textModeAnalyse.text = string.Empty;
		textNumSample.text = string.Empty;
		textDateSample.text = string.Empty;
		textEmplacementSample.text = string.Empty;
		textCommentSample.text = string.Empty;
	}

	// Va effacer les données d'un patient dans l'interface.
	public void EffaceDonneesPatient() {
		textHospitalPatient.text = string.Empty;
		textDoctorPatient.text = string.Empty;
		textIDPatient.text = string.Empty;
		textDateSexeAgePatient.text = string.Empty;
		textNamePatient.text = string.Empty;
		textCommentPatient.text = string.Empty;
	}

	// Va masquer et effacer le type d'analyse dans l'interface.
	public void EffaceTypeAnalyse() {
		panelTypeAnalyse.gameObject.SetActive(false); // Désactiver le panel afin de le masquer totalement
		panelTypeAnalyse.gameObject.GetComponentInChildren<TMP_Text>().text = string.Empty;
	}

	// Va effacer les données du test CBC dans l'interface.
	public void EffaceDonneesCBC() {
		// CBC 1
		textWBC.text = donneeVide;
		textRBC.text = donneeVide;
		textHGB.text = donneeVide;
		textHCT.text = donneeVide;
		textMCV.text = donneeVide;
		textMCH.text = donneeVide;
		textMCHC.text = donneeVide;
		textPLT.text = donneeVide;
		textSignWBC.text = string.Empty;
		textSignRBC.text = string.Empty;
		textSignHGB.text = string.Empty;
		textSignHCT.text = string.Empty;
		textSignMCV.text = string.Empty;
		textSignMCH.text = string.Empty;
		textSignMCHC.text = string.Empty;
		textSignPLT.text = string.Empty;

		textWBC.color = colorBlack;
		textRBC.color = colorBlack;
		textHGB.color = colorBlack;
		textHCT.color = colorBlack;
		textMCV.color = colorBlack;
		textMCH.color = colorBlack;
		textMCHC.color = colorBlack;
		textPLT.color = colorBlack;
		textSignWBC.color = colorBlack;
		textSignRBC.color = colorBlack;
		textSignHGB.color = colorBlack;
		textSignHCT.color = colorBlack;
		textSignMCV.color = colorBlack;
		textSignMCH.color = colorBlack;
		textSignMCHC.color = colorBlack;
		textSignPLT.color = colorBlack;

		// CBC 2
		textRDWSD.text = donneeVide;
		textRDWCV.text = donneeVide;
		textPDW.text = donneeVide;
		textMPV.text = donneeVide;
		textPLCR.text = donneeVide;
		textPCT.text = donneeVide;
		textSignRDWSD.text = string.Empty;
		textSignRDWCV.text = string.Empty;
		textSignPDW.text = string.Empty;
		textSignMPV.text = string.Empty;
		textSignPLCR.text = string.Empty;
		textSignPCT.text = string.Empty;

		textRDWSD.color = colorBlack;
		textRDWCV.color = colorBlack;
		textPDW.color = colorBlack;
		textMPV.color = colorBlack;
		textPLCR.color = colorBlack;
		textPCT.color = colorBlack;
		textSignRDWSD.color = colorBlack;
		textSignRDWCV.color = colorBlack;
		textSignPDW.color = colorBlack;
		textSignMPV.color = colorBlack;
		textSignPLCR.color = colorBlack;
		textSignPCT.color = colorBlack;

		// CBC 3
		textNRBCH.text = donneeVide;
		textNRBCP.text = donneeVide;
		textSignNRBCH.text = string.Empty;
		textSignNRBCP.text = string.Empty;

		textNRBCH.color = colorBlack;
		textNRBCP.color = colorBlack;
		textSignNRBCH.color = colorBlack;
		textSignNRBCP.color = colorBlack;
	}

	// Va effacer les données du test DIFF dans l'interface.
	public void EffaceDonneesDIFF() {
		// DIFF 1
		textNEUTH.text = donneeVide;
		textLYMPHH.text = donneeVide;
		textMONOH.text = donneeVide;
		textEOH.text = donneeVide;
		textBASOH.text = donneeVide;
		textSignNEUTH.text = string.Empty;
		textSignLYMPHH.text = string.Empty;
		textSignMONOH.text = string.Empty;
		textSignEOH.text = string.Empty;
		textSignBASOH.text = string.Empty;

		textNEUTH.color = colorBlack;
		textLYMPHH.color = colorBlack;
		textMONOH.color = colorBlack;
		textEOH.color = colorBlack;
		textBASOH.color = colorBlack;
		textSignNEUTH.color = colorBlack;
		textSignLYMPHH.color = colorBlack;
		textSignMONOH.color = colorBlack;
		textSignEOH.color = colorBlack;
		textSignBASOH.color = colorBlack;

		// DIFF 2
		textNEUTP.text = donneeVide;
		textLYMPHP.text = donneeVide;
		textMONOP.text = donneeVide;
		textEOP.text = donneeVide;
		textBASOP.text = donneeVide;
		textSignNEUTP.text = string.Empty;
		textSignLYMPHP.text = string.Empty;
		textSignMONOP.text = string.Empty;
		textSignEOP.text = string.Empty;
		textSignBASOP.text = string.Empty;

		textNEUTP.color = colorBlack;
		textLYMPHP.color = colorBlack;
		textMONOP.color = colorBlack;
		textEOP.color = colorBlack;
		textBASOP.color = colorBlack;
		textSignNEUTP.color = colorBlack;
		textSignLYMPHP.color = colorBlack;
		textSignMONOP.color = colorBlack;
		textSignEOP.color = colorBlack;
		textSignBASOP.color = colorBlack;

		// DIFF 3
		textIGH.text = donneeVide;
		textIGP.text = donneeVide;
		textSignIGH.text = string.Empty;
		textSignIGP.text = string.Empty;

		textIGH.color = colorBlack;
		textIGP.color = colorBlack;
		textSignIGH.color = colorBlack;
		textSignIGP.color = colorBlack;
	}

	// Va effacer les données du test RET dans l'interface.
	public void EffaceDonneesRET() {
		// RET
		textRETP.text = donneeVide;
		textRETH.text = donneeVide;
		textIRF.text = donneeVide;
		textLFR.text = donneeVide;
		textMFR.text = donneeVide;
		textHFR.text = donneeVide;
		textRETHE.text = donneeVide;
		textSignRETP.text = string.Empty;
		textSignRETH.text = string.Empty;
		textSignIRF.text = string.Empty;
		textSignLFR.text = string.Empty;
		textSignMFR.text = string.Empty;
		textSignHFR.text = string.Empty;
		textSignRETHE.text = string.Empty;

		textRETP.color = colorBlack;
		textRETH.color = colorBlack;
		textIRF.color = colorBlack;
		textLFR.color = colorBlack;
		textMFR.color = colorBlack;
		textHFR.color = colorBlack;
		textRETHE.color = colorBlack;
		textSignRETP.color = colorBlack;
		textSignRETH.color = colorBlack;
		textSignIRF.color = colorBlack;
		textSignLFR.color = colorBlack;
		textSignMFR.color = colorBlack;
		textSignHFR.color = colorBlack;
		textSignRETHE.color = colorBlack;
	}

	// Va effacer les données du test PLTF dans l'interface.
	public void EffaceDonneesPLTF() {
		// PLTF
		textIPF.text = donneeVide;
		textSignIPF.text = string.Empty;

		textIPF.color = colorBlack;
		textSignIPF.color = colorBlack;
	}

	// Va effacer les messages dans la Scroll View WBC Flag(s).
	public void EffaceWBCFlags() {
		textWBCFlags.text = string.Empty;
	}

	// Va effacer les messages dans la Scroll View RBC Flag(s).
	public void EffaceRBCFlags() {
		textRBCFlags.text = string.Empty;
	}

	// Va effacer les messages dans la Scroll View PLT Flag(s).
	public void EffacePLTFlags() {
		textPLTFlags.text = string.Empty;
	}

	// Va effacer les données du test WBC dans l'interface.
	public void EffaceDonneesWBC() {
		// WBC
		textWBCBF.text = donneeVide;
		textSignWBCBF.text = string.Empty;

		textWBCBF.color = colorBlack;
		textSignWBCBF.color = colorBlack;
	}

	// Va effacer les données du test RBC dans l'interface.
	public void EffaceDonneesRBC() {
		// RBC
		textRBCBF.text = donneeVide;
		textSignRBCBF.text = string.Empty;

		textRBCBF.color = colorBlack;
		textSignRBCBF.color = colorBlack;
	}

	// Va effacer les données du test WBC Diff dans l'interface.
	public void EffaceDonneesWBCDiff() {
		// WBC Diff
		textMNH.text = donneeVide;
		textPMNH.text = donneeVide;
		textMNP.text = donneeVide;
		textPMNP.text = donneeVide;
		textTCBFH.text = donneeVide;
		textSignMNH.text = string.Empty;
		textSignPMNH.text = string.Empty;
		textSignMNP.text = string.Empty;
		textSignPMNP.text = string.Empty;
		textSignTCBFH.text = string.Empty;

		textMNH.color = colorBlack;
		textPMNH.color = colorBlack;
		textMNP.color = colorBlack;
		textPMNP.color = colorBlack;
		textTCBFH.color = colorBlack;
		textSignMNH.color = colorBlack;
		textSignPMNH.color = colorBlack;
		textSignMNP.color = colorBlack;
		textSignPMNP.color = colorBlack;
		textSignTCBFH.color = colorBlack;
	}

	// Va effacer les messages dans la Scroll View Actions.
	public void EffaceActions() {
		textActions.text = string.Empty;
	}

	// Va effacer les messages dans la Scroll View Errors Rules.
	public void EffaceErrorsRules() {
		textErrorsRules.text = string.Empty;
	}

	// Va afficher toutes les données d'un échantillon dans l'interface.
	public void AffichageDonneesSample() {
		if (scriptDisplay.currentData == -1) {
			return;
		}

		// Type d'analyse
		panelTypeAnalyse.gameObject.SetActive(true); // Réactiver le panel

		actualSampleRanges = SelectSampleRanges();

		panelTypeAnalyse.gameObject.GetComponentInChildren<TMP_Text>().text = scriptDisplay.samplesData[scriptDisplay.currentData].typeFonctionnement == ModeFonctionnement.Automatique
			? LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "AUTOMATIC")
			: LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "MANUAL");

		// Sample
		textModeAnalyse.text = scriptDisplay.samplesData[scriptDisplay.currentData].modeAnalyse.ToString();
		textNumSample.text = scriptDisplay.samplesData[scriptDisplay.currentData].sampleID;
		textDateSample.text = scriptDisplay.samplesData[scriptDisplay.currentData].dateTest.ToString("dd/MM/yyyy HH:mm:ss");
		textEmplacementSample.text = scriptDisplay.samplesData[scriptDisplay.currentData].emplacementRack;
		textCommentSample.text = scriptDisplay.samplesData[scriptDisplay.currentData].commentSample;

		// Patient
		textHospitalPatient.text = scriptDisplay.samplesData[scriptDisplay.currentData].hospital;
		textDoctorPatient.text = scriptDisplay.samplesData[scriptDisplay.currentData].doctor;
		textIDPatient.text = scriptDisplay.samplesData[scriptDisplay.currentData].patientID;
		textDateSexeAgePatient.text = scriptDisplay.samplesData[scriptDisplay.currentData].PatientBirthday.ToString("dd/MM/yyyy") + " " + scriptDisplay.samplesData[scriptDisplay.currentData].patientSex + " " + scriptDisplay.samplesData[scriptDisplay.currentData].patientYears;
		textNamePatient.text = scriptDisplay.samplesData[scriptDisplay.currentData].patientName;
		textCommentPatient.text = scriptDisplay.samplesData[scriptDisplay.currentData].commentPatient;

		// Tests discrets
		if (scriptDisplay.samplesData[scriptDisplay.currentData].modeAnalyse == TypeMesure.BF) {
			AffichageDonneesSampleWBC(scriptDisplay.samplesData[scriptDisplay.currentData]);
			AffichageDonneesSampleRBC(scriptDisplay.samplesData[scriptDisplay.currentData]);
			AffichageDonneesSampleWBCDiff(scriptDisplay.samplesData[scriptDisplay.currentData]);
		}
		else {
			foreach (TypeTest test in scriptDisplay.samplesData[scriptDisplay.currentData].testsDiscrets) {
				switch (test) {
					case TypeTest.DIFF:
						AffichageDonneesSampleDIFF(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
					case TypeTest.RET:
						AffichageDonneesSampleRET(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
					case TypeTest.PLTF:
						AffichageDonneesSamplePLTF(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
					case TypeTest.WPC: // TODO il n'y a l'air que d'y avoir qu'un graph pour WPC
						break;
					case TypeTest.WBC:
						AffichageDonneesSampleWBC(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
					case TypeTest.RBC:
						AffichageDonneesSampleRBC(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
					case TypeTest.WBCDiff:
						AffichageDonneesSampleWBCDiff(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
					case TypeTest.CBC:
					default:
						AffichageDonneesSampleCBC(scriptDisplay.samplesData[scriptDisplay.currentData]);
						break;
				}
			}
		}

		// Mettre à jour l'affichage du bouton valider
		ChangeBoutonValider();
	}

	// Va ajouter un message dans la Scroll View WBC Flag(s).
	public void MessageWBCFlags(string msg) {
		textWBCFlags.text += msg;
	}

	// Va ajouter un message dans la Scroll View RBC Flag(s).
	public void MessageRBCFlags(string msg) {
		textRBCFlags.text += msg;
	}

	// Va ajouter un message dans la Scroll View PLT Flag(s).
	public void MessagePLTFlags(string msg) {
		textPLTFlags.text += msg;
	}

	// Va changer l'affichage du Navigateur afin de n'avoir que les tableaux nécessaires au mode d'analyse.
	public void ChangeMesure(TypeMesure mesure) {
		// Masquer les tableaux qui ne sont pas nécessaires
		if (mesure == TypeMesure.BF) {
			textCBC.gameObject.SetActive(false);
			textDIFF.gameObject.SetActive(false);
			textRET.gameObject.SetActive(false);
			textPLTF.gameObject.SetActive(false);

			if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT) {
				textTitleWBCFlags.gameObject.SetActive(true);
			}
			else {
				textTitleWBCFlags.gameObject.SetActive(false);
			}

			textTitleRBCFlags.gameObject.SetActive(false);
			textTitlePLTFlags.gameObject.SetActive(false);

			textTitleWBCBF.gameObject.SetActive(true);
			textTitleRBCBF.gameObject.SetActive(true);
			textWBCDiff.gameObject.SetActive(true);

			tableCBC.gameObject.SetActive(false);
			tableDIFF.gameObject.SetActive(false);
			tableRET.gameObject.SetActive(false);
			tablePLTF.gameObject.SetActive(false);

			if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT) {
				scrollViewTitleRBCFlags.gameObject.SetActive(true);
			}
			else {
				scrollViewTitleRBCFlags.gameObject.SetActive(false);
			}

			scrollViewTitleRBCFlags.gameObject.SetActive(false);
			scrollViewTitlePLTFlags.gameObject.SetActive(false);

			tableWBCBF.gameObject.SetActive(true);
			tableRBCBF.gameObject.SetActive(true);
			tableWBCDiff.gameObject.SetActive(true);

			panelGraphWDFEXT.gameObject.SetActive(true);
			panelGraphWNR.gameObject.SetActive(false);
			panelGraphWPC.gameObject.SetActive(false);
			panelGraphRET.gameObject.SetActive(false);
			panelGraphPLTF.gameObject.SetActive(false);
			panelGraphPLT.gameObject.SetActive(false);
		}
		else {
			textCBC.gameObject.SetActive(true);
			textDIFF.gameObject.SetActive(true);
			textRET.gameObject.SetActive(true);
			textPLTF.gameObject.SetActive(true);

			if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT) {
				textTitleWBCFlags.gameObject.SetActive(true);
				textTitleRBCFlags.gameObject.SetActive(true);
				textTitlePLTFlags.gameObject.SetActive(true);
			}
			else {
				textTitleWBCFlags.gameObject.SetActive(false);
				textTitleRBCFlags.gameObject.SetActive(false);
				textTitlePLTFlags.gameObject.SetActive(false);
			}

			textTitleWBCBF.gameObject.SetActive(false);
			textTitleRBCBF.gameObject.SetActive(false);
			textWBCDiff.gameObject.SetActive(false);

			tableCBC.gameObject.SetActive(true);
			tableDIFF.gameObject.SetActive(true);
			tableRET.gameObject.SetActive(true);
			tablePLTF.gameObject.SetActive(true);

			if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT) {
				scrollViewTitleWBCFlags.gameObject.SetActive(true);
				scrollViewTitleRBCFlags.gameObject.SetActive(true);
				scrollViewTitlePLTFlags.gameObject.SetActive(true);
			}
			else {
				scrollViewTitleWBCFlags.gameObject.SetActive(false);
				scrollViewTitleRBCFlags.gameObject.SetActive(false);
				scrollViewTitlePLTFlags.gameObject.SetActive(false);
			}

			tableWBCBF.gameObject.SetActive(false);
			tableRBCBF.gameObject.SetActive(false);
			tableWBCDiff.gameObject.SetActive(false);

			panelGraphWDFEXT.gameObject.SetActive(false);
			panelGraphWNR.gameObject.SetActive(true);
			panelGraphWPC.gameObject.SetActive(true);
			panelGraphRET.gameObject.SetActive(true);
			panelGraphPLTF.gameObject.SetActive(true);
			panelGraphPLT.gameObject.SetActive(true);
		}
	}
	#endregion

	#region Action listeners
	// Action lorsque l'on va appuyer sur un bouton d'onglet dans le Navigateur. Va changer l'onget qui est actuellement affiché.
	public void ChangeBrowserTab(int tab) {
		boutonTabMain.interactable = true;
		boutonTabMessages.interactable = true;
		boutonTabGraph.interactable = true;

		TMP_Text textButtonTabMain = boutonTabMain.transform.Find("TextButton").gameObject.GetComponent<TMP_Text>();
		TMP_Text textButtonTabMessages = boutonTabMessages.transform.Find("TextButton").gameObject.GetComponent<TMP_Text>();
		TMP_Text textButtonTabGraphs = boutonTabGraph.transform.Find("TextButton").gameObject.GetComponent<TMP_Text>();

		textButtonTabMain.fontStyle = FontStyles.Normal;
		textButtonTabMessages.fontStyle = FontStyles.Normal;
		textButtonTabGraphs.fontStyle = FontStyles.Normal;

		panelBrowserData.gameObject.SetActive(false);
		panelBrowserMessages.gameObject.SetActive(false);
		panelBrowserGraphs.gameObject.SetActive(false);

		switch (tab) {
			case 1:
				textButtonTabMessages.fontStyle = FontStyles.Bold;
				boutonTabMessages.interactable = false;
				panelBrowserMessages.gameObject.SetActive(true);
				break;
			case 2:
				textButtonTabGraphs.fontStyle = FontStyles.Bold;
				boutonTabGraph.interactable = false;
				panelBrowserGraphs.gameObject.SetActive(true);
				break;
			case 0:
			default:
				textButtonTabMain.fontStyle = FontStyles.Bold;
				boutonTabMain.interactable = false;
				panelBrowserData.gameObject.SetActive(true);
				break;
		}
	}

	// Action lorsque l'on clique sur le bouton Valider. Va valider l'échantillon.
	public void ButtonValidateClick() {
		ColorBlock colors = buttonValidate.colors;

		if (scriptDisplay.samplesData[scriptDisplay.currentData].validated) {
			colors.normalColor = colorNoValidated;
			buttonValidate.colors = colors;

			buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATE_ANALYSIS") + "\n<size=" + SizeQCPresent + "%>(" + LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "QC_PRESENT") + ")</size>";

			// Changer l'état de validation de l'échantillon
			scriptDisplay.samplesData[scriptDisplay.currentData].validated = false;
		}
		else {
			colors.normalColor = colorValidated;
			buttonValidate.colors = colors;

			buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATED_ANALYSIS");

			// Changer l'état de validation de l'échantillon
			scriptDisplay.samplesData[scriptDisplay.currentData].validated = true;
		}

		// Rafraichir l'affichage de l'Explorateur d'Échantillons
		scriptDisplay.RefreshCurrentExplorerList();
	}
	#endregion
}
