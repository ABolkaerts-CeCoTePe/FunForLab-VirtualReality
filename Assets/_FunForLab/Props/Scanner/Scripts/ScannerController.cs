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
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Classe qui gère le scanner portable et son écran.
/// </summary>
public class ScannerController : MonoBehaviour {
	public UnityEvent OnObjectScanned;
	public GameObject display;
	public Camera cameraLoupe;
	public TMP_Text textTitle;
	public TMP_Text textData;

	public LayerMask layerMaskToIgnore; // Ignorer le layer 6 (Body)

	public LineRenderer lineRenderer;

	private bool actif = false;
	private ScanData lastScan = null;

	private void Start() {
		lineRenderer.SetPositions(new Vector3[2] { transform.position, transform.TransformDirection(Vector3.left) });
	}

	private void FixedUpdate() {
		if (actif) {
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out RaycastHit hit, layerMaskToIgnore)) { // Vu l'orientation de l'objet, left est le devant
				//Debug.Log("Scanner hit : " + hit.transform.gameObject);
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow);
				lineRenderer.SetPositions(new Vector3[2] { transform.position, hit.point });

				ScanController scanController = hit.transform.gameObject.GetComponent<ScanController>();

				if (scanController != null && lastScan != scanController.scanData) { // Si ce qu'on pointe est scannable
					OnObjectScanned.Invoke();
                    Debug.Log("Scanner hit scan : " + hit.transform.gameObject);
					lastScan = scanController.scanData;
					GetScanData(scanController);
				}
			}
			else {
				Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1000, Color.yellow);
				Vector3 endPos = transform.position + (transform.TransformDirection(Vector3.left) * hit.distance);
				lineRenderer.SetPositions(new Vector3[2] { transform.position, endPos });
				lastScan = null;
			}
		}
	}

	// Va activer l'affichage du scanner.
	public void ActiverDisplay(SelectEnterEventArgs args) {
		if ((args.interactorObject as XRBaseInteractor).CompareTag("Player")) { // Activer l'écran que si c'est le Joueur qui le prend
			cameraLoupe.enabled = true;
			display.SetActive(true);
			lineRenderer.enabled = true;
			actif = true;
		}
	}

	// Va désactiver l'affichage du scanner.
	public void DesactiverDisplay() {
		cameraLoupe.enabled = false;
		ResetScanData();
		display.SetActive(false);
		lineRenderer.enabled = false;
		actif = false;
	}

	// Va remettre le message par défaut du scanner.
	public void ResetScanData() {
		textTitle.GetComponent<LocalizeStringEvent>().StringReference.TableEntryReference = "SCANNER_TITLE";
		textData.GetComponent<LocalizeStringEvent>().StringReference.TableEntryReference = "SCANNER_IDLE";
		lastScan = null;
	}

	// Va remplir l'affichage avec les données du scan.
	public void GetScanData(ScanController scan) {
		Debug.Log("Scan : " + scan.scanData.label.GetLocalizedString());
		textTitle.text = scan.scanData.label.GetLocalizedString();

		string description = scan.scanData.description.GetLocalizedString();

		if (scan.scanData.scanType == ScanData.ScanType.Sample) {
			SampleController sampleController = scan.gameObject.GetComponent<SampleController>();

			if (sampleController != null) {
				description += "\n<b>Identification :</b>\n" + sampleController.sampleData.patientName;
				description += "\n" + sampleController.sampleData.PatientBirthday.ToString("dd/MM/yyyy") + " (" + sampleController.sampleData.patientYears + "a) " + sampleController.sampleData.patientSex;
				description += "\n" + sampleController.sampleData.patientID;

				description += "\n<b>Collect :</b>\n" + sampleController.sampleData.DateCollect.ToString("dd/MM/yyyy HH:mm");
				description += "\nBy " + sampleController.sampleData.collectID;

				if (sampleController.sampleData.arm == SampleData.Arm.Left) {
					description += " Left arm";
				}
				else {
					description += " Right arm";
				}

				description += "\n<b>Chemical Composition :</b>\n";

				if (sampleController.sampleData.sampleType == SampleController.SampleType.CBC) { // Voir pour les autres types
					description += "EDTA";
				}
			}
		}
		else if (scan.scanData.scanType == ScanData.ScanType.Automation) {
			HematoAutomatonController controller = scan.gameObject.GetComponent<HematoAutomatonController>();

			if (controller != null) {
				string mode = controller.modeFonctionnement switch {
					HematoAutomatonController.ModeFonctionnement.Manuel => "MAN.",
					_ => "AUTO",
				};

				description += "\n<b>Mode : <color=#" + ColorUtility.ToHtmlStringRGB(controller.ActualLedColor()) + ">" + mode + "</color></b> <b><u>" + controller.typeMesure + "</u></b>\nTests :";

				foreach (HematoAutomatonController.TypeTest typeTest in controller.typeTests) {
					if (typeTest == HematoAutomatonController.TypeTest.PLTF) {
						description += " PLT-F";
					}
					else {
						description += " " + typeTest.ToString();
					}
				}

				description += "\n<b>Reagents :</b>";

				foreach (ReagentData reagent in controller.reagentDatas) {
					switch (reagent.reagent) {
						case ReagentData.Reagent.WPC:
							description += " <color=white>WPC</color> ";

							if (reagent.lvlReagent <= HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[0]) {
								description += "<color=#FF0000>" + reagent.lvlReagent + "%</color>";
							}
							else if (reagent.lvlReagent <= 25) {
								description += "<color=#FFFF00>" + reagent.lvlReagent + "%</color>";
							}
							else {
								description += "<color=#FFFFFF>" + reagent.lvlReagent + "%</color>";
							}

							break;
						case ReagentData.Reagent.PLT:
							description += " <color=#FFCD00>PLT</color> ";

							if (reagent.lvlReagent <= HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[1]) {
								description += "<color=#FF0000>" + reagent.lvlReagent + "%</color>";
							}
							else if (reagent.lvlReagent <= 25) {
								description += "<color=#FFFF00>" + reagent.lvlReagent + "%</color>";
							}
							else {
								description += "<color=#FFFFFF>" + reagent.lvlReagent + "%</color>";
							}

							break;
						case ReagentData.Reagent.RET:
							description += " <color=#EC511B>RET</color> ";

							if (reagent.lvlReagent <= HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[2]) {
								description += "<color=#FF0000>" + reagent.lvlReagent + "%</color>";
							}
							else if (reagent.lvlReagent <= 25) {
								description += "<color=#FFFF00>" + reagent.lvlReagent + "%</color>";
							}
							else {
								description += "<color=#FFFFFF>" + reagent.lvlReagent + "%</color>";
							}

							break;
						case ReagentData.Reagent.WDF:
							description += " <color=#26C7D6>WDF</color> ";

							if (reagent.lvlReagent <= HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[3]) {
								description += "<color=#FF0000>" + reagent.lvlReagent + "%</color>";
							}
							else if (reagent.lvlReagent <= 25) {
								description += "<color=#FFFF00>" + reagent.lvlReagent + "%</color>";
							}
							else {
								description += "<color=#FFFFFF>" + reagent.lvlReagent + "%</color>";
							}

							break;
						case ReagentData.Reagent.WNR:
						default:
							description += " <color=#2CBD91>WNR</color> ";

							if (reagent.lvlReagent <= HematoAutomatonController.QUANTITE_REACTIFS_ANALYSE[4]) {
								description += "<color=#FF0000>" + reagent.lvlReagent + "%</color>";
							}
							else if (reagent.lvlReagent <= 25) {
								description += "<color=#FFFF00>" + reagent.lvlReagent + "%</color>";
							}
							else {
								description += "<color=#FFFFFF>" + reagent.lvlReagent + "%</color>";
							}

							break;
					}
				}

				description += "\n" + controller.display.GetMessageInfo();
			}
		}
		else if (scan.scanData.scanType == ScanData.ScanType.Badge) {
			BadgeController badgeController = scan.gameObject.GetComponent<BadgeController>();

			description += "\n<b>" + LocalizationSettings.StringDatabase.GetLocalizedString(badgeController.stringTable, "ACCESS_BADGE") + "</b>\n" + LocalizationSettings.StringDatabase.GetLocalizedString(badgeController.stringTable, "SECURITY_LVL", arguments: new object[] { "<b>" + badgeController.securityLvl + "</b>" });
		}

		textData.text = description;
	}
}
