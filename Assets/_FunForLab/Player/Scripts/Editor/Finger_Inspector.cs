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
using Codice.Client.Common.GameUI;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(FingerAnimator))]
public class Finger_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Find bones"))
        {
            FindBones();
        }

        if (GUILayout.Button("Save open rotations"))
        {
            SaveOpenRotations();
        }

        if (GUILayout.Button("Save closed rotations"))
        {
            SaveClosedRotations();
        }
        if (GUILayout.Button("Open"))
        {
            Open();
        }
        if (GUILayout.Button("Close"))
        {
            Close();
        }
    }

        private void FindBones()
    {
        FingerAnimator fingerAnimator = (FingerAnimator)target;
        fingerAnimator.SetBones(
            fingerAnimator.transform,
            fingerAnimator.transform.GetChild(0).transform,
            fingerAnimator.transform.GetChild(0).GetChild(0).transform
            );
    }

    private void SaveOpenRotations()
    {
        FingerAnimator fingerAnimator = (FingerAnimator)target;
        fingerAnimator.SetOpenRotations(
            fingerAnimator.Bone1.localEulerAngles,
            fingerAnimator.Bone2.localEulerAngles,
            fingerAnimator.Bone3.localEulerAngles
            );
    }
    private void SaveClosedRotations()
    {
        FingerAnimator fingerAnimator = (FingerAnimator)target;
        fingerAnimator.SetClosedRotations(
            fingerAnimator.Bone1.localEulerAngles,
            fingerAnimator.Bone2.localEulerAngles,
            fingerAnimator.Bone3.localEulerAngles
            );
    }

    private void Open()
    {
        FingerAnimator fingerAnimator = (FingerAnimator)target;
        if (EditorApplication.isPlaying)
        {
            fingerAnimator.Open();
            return;
        }
        fingerAnimator.Bone1.transform.localEulerAngles = fingerAnimator.Bone1_RotationOpen;
        fingerAnimator.Bone2.transform.localEulerAngles = fingerAnimator.Bone2_RotationOpen;
        fingerAnimator.Bone3.transform.localEulerAngles = fingerAnimator.Bone3_RotationOpen;

    }

    private void Close()
    {
        FingerAnimator fingerAnimator = (FingerAnimator)target;
        if (EditorApplication.isPlaying)
        {
            fingerAnimator.Close();
            return;
        }

        fingerAnimator.Bone1.transform.localEulerAngles = fingerAnimator.Bone1_RotationClosed;
        fingerAnimator.Bone2.transform.localEulerAngles = fingerAnimator.Bone2_RotationClosed;
        fingerAnimator.Bone3.transform.localEulerAngles = fingerAnimator.Bone3_RotationClosed;

    }
}