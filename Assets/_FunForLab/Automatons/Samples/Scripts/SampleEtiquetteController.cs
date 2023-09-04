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

/// <summary>
/// Classe qui va g�rer le rendu de l'�tiquette d'un �chantillon.
/// </summary>
public class SampleEtiquetteController : MonoBehaviour {
	private TMP_Text textName;
	private TMP_Text textDateNais;
	private TMP_Text textAgeSexe;
	private TMP_Text textCode;

	private TMP_Text textDateCollect;
	private TMP_Text textById;
	private TMP_Text textArm;

	private TMP_Text textComposition;
	private TMP_Text textBarcode;

	public Camera cameraRender;

	// Va aller r�cup�rer les r�f�rences des �l�ments de l'UI.
	private void GetRef() {
		textName = transform.Find("PanelBackground/PanelCentre/PanelId/TextName").gameObject.GetComponent<TMP_Text>();
		textDateNais = transform.Find("PanelBackground/PanelCentre/PanelId/TextDate").gameObject.GetComponent<TMP_Text>();
		textAgeSexe = transform.Find("PanelBackground/PanelCentre/PanelId/TextAgeSexe").gameObject.GetComponent<TMP_Text>();
		textCode = transform.Find("PanelBackground/PanelCentre/PanelId/TextCode").gameObject.GetComponent<TMP_Text>();

		textDateCollect = transform.Find("PanelBackground/PanelCentre/PanelCollect/TextDateTime").gameObject.GetComponent<TMP_Text>();
		textById = transform.Find("PanelBackground/PanelCentre/PanelCollect/TextBy").gameObject.GetComponent<TMP_Text>();
		textArm = transform.Find("PanelBackground/PanelCentre/PanelCollect/TextArm").gameObject.GetComponent<TMP_Text>();

		textComposition = transform.Find("PanelBackground/PanelCentre/PanelComposition/TextComposition").gameObject.GetComponent<TMP_Text>();
		textBarcode = transform.Find("PanelBackground/PanelDroite/TextBarcode").gameObject.GetComponent<TMP_Text>();
	}

	// Va remplir l'UI avec les donn�es de l'�chantillon.
	public void Populate(SampleData sampleData) {
		if (textName == null) {
			GetRef(); // Va savoir pourquoi, dans l'�diteur avec Awake �a bugge car textName.text fait une NullReferenceException, mais en build tout fonctionne correctement
		}

		textName.text = sampleData.patientName;
		textDateNais.text = sampleData.PatientBirthday.ToString("dd/MM/yyyy");
		textAgeSexe.text = sampleData.patientYears + "a " + sampleData.patientSex;
		textCode.text = sampleData.patientID;

		textDateCollect.text = sampleData.DateCollect.ToString("dd/MM/yyyy HH:mm");
		textById.text = "By " + sampleData.collectID;
		
		textArm.text = sampleData.arm == SampleData.Arm.Left ? "Left arm" : "Right arm";

		switch (sampleData.sampleType) { // TODO Si c'est un tube de QC, je suppose que l'�tiquette sera diff�rente
			case SampleController.SampleType.QC:
				break;
			case SampleController.SampleType.CBC:
			default:
				textComposition.text = "EDTA";
				break;
		}

		textBarcode.text = sampleData.sampleID;
	}

	// Va faire le rendu de l'�tiquette durant 1 frame.
	public Texture2D RenderCamera() {
		Debug.Log("Render Camera Etiquette");

		RenderTexture.active = cameraRender.targetTexture;
		cameraRender.Render();

		Texture2D renderEtiquette = new Texture2D(cameraRender.targetTexture.width, cameraRender.targetTexture.height);
		renderEtiquette.ReadPixels(new Rect(0, 0, cameraRender.targetTexture.width, cameraRender.targetTexture.height), 0, 0);
		renderEtiquette.Apply();

		return renderEtiquette;
	}
}
