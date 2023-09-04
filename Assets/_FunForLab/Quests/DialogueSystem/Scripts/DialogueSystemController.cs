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
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe qui contient des fonctions qui seront appel�es par le sc�nario.
/// </summary>
public class DialogueSystemController : MonoBehaviour
{
	public TableReference tutorialInstructionTable;
	public TableReference stringTable;

	public string currentMainQuest;
	public UnityEvent onQuestEntryChange = new UnityEvent();

	public DirectionGuideController directionGuide;
	public Transform[] directionGuidePositions;

	private Coroutine showGuideCoroutine;

	[SerializeField] private EventManager eventManager;

	private void OnEnable()
	{ // Enregistrer des m�thodes C# afin qu'elles puissent �tre appel�es dans le code Lua du Dialogue System
		Lua.RegisterFunction("ExitToGameMenu", this, SymbolExtensions.GetMethodInfo(() => ExitToGameMenu()));
        Lua.RegisterFunction("LocaliseString", this, SymbolExtensions.GetMethodInfo(() => LocaliseTutorialInstruction(string.Empty)));
        Lua.RegisterFunction("LocalizeObjective", this, SymbolExtensions.GetMethodInfo(() => LocalizeObjective(string.Empty)));
		Lua.RegisterFunction("SetDirectionGuide", this, SymbolExtensions.GetMethodInfo(() => SetDirectionGuide(0)));
		Lua.RegisterFunction("DisableDirectionGuide", this, SymbolExtensions.GetMethodInfo(() => DisableDirectionGuide()));
		Lua.RegisterFunction("LocaliseTutorialInstruction", this, SymbolExtensions.GetMethodInfo(() => LocaliseTutorialInstruction(string.Empty)));
		Lua.RegisterFunction("WaitForSeconds", this, SymbolExtensions.GetMethodInfo(() => WaitForSeconds(float.NaN)));
		Lua.RegisterFunction("StartEvent", this, SymbolExtensions.GetMethodInfo(() => StartEvent(float.NaN)));

    }

	private void OnDisable()
	{
		Lua.UnregisterFunction("ExitToGameMenu");
		Lua.UnregisterFunction("LocalizeObjective");
		Lua.UnregisterFunction("LocaliseString");
        Lua.UnregisterFunction("SetDirectionGuide");
		Lua.UnregisterFunction("DisableDirectionGuide");
		Lua.UnregisterFunction("LocaliseTutorialInstruction");
        Lua.UnregisterFunction("WaitForSeconds");
		Lua.UnregisterFunction("StartEvent");
    }

	private void Awake()
	{
		DialogueLua.SetVariable("CameraMode", (double)PlayerPrefsManager.CameraMode); // Mettre � jour les variables du Dialogue avec les donn�es des PlayerPrefs
		DialogueLua.SetVariable("FreeMove", PlayerPrefsManager.FreeMove);
		DialogueLua.SetVariable("ControlsSimple", PlayerPrefsManager.ControlsSimple);
		DialogueLua.SetVariable("AudioLang", GetDialogueLanguage());

		ResetQuest(currentMainQuest);
		ChangeDialogueLanguage();
		DisableDirectionGuide();
	}

	// Va remettre � z�ro une qu�te ainsi que toutes ses sous-t�ches.
	public static void ResetQuest(string quest)
	{
		Debug.Log("Reset quest " + quest);

		QuestLog.SetQuestState(quest, QuestState.Unassigned);
		int entries = QuestLog.GetQuestEntryCount(quest);

		for (int i = 0; i < entries; i++)
		{
			QuestLog.SetQuestEntryState(quest, i + 1, QuestState.Unassigned);
		}
	}

	// La langue du Dialogue Manager change d'elle m�me vers fr-BE par exemple, alors qu'il est configur� en fr seulement dans Dialogue.
	public static void ChangeDialogueLanguage()
	{
		DialogueManager.SetLanguage(GetDialogueLanguage());
	}

	// Retourne le code de base de la langue selon Language de PlayerPrefsManager.
	public static string GetDialogueLanguage()
	{
		switch (PlayerPrefsManager.Language)
		{
			case PlayerPrefsManager.Lang.English:
			default:
				return "en";
			case PlayerPrefsManager.Lang.French:
				return "fr";
			case PlayerPrefsManager.Lang.DutchBe:
			case PlayerPrefsManager.Lang.DutchNe:
				return "nl";
			case PlayerPrefsManager.Lang.German:
				return "de";
		}
	}

