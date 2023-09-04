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
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerPrefsManager;

/// <summary>
/// Classe qui g�re l'�cran d'affichage du Menu Principal.
/// </summary>
public class GameMenuDisplay : MonoBehaviour {
	#region Variables UI
	private Toggle toggleLanguageEnglish;
	private Toggle toggleLanguageFrench;
	private Toggle toggleLanguageDutchBe;
	private Toggle toggleLanguageDutchNe;
	private Toggle toggleLanguageGerman;

	private Toggle toggleDifficultySecondary;
	private Toggle toggleDifficultyTLM;

	private Toggle toggleCameraDisabled;
	private Toggle toggleCameraSnap;
	private Toggle toggleCameraContinuous;

	private Toggle toggleMoveDisabled;
	private Toggle toggleMoveFree;

	private Toggle toggleControlsSimple;
	private Toggle toggleControlsNormal;

	private Toggle toggleScenarioTutorial;
	private Toggle toggleScenario1;
	#endregion

	private Lang language;
	private DifficultyLevel difficulty;
	private int scenario; // 0 = tuto, 1 = scenario 1
	private CameraModes cameraMode;
	private bool freeMove;
	private bool controlsSimple;

	public LocomotionController locomotionController;
	public ChangeInputActionMap changeInputActionMap;

	#region Messages Unity
	private void Awake() {
		toggleLanguageEnglish = transform.Find("PanelBackground/PanelOptions/OptionLanguage/PanelLanguage/ToggleEnglish").gameObject.GetComponent<Toggle>();
		toggleLanguageFrench = transform.Find("PanelBackground/PanelOptions/OptionLanguage/PanelLanguage/ToggleFrench").gameObject.GetComponent<Toggle>();
		toggleLanguageDutchBe = transform.Find("PanelBackground/PanelOptions/OptionLanguage/PanelLanguage/ToggleDutchBelgium").gameObject.GetComponent<Toggle>();
		toggleLanguageDutchNe = transform.Find("PanelBackground/PanelOptions/OptionLanguage/PanelLanguage/ToggleDutchNetherlands").gameObject.GetComponent<Toggle>();
		toggleLanguageGerman = transform.Find("PanelBackground/PanelOptions/OptionLanguage/PanelLanguage/ToggleGerman").gameObject.GetComponent<Toggle>();

		toggleDifficultySecondary = transform.Find("PanelBackground/PanelOptions/OptionDifficulty/PanelDifficulty/ToggleSecondary").gameObject.GetComponent<Toggle>();
		toggleDifficultyTLM = transform.Find("PanelBackground/PanelOptions/OptionDifficulty/PanelDifficulty/ToggleTLM").gameObject.GetComponent<Toggle>();

		toggleCameraDisabled = transform.Find("PanelBackground/PanelOptions/OptionCameraMode/PanelCameraMode/ToggleDisabled").gameObject.GetComponent<Toggle>();
		toggleCameraSnap = transform.Find("PanelBackground/PanelOptions/OptionCameraMode/PanelCameraMode/ToggleSnap").gameObject.GetComponent<Toggle>();
		toggleCameraContinuous = transform.Find("PanelBackground/PanelOptions/OptionCameraMode/PanelCameraMode/ToggleContinuous").gameObject.GetComponent<Toggle>();

		toggleMoveDisabled = transform.Find("PanelBackground/PanelOptions/OptionMove/PanelMove/ToggleDisabled").gameObject.GetComponent<Toggle>();
		toggleMoveFree = transform.Find("PanelBackground/PanelOptions/OptionMove/PanelMove/ToggleFreeMove").gameObject.GetComponent<Toggle>();

		toggleControlsSimple = transform.Find("PanelBackground/PanelOptions/OptionControls/PanelControls/ToggleSimple").gameObject.GetComponent<Toggle>();
		toggleControlsNormal = transform.Find("PanelBackground/PanelOptions/OptionControls/PanelControls/ToggleNormal").gameObject.GetComponent<Toggle>();

		toggleScenarioTutorial = transform.Find("PanelBackground/PanelOptions/OptionScenario/PanelScenario/ToggleTutorial").gameObject.GetComponent<Toggle>();
		toggleScenario1 = transform.Find("PanelBackground/PanelOptions/OptionScenario/PanelScenario/ToggleScenario1").gameObject.GetComponent<Toggle>();
	}

