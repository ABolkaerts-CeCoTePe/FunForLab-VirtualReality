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
/// Classe qui va modifier l'attachTransform soit de l'interactor soit de l'interactable afin de correspondre � la main qui a grab.
/// </summary>
public class GrabAttachTransform : MonoBehaviour {
	private XRGrabInteractable xRGrabInteractable;
	private XRDirectInteractor xRDirectInteractor;

	// L'�l�ment dont le point d'attache sera chang�
	public enum ChangeElement {
		Interactor, // Les mains
		Interactable // Les objets � grab (fioles, rack, r�actifs, ...)
	}

	public Transform leftHandAttachPoint; // Point d'attache qui sera utilis� pour la main gauche.
	public Transform rightHandAttachPoint; // Point d'attache qui sera utilis� pour la main droite.
	public ChangeElement elementToChange; // On change l'interactor ou l'interactable ?

	#region Messages Unity
	private void Awake() {
		xRGrabInteractable = GetComponent<XRGrabInteractable>();
	}
	#endregion

	#region Action listeners
	// Action lorsque l'on commence � grab un objet. Va changer l'attachTransform soit de la main, soit d'un interactable par celui qui est fix�.
	public void ChangeAttachTransform() {
		// V�rifier si c'est bien une main qui a pris l'interactable
		if (xRGrabInteractable.GetOldestInteractorSelecting() is XRDirectInteractor interactor && interactor.CompareTag("Player")) {
			xRDirectInteractor = interactor;

			if (xRDirectInteractor.name == "LeftHand Controller") {
				if (elementToChange == ChangeElement.Interactor) {
					xRDirectInteractor.attachTransform = leftHandAttachPoint; // Changer l'attachTransform de la main par celui fix� (sera celui dans l'interactable)
				}
				else {
					xRGrabInteractable.attachTransform = leftHandAttachPoint; // Changer l'attachTransform de l'interactable par celui fix� (sera celui de la main)
				}
			}
			else {
				if (elementToChange == ChangeElement.Interactor) {
					xRDirectInteractor.attachTransform = rightHandAttachPoint;
				}
				else {
					xRGrabInteractable.attachTransform = rightHandAttachPoint;
				}
			}
		}
	}

	// Action lorsque l'on arr�te de grab un objet. Va retirer l'attachTransform fix�.
	public void ResetAttachTransform() {
		if (elementToChange == ChangeElement.Interactor && xRDirectInteractor != null) {
			xRDirectInteractor.attachTransform = null; // Enlever l'attachTransform qu'on a mis sur la main
		}
		else if (elementToChange == ChangeElement.Interactable && xRGrabInteractable != null) {
			xRGrabInteractable.attachTransform = null; // Enlever l'attachTransform qu'on a mis sur l'interactable
		}
	}
	#endregion
}
