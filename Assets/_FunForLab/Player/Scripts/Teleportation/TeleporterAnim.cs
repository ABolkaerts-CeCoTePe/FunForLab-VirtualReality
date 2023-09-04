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

/// <summary>
/// Classe qui va faire l'animation d'hover des téléporteurs lorsqu'ils sont pointés.
/// </summary>
public class TeleporterAnim : MonoBehaviour {
	private readonly float maxAlphaIntensity = 1f;
	private readonly float minAlphaIntensity = 0.5f;
	private float currentTime = 0f;
	private bool highlighted = false;

	public float fadeSpeed = 2.2f; // Durée du fondu
	public MeshRenderer meshRenderer; // Mesh renderer du réticule du téléporteur

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
		color.a = currentTime; // Changer l'alpha de la couleur du réticule de téléportation, pour faire une transition
		meshRenderer.material.color = color;
	}
	#endregion

	#region Action listeners
	// Action lorsque le téléporteur se fait Hover. Va activer le surlignage du téléporteur.
	public void StartHighlight() {
		highlighted = true;
	}

	// Action lorsque le téléporteur ne se fait plus Hover. Va désactiver le surlignage du téléporteur.
	public void StopHighlight() {
		highlighted = false;
	}
	#endregion
}
