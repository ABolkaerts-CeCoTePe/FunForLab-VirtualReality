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
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Surcharge de la classe qui g�re l'�cran des questions, en affichant une vid�o en plus.
/// </summary>
public class QuestionsVideoDisplay : QuestionsDisplay { // TODO En vrai, ne pas remplacer directement QuestionsDisplay, car il ne sert plus � rien seul ?
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

	public VideoData videoData; // Objet contenant les donn�es de la vid�o � afficher
	public RawImage videoSupport; // Image sur laquelle on va projeter la vid�o
	public VideoPlayer videoPlayer; // Objet VideoPlayer qui va lire la vid�o
	public bool autoStart; // Est-ce que la vid�o d�marre automatiquement une fois qu'elle est charg�e ?

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
			// Mettre � jour la barre de progression de la vid�o
			progress.fillAmount = (float) videoPlayer.frame / videoPlayer.frameCount;

			// Mettre � jour le timer de la vid�o
			string currentTime = Mathf.Floor((int) videoPlayer.time / 60).ToString("00") + ":" + ((int) videoPlayer.time % 60).ToString("00");

			textTime.text = currentTime + "/" + totalTime;

			// Afficher le sous-titre correspondant au timer de la vid�o
			if (subtitles != null) {
				if (lastSubtitle != null && lastSubtitle.From <= videoPlayer.time && lastSubtitle.To >= videoPlayer.time) {
					textSubtitles.text = lastSubtitle.Text;
				}
				else {
					SubtitleLine subtitleLine = subtitles.FirstOrDefault(x => x.From <= videoPlayer.time && x.To >= videoPlayer.time);

					if (subtitleLine == null) { // Pas de sous-titre � l'instant pr�sent
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

	#region M�thodes publiques
	// Va remplir les champs de la question en y mettant aussi la vid�o.
	public new void PopulateQuestion(QuestionData question, bool say = true) {
		base.PopulateQuestion(question, false);

		if (question != null && question.video != null) {
			Debug.Log("Question with video");
			noVideo = false;
			alreadySay = false;
			SayQuestionVoice();

			videoData = Instantiate(question.video);
			videoPlayer.clip = videoData.videoClip;

			// Est-ce que des sous-titres sont pr�sents ?
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
	// Action lorsque qu'un event PrepareCompleted est re�u. Va mettre la premi�re frame de la vid�o et activer le bouton Play.
	private void PrepareCompleted(VideoPlayer source) {
		// Calculer la dur�e totale de la vid�o
		totalTime = Mathf.Floor((int) videoPlayer.length / 60).ToString("00") + ":" + ((int) videoPlayer.length % 60).ToString("00");

		videoPlayer.Play(); // Lancer pour la premi�re fois la vid�o et avoir la premi�re frame d'affich�e

		if (!autoStart) {
			videoPlayer.Pause(); // Mettre directement en pause la vid�o, pour ne garder que la premi�re frame d'affich�e
		}

		buttonPlay.interactable = true;
		buttonStop.interactable = true;
	}

	// Action lorsque qu'un event LoopPointReached est re�u. Va remettre l'icone Play sur le bouton Play une fois que la vid�o est termin�e.
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

	// Action lorsque l'on clique sur le bouton Play. Va lancer ou mettre en pause la vid�o.
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

	// Action lorsque l'on clique sur le bouton Stop. Va arr�ter la vid�o et la recharger.
	public void ButtonStop() {
		videoPlayer.Stop();

		buttonPlay.interactable = false;
		buttonStop.interactable = false;
		iconButtonPlay.sprite = spritePlay;
		textTime.text = TIMER_IDLE;

		// En faisant Stop, la vid�o n'est plus pr�par�e donc on la recharge.
		videoPlayer.Prepare(); // Pr�parer (charger) la vid�o
	}

	// Action lorsque l'on drag ou clique sur la barre de progression. Va changer la position dans la vid�o.
	public void ProgressInteraction(BaseEventData eventData) {
		PointerEventData pointerData = eventData as PointerEventData;

		if (videoPlayer.isPrepared && RectTransformUtility.ScreenPointToLocalPointInRectangle(progress.rectTransform, pointerData.position, Camera.main, out Vector2 localPoint)) { // Convertir l'endroit cliqu� en pourcentage de progression
			float pourcent = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);

			// Aller � ce pourcentage dans vid�o
			float frame = videoPlayer.frameCount * pourcent;
			videoPlayer.frame = (long) frame;
		}
	}

	// Action lorsque l'on clique sur le bouton R�pondre.
	public new void ValidateAnswer() {
		base.ValidateAnswer();

		ChangeBrowserTab(0);
		alreadySay = true;
	}
	#endregion
}
