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
using RootMotion.FinalIK;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe �tant le script principal des controlleurs, va g�rer les diff�rents inputs, t�l�portation, etc. Utilise le nouveau Input Manager bas� sur les Actions.
/// </summary>
public class MasterControllerAction : MonoBehaviour {
	private static MasterControllerAction instance = null; // Une instance sous forme de singleton, afin d'en avoir seulement un seul
	public static MasterControllerAction Instance {
		get { return instance; }
	}
	
	private XROrigin origin; // Une instance sous forme de singleton, afin d'en avoir seulement un seul
	public XROrigin Origin {
		get { return origin; }
	}

	private ActionBasedController leftController;
	private ActionBasedController rightController;
	private XRInteractorLineVisual leftLineVisual;
	private XRInteractorLineVisual rightLineVisual;
	private XRInteractorLineVisual leftUILineVisual;
	private XRInteractorLineVisual rightUILineVisual;
	private UIInteractorAction leftUIInteractorAction;
	private UIInteractorAction rightUIInteractorAction;
	private ChangeLayerGrab leftChangeLayerGrab;
	private ChangeLayerGrab rightChangeLayerGrab;

	private InteractionLayerMask originalLeftMask;
	private InteractionLayerMask originalRightMask;

	public XRInteractionManager interactionManager; // Le composant XR Interaction Manager
	public ActionBasedController leftHandController;
	public ActionBasedController rightHandController;
	public XRRayInteractor leftTeleportInteractor; // L'interactor de t�l�portation de la main gauche
	public XRRayInteractor rightTeleportInteractor; // L'interactor de t�l�portation de la main droite
	public XRRayInteractor leftUIInteractor; // L'interactor de l'UI de la main gauche
	public XRRayInteractor rightUIInteractor; // L'interactor de l'UI de la main droite

	[HideInInspector]
	public IKSolverVR solver;

	#region Messages Unity
	private void Awake() {
		instance = this;
		origin = GetComponent<XROrigin>();

		leftController = leftTeleportInteractor.GetComponent<ActionBasedController>();
		rightController = rightTeleportInteractor.GetComponent<ActionBasedController>();

		leftLineVisual = leftTeleportInteractor.GetComponent<XRInteractorLineVisual>();
		leftLineVisual.enabled = false;
		rightLineVisual = rightTeleportInteractor.GetComponent<XRInteractorLineVisual>();
		rightLineVisual.enabled = false;

		leftUILineVisual = leftUIInteractor.GetComponent<XRInteractorLineVisual>();
		rightUILineVisual = rightUIInteractor.GetComponent<XRInteractorLineVisual>();

		leftUIInteractorAction = leftUIInteractor.GetComponent<UIInteractorAction>();
		rightUIInteractorAction = rightUIInteractor.GetComponent<UIInteractorAction>();

		leftChangeLayerGrab = leftHandController.GetComponent<ChangeLayerGrab>();
		rightChangeLayerGrab = rightHandController.GetComponent<ChangeLayerGrab>();

		originalLeftMask = leftTeleportInteractor.interactionLayers;
		originalRightMask = rightTeleportInteractor.interactionLayers;

		// D�sactiver au lancement le layer de t�l�portation
		leftTeleportInteractor.interactionLayers = new InteractionLayerMask();
		rightTeleportInteractor.interactionLayers = new InteractionLayerMask();
	}

	private void OnEnable() {
		leftController.selectAction.action.performed += EventSelectActionLeftTeleportRay;
		leftController.selectAction.action.canceled += EventSelectActionLeftTeleport;

		rightController.selectAction.action.performed += EventSelectActionRightTeleportRay;
		rightController.selectAction.action.canceled += EventSelectActionRightTeleport;
	}

	private void OnDisable() {
		leftController.selectAction.action.performed -= EventSelectActionLeftTeleportRay;
		leftController.selectAction.action.canceled -= EventSelectActionLeftTeleport;

		rightController.selectAction.action.performed -= EventSelectActionRightTeleportRay;
		rightController.selectAction.action.canceled -= EventSelectActionRightTeleport;
	}

	private void Start() {
		XRSettings.eyeTextureResolutionScale = 1.2f; // Augmenter l�g�rement la r�solution des textures afin que le rendu soit meilleur

		// Pour fixer temporairement la langue en Anglais pour faire une vid�o de d�mo par exemple
		/*PlayerPrefsManager.Language = PlayerPrefsManager.Lang.English;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int) PlayerPrefsManager.Lang.English];
		Debug.Log("Locale changed : " + LocalizationSettings.AvailableLocales.Locales[(int) PlayerPrefsManager.Lang.English].LocaleName);*/
	}

