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
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Extension de <see cref="TeleportationAnchor"/> qui permet l'activation d'un fondu au noir lors de la téléportation.
/// </summary>
public class TeleportationAnchorExtension : TeleportationAnchor {
	public FadeScreen fadeScreen; // L'objet gérant le fondu au noir du joueur
	public MasterControllerAction masterController; // Le contrôleur principal des manettes

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
		masterController.TeleportationActive(true); // Dire qu'une téléportation est déjà active, afin de ne pas pouvoir se retéléporter

		fadeScreen.FadeIn();
		yield return new WaitForSeconds(fadeScreen.fadeDuration);
		actionEvent.Invoke(args);

		// Pour téléporter le corps en même temps, sinon il se déplaçait rapidement vers le joueur une fois la téléportation finie.
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
