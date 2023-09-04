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
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Surcharge de la classe qui gère l'écran des questions, en affichant une vidéo en plus.
/// </summary>
public class QuestionsVideoDisplay : QuestionsDisplay { // TODO En vrai, ne pas remplacer directement QuestionsDisplay, car il ne sert plus à rien seul ?
	private const string IMAGE_PLAY = "Icons/Free Flat Media Right Icon";
	private const string IMAGE_PAUSE = "Icons/Free Flat Media Pause Icon";
	private const string TIMER_IDLE = "--:--/--:--";

	private Sprite spritePlay;
	private Sprite spritePause;

	private Image panelSubtitles;

	private Button buttonPlay;
	private Button buttonStop;
	private Image iconButtonPlay;
	private Image progress;
	private TMP_Text textTime;
	private TMP_Text textSubtitles;

	private Button boutonTabEnonce;
	private Button boutonTabVideo;

	private RectTransform panelListEnonce;
	private RectTransform panelTabVideo;

	private string totalTime;

	private List<SubtitleLine> subtitles;
	private SubtitleLine lastSubtitle;

	private bool noVideo;
	private bool alreadySay;

	public VideoData videoData; // Objet contenant les données de la vidéo à afficher
	public RawImage videoSupport; // Image sur laquelle on va projeter la vidéo
	public VideoPlayer videoPlayer; // Objet VideoPlayer qui va lire la vidéo
	public bool autoStart; // Est-ce que la vidéo démarre automatiquement une fois qu'elle est chargée ?

	#region Messages Unity
	private new void Awake() {
		base.Awake();

		spritePlay = Resources.Load<Sprite>(IMAGE_PLAY);
		spritePause = Resources.Load<Sprite>(IMAGE_PAUSE);

		panelSubtitles = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelVideo/PanelSubtitles").gameObject.GetComponent<Image>();

		buttonPlay = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelControls/ButtonPlay").gameObject.GetComponent<Button>();
		buttonStop = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelControls/ButtonStop").gameObject.GetComponent<Button>();
		iconButtonPlay = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelControls/ButtonPlay/IconButtonPlay").gameObject.GetComponent<Image>();
		progress = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelControls/Progressbar/Progress").gameObject.GetComponent<Image>();
		textTime = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelControls/TextTime").gameObject.GetComponent<TMP_Text>();
		textSubtitles = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo/PanelVideo/PanelSubtitles/TextSubtitle").gameObject.GetComponent<TMP_Text>();

		boutonTabEnonce = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabs/ButtonTabEnonce").gameObject.GetComponent<Button>();
		boutonTabVideo = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabs/ButtonTabVideo").gameObject.GetComponent<Button>();

		panelListEnonce = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelListEnonce").gameObject.GetComponent<RectTransform>();
		panelTabVideo = transform.Find("PanelBackground/PanelContent/PanelEnonce/PanelTabVideo").gameObject.GetComponent<RectTransform>();
	}

	private void Start() {
		buttonPlay.interactable = false;
		buttonStop.interactable = false;
		textTime.text = TIMER_IDLE;
	}

	private void Update() {
		if (videoPlayer.isPrepared) {
			// Mettre à jour la barre de progression de la vidéo
			progress.fillAmount = (float) videoPlayer.frame / videoPlayer.frameCount;

			// Mettre à jour le timer de la vidéo
			string currentTime = Mathf.Floor((int) videoPlayer.time / 60).ToString("00") + ":" + ((int) videoPlayer.time % 60).ToString("00");

			textTime.text = currentTime + "/" + totalTime;

			// Afficher le sous-titre correspondant au timer de la vidéo
			if (subtitles != null) {
				if (lastSubtitle != null && lastSubtitle.From <= videoPlayer.time && lastSubtitle.To >= videoPlayer.time) {
					textSubtitles.text = lastSubtitle.Text;
				}
				else {
					SubtitleLine subtitleLine = subtitles.FirstOrDefault(x => x.From <= videoPlayer.time && x.To >= videoPlayer.time);

					if (subtitleLine == null) { // Pas de sous-titre à l'instant présent
						panelSubtitles.gameObject.SetActive(false);
						textSubtitles.text = string.Empty;
					}
					else if (lastSubtitle != subtitleLine) {
						panelSubtitles.gameObject.SetActive(true);
						textSubtitles.text = subtitleLine.Text;
					}

					lastSubtitle = subtitleLine;
				}
			}
		}
	}

	private void OnEnable() {
		videoPlayer.prepareCompleted += PrepareCompleted;
		videoPlayer.loopPointReached += EndReached;
	}

	private void OnDisable() {
		videoPlayer.prepareCompleted -= PrepareCompleted;
		videoPlayer.loopPointReached -= EndReached;
	}
	#endregion

