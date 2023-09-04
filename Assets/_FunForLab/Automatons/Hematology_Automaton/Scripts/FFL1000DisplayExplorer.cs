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
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static HematoAutomatonController;
using static FFL1000Display;

/// <summary>
/// Classe qui gère spécifiquement l'Explorateur d'Échantillons de l'écran d'affichage du FFL 1000.
/// </summary>
public class FFL1000DisplayExplorer : MonoBehaviour {
	private const string donneeVide = "----";
	private const int SizeQCPresent = 75;

	#region Variables UI
	// PanelContentExplorer -> PanelTop
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

	// PanelContentExplorer -> PanelList
	private RectTransform scrollViewList;

	// PanelContentExplorer -> PanelData
	private Button buttonUp;
	private Button buttonDown;
	private Button buttonDetails;
	private RectTransform scrollViewData;
	#endregion

	private FFL1000Display scriptDisplay;

	private Color32 colorAlt = new Color32(219, 219, 219, 255);
	private Color32 colorValidated = new Color32(0, 127, 255, 255);
	private Color32 colorNoValidated = new Color32(127, 127, 127, 255);
	private Color32 colorRed = new Color32(255, 0, 0, 255);
	private Color32 colorBlack = new Color32(0, 0, 0, 255);

	private List<GameObject> rowDatas;

	private GameObject rowListModel;
	private GameObject rowListEmpty;
	private GameObject rowListSelected;
	private GameObject rowDataModel;

	private bool LastAnalysisContainsQC {
		get {
			foreach (SampleData data in scriptDisplay.lastAnalysisSamplesData) {
				if (data.sampleType == SampleController.SampleType.QC) {
					return true;
				}
			}

			return false;
		}
	}

	#region Messages Unity
	private void Awake() {
		scriptDisplay = gameObject.GetComponent<FFL1000Display>();
		rowDatas = new List<GameObject>();

		rowListModel = transform.Find("PanelBackground/PanelContentExplorer/PanelList/ScrollViewList/Viewport/Content/RowModel").gameObject;
		rowListEmpty = transform.Find("PanelBackground/PanelContentExplorer/PanelList/ScrollViewList/Viewport/Content/RowEmpty").gameObject;
		rowDataModel = transform.Find("PanelBackground/PanelContentExplorer/PanelData/ScrollViewData/Viewport/Content/RowModel").gameObject;

		// PanelContentExplorer -> PanelTop
		buttonValidate = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/ButtonValidate").gameObject.GetComponent<Button>();
		textModeAnalyse = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoSample/TextModeAnalyse").gameObject.GetComponent<TMP_Text>();
		textNumSample = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoSample/TextNumSample").gameObject.GetComponent<TMP_Text>();
		textDateSample = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoSample/TextDateSample").gameObject.GetComponent<TMP_Text>();
		textEmplacementSample = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoSample/TextEmplacementSample").gameObject.GetComponent<TMP_Text>();
		textCommentSample = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoSample/TextCommentSample").gameObject.GetComponent<TMP_Text>();
		textHospitalPatient = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoPatient/TextHospitalPatient").gameObject.GetComponent<TMP_Text>();
		textDoctorPatient = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoPatient/TextDoctorPatient").gameObject.GetComponent<TMP_Text>();
		textIDPatient = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoPatient/TextIDPatient").gameObject.GetComponent<TMP_Text>();
		textDateSexeAgePatient = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoPatient/TextDateSexeAgePatient").gameObject.GetComponent<TMP_Text>();
		textNamePatient = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoPatient/TextNamePatient").gameObject.GetComponent<TMP_Text>();
		textCommentPatient = transform.Find("PanelBackground/PanelContentExplorer/PanelTop/PanelInfoPatient/TextCommentPatient").gameObject.GetComponent<TMP_Text>();

		// PanelContentExplorer -> PanelList
		scrollViewList = transform.Find("PanelBackground/PanelContentExplorer/PanelList/ScrollViewList/Viewport/Content").gameObject.GetComponent<RectTransform>();

		// PanelContentExplorer -> PanelData
		buttonUp = transform.Find("PanelBackground/PanelContentExplorer/PanelData/ButtonUp").gameObject.GetComponent<Button>();
		buttonDown = transform.Find("PanelBackground/PanelContentExplorer/PanelData/ButtonDown").gameObject.GetComponent<Button>();
		buttonDetails = transform.Find("PanelBackground/PanelContentExplorer/PanelData/ButtonDetails").gameObject.GetComponent<Button>();
		scrollViewData = transform.Find("PanelBackground/PanelContentExplorer/PanelData/ScrollViewData/Viewport/Content").gameObject.GetComponent<RectTransform>();

		PopulateScrollViewData();
	}

