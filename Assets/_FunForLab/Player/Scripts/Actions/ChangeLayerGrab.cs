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
/// Classe qui va changer le layer d'interaction de l'objet grab, afin de ne pas �tre bloqu� lors d'un d�placement par l'objet qu'on tient. Va aussi g�rer l'affichage de la manette pour le tuto.
/// </summary>
public class ChangeLayerGrab : MonoBehaviour {
	private XRDirectInteractor xRDirectInteractor;
	private XRGrabInteractable itemGrabbed;
	private int initialItemLayer;

	// TODO Serait mieux de s�parer l'affichage de la manette dans un autre composant
	public bool showController = false; // Afficher la manette dans la main
	public GameObject controller; // La manette
	public SkinnedMeshRenderer meshRenderer; // Le SkinnedMeshRenderer de la main
	public Material solidMaterial; // Le Material opaque quand on grab avec la main, utilis� que si showController est � true
	public Material transparentMaterial; // Le Material transparent quand on ne grab pas avec la main, utilis� que si showController est � true

	[HideInInspector]
	public bool isGrabbing;

	#region Messages Unity
	private void Awake() {
		xRDirectInteractor = GetComponent<XRDirectInteractor>();
		isGrabbing = false;
	}

	private void Start() {
		if (showController) {
			ShowController();
		}
		else if (controller != null) {
			controller.SetActive(false); // Juste masquer la manette, ne pas rendre la main transparente
		}
	}
	#endregion

	#region M�thodes priv�es
	// Va masquer la manette et remettre la main en opaque.
	private void HideController() {
		if (showController) {
			controller.SetActive(false);
			meshRenderer.materials = (new Material[] { solidMaterial });
		}
	}

	// Va afficher la manette et mettre la main en transparent.
	private void ShowController() {
		if (showController) {
			controller.SetActive(true);
			meshRenderer.materials = (new Material[] { transparentMaterial });
			// TODO Lors du runtime, les controllers sont dupliqu�s ?! [LeftHand Controller] Model Parent et [LeftHand Controller] Attach ! C'est pour �a que 2 mains apparaissent ! POURQUOI ?!
		}
	}
	#endregion

	#region Action listeners
	// Action lorsque l'on commence � grab un objet. Va faire en sorte que les objets grab entrent dans le layer 9 Ignore Body Collisions.
	public void GrabBegin() {
		itemGrabbed = xRDirectInteractor.GetOldestInteractableSelected() as XRGrabInteractable;

		initialItemLayer = itemGrabbed.gameObject.layer;
		itemGrabbed.gameObject.layer = 9; // Layer 9 = Ignore Body Collision

		Transform[] children = itemGrabbed.GetComponentsInChildren<Transform>(true); // Inclure les objets inactifs

		foreach (Transform child in children) { // Faire pareil pour tous les enfants r�cursivement
			child.gameObject.layer = 9;
		}

		isGrabbing = true;

		HideController();
	}

	// Action lorsque l'on arr�te de grab un objet. Va remettre le layer d'origine de l'objet grab une fois d�grab.
	public void GrabEnd() {
		itemGrabbed.gameObject.layer = initialItemLayer;

		Transform[] children = itemGrabbed.GetComponentsInChildren<Transform>(true); // Inclure les objets inactifs

		foreach (Transform child in children) { // Faire pareil pour tous les enfants r�cursivement
			child.gameObject.layer = initialItemLayer;
		}

		isGrabbing = false;
		ShowController();
	}
	#endregion
}
