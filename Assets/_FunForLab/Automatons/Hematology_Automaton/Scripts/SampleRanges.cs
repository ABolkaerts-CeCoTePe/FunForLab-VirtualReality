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
using UnityEngine;

/// <summary>
/// Classe ScriptableObject qui va contenir les ranges de valeurs selon certains critères.
/// </summary>
[CreateAssetMenu(fileName = "SampleRanges", menuName = "Automations/Sample Ranges", order = 1)]
public class SampleRanges : ScriptableObject {
	public char patientSex; // Le sexe auquel ces ranges s'appliquent
	public int patientYearsMin; // L'age min à partir duquel ces ranges s'appliquent
	public int patientYearsMax; // L'age max à partir duquel ces ranges s'appliquent
	public bool pregnant; // Si ces ranges sont pour une personne enceinte
	
	// Si les ranges sont à -1, il n'y a pas de limite

	#region CBC test
	// Données pour un test CBC
	[Header("CBC test")]

	// CBC 1
	[Tooltip("White Blood cell Count")]
	public float wBC_min; // WBC 10^3/µl
	public float wBC_max; // WBC 10^3/µl

	[Tooltip("Red Blood cell Count")]
	public float rBC_min; // RBC 10^6/µl
	public float rBC_max; // RBC 10^6/µl

	[Tooltip("Hemoglobin")]
	public float hGB_min; // HGB ou Hb g/dl
	public float hGB_max; // HGB ou Hb g/dl

	[Tooltip("Hematocrit")]
	public float hCT_min; // HCT %
	public float hCT_max; // HCT %

	[Tooltip("Mean Cell Volume")]
	public float mCV_min; // MCV fl (10^-15 l)
	public float mCV_max; // MCV fl (10^-15 l)

	[Tooltip("Mean Cell Hemoglobin")]
	public float mCH_min; // MCH pg (10^-12 g)
	public float mCH_max; // MCH pg (10^-12 g)

	[Tooltip("Mean Corpuscular Hemoglobin Concentration")]
	public float mCHC_min; // MCHC g/dl (10^-12 g)
	public float mCHC_max; // MCHC g/dl (10^-12 g)

	[Tooltip("Platelets")]
	public float pLT_min; // PLT 10^3/µl
	public float pLT_max; // PLT 10^3/µl

	// CBC 2
	[Space]

	public float rDWSD_min; // RDW-SD fl
	public float rDWSD_max; // RDW-SD fl

	public float rDWCV_min; // RDW-CV %
	public float rDWCV_max; // RDW-CV %

	public float pDW_min; // PDW fl
	public float pDW_max; // PDW fl

	public float mPV_min; // MPV fl
	public float mPV_max; // MPV fl

	public float pLCR_min; // P-LCR %
	public float pLCR_max; // P-LCR %

	public float pCT_min; // PCT %
	public float pCT_max; // PCT %

	// CBC 3
	[Space]

	// Pas de range de valeurs pour eux 2
	public float nRBCH_min; // NRBC# 10^3/µl
	public float nRBCH_max; // NRBC# 10^3/µl

	public float nRBCP_min; // NRBC% %
	public float nRBCP_max; // NRBC% %
	#endregion

	#region DIFF test
	// Données pour un test DIFF
	[Header("DIFF test")]

	// DIFF 1
	public float neutH_min; // NEUT# 10^3/µl
	public float neutH_max; // NEUT# 10^3/µl

	public float lymphH_min; // LYMPH# 10^3/µl
	public float lymphH_max; // LYMPH# 10^3/µl

	public float monoH_min; // MONO# 10^3/µl
	public float monoH_max; // MONO# 10^3/µl

	public float eoH_min; // EO# 10^3/µl
	public float eoH_max; // EO# 10^3/µl

	public float basoH_min; // BASO# 10^3/µl
	public float basoH_max; // BASO# 10^3/µl

	// DIFF 2
	[Space]

	public float neutP_min; // NEUT% %
	public float neutP_max; // NEUT% %

	public float lymphP_min; // LYMPH% %
	public float lymphP_max; // LYMPH% %

	public float monoP_min; // MONO% %
	public float monoP_max; // MONO% %

	public float eoP_min; // EO% %
	public float eoP_max; // EO% %

	public float basoP_min; // BASO% %
	public float basoP_max; // BASO% %

	// DIFF 3
	[Space]

	// Pas de range de valeurs pour eux 2
	public float iGH_min; // IG# 10^3/µl
	public float iGH_max; // IG# 10^3/µl

	public float iGP_min; // IG% %
	public float iGP_max; // IG% %
	#endregion

	#region RET test
	// Données pour un test RET
	[Header("RET test")]

	public float rETP_min; // RET% %
	public float rETP_max; // RET% %

	public float rETH_min; // RET# 10^6/µl
	public float rETH_max; // RET# 10^6/µl

	public float iRF_min; // IRF %
	public float iRF_max; // IRF %

	public float lFR_min; // LFR %
	public float lFR_max; // LFR %

	public float mFR_min; // MFR %
	public float mFR_max; // MFR %

	public float hFR_min; // HFR %
	public float hFR_max; // HFR %

	public float rETHe_min; // RET-He pg
	public float rETHe_max; // RET-He pg
	#endregion

	#region PLT-F test
	// Données pour un test PLT-F
	[Header("PLT-F test")]

	public float iPF_min; // IPF %
	public float iPF_max; // IPF %
	#endregion

	#region WBC BF test
	// Données pour un test WBC BF
	[Header("WBC BF test")]

	public float wBCBF_min; // WBC-BF 10^3/µl
	public float wBCBF_max; // WBC-BF 10^3/µl
	#endregion

	#region RBC BF test
	// Données pour un test RBC BF
	[Header("RBC BF test")]

	public float rBCBF_min; // RBC-BF 10^6/µl
	public float rBCBF_max; // RBC-BF 10^6/µl
	#endregion

	#region WBC Diff BF test
	// Données pour un test WBC Diff BF
	[Header("WBC Differential BF test")]

	public float mNH_min; // MN# 10^3/µl
	public float mNH_max; // MN# 10^3/µl

	public float pMNH_min; // PMN# 10^3/µl
	public float pMNH_max; // PMN# 10^3/µl

	public float mNP_min; // MN% %
	public float mNP_max; // MN% %

	public float pMNP_min; // PMN% %
	public float pMNP_max; // PMN% %

	public float tCBFH_min; // TC-BF# 10^3/µl
	public float tCBFH_max; // TC-BF# 10^3/µl
	#endregion
}
