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

/// <summary>
/// Classe qui permet de g�rer une double porte. S'inspire de Door Double Slide de Creepy Cat.
/// </summary>
public class BadgeDoor : MonoBehaviour {
	//Doors
	public Transform doorL = null;
	public Transform doorR = null;

	private Vector3 initialDoorL;
	private Vector3 initialDoorR;
	private Vector3 doorDirection;

	public AudioSource audioSource;
	public BadgeDisplay badgeDisplay;

	public enum Direction { X, Y, Z };
	public Direction directionType = Direction.X;

	//Control Variables
	public float speed = 2.0f;
	public float openDistance = -2.0f;

	public bool keepOpen = false;

	//Internal... stuff
	private float point = 0.0f;
	private bool opening = false;

	//Record initial positions
	private void Start() {
		if (doorL) {
			initialDoorL = doorL.localPosition;
		}

		if (doorR) {
			initialDoorR = doorR.localPosition;
		}
	}

	//Open or close doors
	private void Update() {
		// Direction selection
		if (directionType == Direction.X) {
			doorDirection = Vector3.right;
		}
		else if (directionType == Direction.Y) {
			doorDirection = Vector3.up;
		}
		else if (directionType == Direction.Z) {
			doorDirection = Vector3.back;
		}

		// If opening
		if (opening) {
			point = Mathf.Lerp(point, 1.0f, Time.deltaTime * speed);
		}
		else {
			point = Mathf.Lerp(point, 0.0f, Time.deltaTime * speed);
		}

		// Move doors
		if (doorL) {
			doorL.localPosition = initialDoorL + (openDistance * point * doorDirection);
		}

		if (doorR) {
			doorR.localPosition = initialDoorR + (openDistance * point * -doorDirection);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (opening && !keepOpen && other.gameObject.CompareTag("Badge")) {
			badgeDisplay.DisplayMessageSecurityLvl();
			ActiveDoors(false);
		}
	}

	public void ActiveDoors(bool open) {
		opening = open;

		if (audioSource != null) {
			audioSource.Play();
		}
	}
}