	private void OnEnable() {
		SceneManager.sceneLoaded += OnLoadFinish;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnLoadFinish;
	}
	#endregion

	#region M�thodes priv�es
	// Va cocher le bon toggle selon le language.
	private void ChangeLanguage(Lang language) {
		switch (language) {
			case Lang.French:
				toggleLanguageFrench.isOn = true;
				break;
			case Lang.DutchBe:
				toggleLanguageDutchBe.isOn = true;
				break;
			case Lang.DutchNe:
				toggleLanguageDutchNe.isOn = true;
				break;
			case Lang.German:
				toggleLanguageGerman.isOn = true;
				break;
			case Lang.English:
			default:
				toggleLanguageEnglish.isOn = true;
				break;
		}
	}

	// Va cocher le bon toggle selon la difficult�.
	private void ChangeDifficulty(DifficultyLevel difficulty) {
		switch (difficulty) {
			case DifficultyLevel.MLT:
				toggleDifficultyTLM.isOn = true;
				break;
			case DifficultyLevel.Secondary:
			default:
				toggleDifficultySecondary.isOn = true;
				break;
		}
	}

	// Va cocher le bon toggle selon le sc�nario.
	private void ChangeScenario(int scenario) {
		switch (scenario) {
			case 1:
				toggleScenario1.isOn = true;
				break;
			case 0:
			default:
				toggleScenarioTutorial.isOn = true;
				break;
		}
	}

	// Va cocher le bon toggle selon le mode de cam�ra.
	private void ChangeCamera(CameraModes camera) {
		switch (camera) {
			case CameraModes.Snap:
				toggleCameraSnap.isOn = true;
				break;
			case CameraModes.Continuous:
				toggleCameraContinuous.isOn = true;
				break;
			case CameraModes.Disabled:
			default:
				toggleCameraDisabled.isOn = true;
				break;
		}
	}

	// Va cocher le bon toggle selon le mode de mouvement.
	private void ChangeMove(bool move) {
		if (!move) {
			toggleMoveDisabled.isOn = true;
		}
		else {
			toggleMoveFree.isOn = true;
		}
	}

	// Va cocher le bon toggle selon le type de contr�les
	private void ChangeControls(bool simple) {
		if (simple) {
			toggleControlsSimple.isOn = true;
		}
		else {
			toggleControlsNormal.isOn = true;
		}
	}

	// Va quitter le jeu.
	private void Quit() {
		Application.Quit(); // Quitter le jeu proprement
	}
	#endregion

	#region Action listeners
	// Action lorsqu'un niveau a fini de se charger. Va charger les donn�es des PlayerPrefs.
	private void OnLoadFinish(Scene scene, LoadSceneMode mode) {
		Debug.Log("Level loaded - " + scene.name + " - mode : " + mode);

		// Si cette ligne n'est pas pr�sente, rien ne fonctionne pour la localisation (�a doit s'init quand on appelle pour la premi�re fois LocalizationSettings sans doute)
		Debug.Log("Default locale : " + LocalizationSettings.SelectedLocale);

		// R�cup�rer les donn�es stock�es dans le PlayerPrefs
		language = PlayerPrefsManager.Language;
		difficulty = Difficulty;
		scenario = Scenario;
		cameraMode = CameraMode;
		freeMove = FreeMove;
		controlsSimple = ControlsSimple;

		// Changer les toggles s�lectionn�s par d�faut selon les param�tres
		ChangeLanguage(language);
		ChangeDifficulty(difficulty);
		ChangeScenario(scenario);
		ChangeCamera(cameraMode);
		ChangeMove(FreeMove);
		ChangeControls(controlsSimple);
	}

