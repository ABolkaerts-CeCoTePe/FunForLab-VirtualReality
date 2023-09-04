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
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

/// <summary>
/// Classe qui gère l'affichage de la Visionneuse de PDF.
/// </summary>
public class PDFController : MonoBehaviour {
	private TMP_Text textTitle;
	private GameObject rawImageModel;
	private RectTransform scrollViewDocument;

	private List<GameObject> pages;

	public TableReference stringTable; // Table de traduction
	public PDFData pDFData; // Données du PDF à afficher

	#region Messages Unity
	private void Awake() {
		pages = new List<GameObject>();

		textTitle = transform.Find("PanelBackground/PanelTitle/TextTitle").gameObject.GetComponent<TMP_Text>();
		rawImageModel = transform.Find("PanelBackground/PanelContent/ScrollViewDocument/Viewport/Content/RawImageModel").gameObject;
		scrollViewDocument = transform.Find("PanelBackground/PanelContent/ScrollViewDocument/Viewport/Content").gameObject.GetComponent<RectTransform>();
	}

	private void Start() {
		ShowPDF();
	}
	#endregion

	#region Méthodes privées
	// Va effacer le PDF actuel de l'affichage.
	private void DeletePDF() {
		pages = new List<GameObject>();

		for (int i = 0; i < pages.Count; i++) {
			Destroy(pages[i]);
		}

		pages.Clear();
		textTitle.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "PDF_VIEWER");
	}

	// Va afficher un nouveau PDF.
	private void ShowPDF() {
		if (pDFData != null) {
			DeletePDF();
			textTitle.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "PDF_VIEWER") + " – " + pDFData.fileName;

			for (int i = 0; i < pDFData.pages.Count; i++) {
				GameObject newPage = Instantiate(rawImageModel, scrollViewDocument);
				newPage.name = "RawImagePage" + i;
				newPage.GetComponent<RawImage>().texture = pDFData.pages[i];
				newPage.SetActive(true);

				pages.Add(newPage);
			}
		}
	}
	#endregion
}
