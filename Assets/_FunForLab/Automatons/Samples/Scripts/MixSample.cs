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
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe permettant de mixer un échantillon.
/// </summary>
public class MixSample : MonoBehaviour {
	private Renderer shaderLiquide;
	private Liquid liquid;
	private int currentShakes;
	private float timer;
	private Vector3 lastTransform;
	private Color initialLiquidColor;
	private Color initialFoamColor;
	private Color initialTopColor;
	private float initialFoam;
	private bool initialFoamAdapt;

	public bool mixed; // Est-ce que le liquide est mixé ou non ?
	public int numberShakes; // Nombre de remuements qu'il faut faire avant de considérer le liquide comme mixé
	public int angleShake; // Angle à atteindre pour considérer qu'on a fait un remuement
	public float secondsBetweenShakes; // Nombre de secondes entre chaque remuement avant de diminuer le nombre de remuements

	[Range(0, 1)]
	public float blendRatio = 0.25f; // < 0.5 = priorité sur le liquide, > 0.5 = priorité sur la mousse

	public bool blendColors = true; // Si vrai, on va mélanger la couleur du liquide et de la mousse
	public Color mixColor; // Fixer une couleur du liquide une fois mixé, utilisé si blendColors est faux

	public bool canUnmix; // Si vrai, le mélange peut revenir à la normale après un certain temps
	public float secondsBeforeUnmix; // Nombre de secondes avant que le liquide redevienne à la normale

	#region Messages Unity
	private void Awake() {
		shaderLiquide = GetComponent<Renderer>();
		liquid = GetComponent<Liquid>();
	}

	private void Start() {
		currentShakes = 0;
		timer = 0;
		lastTransform = transform.rotation.eulerAngles;

		if (mixed) { // Si le liquide est déjà marqué comme mixé au lancement
			MixLiquid();
		}
	}

	private void Update() {
		if (!mixed) {
			timer += Time.deltaTime;

			if (timer >= secondsBetweenShakes) {
				//Debug.Log("Timer reset | currentShakes : " + currentShakes);
				timer = 0;

				if (currentShakes > 0) {
					//Debug.Log("Shake down");
					currentShakes--;
				}
			}

			if (transform.hasChanged) {
				if (DetectShake()) {
					currentShakes++;

					timer = 0;

					if (currentShakes >= numberShakes) {
						MixLiquid();
						currentShakes = 0;
					}
				}

				lastTransform = transform.rotation.eulerAngles;
			}
		}
	}
	#endregion

	#region Méthodes privées
	// Va détecter si l'objet est secoué.
	private bool DetectShake() {
		Vector3 actual = transform.rotation.eulerAngles; // Le shake est détecté aussi dans dans le rack, mais est moins sensible

		float diffX = Mathf.DeltaAngle(lastTransform.x, actual.x);
		float diffY = Mathf.DeltaAngle(lastTransform.y, actual.y);
		float diffZ = Mathf.DeltaAngle(lastTransform.z, actual.z);

		float angle = Mathf.Abs(diffX) + Mathf.Abs(diffY) + Mathf.Abs(diffZ);
		//Debug.Log("Mix angle : " + angle + " | detect : " + angleShake);

		if (angle > angleShake) { // TODO Les shakes peuvent ne pas être détectés si on est en mode PC dans l'éditeur !
			//Debug.Log("Shake detected");
			return true;
		}

		return false;
	}

	// Va mélanger le liquide en fusionnant la couleur du liquide et sa "mousse".
	private void MixLiquid() {
		Color blendColor = mixColor;

		if (blendColors) {
			Color liquidColor = shaderLiquide.material.GetColor("_Tint");
			Color foamColor = shaderLiquide.material.GetColor("_FoamColor");

			blendColor = Color.Lerp(liquidColor, foamColor, blendRatio);
		}

		Color topColor = blendColor + new Color32(15, 15, 15, 255); // Avoir le dessus plus clair que le liquide.

		// Garder les anciennes valeurs
		initialLiquidColor = shaderLiquide.material.GetColor("_Tint");
		initialFoamColor = shaderLiquide.material.GetColor("_FoamColor");
		initialTopColor = shaderLiquide.material.GetColor("_TopColor");
		initialFoam = shaderLiquide.material.GetFloat("_Foam");

		// Changer la couleur du liquide
		shaderLiquide.material.SetColor("_Tint", blendColor);

		// Changer la couleur du dessus du liquide
		shaderLiquide.material.SetColor("_TopColor", topColor);

		// Enlever la mousse du liquide
		shaderLiquide.material.SetFloat("_Foam", 0);
		shaderLiquide.material.SetColor("_FoamColor", blendColor); // Pouvait arriver par moments que la mousse reste, donc on change sa couleur aussi

		// Marqué le liquide comme déjà mixé
		mixed = true;

		// Ne plus adapter la hauteur de la mousse
		initialFoamAdapt = liquid.foamAdapt;
		liquid.foamAdapt = false;

		// TODO envoyer un event pour dire qu'il est mixé ?
		Debug.Log("Fiole " + GetComponentInParent<SampleController>().sampleData.sampleID + " mixé !");

		if (canUnmix) {
			StartCoroutine(UnmixCoroutine());
		}
	}
	#endregion

	#region Coroutines
	// Coroutine qui va démixer le liquide après un certain temps.
	private IEnumerator UnmixCoroutine() {
		yield return new WaitForSeconds(secondsBeforeUnmix);

		Debug.Log("Unmix de " + GetComponentInParent<SampleController>().sampleData.sampleID);

		// Changer la couleur du liquide
		shaderLiquide.material.SetColor("_Tint", initialLiquidColor);

		// Changer la couleur du dessus du liquide
		shaderLiquide.material.SetColor("_TopColor", initialTopColor);

		// Enlever la mousse du liquide
		shaderLiquide.material.SetFloat("_Foam", initialFoam);
		shaderLiquide.material.SetColor("_FoamColor", initialFoamColor);

		// Marqué le liquide comme déjà mixé
		mixed = false;

		liquid.foamAdapt = initialFoamAdapt;
	}
	#endregion
}
