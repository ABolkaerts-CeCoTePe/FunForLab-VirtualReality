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

public class DoorRotate : Door
{
    public float doorOpenAngle = 90.0f;
    public float doorCloseAngle = 0.0f;

    private void Start()
    {
        opening = StartOpen;
    }

    //Open or close doors
    void Update()
    {
        // If opening
        if (opening)
        {
            // Direction selection
            if (directionType == Direction.X)
            {
                doorL.localRotation = Quaternion.Slerp(doorL.localRotation, Quaternion.Euler(doorOpenAngle, 0, 0), Time.deltaTime * speed);
                doorR.localRotation = Quaternion.Slerp(doorR.localRotation, Quaternion.Euler(-doorOpenAngle, 0, 0), Time.deltaTime * speed);
            }
            else if (directionType == Direction.Y)
            {
                doorL.localRotation = Quaternion.Slerp(doorL.localRotation, Quaternion.Euler(0, doorOpenAngle, 0), Time.deltaTime * speed);
                doorR.localRotation = Quaternion.Slerp(doorR.localRotation, Quaternion.Euler(0, -doorOpenAngle, 0), Time.deltaTime * speed);
            }
            else if (directionType == Direction.Z)
            {
                doorL.localRotation = Quaternion.Slerp(doorL.localRotation, Quaternion.Euler(0, 0, doorOpenAngle), Time.deltaTime * speed);
                doorR.localRotation = Quaternion.Slerp(doorR.localRotation, Quaternion.Euler(0, 0, -doorOpenAngle), Time.deltaTime * speed);
            }
        }
        else
        {
            // Direction selection
            if (directionType == Direction.X)
            {
                doorL.localRotation = Quaternion.Slerp(doorL.localRotation, Quaternion.Euler(doorCloseAngle, 0, 0), Time.deltaTime * speed);
                doorR.localRotation = Quaternion.Slerp(doorR.localRotation, Quaternion.Euler(-doorCloseAngle, 0, 0), Time.deltaTime * speed);
            }
            else if (directionType == Direction.Y)
            {
                doorL.localRotation = Quaternion.Slerp(doorL.localRotation, Quaternion.Euler(0, doorCloseAngle, 0), Time.deltaTime * speed);
                doorR.localRotation = Quaternion.Slerp(doorR.localRotation, Quaternion.Euler(0, -doorCloseAngle, 0), Time.deltaTime * speed);
            }
            else if (directionType == Direction.Z)
            {
                doorL.localRotation = Quaternion.Slerp(doorL.localRotation, Quaternion.Euler(0, 0, doorCloseAngle), Time.deltaTime * speed);
                doorR.localRotation = Quaternion.Slerp(doorR.localRotation, Quaternion.Euler(0, 0, -doorCloseAngle), Time.deltaTime * speed);
            }
        }

    }
}
