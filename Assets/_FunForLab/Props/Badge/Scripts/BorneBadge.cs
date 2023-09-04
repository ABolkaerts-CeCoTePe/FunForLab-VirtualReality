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
using UnityEngine.UI;

/// <summary>
/// Classe qui g�re l'affichage d'une borne pour augmenter le niveau de s�curit� d'un badge.
/// </summary>
public class BorneBadge : MonoBehaviour {
	public TMP_Text textMessage;
	public Button buttonConfirm;

	public TableReference stringTable;

	private BadgeController currentBadge;

	private void Start() {
		DisplayMessageBase();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Badge")) {
			Debug.Log("Badge trigger with : " + other.gameObject);

			currentBadge = other.gameObject.GetComponent<BadgeController>();

			if (currentBadge != null) {
				textMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BORNE_SECURITY_MESSAGE", arguments: new object[] { "<b>" + currentBadge.securityLvl + "</b>", "<b>" + (currentBadge.securityLvl + 1) + "</b>" });
				buttonConfirm.interactable = true;
			}
		}

	}

	private void OnTriggerExit(Collider other) {
		DisplayMessageBase();
	}

	// Va afficher le message de base indiquant qu'il faut pr�senter un badge.
	public void DisplayMessageBase() {
		textMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BORNE_DEFAULT_MESSAGE");
		buttonConfirm.interactable = false;
		currentBadge = null;
	}

	// Va augmenter le niveau de s�curit� du badge d'1 cran et afficher le message correspondant.
	public void IncrementSecurityLvl() {
		if (currentBadge != null) {
			Debug.Log("Badge " + currentBadge + " security lvl augment� � " + (currentBadge.securityLvl + 1));

			currentBadge.securityLvl++;
			currentBadge.MessageBadge();

			textMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "BORNE_COMPLETE_MESSAGE", arguments: new object[] { "<b>" + currentBadge.securityLvl + "</b>" });
			buttonConfirm.interactable = false;
		}
	}
}
