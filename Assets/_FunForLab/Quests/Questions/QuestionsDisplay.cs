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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

/// <summary>
/// Classe qui gère l'écran des questions. !! En vrai n'est plus utilisé au profit de QuestionsVideoDisplay qui fait les 2 !
/// </summary>
public class QuestionsDisplay : MonoBehaviour {
	private static readonly string[] letters = new string[] { "A", "B", "C", "D", "E" };

	private int answerSelected = -1;
	private int goodAnswer = -1;
	private readonly List<GameObject> toggles = new List<GameObject>();
	private bool finished = false;

	private VoiceData voiceQuestion;
	private VoiceData voiceGoodAnswer;
	private VoiceData voiceBadAnswer;
	private List<VoiceData> voiceAnswersLetters;

	private VoiceData voiceEnonce;
	private VoiceData voiceComment;
	private List<VoiceData> voiceAnswers;

	[HideInInspector]
	public QuestionFinishEvent questionFinishEvent;

	public BaseQuestionVoiceData baseQuestionVoiceData;
	public QuestionData questionData;
	public AudioSource audioSource;

	#region Variables UI
	public TMP_Text TextEnonce;
	public TMP_Text TextCommentaire;
	public TMP_Text TextMessage;
	public GameObject toggleReponseModele;
	public RectTransform panelReponses;
	public Button buttonValidate;
	#endregion

	// Est-ce que la réponse actuellement choisie est la bonne réponse ?
	public bool IsAnswerGood {
		get {
			if (answerSelected != -1 && goodAnswer != -1) {
				return answerSelected == goodAnswer;
			}

			return false;
		}
	}

