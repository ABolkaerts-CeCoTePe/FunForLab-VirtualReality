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
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Classe statique qui va permettre de parser le contenu d'un fichier .srt en une liste de SubtitlesLine.
/// </summary>
public static class SrtSubtitlesParser {
	// Va parser le contenu d'un fichier .srt en une liste de SubtitlesLine.
	public static List<SubtitleLine> ParseSubtitles(string content) {
		List<SubtitleLine> subtitles = new List<SubtitleLine>();

		Regex regex = new Regex($@"(?<index>\d*\s*)\n(?<start>\d*:\d*:\d*,\d*)\s*-->\s*(?<end>\d*:\d*:\d*,\d*)\s*\n(?<content>.*)\n(?<content2>.*)\n"); // TODO la Regex n'a l'air de supporter que 2 lignes de sous-titres dans le fichier, pas plus
		MatchCollection matches = regex.Matches(content);

		foreach (Match match in matches) {
			GroupCollection groups = match.Groups;

			int ind = int.Parse(groups["index"].Value);
			TimeSpan.TryParse(groups["start"].Value.Replace(',', '.'), out TimeSpan fromtime);
			TimeSpan.TryParse(groups["end"].Value.Replace(',', '.'), out TimeSpan totime);
			string contenttext = groups["content"].Value;

			subtitles.Add(new SubtitleLine(ind, fromtime.TotalSeconds, totime.TotalSeconds, contenttext));
		}

		subtitles.Sort();
		return subtitles;
	}
}
