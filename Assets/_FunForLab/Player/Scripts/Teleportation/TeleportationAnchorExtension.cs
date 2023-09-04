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
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Extension de <see cref="TeleportationAnchor"/> qui permet l'activation d'un fondu au noir lors de la t�l�portation.
/// </summary>
public class TeleportationAnchorExtension : TeleportationAnchor {
	public FadeScreen fadeScreen; // L'objet g�rant le fondu au noir du joueur
	public MasterControllerAction masterController; // Le contr�leur principal des manettes

	protected override void OnSelectEntered(SelectEnterEventArgs args) {
		if (teleportTrigger == TeleportTrigger.OnSelectEntered) {
			StartCoroutine(FadeCoroutine(base.OnSelectEntered, args));
		}
	}

	protected override void OnSelectExited(SelectExitEventArgs args) {
		if (teleportTrigger == TeleportTrigger.OnSelectExited && !args.isCanceled) {
			StartCoroutine(FadeCoroutine(base.OnSelectExited, args));
		}
	}

	protected override void OnActivated(ActivateEventArgs args) {
		if (teleportTrigger == TeleportTrigger.OnActivated) {
			StartCoroutine(FadeCoroutine(base.OnActivated, args));
		}
	}

	protected override void OnDeactivated(DeactivateEventArgs args) {
		if (teleportTrigger == TeleportTrigger.OnDeactivated) {
			StartCoroutine(FadeCoroutine(base.OnDeactivated, args));
		}
	}

	private IEnumerator FadeCoroutine(UnityAction<SelectEnterEventArgs> actionEvent, SelectEnterEventArgs args) {
		masterController.TeleportationActive(true); // Dire qu'une t�l�portation est d�j� active, afin de ne pas pouvoir se ret�l�porter

		fadeScreen.FadeIn();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);
		actionEvent.Invoke(args);

		// Pour t�l�porter le corps en m�me temps, sinon il se d�pla�ait rapidement vers le joueur une fois la t�l�portation finie.
		masterController.solver.AddPlatformMotion(teleportAnchorTransform.position, teleportAnchorTransform.rotation, teleportAnchorTransform.position);

		fadeScreen.FadeOut();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);

		masterController.TeleportationActive(false);
	}

	private IEnumerator FadeCoroutine(UnityAction<SelectExitEventArgs> actionEvent, SelectExitEventArgs args) {
		masterController.TeleportationActive(true);

		fadeScreen.FadeIn();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);
		actionEvent.Invoke(args);

		masterController.solver.AddPlatformMotion(teleportAnchorTransform.position, teleportAnchorTransform.rotation, teleportAnchorTransform.position);

		fadeScreen.FadeOut();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);

		masterController.TeleportationActive(false);
	}

	private IEnumerator FadeCoroutine(UnityAction<ActivateEventArgs> actionEvent, ActivateEventArgs args) {
		masterController.TeleportationActive(true);

		fadeScreen.FadeIn();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);
		actionEvent.Invoke(args);

		masterController.solver.AddPlatformMotion(teleportAnchorTransform.position, teleportAnchorTransform.rotation, teleportAnchorTransform.position);

		fadeScreen.FadeOut();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);

		masterController.TeleportationActive(false);
	}

	private IEnumerator FadeCoroutine(UnityAction<DeactivateEventArgs> actionEvent, DeactivateEventArgs args) {
		masterController.TeleportationActive(true);

		fadeScreen.FadeIn();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);
		actionEvent.Invoke(args);

		masterController.solver.AddPlatformMotion(teleportAnchorTransform.position, teleportAnchorTransform.rotation, teleportAnchorTransform.position);

		fadeScreen.FadeOut();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);

		masterController.TeleportationActive(false);
	}
}
