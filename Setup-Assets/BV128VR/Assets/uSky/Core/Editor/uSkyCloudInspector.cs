using UnityEngine;
using System.Reflection;

namespace UnityEditor
{
	[CustomEditor(typeof(DistanceCloud))]
	public class uSkyCloudInspector : Editor {

		DistanceCloud m_DC;
		Editor currentMaterialEditor;
		Editor m_tmpEditor;
//		Material m_Mat;
		SerializedObject serObj;
		SerializedProperty mat;
//		SerializedProperty DayBrightness;
//		SerializedProperty NightBrightness;

//		private bool showMat = true; // true means assign as default foldout PreDrop

		private void OnEnable () {
			serObj = new SerializedObject (target);
			mat = serObj.FindProperty ("CloudMaterial");
//			NightBrightness = serObj.FindProperty ("NightBrightness");
		}

		public override void OnInspectorGUI (){  

//			DrawDefaultInspector ();
//			EditorGUILayout.Space ();

			serObj.Update ();
			EditorGUILayout.PropertyField (mat, new GUIContent ("Cloud Material", "Put Cloud material here."));
//			NightBrightness.floatValue = EditorGUILayout.Slider("Night Brightness",NightBrightness.floatValue,0f,1f);
			serObj.ApplyModifiedProperties ();

			m_DC = (DistanceCloud)target;
			Material m_Mat = m_DC.CloudMaterial;

			EditorGUI.BeginChangeCheck ();						

			// draw a Titlebar 
//			if (m_Mat != null)
//				showMat = EditorGUILayout.InspectorTitlebar (showMat, m_Mat); 

			InitEditor (m_Mat);

			currentMaterialEditor = m_tmpEditor;
//						if (GUI.changed) {
				if (EditorGUI.EndChangeCheck ()) {
						InitEditor (m_Mat);
					}

			// draw material setting 
//			if (tmpEditor != null && m_Mat != null && showMat) {		// for Titlebar
			if (currentMaterialEditor != null && m_Mat != null) {		// for DrawHeader
					currentMaterialEditor.DrawHeader ();
					currentMaterialEditor.OnInspectorGUI ();
			}

		}

		void InitEditor (Material m_Mat){
			if (m_Mat != null ) {
				m_tmpEditor = CreateEditor (m_Mat);
				
			} 
//			else {
//				currentMaterialEditor = null;
//			}
			if (currentMaterialEditor != null) {
				DestroyImmediate (currentMaterialEditor);
			}
		}
	}
}