	// Action lorsqu'un toggle se fait hover. Va changer sa couleur.
	public void ToggleHoverEnter(Toggle sender) {
		if (sender.interactable) { // Ne le prendre en compte que s'il est activ�
			sender.gameObject.GetComponent<Image>().color = new Color32(255, 191, 127, 255);
		}
	}

	// Action lorsqu'un toggle ne se fait plus hover. Va reset sa couleur.
	public void ToggleHoverExit(Toggle sender) {
		if (sender.interactable) { // Ne le prendre en compte que s'il est activ�
			sender.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
		}
	}

	// Action lorsque l'on appuye sur un toggle de language. Va changer la langue du jeu et mettre � jour les PlayerPrefs.
	public void ToggleLanguageChanged(int langue) {
		language = (Lang) langue;
		PlayerPrefsManager.Language = language;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int) language];
		Debug.Log("Locale changed : " + LocalizationSettings.AvailableLocales.Locales[(int) language].LocaleName);
	}

	// Action lorsque l'on appuye sur un toggle de difficult�. Va changer la difficult� et mettre � jour les PlayerPrefs.
	public void ToggleDifficultyChanged(int diff) {
		difficulty = (DifficultyLevel) diff;
		Difficulty = difficulty;
	}

	// Action lorsque l'on appuye sur un toggle de sc�nario. Va changer le sc�nario actuellement s�lectionn�.
	public void ToggleScenarioChanged(int scenar) {
		scenario = scenar;
		Scenario = scenario;
	}

	// Action lorsque l'on appuye sur un toggle de cam�ra. Va changer le mode de cam�ra et mettre � jour les PlayerPrefs.
	public void ToggleCameraChanged(int camera) {
		cameraMode = (CameraModes) camera;
		CameraMode = cameraMode;
		locomotionController.ChangeCameraMode();
	}

	// Action lorsque l'on appuye sur un toggle de mouvement. Va changer le mode de mouvement et mettre � jour les PlayerPrefs.
	public void ToggleMoveChanged(bool move) {
		freeMove = move;
		FreeMove = freeMove;
		locomotionController.ChangeMove();

		if (!move) { // Ret�l�porter le joueur au centre de la pi�ce devant l'�cran du menu si on d�sactive le mouvement libre
			MasterControllerAction.Instance.gameObject.transform.position = Vector3.zero;
		}
	}

	// Action lorsque l'on appuye sur un toggle de contr�les. Va changer le mode de contr�les et mettre � jour les PlayerPrefs.
	public void ToggleControlsChanged(bool simple) {
		controlsSimple = simple;
		ControlsSimple = controlsSimple;

		if (simple) {
			changeInputActionMap.SwitchControlsToSimple();
		}
		else {
			changeInputActionMap.SwitchControlsToNormal();
		}
	}

	// Action lorsque l'on clique sur le bouton Start. Va sauver les PlayerPrefs et charger le bon sc�nario.
	public void StartGame() {
		Save(); // Sauvegarder les PlayerPrefs

		string scene = scenario switch {
			1 => "Level1-Loader",
			_ => "Level0-Loader",
		};

		// Aller rechercher le fader de la cam�ra principale afin d'activer le fondu au noir
		FadeScreen fader = Camera.main.GetComponentInChildren<FadeScreen>();
		fader.FadeIn();

		StartCoroutine(FadeLoadSceneCoroutine(fader.fadeDuration, scene));
	}

	// Action lorsque l'on clique sur le bouton Quit. Va activer le fondu au noir avant de quitter le jeu.
	public void QuitGame() {
		// Aller rechercher le fader de la cam�ra principale afin d'activer le fondu au noir
		FadeScreen fader = Camera.main.GetComponentInChildren<FadeScreen>();
		fader.FadeIn();
		StartCoroutine(fader.FadeWaitCoroutine(Quit));
	}
	#endregion

	#region Coroutines
	// Coroutine qui va charger une sc�ne apr�s un certain temps d'attente (pour le fondu).
	private IEnumerator FadeLoadSceneCoroutine(float seconds, string scene) {
		yield return new WaitForSeconds(seconds);
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}
	#endregion
}
