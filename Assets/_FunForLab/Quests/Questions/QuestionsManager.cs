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
using UnityEngine.Events;

/// <summary>
/// Classe manager qui gère les questions.
/// </summary>
public class QuestionsManager : MonoBehaviour {
	public GameObject screenQuestions;

	[HideInInspector]
	public bool questionActive; // Est-ce qu'une question est en cours ou non ?

	#region Messages Unity
	private void Awake() {
		questionActive = false;
		screenQuestions.GetComponentInChildren<QuestionsDisplay>().questionFinishEvent.AddListener(QuestionFinishEvent);
	}
	#endregion

	#region Actions Listeners
	// Action lorsqu'un event QuestionFinishEvent est reçu. Va désactiver l'affichage de la question.
	private void QuestionFinishEvent(bool goodAnswer) {
		Debug.Log("UnityEvent Question finish reçu : " + goodAnswer);
		questionActive = false;
		screenQuestions.SetActive(false);
		// TODO Voir que faire ensuite une fois qu'on a reçu la réponse
	}

	// Action lorsqu'un event QuestionEvent est reçu. Va activer l'affichage de la question.
	public void QuestionEvent(QuestionData question) {
		Debug.Log("UnityEvent Question public reçu : " + question);

		if (!questionActive) {
			questionActive = true;
			screenQuestions.SetActive(true);

			QuestionsVideoDisplay questionsVideoDisplay = screenQuestions.GetComponentInChildren<QuestionsVideoDisplay>();

			if (questionsVideoDisplay != null) {
				questionsVideoDisplay.PopulateQuestion(question);
			}
			else {
				screenQuestions.GetComponentInChildren<QuestionsDisplay>().PopulateQuestion(question);
			}

			// TODO Voir si on doit déplacer l'UI à un endroit en particulier ou non
		}
		else {
			Debug.Log("Question déjà active !");
		}
	}
	#endregion
}

/// <summary>
/// Classe event pour afficher une question.
/// </summary>
[Serializable]
public class QuestionEvent : UnityEvent<QuestionData> { }

/// <summary>
/// Classe event pour une question finie.
/// </summary>
[Serializable]
public class QuestionFinishEvent : UnityEvent<bool> { }
