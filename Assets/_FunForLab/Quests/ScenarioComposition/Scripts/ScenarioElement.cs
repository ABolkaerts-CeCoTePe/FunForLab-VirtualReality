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
using System.Collections.Generic;
using UnityEngine.Localization;

/// <summary>
/// Classe qui contient un élément d'un scénario.
/// </summary>
[Serializable]
public class ScenarioElement : IComparable<ScenarioElement> {
	public int order;
	public string code; // Code qui servira à identifier l'élément
	public LocalizedString description;
	public bool done;
	public bool step; // Est-ce une étape importante ? Si elle est franchie, on ignore les changements des étapes précédentes qui pourraient survevir par la suite

	public bool subOrdered; // Respecter l'ordre des sous-éléments ou non ?
	public List<ScenarioSubElement> subElements; // Obligé de faire une autre classe qui ne possède pas de sous-éléments sinon Unity râle comme quoi il pourrait avoir des sous-éléments à l'infini

	public bool SubElementsCompleted {
		get {
			if (subElements.Count == 0) {
				return true;
			}

			foreach (ScenarioSubElement subElement in subElements) {
				if (!subElement.done) {
					return false;
				}
			}

			return true;
		}
	}

	public int SubScenarioCurrent {
		get {
			if (subOrdered) {
				for (int i = 0; i < subElements.Count; i++) {
					if (!subElements[i].done) {
						return i;
					}
				}
			}

			return -1;
		}
	}

	public bool Done {
		get {
			return done;
		}
		set {
			if (value == true && SubElementsCompleted) {
				done = true;
			}
			else {
				done = false;
			}
		}
	}

	public int CompareTo(ScenarioElement other) {
		if (other == null) {
			return 1;
		}

		if (order > other.order) {
			return 1;
		}
		else if (order < other.order) {
			return -1;
		}
		else {
			return 0;
		}
	}
}

/// <summary>
/// Classe qui contient des sous-éléments d'un scénario.
/// </summary>
[Serializable]
public class ScenarioSubElement : IComparable<ScenarioSubElement> {
	public int order;
	public string code; // Code qui servira à identifier l'élément
	public LocalizedString description;
	public bool done;

	public int CompareTo(ScenarioSubElement other) {
		if (other == null) {
			return 1;
		}

		if (order > other.order) {
			return 1;
		}
		else if (order < other.order) {
			return -1;
		}
		else {
			return 0;
		}
	}
}
