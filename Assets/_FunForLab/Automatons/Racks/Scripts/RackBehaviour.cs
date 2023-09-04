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
/// Classe StateMachineBehaviour pour l'animation de la pince du FFL 1000 pour les emplacements de fioles du rack dans l'emplacement du FFL 1000.
/// </summary>
public class RackBehaviour : StateMachineBehaviour {
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Animator[] animators = animator.GetComponentsInParent<Animator>();

		foreach (Animator anim in animators) { // Oui pourrait être bien mieux codé, mais c'était le début du stage et ça marche donc voilà
			if (anim.gameObject.CompareTag("Machine")) {
				int emplacement = anim.GetInteger("RackEmplacement");

				XRSocketInteractorExtension[] interactors = anim.gameObject.GetComponentsInChildren<XRSocketInteractorExtension>();

				// Récupérer la fiole se trouvant à l'emplacement actuel pour la faire redescendre
				foreach (XRSocketInteractorExtension interactor in interactors) {
					if (interactor.gameObject.CompareTag("EmplacementRack")) {
						XRSocketInteractorExtension[] emplacements = ((XRGrabInteractable) interactor.firstInteractableSelected).GetComponentsInChildren<XRSocketInteractorExtension>();

						// Faire redescendre la fiole
						FioleInteraction script = emplacements[10 - emplacement].gameObject.GetComponent<FioleInteraction>();
						script.EventPinceReset();

						break;
					}
				}

				anim.SetInteger("RackEmplacement", emplacement + 1);
				animator.enabled = false; // Désactiver l'animator de la pince

				break;
			}
		}
	}
}
