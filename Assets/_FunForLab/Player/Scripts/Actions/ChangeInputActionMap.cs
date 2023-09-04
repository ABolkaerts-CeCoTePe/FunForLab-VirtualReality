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
using UnityEngine.InputSystem;

/// <summary>
/// Classe qui permet le changement entre les contr�les normaux et simples.
/// </summary>
public class ChangeInputActionMap : MonoBehaviour {
	public InputActionAsset inputActionAssetMain;

	#region Messages Unity
	private void Start() {
		if (PlayerPrefsManager.ControlsSimple) {
			Debug.Log("Start Controls Simple");
			SwitchControlsToSimple();
		}
		else {
			Debug.Log("Start Controls Normal");
			SwitchControlsToNormal();
		}
	}
	#endregion

	#region M�thodes publiques
	// Va passer les contr�les en simple.
	public void SwitchControlsToSimple() {
		inputActionAssetMain["XRI LeftHand/Select"].ApplyBindingOverride("<XRController>{LeftHand}/triggerPressed"); // gripPressed de base
		inputActionAssetMain["XRI LeftHand/Select Value"].ApplyBindingOverride("<XRController>{LeftHand}/trigger"); // grip de base
		inputActionAssetMain["XRI LeftHand/Primary Button"].ApplyBindingOverride("<XRController>{LeftHand}/triggerButton"); // primaryButton de base

		inputActionAssetMain["XRI RightHand/Select"].ApplyBindingOverride("<XRController>{RightHand}/triggerPressed");
		inputActionAssetMain["XRI RightHand/Select Value"].ApplyBindingOverride("<XRController>{RightHand}/trigger");
		inputActionAssetMain["XRI RightHand/Primary Button"].ApplyBindingOverride("<XRController>{RightHand}/triggerButton");

		PlayerPrefsManager.ControlsSimple = true;
	}

	// Va passer les contr�les en complexes (normal).
	public void SwitchControlsToNormal() {
		inputActionAssetMain["XRI LeftHand/Select"].RemoveAllBindingOverrides(); // Enlever les modifications qu'on a fait en Simple
		inputActionAssetMain["XRI LeftHand/Select Value"].RemoveAllBindingOverrides();
		inputActionAssetMain["XRI LeftHand/Primary Button"].RemoveAllBindingOverrides();

		inputActionAssetMain["XRI RightHand/Select"].RemoveAllBindingOverrides();
		inputActionAssetMain["XRI RightHand/Select Value"].RemoveAllBindingOverrides();
		inputActionAssetMain["XRI RightHand/Primary Button"].RemoveAllBindingOverrides();

		PlayerPrefsManager.ControlsSimple = false;
	}
	#endregion
}
