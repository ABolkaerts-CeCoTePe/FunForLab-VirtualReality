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
/// Classe qui va faire l'animation d'hover des t�l�porteurs lorsqu'ils sont point�s.
/// </summary>
public class TeleporterAnim : MonoBehaviour {
	private readonly float maxAlphaIntensity = 1f;
	private readonly float minAlphaIntensity = 0.5f;
	private float currentTime = 0f;
	private bool highlighted = false;

	public float fadeSpeed = 2.2f; // Dur�e du fondu
	public MeshRenderer meshRenderer; // Mesh renderer du r�ticule du t�l�porteur

	#region Messages Unity
	private void Start() {
		meshRenderer = GetComponent<MeshRenderer>();

		Color color = meshRenderer.material.color;
		color.a = currentTime;
		meshRenderer.material.color = color;
	}

	private void Update() {
		if (highlighted) {
			currentTime += Time.deltaTime * fadeSpeed;
		}
		else if (!highlighted) {
			currentTime -= Time.deltaTime * fadeSpeed;
		}

		if (currentTime > maxAlphaIntensity) {
			currentTime = maxAlphaIntensity;
		}
		else if (currentTime < minAlphaIntensity) {
			currentTime = minAlphaIntensity;
		}

		Color color = meshRenderer.material.color;
		color.a = currentTime; // Changer l'alpha de la couleur du r�ticule de t�l�portation, pour faire une transition
		meshRenderer.material.color = color;
	}
	#endregion

	#region Action listeners
	// Action lorsque le t�l�porteur se fait Hover. Va activer le surlignage du t�l�porteur.
	public void StartHighlight() {
		highlighted = true;
	}

	// Action lorsque le t�l�porteur ne se fait plus Hover. Va d�sactiver le surlignage du t�l�porteur.
	public void StopHighlight() {
		highlighted = false;
	}
	#endregion
}
