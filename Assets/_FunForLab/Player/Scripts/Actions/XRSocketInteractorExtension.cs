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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Extension de <see cref="XRSocketInteractor"/> qui corrige l'emplacement du mesh de l'objet lors d'un hover (bas� sur la version 2.0.0.pre7).
/// </summary>
public class XRSocketInteractorExtension : XRSocketInteractor {
	public Transform HoverAttachTransform; // Point d'attache du hover, pour le fixer � un emplacement pr�cis.

	[NonSerialized]
	public readonly Dictionary<IXRInteractable, ValueTuple<MeshFilter, Renderer>[]> m_MeshFilterCache = new Dictionary<IXRInteractable, ValueTuple<MeshFilter, Renderer>[]>();

	/// <summary>
	/// Reusable list of type <see cref="MeshFilter"/> to reduce allocations.
	/// </summary>
	[NonSerialized]
	public static readonly List<MeshFilter> s_MeshFilters = new List<MeshFilter>();

	/// <inheritdoc />
	protected override void OnHoverEntering(HoverEnterEventArgs args) {
		base.OnHoverEntering(args);

		s_MeshFilters.Clear();
		args.interactableObject.transform.GetComponentsInChildren(true, s_MeshFilters);

		if (s_MeshFilters.Count == 0) {
			return;
		}

		(MeshFilter, Renderer)[] interactableTuples = new ValueTuple<MeshFilter, Renderer>[s_MeshFilters.Count];

		for (int i = 0; i < s_MeshFilters.Count; ++i) {
			MeshFilter meshFilter = s_MeshFilters[i];
			interactableTuples[i] = (meshFilter, meshFilter.GetComponent<Renderer>());
		}

		m_MeshFilterCache.Add(args.interactableObject, interactableTuples);
	}

	/// <inheritdoc />
	protected override void OnHoverExiting(HoverExitEventArgs args) {
		base.OnHoverExiting(args);
		m_MeshFilterCache.Remove(args.interactableObject);
	}

	protected override void DrawHoveredInteractables() {
		if (interactableHoverScale <= 0f) {
			return;
		}

		Material materialToDrawWith = hasSelection ? interactableCantHoverMeshMaterial : interactableHoverMeshMaterial;

		if (materialToDrawWith == null) {
			return;
		}

		Camera mainCamera = Camera.main;

		if (mainCamera == null) {
			return;
		}

		foreach (IXRHoverInteractable interactable in interactablesHovered) {
			if (interactable == null || IsSelecting(interactable) || !m_MeshFilterCache.TryGetValue(interactable, out (MeshFilter, Renderer)[] interactableTuples) || interactableTuples == null || interactableTuples.Length == 0) {
				continue;
			}

			foreach ((MeshFilter, Renderer) tuple in interactableTuples) {
				MeshFilter meshFilter = tuple.Item1;
				Renderer meshRenderer = tuple.Item2;

				if (!ShouldDrawHoverMesh(meshFilter, meshRenderer, mainCamera)) {
					continue;
				}

				Matrix4x4 matrix = GetHoverMeshMatrix(interactable, meshFilter, interactableHoverScale);
				Mesh sharedMesh = meshFilter.sharedMesh;

				for (int submeshIndex = 0; submeshIndex < sharedMesh.subMeshCount; ++submeshIndex) {
					Graphics.DrawMesh(
						sharedMesh,
						matrix,
						materialToDrawWith,
						gameObject.layer,
						null, // Draw mesh in all cameras (default value)
						submeshIndex);
				}
			}
		}
	}

	// Cette m�thode � �t� modifi�e afin que si un HoverAttachTransform existe, on fixe la position du hover � cet emplacement.
	Matrix4x4 GetHoverMeshMatrix(IXRInteractable interactable, MeshFilter meshFilter, float hoverScale) {
		Vector3 position;
		Quaternion rotation;
		Vector3 scale;

		if (HoverAttachTransform != null) { // Si on a positionn� un HoverAttachTransform, on fixe la position � cet emplacement
			position = HoverAttachTransform.position;
			rotation = HoverAttachTransform.rotation;
			scale = meshFilter.transform.lossyScale * hoverScale;
		}
		else { // Sinon garder la m�thode de Hover de la version 2.0.0.pre7 (la 2.0.0 finale ne fonctionne plus correctement)
			Transform interactableAttachTransform = interactable.GetAttachTransform(this);
			Vector3 attachOffset = meshFilter.transform.position - interactableAttachTransform.position;
			Vector3 interactableLocalPosition = interactableAttachTransform.InverseTransformDirection(attachOffset) * hoverScale;
			Quaternion interactableLocalRotation = Quaternion.Inverse(Quaternion.Inverse(meshFilter.transform.rotation) * interactableAttachTransform.rotation);

			Transform interactorAttachTransform = GetAttachTransform(interactable);
			position = interactorAttachTransform.position + interactorAttachTransform.rotation * interactableLocalPosition;
			rotation = interactorAttachTransform.rotation * interactableLocalRotation;
			scale = meshFilter.transform.lossyScale * hoverScale;
		}

		return Matrix4x4.TRS(position, rotation, scale);
	}
}
