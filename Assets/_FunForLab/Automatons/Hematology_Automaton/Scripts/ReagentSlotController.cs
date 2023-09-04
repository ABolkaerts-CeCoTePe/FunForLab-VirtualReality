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
using static ReagentData;

/// <summary>
/// Classe qui va gérer un emplacement de réactif.
/// </summary>
public class ReagentSlotController : MonoBehaviour {
	private XRSocketInteractorExtension socketInteractorExtension;

	public Reagent reagentType; // Type de réagent que le slot prend

	#region Messages Unity
	private void Awake() {
		socketInteractorExtension = GetComponent<XRSocketInteractorExtension>();
	}
	#endregion

	#region Action listeners
	// Action lorsqu'un réactif est inséré dans l'emplacement. Va envoyer un event avec le type et les données du réactif.
	public void ReagentPresent() {
		ReagentData reagentData = ((XRGrabInteractable) socketInteractorExtension.firstInteractableSelected).GetComponent<ReagentController>().reagentInstance;

		Debug.Log("Reagent slot " + reagentType + " : " + reagentData.reagent + " - " + reagentData.lvlReagent + "%");
		ReagentSlotEvent.SendReagentSlotEvent(reagentType, reagentData);
	}

	// Action lorsqu'un réactif est retiré de l'emplacement. Va envoyer un event avec seulement le type.
	public void ReagentRemoved() {
		Debug.Log("Reagent slot " + reagentType + " : removed");
		ReagentSlotEvent.SendReagentSlotEvent(reagentType, null);
	}
	#endregion
}
