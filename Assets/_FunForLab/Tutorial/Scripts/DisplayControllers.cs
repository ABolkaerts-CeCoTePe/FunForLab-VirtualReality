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
using UnityEngine.Localization.Tables;

/// <summary>
/// Classe qui g�re l'affichage des contr�les.
/// </summary>
public class DisplayControllers : MonoBehaviour {
	private TMP_Text textTitle;

	private TMP_Text textPrimaryButton;
	private TMP_Text textSecondaryButton;
	private TMP_Text textLeftThumbstick;
	private TMP_Text textRightThumbstick;
	private TMP_Text textTrigger;
	private TMP_Text textGrab;

	// Liste des types de manettes affichables dans le jeu
	public enum ControllerType {
		OculusTouch
	}

	public TableReference stringTable; // Table de traduction
	public ControllerType controllerType; // Type de manette � afficher

	#region Messages Unity
	private void Awake() {
		textTitle = transform.Find("PanelBackground/PanelTitle/TextTitle").gameObject.GetComponent<TMP_Text>();

		textPrimaryButton = transform.Find("PanelBackground/PanelContent/PanelControls/TextPrimaryButton").gameObject.GetComponent<TMP_Text>();
		textSecondaryButton = transform.Find("PanelBackground/PanelContent/PanelControls/TextSecondaryButton").gameObject.GetComponent<TMP_Text>();
		textLeftThumbstick = transform.Find("PanelBackground/PanelContent/PanelControls/TextLeftThumbstick").gameObject.GetComponent<TMP_Text>();
		textRightThumbstick = transform.Find("PanelBackground/PanelContent/PanelControls/TextRightThumbstick").gameObject.GetComponent<TMP_Text>();
		textTrigger = transform.Find("PanelBackground/PanelContent/PanelControls/TextTrigger").gameObject.GetComponent<TMP_Text>();
		textGrab = transform.Find("PanelBackground/PanelContent/PanelControls/TextGrab").gameObject.GetComponent<TMP_Text>();

		textTitle.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "CONTROLS") + " � ";

		textTitle.text += controllerType switch {
			_ => "Oculus Touch",
		};
	}
	#endregion

	#region M�thodes publiques
	// Va mettre en gras ou non le texte du PrimaryButton.
	public void TextPrimaryButtonHover(bool hover) {
		textPrimaryButton.fontStyle = hover ? FontStyles.Bold : FontStyles.Normal;
	}

	// Va mettre en gras ou non le texte du SecondaryButton.
	public void TextSecondaryButtonHover(bool hover) {
		textSecondaryButton.fontStyle = hover ? FontStyles.Bold : FontStyles.Normal;
	}

	// Va mettre en gras ou non le texte du Stick gauche.
	public void TextLeftThumbstickHover(bool hover) {
		textLeftThumbstick.fontStyle = hover ? FontStyles.Bold : FontStyles.Normal;
	}

	// Va mettre en gras ou non le texte du Stick droit.
	public void TextRightThumbstickHover(bool hover) {
		textRightThumbstick.fontStyle = hover ? FontStyles.Bold : FontStyles.Normal;
	}

	// Va mettre en gras ou non le texte du Trigger.
	public void TextTriggerHover(bool hover) {
		textTrigger.fontStyle = hover ? FontStyles.Bold : FontStyles.Normal;
	}

	// Va mettre en gras ou non le texte du Grab.
	public void TextGrabHover(bool hover) {
		textGrab.fontStyle = hover ? FontStyles.Bold : FontStyles.Normal;
	}
	#endregion
}
