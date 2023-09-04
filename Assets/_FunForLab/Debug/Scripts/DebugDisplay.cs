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
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Classe qui va contrôler l'UI de débug.
/// </summary>
public class DebugDisplay : MonoBehaviour {
	public TMP_Text fpsDisplay; // Texte de la barre de titre
	public ScrollRect contentScrollRect; // ScrollView de la console
	public TMP_Text contentDisplay; // Texte de la console

	#region Messages Unity
	private void Start() {
		Debug.Log("System locale = " + LocalizationSettings.SelectedLocale);
		Debug.Log("Game selected locale = " + PlayerPrefsManager.Language);
	}

	private void Update() {
		ShowFPS();
	}

	private void OnEnable() {
		Application.logMessageReceived += HandleLog;
		contentDisplay.text = string.Empty;
	}

	private void OnDisable() {
		Application.logMessageReceived -= HandleLog;
	}
	#endregion

	#region Méthodes privées
	// Va modifier le panel de titre de l'UI pour afficher les FPS.
	private void ShowFPS() {
		fpsDisplay.text = "Debug Console – " + Application.productName + " – " + LocalizationSettings.SelectedLocale.Identifier.Code + " – FPS : " + (int) (1 / Time.unscaledDeltaTime);
	}

	// Va charger le menu principal.
	private void LoadMenu() {
		SceneManager.LoadScene("GameMenu", LoadSceneMode.Single); // Retourner au menu principal.
	}
	#endregion

	#region Action listeners
	// Action lorsqu'un event LogMessageReceived arrive. Va récupérer le message de log et le formatter pour l'UI.
	private void HandleLog(string logString, string stackTrace, LogType type) {
		StringBuilder sb = new StringBuilder();
		sb.Append("[");
		sb.Append(DateTime.Now.ToString("HH:mm:ss"));
		sb.Append("] ");
		sb.Append(logString);
		sb.Append("\n");

        //string msg = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + logString;

        //if (type == LogType.Warning) {
        //	msg = "<color=\"yellow\">" + msg + "</color>";
        //}
        //else if (type == LogType.Error || type == LogType.Exception) {
        //	msg = "<color=\"red\">" + msg + " (<i>" + stackTrace.Split('\n')[0].Split(' ')[0] + "</i>)" + "</color>";
        //}

        contentDisplay.text += sb.ToString();

		StartCoroutine(PushToBottomCoroutine());
	}
	// Action lorsque l'on appuye sur le bouton Exit. Va quitter la scène et charger le menu principal.
	public void ButtonExit() {
		FadeScreen fader = Camera.main.GetComponentInChildren<FadeScreen>(); // Aller rechercher le fader de la caméra principale afin d'activer le fondu au noir
		fader.FadeIn();
		StartCoroutine(fader.FadeWaitCoroutine(LoadMenu)); // Appeler la fonction LoadMenu() une fois que la coroutine à fini.
	}
	#endregion

	#region Coroutines
	// Coroutine qui va faire en sorte d'autoscroll la scrollview à chaque nouveau message.
	private IEnumerator PushToBottomCoroutine() {
		yield return new WaitForEndOfFrame();
		contentScrollRect.verticalNormalizedPosition = 0;
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) contentDisplay.transform);
	}
	#endregion
}
