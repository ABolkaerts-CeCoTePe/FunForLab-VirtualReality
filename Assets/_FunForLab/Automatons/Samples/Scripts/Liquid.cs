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

/// <summary>
/// Classe qui va g�rer un "faux" liquide dans un conteneur. Fonctionne avec le shader Liquid.shader.
/// </summary>
public class Liquid : MonoBehaviour {
	private Renderer shaderLiquide;
	private float liquidAmount;
	private float foamAmount;

	public bool foamAdapt; // Si on veut adapter la hauteur de la mousse

	// TODO La top color est en fait la couleur du c�t� int�rieur du liquide, � modifier pour soit que l'int�rieur soit toujours visible, soit pour vraiment avoir une couleur au-dessus !

	#region Messages Unity
	private void Awake() {
		shaderLiquide = GetComponent<Renderer>();
		liquidAmount = shaderLiquide.material.GetFloat("_FillAmount");
		foamAmount = shaderLiquide.material.GetFloat("_Foam");
	}

	private void Update() {
		if (transform.hasChanged) { // Ne calculer que quand la position de l'objet change
			// Comme les nouvelles fioles sont bien centr�e, l'objet � l'envers (t�te en bas) ne pose plus de probl�me

			// transform.TransformDirection(Vector3.up) permet de voir la diff�rence d'inclinaison
			Vector3 b = transform.TransformDirection(Vector3.up); // (0, 1, 0) de base droit, (1, 0, 0) couch� vers la gauche, (-1, 0, 0) couch� vers la droite, (0, 0, -1) couch� vers l'avant, (0, 0, 1) couch� vers l'arri�re

			float sum = Mathf.Abs(b.x) + Mathf.Abs(b.z);

			// Compenser la hauteur du liquide afin qu'il ne sorte pas du cylindre
			float w = 0.24f * sum;
			w = w > 1 ? 1 : w; // Si w > 1, le fixer � 1, sinon le laisser tel quel

			float newFill = liquidAmount - w;
			shaderLiquide.material.SetFloat("_FillAmount", newFill);

			if (foamAdapt) {
				// Diminuer la hauteur de la "mousse" afin que le liquide reste coh�rent
				float f = 0.36f * sum;
				f = f > 1 ? 1 : f; // Si f > 1, le fixer � 1, sinon le laisser tel quel

				float newFoam = foamAmount - f;
				shaderLiquide.material.SetFloat("_Foam", newFoam);
			}

			transform.hasChanged = false;
		}
	}
	#endregion
}
