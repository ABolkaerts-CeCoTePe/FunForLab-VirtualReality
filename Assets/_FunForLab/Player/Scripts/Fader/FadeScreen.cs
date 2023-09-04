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
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe qui va gérer le fondu au noir de l'affichage (lors d'une téléportation par exemple). Fonctionne avec le shader FadeShader.shader.
/// </summary>
public class FadeScreen : MonoBehaviour {
	private Renderer rend;

	public float fadeDuration = 1f; // Durée du fondu
	public Color fadeColor; // Couleur du fondu
	public bool fadeOnStart = false; // Activer le fondu lors du lancement ?

	#region Messages Unity
	private void Start() {
		rend = GetComponent<Renderer>();

		if (fadeOnStart) {
			FadeOut();
		}
	}
	#endregion

	#region Méthodes privées
	// Va appeler la coroutine du fondu, est générique.
	private void Fade(float alphaIn, float alphaOut) {
		StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
	}
	#endregion

	#region Méthodes publiques
	// Va activer le fondu de visible à noir.
	public void FadeIn() {
		Fade(0, 1);
	}

	// Va activer le fondu de noir à visible.
	public void FadeOut() {
		Fade(1, 0);
	}
	#endregion

	#region Coroutines
	// Coroutine qui va faire le fondu à proprement parler.
	private IEnumerator FadeCoroutine(float alphaIn, float alphaOut) {
		float timer = 0;

		while (timer <= fadeDuration) {
			Color newColor = fadeColor;
			newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
			rend.material.SetColor("_Color", newColor);

			timer += Time.deltaTime;
			yield return null;
		}

		Color endColor = fadeColor;
		endColor.a = alphaOut;
		rend.material.SetColor("_Color", endColor);
	}

	// Coroutine qui va faire un wait pendant toute la durée d'un fade.
	public IEnumerator FadeWaitCoroutine(Action action) {
		yield return new WaitForSeconds(fadeDuration);

		// Appeler la méthode passée en paramètre une fois fini.
		action();
	}
	#endregion
}
