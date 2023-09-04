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
/// Classe qui va forcer le d�grab d'un objet si on est � une certaine distance de lui.
/// </summary>
public class ForceDegrab : MonoBehaviour {
	private XRGrabInteractable grabInteractable;
	private XRDirectInteractor interactor;

	public Transform handler; // Objet que l'on va grab
	public float distance = 0.25f; // Distance avant de forcer le d�grab

	#region Messages Unity
	private void Start() {
		grabInteractable = GetComponent<XRGrabInteractable>();
		interactor = null;
	}

	private void Update() {
		if (interactor != null && Vector3.Distance(handler.position, interactor.gameObject.transform.position) > distance) {
			interactor.allowSelect = false; // Forcer le fait de d�tacher l'objet
			interactor = null;
		}
	}
	#endregion

	#region Action listeners
	// Action lorsque que l'on commence � grab. Va m�moriser l'interactor qui a grab l'objet.
	public void GrabBegin() {
		interactor = (XRDirectInteractor) grabInteractable.GetOldestInteractorSelecting();
	}

	// Action lorsque que l'on commence � grab. Va directement d�grab pour ne pas maintenir l'objet en main.
	public void GrabAutoEnd() {
		interactor = (XRDirectInteractor) grabInteractable.GetOldestInteractorSelecting();
		interactor.allowSelect = false;
		interactor = null;
	}

	// Action lorsque que l'on arr�te de grab. Va oublier l'interactor qui a grab l'objet.
	public void GrabEnd() {
		interactor = null;
	}
	#endregion
}
