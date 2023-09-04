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
/// Classe qui va simuler les vagues sur le "faux" liquide. Fonctionne avec le shader Liquid.shader.
/// Récupéré et adapté depuis https://pastebin.com/6EXJHAgA.
/// </summary>
public class Wobble : MonoBehaviour {
	private Renderer rend;
	private Vector3 lastPos;
	private Vector3 velocity;
	private Vector3 lastRot;
	private Vector3 angularVelocity;
	private float wobbleAmountX;
	private float wobbleAmountZ;
	private float wobbleAmountToAddX;
	private float wobbleAmountToAddZ;
	private float pulse;
	private float time = 0.5f;

	public float MaxWobble = 0.03f; // Oscillation maximale
	public float WobbleSpeed = 1f; // Vitesse d'oscillation
	public float Recovery = 1f; // Temps avant que le liquide se stabilise

	#region Messages Unity
	private void Start() {
		rend = GetComponent<Renderer>();
	}

	private void Update() {
		time += Time.deltaTime;

		// Diminuer l'oscillation avec le temps
		wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * Recovery);
		wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * Recovery);

		// Faire une onde sinusoïdale d'oscillation décroissante
		pulse = 2 * Mathf.PI * WobbleSpeed;
		wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
		wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

		// L'envoyer au shader
		rend.material.SetFloat("_WobbleX", wobbleAmountX);
		rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

		// Vélocité
		velocity = (lastPos - transform.position) / Time.deltaTime;
		angularVelocity = transform.rotation.eulerAngles - lastRot;

		// Ajouter une vitesse fixée (clamped) à l'oscillation
		wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
		wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

		// Garder la dernière position et rotation
		lastPos = transform.position;
		lastRot = transform.rotation.eulerAngles;
	}
	#endregion
}
