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
using UnityEngine.UI;

public class HematoAutomatonDemoController : MonoBehaviour
{

    [Range(0, 10)]
    public float waitBeforeMode;
    [Range(0, 10)]
    public float waitBeforeWB;
    [Range(0, 10)]
    public float waitBeforeStart;
    [Range(0, 10)]
    public float waitBeforeExplorateur;
    [Range(0, 30)]
    public float waitBeforeRowClick;
    [Range(0, 10)]
    public float waitBeforeDetails;

    public FFL1000Display display;
    public FFL1000DisplayExplorer explorer;
    public GameObject contentHolder;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AnimateFFL1000());  
    }

    IEnumerator AnimateFFL1000()
    {
        yield return new WaitForSeconds(waitBeforeMode);
        display.AfficherPanelMode();
        yield return new WaitForSeconds(waitBeforeWB);
        display.OKPanelMode();
        yield return new WaitForSeconds(waitBeforeStart);
        display.LancerAnalyse();
        yield return new WaitForSeconds(waitBeforeExplorateur);
        display.ChangeMenuActif(4);
        yield return new WaitForSeconds(waitBeforeRowClick);
        explorer.RowClick(contentHolder.transform.GetChild(4).GetComponent<Image>());
        yield return new WaitForSeconds(waitBeforeDetails);
        explorer.ButtonDetailsClick();
    }

}
