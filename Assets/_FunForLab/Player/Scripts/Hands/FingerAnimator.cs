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
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

[System.Serializable]
public class FingerAnimator : MonoBehaviour
{
    [field:Header("Bones")]
    [field:SerializeField]public Transform Bone1 { get; private set; }
    [field: SerializeField]public Transform Bone2 { get; private set; }
    [field: SerializeField]public Transform Bone3 { get; private set; }

    [Header("Animation Speed")]
    [Tooltip("Time in second for the animations to complete")]
    [SerializeField] private float _animationSpeed = 3f;

    [field:Header("Open Rotation")]
    [field: SerializeField] public Vector3 Bone1_RotationOpen { get; private set; }
    [field: SerializeField] public Vector3 Bone2_RotationOpen { get; private set; }
    [field: SerializeField] public Vector3 Bone3_RotationOpen { get; private set; }

    [field:Header("Close Rotation")]
    [field: SerializeField] public Vector3 Bone1_RotationClosed { get; private set; }
    [field: SerializeField] public Vector3 Bone2_RotationClosed { get; private set; }
    [field: SerializeField] public Vector3 Bone3_RotationClosed { get; private set; }

    private float _percentageCurrent = 0;
    private float _percentageTarget = 0;

    public void Open()
    {
        SetTarget(0);
    }

    public void Close()
    {
        SetTarget(1);
    }

    public void SetTarget(float percentage)
    {
        _percentageTarget = percentage;
    }

    public void LateUpdate()
    {
        UpdateFingerRotation();
    }

    private void UpdateFingerRotation()
    {
//        if (_percentageCurrent == _percentageTarget) return;

        float step = Time.deltaTime / _animationSpeed;

        if(_percentageCurrent > _percentageTarget) //Closing
        {
            _percentageCurrent -= step;
            if(_percentageCurrent <_percentageTarget) _percentageCurrent = _percentageTarget; //last step

        }else if( _percentageCurrent < _percentageTarget) // Opening
        {
            _percentageCurrent += step;
            if(_percentageCurrent>_percentageTarget) _percentageCurrent = _percentageTarget; //last step
        }

        ApplyRotation(_percentageCurrent);

    }

    private void ApplyRotation(float percentage)
    {
        Bone1.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Bone1_RotationOpen), Quaternion.Euler(Bone1_RotationClosed), percentage);
        Bone2.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Bone2_RotationOpen), Quaternion.Euler(Bone2_RotationClosed), percentage);
        Bone3.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(Bone3_RotationOpen), Quaternion.Euler(Bone3_RotationClosed), percentage);
    }

    public void SetBones(Transform bone1, Transform bone2, Transform bone3)
    {
        Bone1 = bone1;
        Bone2 = bone2;
        Bone3 = bone3;
    }

    public void SetOpenRotations(Vector3 rotation1, Vector3 rotation2, Vector3 rotation3 )
    {
        Bone1_RotationOpen = rotation1;
        Bone2_RotationOpen = rotation2;
        Bone3_RotationOpen = rotation3;
    }

    public void SetClosedRotations(Vector3 rotation1, Vector3 rotation2, Vector3 rotation3)
    {
        Bone1_RotationClosed = rotation1;
        Bone2_RotationClosed = rotation2;
        Bone3_RotationClosed = rotation3;
    }
}
