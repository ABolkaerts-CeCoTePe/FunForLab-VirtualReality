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
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe qui gère les animations des mains. Utilise le nouveau Input Manager basé sur les Actions.
/// </summary>
public class HandAnimAction : MonoBehaviour {
	private int animParamIndexFlex;
	private int animParamIndexPinch;
	private float gripState = 0f;
	private float triggerState = 0f;

	public const string ANIM_PARAM_NAME_FLEX = "Flex";
	public const string ANIM_PARAM_NAME_PINCH = "Pinch";

	public ActionBasedController controller; // Contrôleur à partir duquel on va récupérer les inputs
	public Animator animator; // Animator de la main
	public float animFrames = 4f; // Nombre de frames pour l'animation

	#region Messages Unity
	private void OnEnable() {
		controller.selectActionValue.action.performed += EventHandGrip; // S'enregistrer à l'event de l'action
		controller.selectActionValue.action.canceled += EventResetHandGrip; // Quand le bouton est relaché
		controller.activateActionValue.action.performed += EventHandTrigger;
		controller.activateActionValue.action.canceled += EventResetHandTrigger;
	}

	private void OnDisable() {
		controller.selectActionValue.action.performed -= EventHandGrip;
		controller.selectActionValue.action.canceled -= EventResetHandGrip;
		controller.activateActionValue.action.performed -= EventHandTrigger;
		controller.activateActionValue.action.canceled -= EventResetHandTrigger;
	}

	private void Start() {
		Collider[] colliders = GetComponentsInChildren<Collider>().Where(childCollider => !childCollider.isTrigger).ToArray();

		foreach (Collider collider in colliders) {
			collider.enabled = true;
		}

		animParamIndexFlex = Animator.StringToHash(ANIM_PARAM_NAME_FLEX);
		animParamIndexPinch = Animator.StringToHash(ANIM_PARAM_NAME_PINCH);
	}
	#endregion

	#region Méthodes privées
	// Va faire l'animation de Grab de la main.
	private void HandGrip(float grip) {
		float gripStateDelta = grip - gripState;

		if (gripStateDelta > 0f) {
			gripState = Mathf.Clamp(gripState + 1 / animFrames, 0f, grip);
		}
		else if (gripStateDelta < 0f) {
			gripState = Mathf.Clamp(gripState - 1 / animFrames, grip, 1);
		}
		else {
			gripState = grip;
		}

		animator.SetFloat(animParamIndexFlex, gripState);
	}

	// Va faire l'animation de Trigger de la main.
	private void HandTrigger(float trigger) {
		float triggerStateDelta = trigger - triggerState;

		if (triggerStateDelta > 0f) {
			triggerState = Mathf.Clamp(triggerState + 1 / animFrames, 0f, trigger);
		}
		else if (triggerStateDelta < 0f) {
			triggerState = Mathf.Clamp(triggerState - 1 / animFrames, trigger, 1);
		}
		else {
			triggerState = trigger;
		}

		animator.SetFloat(animParamIndexPinch, triggerState);
	}

	// Va remettre à zéro l'animation de Grab de la main.
	private void ResetHandGrip() {
		animator.SetFloat(animParamIndexFlex, 0f);
	}

	// Va remettre à zéro l'animation de Trigger de la main.
	private void ResetHandTrigger() {
		animator.SetFloat(animParamIndexPinch, 0f);
	}
	#endregion

	#region Méthodes publiques
	// Va remettre à zéro toutes les animations de la main. (Non utilisé)
	public void ResetAnimation() {
		ResetHandGrip();
		ResetHandTrigger();
	}
	#endregion

	#region Action listeners
	// Action lors d'un event Select commence.
	private void EventHandGrip(InputAction.CallbackContext ctx) {
		HandGrip(ctx.ReadValue<float>());
	}

	// Action lors d'un event Select se termine.
	private void EventResetHandGrip(InputAction.CallbackContext ctx) {
		ResetHandGrip();
	}

	// Action lors d'un event Activate commence.
	private void EventHandTrigger(InputAction.CallbackContext ctx) {
		HandTrigger(ctx.ReadValue<float>());
	}

	// Action lors d'un event Activate se termine.
	private void EventResetHandTrigger(InputAction.CallbackContext ctx) {
		ResetHandTrigger();
	}
	#endregion
}