	private void Update() {
		if (PlayerPrefsManager.ControlsSimple) {
			if (leftLineVisual.enabled == true && leftChangeLayerGrab.isGrabbing) {
				leftLineVisual.enabled = false; // D�sactiver le laser de t�l�portation si on tient un objet en main
				leftTeleportInteractor.interactionLayers = new InteractionLayerMask();
			}
			else if (rightLineVisual.enabled == true && rightChangeLayerGrab.isGrabbing) {
				rightLineVisual.enabled = false;
				rightTeleportInteractor.interactionLayers = new InteractionLayerMask();
			}

			if (leftUIInteractorAction.isPointingUi && leftChangeLayerGrab.isGrabbing) {
				leftUILineVisual.enabled = false; // D�sactiver le laser de l'UI si on tient un objet en main
				leftUIInteractor.enabled = false;
			}
			else if (rightUIInteractorAction.isPointingUi && rightChangeLayerGrab.isGrabbing) {
				rightUILineVisual.enabled = false;
				rightUIInteractor.enabled = false;
			}
		}
	}
	#endregion

	#region M�thodes publiques
	// Va activer ou non le fait de pouvoir se t�l�porter.
	public void TeleportationActive(bool teleport) {
		if (teleport) {
			leftController.selectAction.action.Disable();
			rightController.selectAction.action.Disable();
		}
		else {
			leftController.selectAction.action.Enable();
			rightController.selectAction.action.Enable();
		}
	}

	// Va afficher le raycast de t�l�portation de la main gauche en changeant son layer, et d�sactiver l'interaction avec l'UI.
	public void LeftTeleportRay() {
		if (!leftUIInteractorAction.isPointingUi || !PlayerPrefsManager.ControlsSimple) { // Ne pas activer le raycast de t�l�portation si on pointe actuellement une UI avec les contr�les simples
			leftLineVisual.enabled = true;
			leftUILineVisual.enabled = false; // D�sactiver le laser de l'UI si le bouton pour se t�l�porter est press�
			leftUIInteractor.enabled = false;

			leftTeleportInteractor.interactionLayers = originalLeftMask;
		}
	}

	// Va activer la t�l�portation de la main gauche et remettre les layers comme avant, et r�activer l'interaction avec l'UI.
	public void LeftTeleport() {
		leftLineVisual.enabled = false;
		leftUILineVisual.enabled = true; // R�activer le laser de l'UI si le bouton pour se t�l�porter est rel�ch�
		leftUIInteractor.enabled = true;

		IXRSelectInteractable interactable = (IXRSelectInteractable) leftTeleportInteractor.GetOldestInteractableHovered();
		leftTeleportInteractor.interactionLayers = new InteractionLayerMask();

		if (interactable is BaseTeleportationInteractable) {
			interactionManager.SelectEnter(leftTeleportInteractor, interactable);
		}
	}

	// Va afficher le raycast de t�l�portation de la main droite en changeant son layer, et d�sactiver l'interaction avec l'UI.
	public void RightTeleportRay() {
		if (!rightUIInteractorAction.isPointingUi || !PlayerPrefsManager.ControlsSimple) { // Ne pas activer le raycast de t�l�portation si on pointe actuellement une UI avec les contr�les simples
			rightLineVisual.enabled = true;
			rightUILineVisual.enabled = false; // D�sactiver le laser de l'UI si le bouton pour se t�l�porter est press�
			rightUIInteractor.enabled = false;

			rightTeleportInteractor.interactionLayers = originalRightMask;
		}
	}

	// Va activer la t�l�portation de la main droite et remettre les layers comme avant, et r�activer l'interaction avec l'UI.
	public void RightTeleport() {
		rightLineVisual.enabled = false;
		rightUILineVisual.enabled = true; // R�activer le laser de l'UI si le bouton pour se t�l�porter est rel�ch�
		rightUIInteractor.enabled = true;

		IXRSelectInteractable interactable = (IXRSelectInteractable) rightTeleportInteractor.GetOldestInteractableHovered();
		rightTeleportInteractor.interactionLayers = new InteractionLayerMask();

		if (interactable is BaseTeleportationInteractable) {
			interactionManager.SelectEnter(rightTeleportInteractor, interactable);
		}
	}
	#endregion

	#region Action listeners
	// Action lors d'un event Select commence. Va activer le fait de pouvoir se t�l�porter.
	private void EventSelectActionLeftTeleportRay(InputAction.CallbackContext ctx) {
		LeftTeleportRay();
	}

	// Action lors d'un event Select se termine. Va d�sactiver le fait de pouvoir se t�l�porter.
	private void EventSelectActionLeftTeleport(InputAction.CallbackContext ctx) {
		LeftTeleport();
	}

	// Action lors d'un event Select commence. Va activer le fait de pouvoir se t�l�porter.
	private void EventSelectActionRightTeleportRay(InputAction.CallbackContext ctx) {
		RightTeleportRay();
	}

	// Action lors d'un event Select se termine. Va d�sactiver le fait de pouvoir se t�l�porter.
	private void EventSelectActionRightTeleport(InputAction.CallbackContext ctx) {
		RightTeleport();
	}
	#endregion
}
