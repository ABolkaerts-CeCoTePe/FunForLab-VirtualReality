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
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static ReagentData;

/// <summary>
/// Classe qui va g�rer un emplacement de r�actif.
/// </summary>
public class ReagentSlotController : MonoBehaviour {
	private XRSocketInteractorExtension socketInteractorExtension;

	public Reagent reagentType; // Type de r�agent que le slot prend

	#region Messages Unity
	private void Awake() {
		socketInteractorExtension = GetComponent<XRSocketInteractorExtension>();
	}
	#endregion

	#region Action listeners
	// Action lorsqu'un r�actif est ins�r� dans l'emplacement. Va envoyer un event avec le type et les donn�es du r�actif.
	public void ReagentPresent() {
		ReagentData reagentData = ((XRGrabInteractable) socketInteractorExtension.firstInteractableSelected).GetComponent<ReagentController>().reagentInstance;

		Debug.Log("Reagent slot " + reagentType + " : " + reagentData.reagent + " - " + reagentData.lvlReagent + "%");
		ReagentSlotEvent.SendReagentSlotEvent(reagentType, reagentData);
	}

	// Action lorsqu'un r�actif est retir� de l'emplacement. Va envoyer un event avec seulement le type.
	public void ReagentRemoved() {
		Debug.Log("Reagent slot " + reagentType + " : removed");
		ReagentSlotEvent.SendReagentSlotEvent(reagentType, null);
	}
	#endregion
}
