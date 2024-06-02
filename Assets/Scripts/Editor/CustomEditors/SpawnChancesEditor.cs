using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors
{
    [CustomEditor(typeof(SpawnChances))]
    public class SpawnChancesEditor : UnityEditor.Editor
    {
        private bool _commonLock;
        private bool _scatterLock;
        private bool _multiplierLock;
        private TypeChances _typeChances;
        private SpawnChances SpawnChances => (SpawnChances)target;

        private void OnEnable()
        {
            _typeChances = SpawnChances.typeChances;
        }

        public override void OnInspectorGUI()
        {
            RenderTypeChanceSliders();
        }

        private void RenderTypeChanceSliders()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                var boldLabelStyle = new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold
                };
                EditorGUILayout.LabelField("Chances", boldLabelStyle);

                EditorGUI.BeginChangeCheck();

                var oldCommonChance = _typeChances.commonChance;
                _typeChances.commonChance = EditorGUILayout.Slider("Common Chance", oldCommonChance, 0.001f, 1f);

                var oldScatterChance = _typeChances.scatterChance;
                _typeChances.scatterChance = EditorGUILayout.Slider("Scatter Chance", oldScatterChance, 0.001f, 1f);

                var oldMultiplierChance = _typeChances.multiplierChance;
                _typeChances.multiplierChance = EditorGUILayout.Slider("Multiplier Chance", oldMultiplierChance, 0.001f, 1f);

                EditorGUILayout.EndVertical();
            }
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(20f));
                EditorGUILayout.LabelField("Lock", GUILayout.Width(40f));
                _commonLock = EditorGUILayout.Toggle(_commonLock, GUILayout.Width(20f));
                _scatterLock = EditorGUILayout.Toggle(_scatterLock, GUILayout.Width(20f));
                _multiplierLock = EditorGUILayout.Toggle(_multiplierLock, GUILayout.Width(20f));
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                NormalizeChances();
                ApplyChanges();
            }
        }

        private void NormalizeChances()
        {
            float lockedTotal = 0f;
            float unlockedTotal = 0f;
            
            if (_commonLock) lockedTotal += _typeChances.commonChance;
            else unlockedTotal += _typeChances.commonChance;

            if (_scatterLock) lockedTotal += _typeChances.scatterChance;
            else unlockedTotal += _typeChances.scatterChance;

            if (_multiplierLock) lockedTotal += _typeChances.multiplierChance;
            else unlockedTotal += _typeChances.multiplierChance;

            float scale = (1f - lockedTotal) / unlockedTotal;

            if (!_commonLock) _typeChances.commonChance *= scale;
            if (!_scatterLock) _typeChances.scatterChance *= scale;
            if (!_multiplierLock) _typeChances.multiplierChance *= scale;
            
            _typeChances.commonChance = RoundUpToThreeDecimals(_typeChances.commonChance);
            _typeChances.scatterChance = RoundUpToThreeDecimals(_typeChances.scatterChance);
            _typeChances.multiplierChance = RoundUpToThreeDecimals(_typeChances.multiplierChance);
        }

        private float RoundUpToThreeDecimals(float value)
        {
            return Mathf.Ceil(value * 1000f) / 1000f;
        }
        
        private void ApplyChanges()
        {
            SpawnChances.typeChances = _typeChances;
            EditorUtility.SetDirty(SpawnChances);
        }
    }
}
