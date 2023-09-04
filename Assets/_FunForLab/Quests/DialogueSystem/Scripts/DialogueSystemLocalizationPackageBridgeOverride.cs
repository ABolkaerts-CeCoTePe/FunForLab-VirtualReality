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
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Modification de la classe DialogueSystemLocalizationPackageBridge de Dialogue System, en empêchant l'event SelectedLocaleChanged d'être fait.
/// </summary>
public class DialogueSystemLocalizationPackageBridgeOverride : MonoBehaviour {

	public List<LocalizedStringTable> localizedStringTables;
	public Locale defaultLocale;
	public string uniqueFieldTitle = "Guid";

	private readonly List<UnityEngine.Localization.Tables.StringTable> tables = new List<UnityEngine.Localization.Tables.StringTable>();

	private IEnumerator Start() {
		yield return LocalizationSettings.InitializationOperation;
		yield return null;
		UpdateActorDisplayNames();
		Localization.language = LocalizationSettings.SelectedLocale.Identifier.Code;
		CacheStringTables();
	}

	public void CacheStringTables() {
		tables.Clear();
		foreach (LocalizedStringTable table in localizedStringTables) {
			if (table != null) {
				tables.Add(table.GetTable());
			}
		}
	}

	public void UpdateActorDisplayNames() {
		Locale locale = LocalizationSettings.SelectedLocale;
		Localization.language = locale.Identifier.Code;

		foreach (Actor actor in DialogueManager.masterDatabase.actors) {
			string guid = actor.LookupValue(uniqueFieldTitle);

			if (!string.IsNullOrEmpty(guid)) {
				foreach (UnityEngine.Localization.Tables.StringTable table in tables) {
					UnityEngine.Localization.Tables.StringTableEntry stringTableEntry = table[guid];

					if (stringTableEntry != null) {
						string fieldTitle = (locale == defaultLocale) ? "Display Name" : ("Display Name " + locale.Identifier.Code);
						DialogueLua.SetActorField(actor.Name, fieldTitle, stringTableEntry.LocalizedValue);
						break;
					}
				}
			}
		}
	}

	public void OnConversationLine(Subtitle subtitle) {
		if (string.IsNullOrEmpty(subtitle.formattedText.text)) {
			return;
		}

		string guid = Field.LookupValue(subtitle.dialogueEntry.fields, uniqueFieldTitle);

		foreach (UnityEngine.Localization.Tables.StringTable table in tables) {
			UnityEngine.Localization.Tables.StringTableEntry stringTableEntry = table[guid];

			if (stringTableEntry != null) {
				string localizedValue = stringTableEntry.LocalizedValue;
				subtitle.formattedText = FormattedText.Parse(localizedValue);
				break;
			}
		}
	}

	public void OnConversationResponseMenu(Response[] responses) {
		foreach (Response response in responses) {
			string guid = Field.LookupValue(response.destinationEntry.fields, uniqueFieldTitle);

			foreach (UnityEngine.Localization.Tables.StringTable table in tables) {
				UnityEngine.Localization.Tables.StringTableEntry stringTableEntry = table[guid + "_MenuText"];

				if (stringTableEntry != null) {
					string localizedValue = (stringTableEntry != null) ? stringTableEntry.LocalizedValue : table[guid].LocalizedValue;
					response.formattedText = FormattedText.Parse(localizedValue);
					break;
				}
			}
		}
	}
}
