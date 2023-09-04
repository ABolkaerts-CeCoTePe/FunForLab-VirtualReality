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
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe qui va modifier la rotation de l'objet pour s'accorder à celle des manettes du joueur. Utilise le nouveau Input Manager basé sur les Actions.
/// </summary>
public class ControllerUIAction : MonoBehaviour {
	public ActionBasedController controller; // Le contrôleur de la main
	public XRNode hand; // Main gauche ou droite ?
	public InputActionReference actionPrimaryButton; // Référence de l'input du bouton principal (A ou X)
	public InputActionReference actionSecondaryButton; // Référence de l'input du bouton secondaire (B ou Y)
	public InputActionReference actionThumbstick; // Référence de l'input du stick
	public InputActionReference actionTrigger; // Référence de l'input du bouton trigger
	public InputActionReference actionGrip; // Référence de l'input du bouton grip

	public DisplayControllers displayControllers; // UI qui va afficher le rendu des manettes
	public GameObject hoverPrimaryButton; // Emplacement du hover du bouton principal
	public GameObject hoverSecondaryButton; // Emplacement du hover du bouton secondaire
	public GameObject hoverThumbstick; // Emplacement du hover du stick
	public GameObject hoverTrigger; // Emplacement du hover du bouton trigger
	public GameObject hoverGrip; // Emplacement du hover du bouton grip

	#region Messages Unity
	private void OnEnable() {
		controller.rotationAction.action.performed += EventRotation; // Récupérer la rotation de la manette

		actionPrimaryButton.action.performed += EventPrimaryButton;
		actionPrimaryButton.action.canceled += EventPrimaryButton;

		actionSecondaryButton.action.performed += EventSecondaryButton;
		actionSecondaryButton.action.canceled += EventSecondaryButton;

		actionThumbstick.action.performed += EventThumbstickPerformed;
		actionThumbstick.action.canceled += EventThumbstickCanceled;

		actionTrigger.action.performed += EventTrigger;
		actionTrigger.action.canceled += EventTrigger;

		actionGrip.action.performed += EventGrab;
		actionGrip.action.canceled += EventGrab;
	}

	private void OnDisable() {
		controller.rotationAction.action.performed -= EventRotation;

		actionPrimaryButton.action.performed -= EventPrimaryButton;
		actionPrimaryButton.action.canceled -= EventPrimaryButton;

		actionSecondaryButton.action.performed -= EventSecondaryButton;
		actionSecondaryButton.action.canceled -= EventSecondaryButton;

		actionThumbstick.action.performed -= EventThumbstickPerformed;
		actionThumbstick.action.canceled -= EventThumbstickCanceled;

		actionTrigger.action.performed -= EventTrigger;
		actionTrigger.action.canceled -= EventTrigger;

		actionGrip.action.performed -= EventGrab;
		actionGrip.action.canceled -= EventGrab;
	}

	private void Start() {
		hoverPrimaryButton.SetActive(false);
		hoverSecondaryButton.SetActive(false);
		hoverThumbstick.SetActive(false);
		hoverTrigger.SetActive(false);
		hoverGrip.SetActive(false);
	}
	#endregion

	#region Méthodes privées
	// Va modifier la rotation de l'objet.
	private void ControllerRotation(Quaternion rotation) {
		transform.rotation = rotation;
	}
	#endregion

	#region Action listeners
	// Action lors d'un event de Rotation. Va modifier la rotation de la manette.
	private void EventRotation(InputAction.CallbackContext ctx) {
		ControllerRotation(ctx.ReadValue<Quaternion>());
	}

	// Action lors d'un event PrimaryButton. Va activer ou non le Hover du bouton.
	private void EventPrimaryButton(InputAction.CallbackContext ctx) {
		bool value = ctx.ReadValueAsButton();
		hoverPrimaryButton.SetActive(value);
		displayControllers.TextPrimaryButtonHover(value);
	}

	// Action lors d'un event SecondaryButton. Va activer ou non le Hover du bouton.
	private void EventSecondaryButton(InputAction.CallbackContext ctx) {
		bool value = ctx.ReadValueAsButton();
		hoverSecondaryButton.SetActive(value);
		displayControllers.TextSecondaryButtonHover(value);
	}

	// Action lors d'un event Thumbstick commence. Va activer le Hover du stick.
	private void EventThumbstickPerformed(InputAction.CallbackContext ctx) {
		Vector2 v = ctx.ReadValue<Vector2>();

		if (Mathf.Abs(v.x) > InputSystem.settings.defaultDeadzoneMin || Mathf.Abs(v.y) > InputSystem.settings.defaultDeadzoneMin) { // N'activer le hover que si on dépasse la zone morte de la manette
			hoverThumbstick.SetActive(true);

			if (hand == XRNode.LeftHand) {
				displayControllers.TextLeftThumbstickHover(true);
			}
			else {
				displayControllers.TextRightThumbstickHover(true);
			}

		}
		else {
			hoverThumbstick.SetActive(false);

			if (hand == XRNode.LeftHand) {
				displayControllers.TextLeftThumbstickHover(false);
			}
			else {
				displayControllers.TextRightThumbstickHover(false);
			}
		}
	}

	// Action lors d'un event Thumbstick se termine. Va désactiver le Hover du stick.
	private void EventThumbstickCanceled(InputAction.CallbackContext ctx) {
		hoverThumbstick.SetActive(false);

		if (hand == XRNode.LeftHand) {
			displayControllers.TextLeftThumbstickHover(false);
		}
		else {
			displayControllers.TextRightThumbstickHover(false);
		}
	}

	// Action lors d'un event Trigger. Va activer ou non le Hover du bouton.
	private void EventTrigger(InputAction.CallbackContext ctx) {
		bool value = ctx.ReadValueAsButton();
		hoverTrigger.SetActive(value);
		displayControllers.TextTriggerHover(value);
	}

	// Action lors d'un event Grip. Va activer ou non le Hover du bouton.
	private void EventGrab(InputAction.CallbackContext ctx) {
		bool value = ctx.ReadValueAsButton();
		hoverGrip.SetActive(value);
		displayControllers.TextGrabHover(value);
	}
	#endregion
}
