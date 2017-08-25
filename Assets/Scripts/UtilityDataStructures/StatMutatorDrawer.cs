using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

[System.Serializable]
public class StatMutatorPrototype
{
	[SerializeField]
	private ActorStatsDeclaration stat;

	[SerializeField]
	private MutatorBehaviour Behaviour;

	[SerializeField]
	private MutatorDuration Duration;

	[SerializeField]
	private MutatorType Type;

	[SerializeField]
	private float Value;

	[SerializeField]
	private float TimeDuration;

	[SerializeField]
	private bool ValuePerTime;

	public ActorStatsDeclaration StatMutated {get {return stat;}}

	public StatMutator CreateStatMutatorFromPrototype()
	{
		switch (Duration) 
		{
			case MutatorDuration.Immidiate:
				switch (Type) 
				{
					case MutatorType.Flat:
						return StatMutatorFactory.ImmidiateMutatorFlat (Value);

					case MutatorType.Multiplicator:
						return StatMutatorFactory.ImmidiateMutatorMultiplicator (Value);
				}
				break;

			case MutatorDuration.Constant:
				switch (Type) 
				{
					case MutatorType.Flat:
						switch (Behaviour) 
						{
						case MutatorBehaviour.ValueConstant:
							return StatMutatorFactory.ConstantMutatorFlat (Value);

						case MutatorBehaviour.ValuePerSecond:
							return StatMutatorFactory.ConstantMutatorFlatValuePerSecond (Value);
						}
						break;

					case MutatorType.Multiplicator:
						return StatMutatorFactory.ConstantMutatorMultiplicator (Value);
				}
				break;

			case MutatorDuration.TimeElapsed:
				switch (Type) 
				{
					case MutatorType.Flat:
						switch (Behaviour) 
						{
							case MutatorBehaviour.ValueConstant:
								return StatMutatorFactory.TimeElapsedMutatorFlat (Value, TimeDuration);

							case MutatorBehaviour.ValueDecreasing:
								return StatMutatorFactory.TimeElapsedMutatorValueDecrease (Value, TimeDuration);

							case MutatorBehaviour.ValuePerSecond:
								if (ValuePerTime) 
								{
									return StatMutatorFactory.TimeElapsedMutatorValuePerTime (Value, TimeDuration);
								}
								else 
								{
									return StatMutatorFactory.TimeElapsedMutatorValuePerSecond (Value, TimeDuration);
								}
						}
						break;

					case MutatorType.Multiplicator:
						return StatMutatorFactory.TimeElapsedMutatorMultiplicator (Value, TimeDuration);
				}
				break;
		}

		throw new System.ArgumentException("Prohibited combination of properties!");
	}
}
/*
public abstract class StatMutatorDrawerAbstract : PropertyDrawer 
{
	private StatMutatorPrototype _Mutator;
	private bool _Foldout;
	private const float kButtonWidth = 18f;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		CheckInitialize(property, label);

		if (_Foldout) 
		{
			return 7 * 17f;
		}

		return 17f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		CheckInitialize(property, label);

		position.height = 17f;

		var foldoutRect = position;
		foldoutRect.width -= 2 * kButtonWidth;
		EditorGUI.BeginChangeCheck();
		_Foldout = EditorGUI.Foldout(foldoutRect, _Foldout, label, true);
		if (EditorGUI.EndChangeCheck())
			EditorPrefs.SetBool(label.text, _Foldout);

		var buttonRect = position;
		buttonRect.x = position.width - kButtonWidth + position.x;
		buttonRect.width = kButtonWidth + 2;	

		buttonRect.x -= kButtonWidth;

		if (!_Foldout)
			return;
	}

	private void CheckInitialize(SerializedProperty property, GUIContent label)
	{/*
		if (_Dictionary == null)
		{
			var target = property.serializedObject.targetObject;
			_Dictionary = fieldInfo.GetValue(target) as SerializableDictionary<TK, TV>;
			if (_Dictionary == null)
			{
				_Dictionary = new SerializableDictionary<TK, TV>();
				fieldInfo.SetValue(target, _Dictionary);
			}

			_Foldout = EditorPrefs.GetBool(label.text);
		}

	}
}

[CustomPropertyDrawer(typeof(StatMutatorDrawerAbstract))]
public class StatMutatorDrawer {}
*/