	private void Start() {
		scriptDisplay.samplesData = new List<SampleData>();
		rowListSelected = null;

		EffaceDonneesSample();
		EffaceDonneesPatient();

		ResetLastAnalysisSamples();
	}
	#endregion

	#region Méthode privées
	// Va remplir la ligne avec les données.
	private void ListRowData(GameObject row, SampleData data) {
		TMP_Text validated = row.transform.Find("ColumnData/TextValidated").gameObject.GetComponent<TMP_Text>();
		TMP_Text numSample = row.transform.Find("ColumnData/TextNumSample").gameObject.GetComponent<TMP_Text>();
		TMP_Text mode = row.transform.Find("ColumnData/TextMode").gameObject.GetComponent<TMP_Text>();
		TMP_Text tests = row.transform.Find("ColumnData/TextTests").gameObject.GetComponent<TMP_Text>();
		TMP_Text date = row.transform.Find("ColumnData/TextDate").gameObject.GetComponent<TMP_Text>();
		TMP_Text emplacement = row.transform.Find("ColumnData/TextEmplacement").gameObject.GetComponent<TMP_Text>();
		TMP_Text fonctionnement = row.transform.Find("ColumnData/TextFonctionnement").gameObject.GetComponent<TMP_Text>();
		TMP_Text positif = row.transform.Find("ColumnData/TextPositive").gameObject.GetComponent<TMP_Text>();
		TMP_Text actions = row.transform.Find("ColumnData/TextAction").gameObject.GetComponent<TMP_Text>();
		TMP_Text errors = row.transform.Find("ColumnData/TextErrors").gameObject.GetComponent<TMP_Text>();
		TMP_Text distrib = row.transform.Find("ColumnData/TextDistrib").gameObject.GetComponent<TMP_Text>();
		TMP_Text flagsWBC = row.transform.Find("ColumnData/TextFlagsWBC").gameObject.GetComponent<TMP_Text>();
		TMP_Text flagsRBC = row.transform.Find("ColumnData/TextFlagsRBC").gameObject.GetComponent<TMP_Text>();
		TMP_Text flagsPLT = row.transform.Find("ColumnData/TextFlagsPLT").gameObject.GetComponent<TMP_Text>();

		validated.text = data.validated ? LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATED_LETTER") : string.Empty;
		numSample.text = data.sampleID;
		mode.text = data.modeAnalyse.ToString();

		string msg = string.Empty;

		if (data.testsDiscrets != null && data.testsDiscrets.Length > 0) {
			foreach (TypeTest test in data.testsDiscrets) {
				if (test == TypeTest.PLTF) {
					msg += "PLT-F";
				}
				else {
					msg += test.ToString();
				}

				msg += " ";
			}
		}

		tests.text = msg;

		date.text = data.dateTest.ToString("dd/MM/yyyy HH:mm:ss");
		emplacement.text = data.emplacementRack;

		if (data.typeFonctionnement == ModeFonctionnement.Automatique) {
			fonctionnement.text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "AUTOMATIC");
		}
		else {
			fonctionnement.text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "MANUAL");
		}

		positif.text = data.positive ? "P" : string.Empty;
		actions.text = data.actions;
		errors.text = data.errors;

		// TODO Distribution a l'air calculé à partir des graphiques de RBC et PLT
		distrib.text = string.Empty;

		flagsWBC.text = data.wBCFlags;
		flagsRBC.text = data.rBCFlags;
		flagsPLT.text = data.pLTFlags;
	}

	// Va changer la couleur de la ligne.
	private void ChangeRowColor(GameObject row, Color32 color) {
		Image[] columns = row.transform.GetComponentsInChildren<Image>();

		foreach (Image column in columns) {
			if (column.gameObject.name == "ColumnData") {
				column.color = color;
			}
		}
	}

	// Va remettre la couleur par défaut sur la ligne actuellement sélectionnée.
	private void ResetSelectedRowColor() {
		if (rowListSelected != null) {
			int row = int.Parse(rowListSelected.name.Substring(3));

			if (row % 2 == 0) { // Enlever la couleur de sélection sur l'ancienne ligne sélectionnée
				ChangeRowColor(rowListSelected, colorAlt);
			}
			else {
				ChangeRowColor(rowListSelected, new Color32(239, 239, 239, 255));
			}
		}
	}

	// Va générer les lignes de la ScrollViewData en prenant les informations de FFL1000TestList.
	private void PopulateScrollViewData() {
		foreach (FFL1000Test test in FFL1000TestList.tests) {
			GameObject duplicate = Instantiate(rowDataModel, scrollViewData);
			duplicate.name = "RowData" + test.variable;
			duplicate.SetActive(true);

			TMP_Text item = duplicate.transform.Find("ColumnData/TextItem").gameObject.GetComponent<TMP_Text>();
			TMP_Text data = duplicate.transform.Find("ColumnData/TextData").gameObject.GetComponent<TMP_Text>();
			TMP_Text signData = duplicate.transform.Find("ColumnData/TextSignData").gameObject.GetComponent<TMP_Text>();
			TMP_Text unit = duplicate.transform.Find("ColumnData/TextUnit").gameObject.GetComponent<TMP_Text>();

			item.text = test.nom;
			data.text = donneeVide;
			signData.text = string.Empty;
			unit.text = test.unite;

			rowDatas.Add(duplicate);
		}
	}

	// Va mettre à jour le texte du bouton valider selon l'échantillon.
	private void ChangeBoutonValider(int pos) {
		ColorBlock colors = buttonValidate.colors;

		if (scriptDisplay.samplesData[pos].validated) {
			buttonValidate.interactable = true;

			colors.normalColor = colorValidated;
			buttonValidate.colors = colors;

			buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATED_ANALYSIS");
		}
		else {
			colors.normalColor = colorNoValidated;
			buttonValidate.colors = colors;

			if (scriptDisplay.samplesData[pos].qcPresent) {
				if (scriptDisplay.samplesData[pos].sampleType == SampleController.SampleType.CBC) {
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

	// Va afficher toutes les données d'un échantillon dans l'interface.
	public void AffichageDonneesSample(SampleData data) {
		// Sample
		textModeAnalyse.text = data.modeAnalyse.ToString();
		textNumSample.text = data.sampleID;
		textDateSample.text = data.dateTest.ToString("dd/MM/yyyy HH:mm:ss");
		textEmplacementSample.text = data.emplacementRack;
		textCommentSample.text = data.commentSample;

		// Patient
		textHospitalPatient.text = data.hospital;
		textDoctorPatient.text = data.doctor;
		textIDPatient.text = data.patientID;
		textDateSexeAgePatient.text = data.PatientBirthday.ToString("dd/MM/yyyy") + " " + data.patientSex + " " + data.patientYears;
		textNamePatient.text = data.patientName;
		textCommentPatient.text = data.commentPatient;
	}

	// Va ajouter les données d'un échantillon dans la ScrollViewList
	public void AddSampleList(SampleData data) {
		if (scriptDisplay.samplesData.Count == 0) {
			rowListEmpty.SetActive(false); // Comme on rajoute des éléments, retirer la ligne RowEmpty
			buttonUp.interactable = true;
			buttonDown.interactable = true;
		}

		GameObject duplicate = Instantiate(rowListModel, scrollViewList);
		duplicate.name = "Row" + scriptDisplay.samplesData.Count;
		duplicate.SetActive(true);

		ListRowData(duplicate, data);

		if (scriptDisplay.samplesData.Count % 2 == 0) { // Uniquement pour les lignes paires, changer la couleur afin d'avoir une ligne sur 2 d'une couleur différente
			ChangeRowColor(duplicate, colorAlt);
		}

		SampleData newData = Instantiate(data); // Créer une nouvelle instance du ScriptableObject, sinon les anciens étaient modifiés aussi (1 seule réf)
		newData.typeFonctionnement = data.typeFonctionnement;
		newData.modeAnalyse = data.modeAnalyse;
		newData.testsDiscrets = data.testsDiscrets;
		newData.dateTest = data.dateTest;
		newData.emplacementRack = data.emplacementRack;
		newData.qcPresent = data.qcPresent;

		scriptDisplay.lastAnalysisSamplesData.Add(newData);

		if (LastAnalysisContainsQC) {
			data.qcPresent = true;

			// Mettre à jour les autres échantillons qui sont passés durant cette analyse
			foreach (SampleData lastSample in scriptDisplay.lastAnalysisSamplesData) {
				foreach (SampleData sample in scriptDisplay.samplesData) {
					if (lastSample.sampleID == sample.sampleID && lastSample.dateTest == sample.dateTest && lastSample.sampleType == sample.sampleType) {
						sample.qcPresent = true;
						lastSample.qcPresent = true;
					}
				}
			}
		}
		else {
			data.qcPresent = false;
		}

		newData = Instantiate(data);
		newData.typeFonctionnement = data.typeFonctionnement;
		newData.modeAnalyse = data.modeAnalyse;
		newData.testsDiscrets = data.testsDiscrets;
		newData.dateTest = data.dateTest;
		newData.emplacementRack = data.emplacementRack;
		newData.qcPresent = data.qcPresent;

		scriptDisplay.samplesData.Add(newData);
	}

	// Va afficher les données de l'échantillon dans la ScrollViewData.
	public void AffichageDonneesTests(SampleData data) {
		SampleRanges actualSampleRanges = SelectSampleRanges();

		for (int i = 0; i < rowDatas.Count; i++) {
			GameObject row = rowDatas[i];

			TMP_Text dataValue = row.transform.Find("ColumnData/TextData").gameObject.GetComponent<TMP_Text>();
			TMP_Text signData = row.transform.Find("ColumnData/TextSignData").gameObject.GetComponent<TMP_Text>();

			if (data.modeAnalyse == TypeMesure.BF) {
				if (FFL1000TestList.tests[i].test == TypeTest.WBC || FFL1000TestList.tests[i].test == TypeTest.RBC || FFL1000TestList.tests[i].test == TypeTest.WBCDiff) {
					row.SetActive(true);

					dataValue.text = string.Format(FFL1000TestList.tests[i].format, data.GetType().GetField(FFL1000TestList.tests[i].variable).GetValue(data));
					dataValue.color = colorBlack;
					signData.text = string.Empty;
					signData.color = colorBlack;
				}
				else {
					row.SetActive(false);
				}
			}
			else {
				if (data.testsDiscrets.Contains(FFL1000TestList.tests[i].test)) {
					row.SetActive(true);

					dataValue.text = string.Format(FFL1000TestList.tests[i].format, data.GetType().GetField(FFL1000TestList.tests[i].variable).GetValue(data));

					if (PlayerPrefsManager.Difficulty == PlayerPrefsManager.DifficultyLevel.MLT && actualSampleRanges != null) {
						float min_range = (float) actualSampleRanges.GetType().GetField(FFL1000TestList.tests[i].variable + "_min").GetValue(actualSampleRanges);
						float max_range = (float) actualSampleRanges.GetType().GetField(FFL1000TestList.tests[i].variable + "_max").GetValue(actualSampleRanges);

						if (min_range != -1 && float.Parse(dataValue.text) < min_range) {
							signData.text = "-";
							dataValue.color = colorRed;
							signData.color = colorRed;
						}
						else if (max_range != -1 && float.Parse(dataValue.text) > max_range) {
							signData.text = "+";
							dataValue.color = colorRed;
							signData.color = colorRed;
						}
						else {
							signData.text = string.Empty;
							dataValue.color = colorBlack;
							signData.color = colorBlack;
						}
					}

					//signData.text = string.Empty;
				}
				else {
					row.SetActive(false);

					dataValue.text = donneeVide;
					dataValue.color = colorBlack;
					signData.text = string.Empty;
					signData.color = colorBlack;
				}
			}
		}
	}

	// Va effacer la liste des échantillons de la dernière analyse.
	public void ResetLastAnalysisSamples() {
		scriptDisplay.lastAnalysisSamplesData = new List<SampleData>();
		buttonValidate.interactable = false;

		ColorBlock colors = buttonValidate.colors;
		colors.normalColor = colorNoValidated;
		buttonValidate.colors = colors;

		buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATE_ANALYSIS") + "\n<size=" + SizeQCPresent + "%>(" + LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "NO_QC_PRESENT") + ")</size>";
	}

	// Va rafraichir la ligne donnée dans la liste des échantillons.
	public void RefreshRowData(int row) {
		Transform child = scrollViewList.Find("Row" + row);

		if (child != null) {
			ListRowData(child.gameObject, scriptDisplay.samplesData[row]);

			// Mettre à jour l'affichage du bouton valider
			ChangeBoutonValider(row);
		}
	}
	#endregion

	#region Action listeners
	// Action lorsqu'une ligne se fait hover. Va changer sa couleur pour mieux la voir.
	public void RowHoverEnter(Image sender) {
		if (sender.gameObject != rowListSelected) { // Ne pas faire le hover si c'est la ligne actuellement sélectionnée
			ChangeRowColor(sender.gameObject, new Color32(255, 191, 127, 255));
		}
	}

	// Action lorsqu'une ligne ne se fait plus hover. Va remettre sa couleur d'origine.
	public void RowHoverExit(Image sender) {
		if (sender.gameObject != rowListSelected) { // Ne pas faire le hover si c'est la ligne actuellement sélectionnée
			int row = int.Parse(sender.name.Substring(3));

			if (row % 2 == 0) {
				ChangeRowColor(sender.gameObject, colorAlt);
			}
			else {
				ChangeRowColor(sender.gameObject, new Color32(239, 239, 239, 255));
			}
		}
	}

	// Action lorsque l'on clique sur une ligne. Va afficher ses données dans la zone de droite et dans le menu Navigateur de Données.
	public void RowClick(Image sender) {
		ResetSelectedRowColor();

		rowListSelected = sender.gameObject;

		ChangeRowColor(sender.gameObject, new Color32(255, 191, 0, 255));

		int row = int.Parse(sender.gameObject.name.Substring(3));
		scriptDisplay.currentData = row;

		AffichageDonneesSample(scriptDisplay.samplesData[row]);
		AffichageDonneesTests(scriptDisplay.samplesData[row]);

		buttonDetails.interactable = true;

		// Mettre à jour l'affichage du bouton valider
		ChangeBoutonValider(row);

		// Mettre à jour l'affichage dans le menu Navigateur de Données
		scriptDisplay.EffaceBrowser();
		scriptDisplay.ChangeMesure(scriptDisplay.samplesData[row].modeAnalyse);
		scriptDisplay.AffichageDonneesSample();
	}

	// Action lorsque l'on clique sur les boutons Haut ou Bas. Va changer l'échantillon actuellement sélectionné.
	public void ButtonMoveClick(int pos) {
		int newPos = 0;

		if (rowListSelected != null) {
			newPos = int.Parse(rowListSelected.name.Substring(3)) + pos;

			if (newPos < 0) {
				newPos = 0;
			}
			else if (newPos > scriptDisplay.samplesData.Count) {
				newPos = scriptDisplay.samplesData.Count;
			}
		}

		Transform transform = scrollViewList.Find("Row" + newPos);

		if (transform != null) {
			ResetSelectedRowColor();

			rowListSelected = transform.gameObject;
			scriptDisplay.currentData = newPos;

			ChangeRowColor(rowListSelected, new Color32(255, 191, 0, 255));

			AffichageDonneesSample(scriptDisplay.samplesData[newPos]);
			AffichageDonneesTests(scriptDisplay.samplesData[newPos]);

			buttonDetails.interactable = true;

			// Mettre à jour l'affichage du bouton valider
			ChangeBoutonValider(newPos);

			// Mettre à jour l'affichage dans le menu Navigateur de Données
			scriptDisplay.EffaceBrowser();
			scriptDisplay.ChangeMesure(scriptDisplay.samplesData[newPos].modeAnalyse);
			scriptDisplay.AffichageDonneesSample();
		}
	}

	// Action lorsque l'on clique sur le bouton Détails. Va passer l'affichage dans le menu Navigateur de Données.
	public void ButtonDetailsClick() {
		int row = int.Parse(rowListSelected.name.Substring(3));
		scriptDisplay.currentData = row;

		// Passer sur le menu du Navigateur de Données
		scriptDisplay.ChangeMenuActif((int) DisplayMenu.DataBrowser);

		scriptDisplay.EffaceBrowser();
		scriptDisplay.ChangeMesure(scriptDisplay.samplesData[row].modeAnalyse);
		scriptDisplay.AffichageDonneesSample();
	}

	// Action lorsque l'on clique sur le bouton Valider. Va valider l'échantillon.
	public void ButtonValidateClick() {
		int row = int.Parse(rowListSelected.name.Substring(3));

		ColorBlock colors = buttonValidate.colors;

		if (scriptDisplay.samplesData[row].validated) {
			colors.normalColor = colorNoValidated;
			buttonValidate.colors = colors;

			buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATE_ANALYSIS") + "\n<size=" + SizeQCPresent + "%>(" + LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "QC_PRESENT") + ")</size>";

			// Changer l'état de validation de l'échantillon
			scriptDisplay.samplesData[row].validated = false;
		}
		else {
			colors.normalColor = colorValidated;
			buttonValidate.colors = colors;

			buttonValidate.GetComponentInChildren<TMP_Text>().text = LocalizationSettings.StringDatabase.GetLocalizedString(scriptDisplay.stringTable, "VALIDATED_ANALYSIS");

			// Changer l'état de validation de l'échantillon
			scriptDisplay.samplesData[row].validated = true;
		}

		// Rafraîchir la ligne de la liste
		ListRowData(rowListSelected, scriptDisplay.samplesData[row]);

		// Rafraîchir le Navigateur
		scriptDisplay.EffaceBrowser();
		scriptDisplay.ChangeMesure(scriptDisplay.samplesData[row].modeAnalyse);
		scriptDisplay.AffichageDonneesSample();
	}
	#endregion
}