	#region Messages Unity
	protected void Awake() {
		Debug.Log("Question voice lang : " + PlayerPrefsManager.Language);

		voiceQuestion = null;
		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataQuestion) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceQuestion = voiceDataLang.voiceData;
			}
		}

		voiceGoodAnswer = null;
		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataGoodAnswer) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceGoodAnswer = voiceDataLang.voiceData;
			}
		}

		voiceBadAnswer = null;
		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataBadAnswer) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceBadAnswer = voiceDataLang.voiceData;
			}
		}

		voiceAnswersLetters = new List<VoiceData>();
		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataA) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceAnswersLetters.Add(voiceDataLang.voiceData);
			}
		}

		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataB) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceAnswersLetters.Add(voiceDataLang.voiceData);
			}
		}

		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataC) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceAnswersLetters.Add(voiceDataLang.voiceData);
			}
		}

		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataD) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceAnswersLetters.Add(voiceDataLang.voiceData);
			}
		}

		foreach (VoiceDataLang voiceDataLang in baseQuestionVoiceData.voicesDataE) {
			if (voiceDataLang.lang == PlayerPrefsManager.Language) {
				voiceAnswersLetters.Add(voiceDataLang.voiceData);
			}
		}
	}
	#endregion

	#region Méthodes privées
	// Va supprimer tous les toggles de réponses.
	private void RemoveToggles() {
		foreach (RectTransform child in panelReponses) {
			//Debug.Log("child : " + child.gameObject.name + " modele : " + toggleReponseModele.name);
			if (!ReferenceEquals(child.gameObject, toggleReponseModele)) {
				Destroy(child.gameObject);
			}
		}
	}
	#endregion

	#region Méthodes publiques
	// Va remplir l'affichage avec toutes les données de la question.
	public void PopulateQuestion(QuestionData question, bool say = true) {
		if (question != null) {
			questionData = Instantiate(question);
			toggles.Clear();
			RemoveToggles();
			answerSelected = -1;
			finished = false;

			TextEnonce.text = questionData.enonce.GetLocalizedString(); // !! Le texte ne se mettra pas à jour automatiquement (mais c'est pas grave car on ne change pas la langue du jeu à la volée)
			TextCommentaire.text = questionData.comment.GetLocalizedString();
			TextCommentaire.gameObject.SetActive(false);
			buttonValidate.interactable = true;
			buttonValidate.GetComponentInChildren<LocalizeStringEvent>().StringReference.TableEntryReference = "RESPOND";
			TextMessage.gameObject.SetActive(false);

			for (int i = 0; i < questionData.answers.Count; i++) {
				GameObject newToggle = Instantiate(toggleReponseModele, panelReponses);

				newToggle.name = "ToggleReponse_" + i;
				newToggle.GetComponentInChildren<TMP_Text>().text = letters[i] + ") " + questionData.answers[i].answer.GetLocalizedString();
				newToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => ToggleAnswerChanged(newToggle.name, value));
				newToggle.SetActive(true);

				toggles.Add(newToggle);

				if (questionData.answers[i].solution) {
					goodAnswer = i;
					Debug.Log("Answer " + i + " : " + questionData.answers[i].solution + " | goodAnswer : " + goodAnswer);
				}
			}

			voiceEnonce = null;
			foreach (VoiceDataLang voiceDataLang in questionData.voicesDataEnonce) {
				if (voiceDataLang.lang == PlayerPrefsManager.Language) {
					voiceEnonce = voiceDataLang.voiceData;
				}
			}

			voiceComment = null;
			foreach (VoiceDataLang voiceDataLang in questionData.voicesDataComment) {
				if (voiceDataLang.lang == PlayerPrefsManager.Language) {
					voiceComment = voiceDataLang.voiceData;
				}
			}

			voiceAnswers = new List<VoiceData>();
			for (int i = 0; i < questionData.answers.Count; i++) {
				foreach (VoiceDataLang voiceDataLang in questionData.answers[i].voicesDataAnswer) {
					if (voiceDataLang.lang == PlayerPrefsManager.Language) {
						voiceAnswers.Add(voiceDataLang.voiceData);
					}
				}
			}

			if (say) {
				SayStatementAndAnswersVoice();
			}
		}
	}

	// Va activer la coroutine de la voix "Question".
	public void SayQuestionVoice() {
		StartCoroutine(PlayQuestionVoice());
	}

	// Va activer la coroutine de la voix "Question" + "Enonce" + "Réponses".
	public void SayStatementAndAnswersVoice(bool sayQuestion = true) {
		StartCoroutine(PlayStatementVoice(sayQuestion));
	}

	// Va activer la coroutine de la voix "Bonne Réponse".
	public void SayGoodAnswerVoice() {
		StopAllCoroutines();
		StartCoroutine(PlayGoodAnswerVoice());
	}

	// Va activer la coroutine de la voix "Mauvaise Réponse".
	public void SayBadAnswerVoice() {
		StopAllCoroutines();
		StartCoroutine(PlayBadAnswerVoice());
	}
	#endregion

	#region Coroutines
	// Coroutine qui va jouer la voix "Question".
	private IEnumerator PlayQuestionVoice() {
		if (voiceQuestion != null && voiceEnonce != null) {
			audioSource.clip = voiceQuestion.sourceFile;
			audioSource.Play();

			yield return new WaitForSeconds(voiceQuestion.sourceFile.length);
		}
	}

	// Coroutine qui va jouer les voix "Question" + "Enonce", puis va lancer la coroutine PlayAnswersVoice().
	private IEnumerator PlayStatementVoice(bool sayQuestion = true) {
		if (voiceQuestion != null && voiceEnonce != null) {
			if (sayQuestion) {
				audioSource.clip = voiceQuestion.sourceFile;
				audioSource.Play();

				yield return new WaitForSeconds(voiceQuestion.sourceFile.length + 0.25f); // Rajouter un délai
			}

			audioSource.clip = voiceEnonce.sourceFile;
			audioSource.Play();

			yield return new WaitForSeconds(voiceEnonce.sourceFile.length + 0.5f);
		}

		yield return StartCoroutine(PlayAnswersVoice());
	}

	// Coroutine qui va jouer la voix "Bonne Réponse", puis va lancer la coroutine PlayCommentVoice().
	private IEnumerator PlayGoodAnswerVoice() {
		if (voiceGoodAnswer != null) {
			audioSource.clip = voiceGoodAnswer.sourceFile;
			audioSource.Play();

			yield return new WaitForSeconds(voiceGoodAnswer.sourceFile.length + 0.5f);
		}

		yield return StartCoroutine(PlayCommentVoice());
	}

	// Coroutine qui va jouer la voix "Mauvaise Réponse", puis va lancer la coroutine PlayCommentVoice().
	private IEnumerator PlayBadAnswerVoice() {
		if (voiceBadAnswer != null) {
			audioSource.clip = voiceBadAnswer.sourceFile;
			audioSource.Play();

			yield return new WaitForSeconds(voiceBadAnswer.sourceFile.length + 0.5f);
		}

		yield return StartCoroutine(PlayCommentVoice());
	}

	// Coroutine qui va jouer la voix "Commentaire".
	private IEnumerator PlayCommentVoice() {
		if (voiceComment != null) {
			audioSource.clip = voiceComment.sourceFile;
			audioSource.Play();

			yield return new WaitForSeconds(voiceComment.sourceFile.length);
		}
	}

	// Coroutine qui va jouer les voix des réponses, avec la lettre qui va avec.
	private IEnumerator PlayAnswersVoice() {
		if (voiceAnswersLetters != null && voiceAnswers != null) {
			for (int i = 0; i < voiceAnswers.Count; i++) {
				audioSource.clip = voiceAnswersLetters[i].sourceFile;
				audioSource.Play();

				yield return new WaitForSeconds(voiceQuestion.sourceFile.length + 0.1f);

				audioSource.clip = voiceAnswers[i].sourceFile;
				audioSource.Play();

				yield return new WaitForSeconds(voiceAnswers[i].sourceFile.length + 0.25f);
			}
		}
	}
	#endregion

	#region Action listeners
	// Action lorsqu'un toggle se fait hover. Va changer sa couleur.
	public void ToggleHoverEnter(Toggle sender) {
		if (sender.interactable) { // Ne le prendre en compte que s'il est activé
			sender.gameObject.GetComponent<Image>().color = new Color32(255, 191, 127, 63);
		}
	}

	// Action lorsqu'un toggle ne se fait plus hover. Va reset sa couleur.
	public void ToggleHoverExit(Toggle sender) {
		if (sender.interactable) { // Ne le prendre en compte que s'il est activé
			sender.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 63);
		}
	}

	// Action lorsque l'on appuye sur un toggle de réponse. Va changer la réponse actuellement sélectionnée.
	public void ToggleAnswerChanged(string answer, bool value) {
		int numAnswer = int.Parse(answer.Split('_')[1]);
		answerSelected = value ? numAnswer : -1;
	}

	// Action lorsque l'on clique sur le bouton pour répondre. Va soit confirmer la réponse ou fermer la fenêtre.
	public void ValidateAnswer() {
		if (finished) {
			questionFinishEvent.Invoke(IsAnswerGood);
			return;
		}

		Debug.Log("Selected answer : " + answerSelected + " | good answer : " + goodAnswer + " | toggles : " + toggles.Count);

		if (answerSelected != -1) {
			foreach (GameObject toggle in toggles) {
				toggle.GetComponent<Toggle>().interactable = false;

				int numAnswer = int.Parse(toggle.name.Split('_')[1]);

				if (numAnswer == goodAnswer) {
					toggle.GetComponent<Image>().color = new Color32(63, 255, 63, 63);
				}
				else {
					toggle.GetComponent<Image>().color = new Color32(255, 63, 63, 63);
				}
			}

			TextCommentaire.gameObject.SetActive(true);
			buttonValidate.GetComponentInChildren<LocalizeStringEvent>().StringReference.TableEntryReference = "CLOSE";
			TextMessage.gameObject.SetActive(true);
			finished = true;

			if (IsAnswerGood) {
				Debug.Log("Bonne réponse !");
				TextMessage.GetComponent<LocalizeStringEvent>().StringReference.TableEntryReference = "GOOD_ANSWER";
				TextMessage.color = new Color32(63, 255, 63, 255);

				SayGoodAnswerVoice();
			}
			else {
				TextMessage.GetComponent<LocalizeStringEvent>().StringReference.TableEntryReference = "BAD_ANSWER";
				TextMessage.color = new Color32(255, 63, 63, 255);

				SayBadAnswerVoice();
			}
		}
	}
	#endregion
}
