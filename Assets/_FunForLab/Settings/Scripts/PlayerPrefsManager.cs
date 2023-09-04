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
using UnityEngine;

/// <summary>
/// Classe statique qui gère toutes les PlayerPrefs.
/// </summary>
public static class PlayerPrefsManager {
	private static readonly string KEY_LANG = "language";
	private static readonly string KEY_DIFFICULTY = "difficulty";
	private static readonly string KEY_SCENARIO = "scenario";
	private static readonly string KEY_CAMERA_MODE = "camera_mode";
	private static readonly string KEY_FREE_MOVE = "free_move";
	private static readonly string KEY_CONTROLS_SIMPLE = "controls_simple";
	private static readonly string KEY_PLAYER_MODEL = "player_model";

	private static Lang language;
	private static DifficultyLevel difficulty;
	private static int scenario;
	private static CameraModes camera_snap;
	private static bool free_move;
	private static bool controls_simple;
	private static PlayerModels player_model;

	// Liste des langues du jeu
	public enum Lang {
		English,
		French,
		DutchBe,
		DutchNe,
		German
	}

	// Liste des difficultés du jeu
	public enum DifficultyLevel {
		Secondary,
		MLT
	}

	// Liste des choix de caméra
	public enum CameraModes {
		Disabled,
		Snap,
		Continuous
	}

	public enum PlayerModels {
		SpaceSuit,
		MedicalSuit
	}

	public static Lang Language {
		get {
			return language;
		}
		set {
			language = value;
			PlayerPrefs.SetInt(KEY_LANG, (int) value);
		}
	}

	public static DifficultyLevel Difficulty {
		get {
			return difficulty;
		}
		set {
			difficulty = value;
			PlayerPrefs.SetInt(KEY_DIFFICULTY, (int) value);
		}
	}

	public static int Scenario {
		get {
			return scenario;
		}
		set {
			scenario = value;
			PlayerPrefs.SetInt(KEY_SCENARIO, value);
		}
	}

	public static CameraModes CameraMode {
		get {
			return camera_snap;
		}
		set {
			camera_snap = value;
			PlayerPrefs.SetInt(KEY_CAMERA_MODE, (int) value);
		}
	}

	public static bool FreeMove {
		get {
			return free_move;
		}
		set {
			free_move = value;
			PlayerPrefs.SetInt(KEY_FREE_MOVE, value ? 1 : 0); // Mettra 1 si true, et 0 si false
		}
	}

	public static bool ControlsSimple {
		get {
			return controls_simple;
		}
		set {
			controls_simple = value;
			PlayerPrefs.SetInt(KEY_CONTROLS_SIMPLE, value ? 1 : 0);
		}
	}

	public static PlayerModels PlayerModel {
		get {
			return player_model;
		}
		set {
			player_model = value;
			PlayerPrefs.SetInt(KEY_PLAYER_MODEL, (int) value);
		}
	}


	#region Constructeur
	// Constructeur statique pour récupérer automatiquement les données.
	static PlayerPrefsManager() {
		if (PlayerPrefs.HasKey(KEY_LANG)) { // Récupérer les clés stockées
			Debug.Log("PlayerPrefs exists !");
			language = (Lang) PlayerPrefs.GetInt(KEY_LANG);
			difficulty = (DifficultyLevel) PlayerPrefs.GetInt(KEY_DIFFICULTY);
			scenario = PlayerPrefs.GetInt(KEY_SCENARIO);
			camera_snap = (CameraModes) PlayerPrefs.GetInt(KEY_CAMERA_MODE);
			FreeMove = Convert.ToBoolean(PlayerPrefs.GetInt(KEY_FREE_MOVE));
			ControlsSimple = Convert.ToBoolean(PlayerPrefs.GetInt(KEY_CONTROLS_SIMPLE));
			PlayerModel = (PlayerModels) PlayerPrefs.GetInt(KEY_PLAYER_MODEL);
		}
		else { // Si les clés n'existent pas, on les crées
			Debug.Log("No PlayerPrefs !");
			PlayerPrefs.DeleteAll();

			Language = 0;
			Difficulty = 0;
			Scenario = 0;
			CameraMode = 0;
			FreeMove = false;
			ControlsSimple = true;
			PlayerModel = 0;

			Save();
		}
	}
	#endregion

	#region Méthodes publiques
	// Va sauvegarder les PlayerPrefs sur disque.
	public static void Save() {
		PlayerPrefs.Save(); // Est appelé automatiquement lors d'OnApplicationQuit().
	}
	#endregion
}