	#region Méthodes publiques
	// Va remplir les champs de la question en y mettant aussi la vidéo.
	public new void PopulateQuestion(QuestionData question, bool say = true) {
		base.PopulateQuestion(question, false);

		if (question != null && question.video != null) {
			Debug.Log("Question with video");
			noVideo = false;
			alreadySay = false;
			SayQuestionVoice();

			videoData = Instantiate(question.video);
			videoPlayer.clip = videoData.videoClip;

			// Est-ce que des sous-titres sont présents ?
			if (videoData.subtitles != null) {
				subtitles = SrtSubtitlesParser.ParseSubtitles(videoData.subtitles.text);
			}
			else {
				subtitles = null;
				panelSubtitles.gameObject.SetActive(false);
				textSubtitles.text = string.Empty;
			}

			videoPlayer.Prepare();
			ChangeBrowserTab(1);
		}
		else {
			Debug.Log("Question without video");
			noVideo = true;
			boutonTabVideo.interactable = false;
			panelTabVideo.gameObject.SetActive(false);
			SayStatementAndAnswersVoice();
		}
	}
	#endregion

	#region Action listeners
	// Action lorsque qu'un event PrepareCompleted est reçu. Va mettre la première frame de la vidéo et activer le bouton Play.
	private void PrepareCompleted(VideoPlayer source) {
		// Calculer la durée totale de la vidéo
		totalTime = Mathf.Floor((int) videoPlayer.length / 60).ToString("00") + ":" + ((int) videoPlayer.length % 60).ToString("00");

		videoPlayer.Play(); // Lancer pour la première fois la vidéo et avoir la première frame d'affichée

		if (!autoStart) {
			videoPlayer.Pause(); // Mettre directement en pause la vidéo, pour ne garder que la première frame d'affichée
		}

		buttonPlay.interactable = true;
		buttonStop.interactable = true;
	}

	// Action lorsque qu'un event LoopPointReached est reçu. Va remettre l'icone Play sur le bouton Play une fois que la vidéo est terminée.
	private void EndReached(VideoPlayer source) {
		iconButtonPlay.sprite = spritePlay;

		if (!alreadySay) {
			SayStatementAndAnswersVoice(false);
			ChangeBrowserTab(0);
			alreadySay = true;
		}
	}

	// Action lorsque l'on clique sur un bouton permettant de changer d'onglet.
	public void ChangeBrowserTab(int tab) {
		TMP_Text textButtonTabEnonce = boutonTabEnonce.transform.Find("TextButton").gameObject.GetComponent<TMP_Text>();
		TMP_Text textButtonTabVideo = boutonTabVideo.transform.Find("TextButton").gameObject.GetComponent<TMP_Text>();

		boutonTabEnonce.interactable = true;
		textButtonTabEnonce.fontStyle = FontStyles.Normal;
		panelListEnonce.gameObject.SetActive(false);

		if (!noVideo) {
			boutonTabVideo.interactable = true;
			textButtonTabVideo.fontStyle = FontStyles.Normal;
			panelTabVideo.gameObject.SetActive(false);
		}

		switch (tab) {
			case 1:
				textButtonTabVideo.fontStyle = FontStyles.Bold;
				boutonTabVideo.interactable = false;
				panelTabVideo.gameObject.SetActive(true);
				break;
			case 0:
			default:
				textButtonTabEnonce.fontStyle = FontStyles.Bold;
				boutonTabEnonce.interactable = false;
				panelListEnonce.gameObject.SetActive(true);
				break;
		}
	}

	// Action lorsque l'on clique sur le bouton Play. Va lancer ou mettre en pause la vidéo.
	public void ButtonPlay() {
		if (videoPlayer.isPrepared) {
			if (!videoPlayer.isPaused) {
				videoPlayer.Pause();
				iconButtonPlay.sprite = spritePlay;
			}
			else {
				videoPlayer.Play();
				iconButtonPlay.sprite = spritePause;
			}
		}
	}

	// Action lorsque l'on clique sur le bouton Stop. Va arrêter la vidéo et la recharger.
	public void ButtonStop() {
		videoPlayer.Stop();

		buttonPlay.interactable = false;
		buttonStop.interactable = false;
		iconButtonPlay.sprite = spritePlay;
		textTime.text = TIMER_IDLE;

		// En faisant Stop, la vidéo n'est plus préparée donc on la recharge.
		videoPlayer.Prepare(); // Préparer (charger) la vidéo
	}

	// Action lorsque l'on drag ou clique sur la barre de progression. Va changer la position dans la vidéo.
	public void ProgressInteraction(BaseEventData eventData) {
		PointerEventData pointerData = eventData as PointerEventData;

		if (videoPlayer.isPrepared && RectTransformUtility.ScreenPointToLocalPointInRectangle(progress.rectTransform, pointerData.position, Camera.main, out Vector2 localPoint)) { // Convertir l'endroit cliqué en pourcentage de progression
			float pourcent = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);

			// Aller à ce pourcentage dans vidéo
			float frame = videoPlayer.frameCount * pourcent;
			videoPlayer.frame = (long) frame;
		}
	}

	// Action lorsque l'on clique sur le bouton Répondre.
	public new void ValidateAnswer() {
		base.ValidateAnswer();

		ChangeBrowserTab(0);
		alreadySay = true;
	}
	#endregion
}
