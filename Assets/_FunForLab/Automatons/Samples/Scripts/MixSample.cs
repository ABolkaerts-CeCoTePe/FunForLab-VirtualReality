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
using System.Collections;
using UnityEngine;

/// <summary>
/// Classe permettant de mixer un �chantillon.
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

	public bool mixed; // Est-ce que le liquide est mix� ou non ?
	public int numberShakes; // Nombre de remuements qu'il faut faire avant de consid�rer le liquide comme mix�
	public int angleShake; // Angle � atteindre pour consid�rer qu'on a fait un remuement
	public float secondsBetweenShakes; // Nombre de secondes entre chaque remuement avant de diminuer le nombre de remuements

	[Range(0, 1)]
	public float blendRatio = 0.25f; // < 0.5 = priorit� sur le liquide, > 0.5 = priorit� sur la mousse

	public bool blendColors = true; // Si vrai, on va m�langer la couleur du liquide et de la mousse
	public Color mixColor; // Fixer une couleur du liquide une fois mix�, utilis� si blendColors est faux

	public bool canUnmix; // Si vrai, le m�lange peut revenir � la normale apr�s un certain temps
	public float secondsBeforeUnmix; // Nombre de secondes avant que le liquide redevienne � la normale

	#region Messages Unity
	private void Awake() {
		shaderLiquide = GetComponent<Renderer>();
		liquid = GetComponent<Liquid>();
	}

	private void Start() {
		currentShakes = 0;
		timer = 0;
		lastTransform = transform.rotation.eulerAngles;

		if (mixed) { // Si le liquide est d�j� marqu� comme mix� au lancement
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

	#region M�thodes priv�es
	// Va d�tecter si l'objet est secou�.
	private bool DetectShake() {
		Vector3 actual = transform.rotation.eulerAngles; // Le shake est d�tect� aussi dans dans le rack, mais est moins sensible

		float diffX = Mathf.DeltaAngle(lastTransform.x, actual.x);
		float diffY = Mathf.DeltaAngle(lastTransform.y, actual.y);
		float diffZ = Mathf.DeltaAngle(lastTransform.z, actual.z);

		float angle = Mathf.Abs(diffX) + Mathf.Abs(diffY) + Mathf.Abs(diffZ);
		//Debug.Log("Mix angle : " + angle + " | detect : " + angleShake);

		if (angle > angleShake) { // TODO Les shakes peuvent ne pas �tre d�tect�s si on est en mode PC dans l'�diteur !
			//Debug.Log("Shake detected");
			return true;
		}

		return false;
	}

	// Va m�langer le liquide en fusionnant la couleur du liquide et sa "mousse".
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

		// Marqu� le liquide comme d�j� mix�
		mixed = true;

		// Ne plus adapter la hauteur de la mousse
		initialFoamAdapt = liquid.foamAdapt;
		liquid.foamAdapt = false;

		// TODO envoyer un event pour dire qu'il est mix� ?
		Debug.Log("Fiole " + GetComponentInParent<SampleController>().sampleData.sampleID + " mix� !");

		if (canUnmix) {
			StartCoroutine(UnmixCoroutine());
		}
	}
	#endregion

	#region Coroutines
	// Coroutine qui va d�mixer le liquide apr�s un certain temps.
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

		// Marqu� le liquide comme d�j� mix�
		mixed = false;

		liquid.foamAdapt = initialFoamAdapt;
	}
	#endregion
}
