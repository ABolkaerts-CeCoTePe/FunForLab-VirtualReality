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
using RootMotion.FinalIK;
using UnityEngine;
using static PlayerPrefsManager;

/// <summary>
/// Classe qui va gérer le modèle du joueur et le fait d'en changer.
/// </summary>
public class PlayerModelController : MonoBehaviour {
	public static PlayerModelController Instance { get; private set; } = null;

	private GameObject _currentModel;
	public PlayerModels PlayerModel;
/*
	public Transform LeftThumbTarget;
	public Transform LeftIndexTarget;
	public Transform LeftMiddleTarget;
	public Transform LeftRingTarget;
	public Transform LeftPinkyTarget;

	public Transform RightThumbTarget;
	public Transform RightIndexTarget;
	public Transform RightMiddleTarget;
	public Transform RightRingTarget;
	public Transform RightPinkyTarget;*/

	public PlayerModelTargets[] PlayerModels;

	private BeltController _beltController;

	private void Awake() {
		Instance = this;
	//	_beltController = GetComponent<BeltController>();
	}

	private void Start() {
		Debug.Log("Start Skin = " + PlayerModel);
		ChangeSkinModel(PlayerModel);
	}
	public void PlantFeet(bool state)
	{
        VRIK modelVRIK = _currentModel.GetComponent<VRIK>();
		modelVRIK.solver.plantFeet = state;
	}
	// Va changer le skin du joueur, en remettant les positions des doigts et de la ceinture aux bons endroits.
	public void ChangeSkinModel(PlayerModels skin) {
		PlayerModel = skin;

		if (_currentModel != null) {
			Destroy(_currentModel);
		}

		_currentModel = Instantiate(PlayerModels[(int) PlayerModel].prefabModel, gameObject.transform);

		VRIK modelVRIK = _currentModel.GetComponent<VRIK>();
		modelVRIK.solver.spine.headTarget = PlayerModels[(int) PlayerModel].modelHeadTarget;
		modelVRIK.solver.leftArm.target = PlayerModels[(int) PlayerModel].modelLeftHandTarget;
		modelVRIK.solver.rightArm.target = PlayerModels[(int) PlayerModel].modelRightHandTarget;
/*
		FingerRig[] fingerRigs = _currentModel.GetComponentsInChildren<FingerRig>();

		fingerRigs[0].fingers[0].target = LeftThumbTarget;
		fingerRigs[0].fingers[1].target = LeftIndexTarget;
		fingerRigs[0].fingers[2].target = LeftMiddleTarget;
		fingerRigs[0].fingers[3].target = LeftRingTarget;
		fingerRigs[0].fingers[4].target = LeftPinkyTarget;

		fingerRigs[1].fingers[0].target = RightThumbTarget;
		fingerRigs[1].fingers[1].target = RightIndexTarget;
		fingerRigs[1].fingers[2].target = RightMiddleTarget;
		fingerRigs[1].fingers[3].target = RightRingTarget;
		fingerRigs[1].fingers[4].target = RightPinkyTarget;
*/
		MasterControllerAction.Instance.solver = (IKSolverVR) _currentModel.GetComponent<VRIK>().GetIKSolver();
		//PlayerModel = skin; // Garder le dernier skin en mémoire, pour rejouer directement avec

	//	_beltController.ChangeModelKeepBelt(_currentModel.GetComponent<BeltTransform>().beltPos);
	}
}
