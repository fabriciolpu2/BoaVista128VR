// Only useful in Unity 4. It shows "slider + float" field for the range setting in the material properties
// To use this add [RangeSlider] in front of the range setting.
// Example: [RangeSlider] _MyValue("My Value", Range (0, 1.0)) = 1

using UnityEngine;

namespace UnityEditor
{
internal class RangeSliderDrawer : MaterialPropertyDrawer
{
	public override void OnGUI(Rect position, MaterialProperty property, string label, MaterialEditor editor)
    {
		// Use default labelWidth
		EditorGUIUtility.labelWidth = 0 ;

		Vector2 rangeLimits = property.rangeLimits;
		float value = property.floatValue;

		// Detect any changes to the material
		EditorGUI.BeginChangeCheck ();

		value = EditorGUI.Slider(position,label, value, rangeLimits.x, rangeLimits.y); 
//		value = EditorGUILayout.Slider(label, value, rangeLimits.x, rangeLimits.y);

		if (EditorGUI.EndChangeCheck ())
		{
			property.floatValue = value;
		}
	}
}
}
