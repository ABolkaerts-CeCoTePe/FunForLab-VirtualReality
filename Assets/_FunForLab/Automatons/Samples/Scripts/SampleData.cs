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
using System.Globalization;
using UnityEngine;
using static HematoAutomatonController;
using static SampleController;

/// <summary>
/// Classe ScriptableObject qui va contenir les donn�es sur l'�chantillon.
/// </summary>
[CreateAssetMenu(fileName = "SampleData", menuName = "Samples/Sample Data", order = 1)]
public class SampleData : ScriptableObject {
	// TODO Voir pour toutes les donn�es � ajouter

	public enum Arm {
		Left,
		Right
	}

	#region Sample info
	[Header("Sample info")]

	public SampleType sampleType; // Type de l'�chantillon

	[HideInInspector]
	public ModeFonctionnement typeFonctionnement; // Mode Automatique ou Manuel

	[HideInInspector]
	public TypeMesure modeAnalyse; // Type de mesure que l'automate � effectu�

	[HideInInspector]
	public TypeTest[] testsDiscrets; // Tests discrets que l'automate � effectu�

	public bool positive; // Si le test est positif ou non
	public bool validated; // Si le test est valid� ou non

	public string sampleID; // Identifiant de l'�chantillon

	public string dateCollect; // Date de la prise de sang

	public DateTime DateCollect {
		get
		{
			return DateTime.ParseExact(dateCollect, "dd/MM/yyyy hh:mm", null);
		}
	}

	public string collectID; // Identifiant de la personne qui a fait la prise de sang
	public Arm arm; // Quel bras pour la prise de sang (gauche ou droit)

	[HideInInspector]
	public DateTime dateTest; // Date du test (pas de la prise de sang)

	[HideInInspector]
	public string emplacementRack; // Emplacement du test dans le rack

	[HideInInspector]
	public bool qcPresent; // Est-ce qu'un �chantillon de QC �tait pr�sent durant l'analyse ou non ?

	[HideInInspector]
	public bool mixed; // Est-ce que l'�chantillon est mix� au moment de l'analyse ou non ?

	public string commentSample; // Commentaire sur l'�chantillon
	#endregion

	#region Patient info
	[Header("Patient info")]
	public string hospital; // Nom de l'h�pital
	public string doctor; // Nom du m�decin
	public string patientID; // ID du patient
	public string patientBirthday; // Date de naissance du patient (sera convertie en DateTime car pas moyen d'avoir de s�lecteur de date dans l'inspecteur Unity)

	public DateTime PatientBirthday {
		get {
            return DateTime.ParseExact(patientBirthday, "dd/MM/yyyy", null); 
		}
	}

	public char patientSex; // Sexe du patient (M ou F)
	public int patientYears; // TODO Age du patient pourrait �tre juste calcul�, on a la date de naissance !
	public string patientName; // Nom du patient
	public string commentPatient; // Commentaire sur le patient
	#endregion

	#region CBC test
	// Donn�es pour un test CBC
	[Header("CBC test")]

	// CBC 1
	[Tooltip("White Blood cell Count")]
	public float wBC; // WBC 10^3/�l (bon entre 4.3 - 11.9)

	[Tooltip("Red Blood cell Count")]
	public float rBC; // RBC 10^6/�l (bon entre 4.0 - 5.2)

	[Tooltip("Hemoglobin")]
	public float hGB; // HGB ou Hb g/dl (bon entre 11.7 - 14.0)

	[Tooltip("Hematocrit")]
	public float hCT; // HCT % (bon entre 35.2 - 45.4)

	[Tooltip("Mean Cell Volume")]
	public float mCV; // MCV fl (10^-15 l) (bon entre 80.9 - 97.3)

	[Tooltip("Mean Cell Hemoglobin")]
	public float mCH; // MCH pg (10^-12 g) (bon entre 26.5 - 32.6)

	[Tooltip("Mean Corpuscular Hemoglobin Concentration")]
	public float mCHC; // MCHC g/dl (10^-12 g) (bon entre 330 - 360) peut �tre calcul� (vaut HGB / HCT)

	[Tooltip("Platelets")]
	public int pLT; // PLT 10^3/�l (bon entre 170 - 400)

	// CBC 2
	[Space]

	public float rDWSD; // RDW-SD fl
	public float rDWCV; // RDW-CV %
	public float pDW; // PDW fl
	public float mPV; // MPV fl
	public float pLCR; // P-LCR %
	public float pCT; // PCT %

	// CBC 3
	[Space]

	// Pas de range de valeurs pour eux 2
	public float nRBCH; // NRBC# 10^3/�l
	public float nRBCP; // NRBC% %
	#endregion

	#region DIFF test
	// Donn�es pour un test DIFF
	[Header("DIFF test")]

	// DIFF 1
	public float neutH; // NEUT# 10^3/�l
	public float lymphH; // LYMPH# 10^3/�l
	public float monoH; // MONO# 10^3/�l
	public float eoH; // EO# 10^3/�l
	public float basoH; // BASO# 10^3/�l

	// DIFF 2
	[Space]

	public float neutP; // NEUT% %
	public float lymphP; // LYMPH% %
	public float monoP; // MONO% %
	public float eoP; // EO% %
	public float basoP; // BASO% %

	// DIFF 3
	[Space]

	// Pas de range de valeurs pour eux 2
	public float iGH; // IG# 10^3/�l
	public float iGP; // IG% %
	#endregion

	#region RET test
	// Donn�es pour un test RET
	[Header("RET test")]

	public float rETP; // RET% %
	public float rETH; // RET# 10^6/�l
	public float iRF; // IRF %
	public float lFR; // LFR %
	public float mFR; // MFR %
	public float hFR; // HFR %
	public float rETHe; // RET-He pg
	#endregion

	#region PLT-F test
	// Donn�es pour un test PLT-F
	[Header("PLT-F test")]

	public float iPF; // IPF %
	#endregion

	#region WBC BF test
	// Donn�es pour un test WBC BF
	[Header("WBC BF test")]

	public float wBCBF; // WBC-BF 10^3/�l
	#endregion

	#region RBC BF test
	// Donn�es pour un test RBC BF
	[Header("RBC BF test")]

	public float rBCBF; // RBC-BF 10^6/�l
	#endregion

	#region WBC Diff BF test
	// Donn�es pour un test WBC Diff BF
	[Header("WBC Differential BF test")]

	public float mNH; // MN# 10^3/�l
	public float pMNH; // PMN# 10^3/�l
	public float mNP; // MN% %
	public float pMNP; // PMN% %
	public float tCBFH; // TC-BF# 10^3/�l
	#endregion

	#region Messages
	// Donn�es pour un test WBC Diff BF
	[Header("Flags / Messages")]

	public string wBCFlags;
	public string rBCFlags;
	public string pLTFlags;

	public string actions;
	public string errors;
	#endregion
}
