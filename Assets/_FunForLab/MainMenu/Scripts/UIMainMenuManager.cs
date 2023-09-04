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
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    private float _initMainMenuScale, currentMainMenuScale = 600f;
    private float _destMainMenuScale = 1920f;

    [SerializeField] private string introSceneName;
    [SerializeField] private string tutorialSceneName;
    [SerializeField] private string chapter1SceneName;
    [SerializeField] private string chapter2SceneName;
    [SerializeField] private string chapter3SceneName;
    [SerializeField] private string conclusionSceneName;


    #region UI Components

    [Header("Pop-out")]
    [SerializeField] public float mainMenuScaleRightSpeed = 1f;
    [SerializeField] public float mainMenuScaleUpSpeed = 1f;
    [SerializeField] private GameObject popOutPanel;
    [SerializeField] private RectTransform popOutPanelRect;
    [SerializeField] private RectTransform canvasTransform;

    [Header("Left Panel")]
    [SerializeField] private Button newStoryButton;
    [SerializeField] private Button chaptersButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Story Mode Panel")]
    [SerializeField] private GameObject storyModePanel;
    [SerializeField] private Button startStoryButton;

    [Header("Chapters Panel")]
    [SerializeField] private GameObject chaptersPanel;
    [SerializeField] private Button startPrologueButton;
    [SerializeField] private Button startTutorialButton;
    [SerializeField] private Button startChapter1Button;
    [SerializeField] private Button startChapter2Button;
    [SerializeField] private Button startChapter3Button;
    [SerializeField] private Button startConclusionButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    #endregion

    #region Listeners
    private void Awake()
    {
        #region Pop-out
        newStoryButton.onClick.AddListener(StartPrologue);
        chaptersButton.onClick.AddListener(MenuPopout);
        settingsButton.onClick.AddListener(MenuPopout);
        #endregion

        #region Left Panel
        newStoryButton.onClick.AddListener(StartPrologue);
        chaptersButton.onClick.AddListener(ActivateChaptersPanel);
        settingsButton.onClick.AddListener(ActivateSettingsPanel);

        exitButton.onClick.AddListener(QuitGame);
        #endregion

        #region Story Mode Panel
        startStoryButton.onClick.AddListener(StartPrologue);
        #endregion

        #region Chapters Panel
        
        startPrologueButton.onClick.AddListener(StartPrologue);
        startTutorialButton.onClick.AddListener(StartTutorial);
        startChapter1Button.onClick.AddListener(StartChapter1);
        startChapter2Button.onClick.AddListener(StartChapter2);
        startChapter3Button.onClick.AddListener(StartChapter3);
        startConclusionButton.onClick.AddListener(StartConclusion);
        
        #endregion

        #region Settings Panel

        #endregion

    }
    private void OnDestroy()
    {
        #region Pop-out
        newStoryButton.onClick.RemoveAllListeners();
        chaptersButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        #endregion

        #region Left Panel
        newStoryButton.onClick.RemoveAllListeners();
        chaptersButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();

        exitButton.onClick.RemoveAllListeners();
        #endregion

        #region Story Mode Panel
        startStoryButton.onClick.RemoveAllListeners();
        #endregion

        #region Chapters Panel
        
        startPrologueButton.onClick.RemoveAllListeners();
        startTutorialButton.onClick.RemoveAllListeners();
        startChapter1Button.onClick.RemoveAllListeners();
        startChapter2Button.onClick.RemoveAllListeners();
        startChapter3Button.onClick.RemoveAllListeners();
        startConclusionButton.onClick.RemoveAllListeners();
        
        #endregion

        #region Settings Panel

        #endregion
    }
    #endregion

    #region Animations
    private void MenuPopout()
    {
        StartCoroutine(PopOut());
        newStoryButton.onClick.RemoveListener(MenuPopout);
        chaptersButton.onClick.RemoveListener(MenuPopout);
        settingsButton.onClick.RemoveListener(MenuPopout);
    }
    IEnumerator PopOut()
    {
        ActivateMainPanel(false);
        while (currentMainMenuScale < _destMainMenuScale)
        {
            currentMainMenuScale += mainMenuScaleRightSpeed;
            canvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentMainMenuScale);
            canvasTransform.ForceUpdateRectTransforms();
            yield return new WaitForSeconds(0.02f);
        }
        ActivateMainPanel();
        yield return null;
    }

    private void ActivateMainPanel(bool state = true)
    {
        popOutPanel.SetActive(state);
        //StartCoroutine(AnimateScalePopOutPanel());
    }

    IEnumerator AnimateScalePopOutPanel()
    {
        Vector3 scale = Vector3.zero;
        Vector3 scaleIncrementor = new Vector3(mainMenuScaleUpSpeed, mainMenuScaleUpSpeed, mainMenuScaleUpSpeed);
        popOutPanelRect.localScale = scale;

        while (popOutPanelRect.localScale.x < 1)
        {
            scale += scaleIncrementor;
            popOutPanelRect.localScale = scale;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }
    #endregion

    #region Activate panels
    private void ActivateStoryModePanel()
    {
        storyModePanel.SetActive(true);
        chaptersPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
    private void ActivateChaptersPanel()
    {
        storyModePanel.SetActive(false);
        chaptersPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    private void ActivateSettingsPanel()
    {
        storyModePanel.SetActive(false);
        chaptersPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    #endregion

    #region Chapters Manager
    private void StartPrologue()
    {
        StartChapter(introSceneName);
    }

    private void StartTutorial()
    {
        //SceneManager.LoadScene(1);
        StartChapter(tutorialSceneName);
    }
    private void StartChapter1()
    {
        //SceneManager.LoadScene(1);
        PlayerPrefs.SetString("Level", "Sylvia");
        StartChapter(chapter1SceneName);
    }
    private void StartChapter2()
    {
        //SceneManager.LoadScene(1);
        PlayerPrefs.SetString("Level", "Aureliano");
        StartChapter(chapter2SceneName);
    }
    private void StartChapter3()
    {
        //SceneManager.LoadScene(1);
        PlayerPrefs.SetString("Level", "MelZ");
        StartChapter(chapter3SceneName);
    }
    private void StartConclusion()
    {
        //SceneManager.LoadScene(1);
        StartChapter(conclusionSceneName);
    }

    private void StartChapter(string chapterName)
    {
        StartCoroutine(LoadSceneAsync(chapterName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            yield return null;
        }
    }
    #endregion

    #region Quit
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion
}
