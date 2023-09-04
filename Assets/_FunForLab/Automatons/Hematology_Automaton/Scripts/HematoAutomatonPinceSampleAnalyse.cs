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
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe StateMachineBehaviour qui va gérer l'analyse des données d'un échantillon.
/// </summary>
public class HematoAutomatonPinceSampleAnalyse : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Animator[] animators = animator.GetComponentsInParent<Animator>();

		foreach (Animator anim in animators) { // Oui pourrait être bien mieux codé, mais c'était le début du stage et ça marche donc voilà
			if (anim.gameObject.CompareTag("Machine")) {
				HematoAutomatonController scriptFFL = anim.gameObject.GetComponent<HematoAutomatonController>();
				int emplacement = anim.GetInteger("RackEmplacement");

				XRSocketInteractorExtension[] interactors = anim.gameObject.GetComponentsInChildren<XRSocketInteractorExtension>();

				// Récupérer la fiole se trouvant à l'emplacement actuel pour l'analyser
				foreach (XRSocketInteractorExtension interactor in interactors) {
					if (interactor.gameObject.CompareTag("EmplacementRack")) {
						XRSocketInteractorExtension[] emplacements = ((XRGrabInteractable) interactor.firstInteractableSelected).GetComponentsInChildren<XRSocketInteractorExtension>();

						XRGrabInteractable fiole = (XRGrabInteractable) emplacements[10 - emplacement].firstInteractableSelected;

						// Analyser les données de la fiole
						SampleController data = fiole.gameObject.GetComponent<SampleController>();
						data.sampleData.typeFonctionnement = scriptFFL.modeFonctionnement;
						data.sampleData.modeAnalyse = scriptFFL.typeMesure;
						data.sampleData.testsDiscrets = scriptFFL.typeTests;
						data.sampleData.dateTest = DateTime.Now;
						data.sampleData.emplacementRack = string.Format("1 - {0:D2}", emplacement);

						// Récupérer si l'échantillon était mixé ou non au moment de l'analyse
						MixSample mixSample = fiole.gameObject.GetComponentInChildren<MixSample>();

						if (mixSample != null) {
							data.sampleData.mixed = mixSample.mixed;
						}

						// Diminuer le niveau des réactifs
						scriptFFL.ConsumeReagents(HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE);
						scriptFFL.display.NiveauxReagents(scriptFFL.reagentDatas);

						// Afficher les données dans l'interface
						scriptFFL.display.AddSampleList(data.sampleData);

						break;
					}
				}

				break;
			}
		}
	}
}
