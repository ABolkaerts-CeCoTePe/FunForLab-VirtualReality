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
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Classe qui gère le Robot, et ses intéractions avec le Dialogue System.
/// </summary>
public class RobotController : MonoBehaviour {
	[SerializeField] private NavMeshAgent agent;
    public Transform[] teleportsPositions;
//	private float angularRotationSpeed = 10f;

    RotateTowardPlayer rotateScript;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rotateScript = GetComponent<RotateTowardPlayer>();
    }

    private void OnEnable() { // Enregistrer des méthodes C# afin qu'elles puissent être appelées dans le code Lua du Dialogue System
		Lua.RegisterFunction("RobotTeleportToPosition", this, SymbolExtensions.GetMethodInfo(() => GoToDestination(0)));
        Lua.RegisterFunction("RobotGoToPos", this, SymbolExtensions.GetMethodInfo(() => RobotGoToPos(string.Empty)));
    }

	private void OnDisable() {
		Lua.UnregisterFunction("RobotTeleportToPosition");
        Lua.UnregisterFunction("RobotGoToPos");
    }

    public void GoToDestination(double pos)
    {
        Debug.Log("Robot Go to Pos " + pos + " : " + teleportsPositions[(int)pos].position + " !");
        agent.destination = teleportsPositions[(int)pos].position;
        StartCoroutine(WaitAndCheckReachedDestination(1f / 60f, pos));
    }

    public bool ReachedDestination()
	{
        if (agent.hasPath) return false;
		if (agent.velocity.sqrMagnitude != 0f) return false;
        if (agent.pathPending) return false;
		if (agent.remainingDistance > agent.stoppingDistance) return false;
		return true;
    }
	 
	// Va téléporter le robot à la position indiqué.
	public void RobotTeleportToPosition(double pos) {
		try {
			Debug.Log("Robot teleportation to pos " + pos + " : " + teleportsPositions[(int) pos].position + " !");
			gameObject.transform.SetPositionAndRotation(teleportsPositions[(int) pos].position, teleportsPositions[(int) pos].rotation);
            
            //rotateScript.enabled = false;
        }
		catch (Exception) {
			Debug.Log("Robot teleportation pos outside of range !");
		}
	}

    public void RobotGoToPos(string posName)
    {

        try {
			Debug.Log("Robot teleportation to pos " + posName + " !");
            GoToDestination(GetIndexByName(posName));
        }
		catch (Exception) {
            Debug.Log("Robot teleportation target not found !");
        }
    }

    private int GetIndexByName(string name)
    {
        try
        {
            for (int i = 0; i < teleportsPositions.Length; i++)
            {
                if (teleportsPositions[i].name == name) return i;
            }
            return -1;
        }
        catch(Exception)
        {
            Debug.Log("Robot teleportation target not found !");
        }
        return -1;
        
    }

    private IEnumerator WaitAndCheckReachedDestination(float waitTime, double pos)
    {
        while (!ReachedDestination())
        {
            yield return new WaitForSeconds(waitTime);
        }
        rotateScript.enabled = true;
    }

}