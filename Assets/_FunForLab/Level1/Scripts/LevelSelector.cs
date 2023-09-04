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
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public enum Level
{
    Level1_Sylvia,
    Level1_Olaf,
    Level2_Aureliano,
    Level3_MelZ
}
public class LevelSelector : MonoBehaviour
{
    public bool ForceLevelInEditor = true;
    public Level SelectedLevel;

    public LevelContent Level1_Sylvia_Content;
    public LevelContent Level1_Olaf_Content;
    public LevelContent Level2_Aureliano_Content;
    public LevelContent Level3_MelZ_Content;

    public ParentConstraint TextParentConstraint;

    private void Awake()
    {
#if UNITY_EDITOR
        if (ForceLevelInEditor) return;
#endif
        switch (PlayerPrefs.GetString("Level", "Sylvia"))
        {
            case "Sylvia":
                SelectedLevel = Level.Level1_Sylvia;
                break;

            case "Olaf":
                SelectedLevel = Level.Level1_Olaf;
                break;

            case "Aureliano":
                SelectedLevel = Level.Level2_Aureliano;
                break;

            case "MelZ":
                SelectedLevel = Level.Level3_MelZ;
                break;

            default:
                SelectedLevel = Level.Level1_Sylvia;
                break;
        }
    }

    private void Start()
    {
        LoadContent();
    }

    private void LoadContent()
    {
        switch (SelectedLevel)
        {
            case Level.Level1_Sylvia:
                TextParentConstraint.RemoveSource(3);
                TextParentConstraint.RemoveSource(2);
                TextParentConstraint.RemoveSource(1);
                break;
            case Level.Level1_Olaf:
                TextParentConstraint.RemoveSource(3);
                TextParentConstraint.RemoveSource(2);
                TextParentConstraint.RemoveSource(0);
                break;
            case Level.Level2_Aureliano:
                TextParentConstraint.RemoveSource(3);
                TextParentConstraint.RemoveSource(1);
                TextParentConstraint.RemoveSource(0);
                break;
            case Level.Level3_MelZ:
                TextParentConstraint.RemoveSource(2);
                TextParentConstraint.RemoveSource(1);
                TextParentConstraint.RemoveSource(0);
                break;
        }

        Level1_Sylvia_Content.SetActive(SelectedLevel == Level.Level1_Sylvia);
        Level1_Olaf_Content.SetActive(SelectedLevel == Level.Level1_Olaf);
        Level2_Aureliano_Content.SetActive(SelectedLevel == Level.Level2_Aureliano);
        Level3_MelZ_Content.SetActive(SelectedLevel == Level.Level3_MelZ);
    }

    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 0:
                SceneManager.LoadScene("MainMenu");
                return;
            case 1:
                PlayerPrefs.SetString("Level", "Sylvia");
                SceneManager.LoadScene("Level1-Game");
                return;

            case 2:
                PlayerPrefs.SetString("Level", "Olaf");
                SceneManager.LoadScene("Level1-Game");
                return;

            case 3:
                PlayerPrefs.SetString("Level", "Aureliano");
                SceneManager.LoadScene("Level1-Game");
                return;

            case 4:
                PlayerPrefs.SetString("Level", "MelZ");
                SceneManager.LoadScene("Level1-Game");
                return;
            case 5:
                SceneManager.LoadScene("Outro");
                return;
            default:
                SceneManager.LoadScene("MainMenu");
                return;
        }
        
    }
}
