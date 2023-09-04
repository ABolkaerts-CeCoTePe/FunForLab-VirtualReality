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
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe g�rant le changement du syst�me de locomotion entre les diff�rents types de cam�ra et de mouvement.
/// </summary>
public class LocomotionController : MonoBehaviour {
	private ActionBasedContinuousTurnProvider actionBasedContinuousTurnProvider;
	private ActionBasedSnapTurnProvider actionBasedSnapTurnProvider;
	private ActionBasedContinuousMoveProvider actionBasedContinuousMoveProvider;

	#region Messages Unity
	private void Awake() {
		actionBasedContinuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
		actionBasedSnapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
		actionBasedContinuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
	}

	private void Start() {
		ChangeCameraMode();
		ChangeMove();
	}
	#endregion

	#region M�thodes publiques
	// Va changer le mode de fonctionnement de la cam�ra.
	public void ChangeCameraMode() {
		Debug.Log("Camera mode : " + PlayerPrefsManager.CameraMode);

		switch (PlayerPrefsManager.CameraMode) {
			case PlayerPrefsManager.CameraModes.Disabled:
				actionBasedContinuousTurnProvider.enabled = false;
				actionBasedSnapTurnProvider.enabled = false;
				break;
			case PlayerPrefsManager.CameraModes.Snap:
				actionBasedContinuousTurnProvider.enabled = false;
				actionBasedSnapTurnProvider.enabled = true;
				break;
			case PlayerPrefsManager.CameraModes.Continuous:
				actionBasedContinuousTurnProvider.enabled = true;
				actionBasedSnapTurnProvider.enabled = false;
				break;
		}
	}

	// Va changer le type de mouvement entre libre ou non.
	public void ChangeMove() {
		Debug.Log("Free move : " + PlayerPrefsManager.FreeMove);
		actionBasedContinuousMoveProvider.enabled = PlayerPrefsManager.FreeMove;
	}
	#endregion
}
