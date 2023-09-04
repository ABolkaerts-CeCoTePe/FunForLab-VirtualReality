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
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimator : MonoBehaviour
{
    [Header("Input action references")]
    [SerializeField] private InputActionReference primary;
    [SerializeField] private InputActionReference trigger;
    [SerializeField] private InputActionReference grip;

    [Header("Fingers animators")]
    [SerializeField] private FingerAnimator _thumbAnimator;
    [SerializeField] private FingerAnimator _indexAnimator;
    [SerializeField] private FingerAnimator _midAnimator;
    [SerializeField] private FingerAnimator _ringAnimator;
    [SerializeField] private FingerAnimator _pinkyAnimator;

    private void OnEnable()
    {
        primary.action.performed += EventPrimary;
        primary.action.canceled += EventPrimaryReleased;

        trigger.action.performed += EventTrigger;
        trigger.action.canceled += EventTriggerReleased;

        grip.action.performed += EventGrip;
        grip.action.canceled += EventGripReleased;
    }

    private void OnDisable()
    {
        primary.action.performed -= EventPrimary;
        primary.action.canceled -= EventTrigger;

        trigger.action.performed -= EventTrigger;
        trigger.action.canceled -= EventTriggerReleased;

        grip.action.performed -= EventGrip;
        grip.action.canceled -= EventGripReleased;
    }
    void Start()
    {
        Open();
    }

    #region Event listeners
    // Trigger pressed
    private void EventTrigger(InputAction.CallbackContext ctx)
    {
        Trigger(ctx.ReadValue<float>());
    }
    private void EventTriggerReleased(InputAction.CallbackContext ctx)
    {
        Trigger(0);
    }


    // Grip pressed
    private void EventGrip(InputAction.CallbackContext ctx)
    {
        Grip(ctx.ReadValue<float>());
    }

    private void EventGripReleased(InputAction.CallbackContext ctx)
    {
        Grip(0);
    }

    // Primary pressed
    private void EventPrimary(InputAction.CallbackContext ctx)
    {
        Primary(ctx.ReadValue<float>());
    }

    private void EventPrimaryReleased(InputAction.CallbackContext ctx)
    {
        Primary(0);
    }

    #endregion

    #region Animations
    // On trigger -> Index
    private void Trigger(float trigger)
    {
        _indexAnimator.SetTarget(trigger);
    }


    // On grip -> mid, ring and pinky
    private void Grip(float grip)
    {
        _midAnimator.SetTarget(grip);
        _ringAnimator.SetTarget(grip);
        _pinkyAnimator.SetTarget(grip);
    }

    private void Primary(float primary)
    {
        _thumbAnimator.SetTarget(primary);
    }

    private void Close(float close)
    {
        _thumbAnimator.SetTarget(close);
        _indexAnimator.SetTarget(close);
        _midAnimator.SetTarget(close);
        _ringAnimator.SetTarget(close);
        _pinkyAnimator.SetTarget(close);
    }

    public void Open()
    {
        _thumbAnimator.Open();
        _indexAnimator.Open();
        _midAnimator.Open();
        _ringAnimator.Open();
        _pinkyAnimator.Open();
    }

    public void Close()
    {
        _thumbAnimator.Close();
        _indexAnimator.Close();
        _midAnimator.Close();
        _ringAnimator.Close();
        _pinkyAnimator.Close();
    }
    #endregion
}