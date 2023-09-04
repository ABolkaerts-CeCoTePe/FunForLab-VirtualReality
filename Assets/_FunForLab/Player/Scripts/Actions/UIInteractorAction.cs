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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe qui va gérer l'interaction d'un contrôleur vers l'UI. Utilise le nouveau Input Manager basé sur les Actions.
/// </summary>
public class UIInteractorAction : MonoBehaviour {
	private XRRayInteractor interactor;
	private ActionBasedController controller;
	private XRInteractorLineVisual lineVisual;
	private Color validColor;
	private Color selectColor;
	private Color invisibleColor = new Color(0, 0, 0, 0);
	private GameObject precHover;
	private Gradient initialValidColorGradient;
	private Canvas lastCanvas;
	private Selectable lastSelectable;
	private bool currentPress = false;

	private readonly Dictionary<Canvas, RawImage> canvasReticlesCache = new Dictionary<Canvas, RawImage>(); // Cache des canvas avec leur reticule pour ne pas les récupérer à chaque fois

	public Gradient selectColorGradient; // Couleur du raycast et du réticule quand on appuye sur le trigger
	public XRNode hand; // Main gauche ou droite ?

	[HideInInspector]
	public bool isPointingUi;

	#region Messages Unity
	private void Awake() {
		interactor = GetComponent<XRRayInteractor>();
		controller = GetComponent<ActionBasedController>();
		lineVisual = GetComponent<XRInteractorLineVisual>();

		// Récupérer la première couleur du gradient de XRInteractorLineVisual afin que le réticule ait la même couleur.
		initialValidColorGradient = lineVisual.validColorGradient;
		validColor = lineVisual.validColorGradient.colorKeys[0].color;
		selectColor = selectColorGradient.colorKeys[0].color;

		precHover = null;
		isPointingUi = false;
	}

	private void OnEnable() {
		controller.uiPressAction.action.performed += EventUIPress;
		controller.uiPressAction.action.canceled += EventUIPress;
	}

	private void OnDisable() {
		controller.uiPressAction.action.performed -= EventUIPress;
		controller.uiPressAction.action.canceled -= EventUIPress;
	}

	private void Update() {
		bool hit = interactor.TryGetHitInfo(out Vector3 position, out _, out _, out _);
		interactor.TryGetCurrentRaycast(out _, out _, out RaycastResult? uiRaycastHit, out _, out _);
		//bool hit = interactor.TryGetHitInfo(out Vector3 position, out Vector3 normal, out int positionInLine, out bool isValidTarget);
		//bool h = interactor.TryGetCurrentRaycast(out RaycastHit? raycastHit, out int raycastHitIndex, out RaycastResult? uiRaycastHit, out int uiRaycastHitIndex, out bool isUIHitClosest);

		if (hit && uiRaycastHit.HasValue) { // Est-ce que le raycast pointe quelque chose, et que ce quelque chose est une UI ?
			Canvas canvas = uiRaycastHit.Value.gameObject.GetComponentInParent<Canvas>();

			if (canvas != null) {
				Canvas rootCanvas = canvas.rootCanvas;
				Vector3 localPosition3D = rootCanvas.transform.InverseTransformPoint(position);

				isPointingUi = true;

				if (!canvasReticlesCache.ContainsKey(rootCanvas)) { // Est-ce que le réticule de cette UI se trouve dans le cache ?
					RawImage reticle;

					if (hand == XRNode.LeftHand) {
						reticle = rootCanvas.gameObject.transform.Find("ReticleLeft").gameObject.GetComponent<RawImage>();
					}
					else {
						reticle = rootCanvas.gameObject.transform.Find("ReticleRight").gameObject.GetComponent<RawImage>();
					}

					if (reticle != null) { // Si l'UI possède un reticule
						canvasReticlesCache.Add(rootCanvas, reticle);
					}
				}

				canvasReticlesCache[rootCanvas].gameObject.GetComponent<RectTransform>().localPosition = (Vector2) localPosition3D; // Vector2 afin de ne pas avoir l'axe z

				foreach (KeyValuePair<Canvas, RawImage> value in canvasReticlesCache) { // Changer la couleur du réticule
					if (value.Key == rootCanvas) {
						if (currentPress) {
							value.Value.color = selectColor;
						}
						else {
							value.Value.color = validColor;
						}
					}
					else {
						value.Value.color = invisibleColor;
					}
				}

				Selectable selectable = uiRaycastHit.Value.gameObject.GetComponentInParent<Selectable>();
				lastSelectable = selectable;

				// Voir si ce qu'on pointe est un élément sélectable et qu'il est activé
				if (selectable != null && selectable.interactable) {
					if (precHover != uiRaycastHit.Value.gameObject) { // Pour ne vibrer que lors du premier hover
						HapticController.SendHapticHover(hand);
					}
				}

				// Effacer les réticules des autres canvas quand on ne les pointent pas
				if (lastCanvas != rootCanvas) {
					lastCanvas = rootCanvas;
				}
			}
		}
		else { // Si on ne pointe rien, on masque tous les réticules
			foreach (KeyValuePair<Canvas, RawImage> value in canvasReticlesCache) {
				value.Value.color = invisibleColor;
			}

			isPointingUi = false;
		}

		if (uiRaycastHit.HasValue) {
			precHover = uiRaycastHit.Value.gameObject;
		}
	}
	#endregion

	#region Méthodes privées
	// Va changer la couleur et la taille du réticule et du raycast.
	private void UIPress(bool press) {
		currentPress = press;
		lineVisual.validColorGradient = press ? selectColorGradient : initialValidColorGradient; // Changer la couleur du ray selon si le bouton est pressé ou non

		if (press) {
			if (lastCanvas != null)
			{
                canvasReticlesCache[lastCanvas].color = selectColor; // Changer la couleur et agrandir légèrement la taille du réticule quand on appuye sur le trigger
                canvasReticlesCache[lastCanvas].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 0f);
            }

            if (lastSelectable != null && lastSelectable.interactable) { // Voir si le dernier élément pointé est un élément sélectable et qu'il est activé
				HapticController.SendHapticSelect(hand);
			}
		}
		else {
			if (lastCanvas != null)
			{
				canvasReticlesCache[lastCanvas].color = validColor;
				canvasReticlesCache[lastCanvas].gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0f);
			}
		}
	}
	#endregion

	#region Action listeners
	// Action lors d'un event UI Press. Va changer la couleur et la tailler du réticule et du raycast.
	private void EventUIPress(InputAction.CallbackContext ctx) {
		UIPress(ctx.ReadValueAsButton());
	}
	#endregion
}
