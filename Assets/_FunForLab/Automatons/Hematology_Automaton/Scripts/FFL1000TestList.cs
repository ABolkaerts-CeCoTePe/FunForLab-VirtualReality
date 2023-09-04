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
using System.Collections.Generic;
using static HematoAutomatonController;

/// <summary>
/// Classe statique qui contient toutes les données des tests.
/// </summary>
public static class FFL1000TestList {
	public static List<FFL1000Test> tests = new List<FFL1000Test>() {
		new FFL1000Test() { test = TypeTest.CBC, variable = "wBC", format = "{0,7:0.00}", nom = "WBC", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "rBC", format = "{0,7:0.00}", nom = "RBC", unite = "10^6/µl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "hGB", format = "{0,7:0.0}", nom = "HGB", unite = "g/dl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "hCT", format = "{0,7:0.0}", nom = "HCT", unite = "%" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "mCV", format = "{0,7:0.0}", nom = "MCV", unite = "fl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "mCH", format = "{0,7:0.0}", nom = "MCH", unite = "pg" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "mCHC", format = "{0,7:0.0}", nom = "MCHC", unite = "g/dl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "pLT", format = "{0,7}", nom = "PLT", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "rDWSD", format = "{0,7:0.0}", nom = "RDW-SD", unite = "fl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "rDWCV", format = "{0,7:0.0}", nom = "RDW-CV", unite = "%" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "pDW", format = "{0,7:0.0}", nom = "PDW", unite = "fl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "mPV", format = "{0,7:0.0}", nom = "MPV", unite = "fl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "pLCR", format = "{0,7:0.0}", nom = "P-LCR", unite = "%" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "pCT", format = "{0,7:0.00}", nom = "PCT", unite = "%" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "nRBCH", format = "{0,7:0.00}", nom = "NRBC#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.CBC, variable = "nRBCP", format = "{0,7:0.0}", nom = "NRBC%", unite = "%" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "neutH", format = "{0,7:0.00}", nom = "NEUT#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "lymphH", format = "{0,7:0.00}", nom = "LYMPH#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "monoH", format = "{0,7:0.00}", nom = "MONO#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "eoH", format = "{0,7:0.00}", nom = "EO#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "basoH", format = "{0,7:0.00}", nom = "BASO#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "neutP", format = "{0,7:0.0}", nom = "NEUT%", unite = "%" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "lymphP", format = "{0,7:0.0}", nom = "LYMPH%", unite = "%" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "monoP", format = "{0,7:0.0}", nom = "MONO%", unite = "%" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "eoP", format = "{0,7:0.0}", nom = "EO%", unite = "%" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "basoP", format = "{0,7:0.0}", nom = "BASO%", unite = "%" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "iGH", format = "{0,7:0.00}", nom = "IG#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.DIFF, variable = "iGP", format = "{0,7:0.0}", nom = "IG%", unite = "%" },
		new FFL1000Test() { test = TypeTest.RET, variable = "rETP", format = "{0,7:0.00}", nom = "RET%", unite = "%" },
		new FFL1000Test() { test = TypeTest.RET, variable = "rETH", format = "{0,7:0.0000}", nom = "RET#", unite = "10^6/µl" },
		new FFL1000Test() { test = TypeTest.RET, variable = "iRF", format = "{0,7:0.0}", nom = "IRF", unite = "%" },
		new FFL1000Test() { test = TypeTest.RET, variable = "lFR", format = "{0,7:0.0}", nom = "LFR", unite = "%" },
		new FFL1000Test() { test = TypeTest.RET, variable = "mFR", format = "{0,7:0.0}", nom = "MFR", unite = "%" },
		new FFL1000Test() { test = TypeTest.RET, variable = "hFR", format = "{0,7:0.0}", nom = "HFR", unite = "%" },
		new FFL1000Test() { test = TypeTest.RET, variable = "rETHe", format = "{0,7:0.0}", nom = "RET-He", unite = "pg" },
		new FFL1000Test() { test = TypeTest.PLTF, variable = "iPF", format = "{0,7:0.0}", nom = "IPF", unite = "%" },
		new FFL1000Test() { test = TypeTest.WBC, variable = "wBCBF", format = "{0,7:0.000}", nom = "WBC-BF", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.RBC, variable = "rBCBF", format = "{0,7:0.000}", nom = "RBC-BF", unite = "10^6/µl" },
		new FFL1000Test() { test = TypeTest.WBCDiff, variable = "mNH", format = "{0,7:0.000}", nom = "MN#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.WBCDiff, variable = "pMNH", format = "{0,7:0.000}", nom = "PMN#", unite = "10^3/µl" },
		new FFL1000Test() { test = TypeTest.WBCDiff, variable = "mNP", format = "{0,7:0.0}", nom = "MN%", unite = "%" },
		new FFL1000Test() { test = TypeTest.WBCDiff, variable = "pMNP", format = "{0,7:0.0}", nom = "PMN%", unite = "%" },
		new FFL1000Test() { test = TypeTest.WBCDiff, variable = "tCBFH", format = "{0,7:0.000}", nom = "TC-BF#", unite = "10^3/µl" }
	};
}

/// <summary>
/// Classe qui contient une donnée de test.
/// </summary>
public class FFL1000Test {
	public TypeTest test;
	public string variable;
	public string format;
	public string nom;
	public string unite;
}
