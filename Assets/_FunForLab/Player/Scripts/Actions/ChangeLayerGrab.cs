/*
 * Copyright 2023 Interreg V EMR 145 - FunForLab
 *
 * Licensed under the EUPL, Version 1.2 or – as soon they will be approved by the European Commission - subsequent versions of the EUPL (the "Licence");
 * You may not use this work except in compliance with the Licence.
 * You may obtain a copy of the Licence at:
 *
 * https://joinup.ec.europa.eu/software/page/eupl
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the Licence is distributed on an "AS IS" basis,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the Licence for the specific language governing permissions and limitations under the Licence.
 * 
 *  Author: Aurélien Bolkaerts
 *  
 *  Project: Interreg V EMR 145 - FunForLab
 *  Website: http://funforlab.eu
 *  Project Date: March 2021 - August 2023
 *  Contributors:
 *      Centre de Recherche des Instituts Groupés de la Haute Ecole Libre Mosane (CRIG):
 *          - Isabelle Bragard
 *          - Birgit Quinting 
 *          - Sonia El Guendi
 *          - Annabelle Lejeune
 *          - Ingrid Hamer
 *          - Mélanie Zenner
 *          - Jérome Foguenne
 *      Centre de recherche et de formation continue de la Haute Ecole Namur Liège Luxembourg (FoRS):
 *          - Julien Lecointre
 *          - Simon Daniau
 *          - Laura Ramonfosse
 *          - Christophe Clément
 *          - Amandine Schreiber
 *      Ausbildungsakademie für Gesundheitsberufe, Uniklinik RWTH Aachen (UKAachen):
 *          - Eva Schönen
 *          - Giannina Lindt
 *          - Patricia Büts
 *          - Silvia Schneiders
 *          - Miriam Scheld
 *          - Monika Krichel-Frings
 *          - Christiane  Stickelmann
 *      Centre de Coopération Technique et Pédagogique (CeCoTePe):
 *          - Frédéric Kotnik
 *          - Brian Deschamps
 *          - Mélanie Zenner
 *          - Aurélien Bolkaerts
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
/// Classe qui va changer le layer d'interaction de l'objet grab, afin de ne pas être bloqué lors d'un déplacement par l'objet qu'on tient. Va aussi gérer l'affichage de la manette pour le tuto.
/// </summary>
public class ChangeLayerGrab : MonoBehaviour {
	private XRDirectInteractor xRDirectInteractor;
	private XRGrabInteractable itemGrabbed;
	private int initialItemLayer;

	// TODO Serait mieux de séparer l'affichage de la manette dans un autre composant
	public bool showController = false; // Afficher la manette dans la main
	public GameObject controller; // La manette
	public SkinnedMeshRenderer meshRenderer; // Le SkinnedMeshRenderer de la main
	public Material solidMaterial; // Le Material opaque quand on grab avec la main, utilisé que si showController est à true
	public Material transparentMaterial; // Le Material transparent quand on ne grab pas avec la main, utilisé que si showController est à true

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

	#region Méthodes privées
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
			// TODO Lors du runtime, les controllers sont dupliqués ?! [LeftHand Controller] Model Parent et [LeftHand Controller] Attach ! C'est pour ça que 2 mains apparaissent ! POURQUOI ?!
		}
	}
	#endregion

	#region Action listeners
	// Action lorsque l'on commence à grab un objet. Va faire en sorte que les objets grab entrent dans le layer 9 Ignore Body Collisions.
	public void GrabBegin() {
		itemGrabbed = xRDirectInteractor.GetOldestInteractableSelected() as XRGrabInteractable;

		initialItemLayer = itemGrabbed.gameObject.layer;
		itemGrabbed.gameObject.layer = 9; // Layer 9 = Ignore Body Collision

		Transform[] children = itemGrabbed.GetComponentsInChildren<Transform>(true); // Inclure les objets inactifs

		foreach (Transform child in children) { // Faire pareil pour tous les enfants récursivement
			child.gameObject.layer = 9;
		}

		isGrabbing = true;

		HideController();
	}

	// Action lorsque l'on arrête de grab un objet. Va remettre le layer d'origine de l'objet grab une fois dégrab.
	public void GrabEnd() {
		itemGrabbed.gameObject.layer = initialItemLayer;

		Transform[] children = itemGrabbed.GetComponentsInChildren<Transform>(true); // Inclure les objets inactifs

		foreach (Transform child in children) { // Faire pareil pour tous les enfants récursivement
			child.gameObject.layer = initialItemLayer;
		}

		isGrabbing = false;
		ShowController();
	}
	#endregion
}
