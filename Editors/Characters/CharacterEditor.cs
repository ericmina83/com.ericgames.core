using UnityEngine;
using UnityEditor;
using EricGames.Runtime.Characters;
using EricGames.Runtime.StateMachine;

namespace EricGames.Editors.Characters
{
    [CustomEditor(typeof(Character))]
    public class CharacterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var character = target as Character;
            var stateMachine = character.GetType().GetProperty("stateMachine").GetValue(character, null) as StateMachine<Character.State>;
            EditorGUILayout.LabelField(stateMachine.CurrState.ToString());
        }
    }
}