	// Est appel� automatiquement car DialogueManager fait des BroadcastMessage sur le nom de cette fonction � chaque changement dans une subtask.
	public void OnQuestEntryStateChange(QuestEntryArgs args)
	{ // Quand toutes les subtasks sont compl�t�es, la qu�te de base ne l'est pas ! Il faut le faire manuellement !
		if (QuestLog.IsQuestActive(args.questName))
		{
			int numEntries = QuestLog.GetQuestEntryCount(args.questName);
			int numEntriesSuccessful = 0;

			for (int i = 1; i <= numEntries; i++)
			{
				if (QuestLog.GetQuestEntryState(args.questName, i) == QuestState.Success)
				{
					numEntriesSuccessful++;
				}
			}

			if (numEntriesSuccessful == numEntries)
			{
				Debug.Log(args.questName + " success !");
				QuestLog.CompleteQuest(args.questName);
				DisableDirectionGuide();
			}

			onQuestEntryChange?.Invoke(); // Appeler l'event (sera DrawScenarioElements() de QuestLogToDoList)
		}
	}

	// Action lorsque l'on appuye sur le bouton Exit. Va quitter la sc�ne et charger le menu principal.
	public void ExitToGameMenu()
	{
		Debug.Log("Exit To Game Menu !");
		FadeScreen fader = Camera.main.GetComponentInChildren<FadeScreen>(); // Aller rechercher le fader de la cam�ra principale afin d'activer le fondu au noir
		fader.FadeIn();
		StartCoroutine(fader.FadeWaitCoroutine(LoadMenu)); // Appeler la fonction LoadMenu() une fois que la coroutine � fini.
	}

	// Va charger la sc�ne du Menu Principal.
	private void LoadMenu()
	{
		SceneManager.LoadScene("GameMenu", LoadSceneMode.Single); // Retourner au menu principal.
	}

	// Va permettre de retourner au Dialogue Lua un �l�ment qui est dans la table de localisation.
	public string LocalizeObjective(string key)
	{
		return LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, "OBJECTIVE") + " " + LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, key);
	}

	public string LocaliseTutorialInstruction(string key)
	{
		return WrapText(LocalizationSettings.StringDatabase.GetLocalizedString(tutorialInstructionTable, key),50);
	}
    public string LocaliseString(string key)
    {
        return WrapText(LocalizationSettings.StringDatabase.GetLocalizedString(stringTable, key), 55);
    }

    public string WrapText(string text, int maxCharacters)
    {
        string wrappedText = "";
        int characterCount = 0;

        string[] words = text.Split(' ');

        foreach (string word in words)
        {
            // V�rifier si l'ajout de ce mot d�passe la limite de caract�res
            if (characterCount + word.Length > maxCharacters)
            {
                wrappedText += "\n";
                characterCount = 0;
            }

            wrappedText += word + " ";
            characterCount += word.Length + 1; // +1 pour prendre en compte l'espace

            // V�rifier si le mot d�passe d�j� la limite de caract�res
            if (word.Length > maxCharacters)
            {
                wrappedText += "\n";
                characterCount = 0;
            }
        }

        return wrappedText;
    }

    // Va d�finir la nouvelle direction du guide.
    public void SetDirectionGuide(double index)
	{
		try
		{
			Debug.Log("Set Direction Guide to index " + index + " : " + directionGuidePositions[(int)index].position + " !");
			directionGuide.gameObject.SetActive(false);
			directionGuide.SetDirection(directionGuidePositions[(int)index]);

			if (showGuideCoroutine != null)
			{ // Si la coroutine est d�j� activ�e, on la stoppe avant de la relancer, pour recommencer le timer de 30 secondes
				StopCoroutine(showGuideCoroutine);
			}

			showGuideCoroutine = StartCoroutine(ShowDelayedDirectionGuide());
		}
		catch (Exception)
		{
			Debug.Log("Set Direction Guide index outside of range !");
		}
	}

	// Version pour l'Editeur car pas moyen d'avoir une m�thode avec double comme param�tre (et Lua ne fonctionne qu'avec des double).
	public void SetDirectionGuideEditor(int index)
	{
		SetDirectionGuide(index);
	}

	// Va d�sactiver le guide.
	public void DisableDirectionGuide()
	{
		if (showGuideCoroutine != null)
		{
			StopCoroutine(showGuideCoroutine);
			showGuideCoroutine = null;
		}

		directionGuide.gameObject.SetActive(false);
	}

	// Coroutine qui va activer la fl�che de direction apr�s 30 secondes.
	private IEnumerator ShowDelayedDirectionGuide()
	{
		yield return new WaitForSeconds(30); // N'activer le guide qu'apr�s 30 secondes
		Debug.Log("ShowDelayedDirectionGuide ended");
		directionGuide.gameObject.SetActive(true);
		showGuideCoroutine = null;
	}

	private void WaitForSeconds(float seconds)
	{
		StartCoroutine(Waiter(seconds));
	}

	private IEnumerator Waiter(float seconds)
	{
		yield return new WaitForSeconds(seconds);
	}

	private void StartEvent(float eventIndex)
	{
		eventManager.LaunchEvent((int)eventIndex);
	}
}
