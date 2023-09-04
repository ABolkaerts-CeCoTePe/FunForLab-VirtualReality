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
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

/// <summary>
/// Classe qui g�re l'affichage d'un scanner de badge pour une porte. Va d�tecter si un badge est � proximit� et ouvrir ou non la porte.
/// </summary>
public class BadgeDisplay : MonoBehaviour {
	public TMP_Text[] textMessages;
	public Image[] panelMessages;

	public BadgeDoor badgeDoor;
	public int securityLvl;

	public bool keepDoorOpen = false; // Laisser la porte ouverte ?

	public TableReference stringTable;

	public UnityEvent onDoorOpen = new UnityEvent();

	private bool accesGranted = false;
	private TeleportRobot teleportRobot = null;

	private void Awake() {
		teleportRobot = GetComponent<TeleportRobot>();
	}

	private void Start() {
		DisplayMessageSecurityLvl();

		if (keepDoorOpen) {
			badgeDoor.keepOpen = true;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Badge")) {
			Debug.Log("Badge trigger with : " + other.gameObject);

			BadgeController badgeController = other.gameObject.GetComponent<BadgeController>();

			if (badgeController != null && badgeController.securityLvl >= securityLvl) {


				AccessGranted();
			}
			else {
				//DisplayMessageAccesRefused();
			}
		}
	}

    private void AccessGranted()
    {
        accesGranted = true;
        //DisplayMessageAccesGranted();
        badgeDoor.ActiveDoors(true);

        onDoorOpen?.Invoke(); // Appeler l'event (pour d�sactiver la fl�che de direction)

        //QuestLog.SetQuestEntryState("QuestTutorial", 1, QuestState.Success); // TODO Voir s'il n'y a pas moyen d'avoir un composant � part pour faire cela
        DialogueManager.ShowAlert(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "DOOR_OPEN"));

        if (teleportRobot != null)
        {
            teleportRobot.TeleportRobotToPos();
        }
    }

    private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Badge") && !accesGranted) {
			DisplayMessageSecurityLvl();
		}
	}

	public bool IsDoorOpen {
		get { return accesGranted; }
	}

	// Va afficher le message indiquant le niveau de s�curit�.
	public void DisplayMessageSecurityLvl() {
		foreach (Image panelMessage in panelMessages) {
			panelMessage.color = new Color(255, 255, 255, 0.12f);
		}
		
		foreach (TMP_Text textMessage in textMessages) {
			textMessage.text = LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "SECURITY_LVL", arguments: new object[] { "<size=384><color=\"red\">" + securityLvl + "</color></size>" });
		}
		
		accesGranted = false;
	}
/*
	// Va afficher le message indiquant que l'acc�s est autoris�.
	public void DisplayMessageAccesGranted() {
		foreach (Image panelMessage in panelMessages) {
			panelMessage.color = new Color(0, 255, 0, 127);
		}

		foreach (TMP_Text textMessage in textMessages) {
			textMessage.text = "<b>" + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "AUTHORIZED_ACCESS") + "</b>";
		}
	}

	// Va afficher le message indiquant que l'acc�s est refus�.
	public void DisplayMessageAccesRefused() {
		foreach (Image panelMessage in panelMessages) {
			panelMessage.color = new Color(255, 0, 0, 127);
		}

		foreach (TMP_Text textMessage in textMessages) {
			textMessage.text = "<b>" + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "ACCESS_DENIED") + "</b>";
		}
	}
*/
}
