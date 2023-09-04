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
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe qui va g�rer le fondu au noir de l'affichage (lors d'une t�l�portation par exemple). Fonctionne avec le shader FadeShader.shader.
/// </summary>
public class FadeScreen : MonoBehaviour {
	private Renderer rend;

	public float fadeDuration = 1f; // Dur�e du fondu
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

	#region M�thodes priv�es
	// Va appeler la coroutine du fondu, est g�n�rique.
	private void Fade(float alphaIn, float alphaOut) {
		StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
	}
	#endregion

	#region M�thodes publiques
	// Va activer le fondu de visible � noir.
	public void FadeIn() {
		Fade(0, 1);
	}

	// Va activer le fondu de noir � visible.
	public void FadeOut() {
		Fade(1, 0);
	}
	#endregion

	#region Coroutines
	// Coroutine qui va faire le fondu � proprement parler.
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

	// Coroutine qui va faire un wait pendant toute la dur�e d'un fade.
	public IEnumerator FadeWaitCoroutine(Action action) {
		yield return new WaitForSeconds(fadeDuration);

		// Appeler la m�thode pass�e en param�tre une fois fini.
		action();
	}
	#endregion
}
