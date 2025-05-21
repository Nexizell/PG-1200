using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public abstract class GuiElementBase : MonoBehaviour, IDisposable
	{
		private static readonly Dictionary<string, List<GuiElementBase>> Stacks = new Dictionary<string, List<GuiElementBase>>();

		public string StackName = string.Empty;

		public bool NameAsId;

		public abstract bool IsVisible { get; }

		public string ElementName
		{
			get
			{
				return base.gameObject.nameNoClone();
			}
		}

		public bool IsTopInStack
		{
			get
			{
				if (!InStack(this))
				{
					return false;
				}
				return Stacks[StackName].Last().GetHashCode() == GetHashCode();
			}
		}

		private static bool Push(GuiElementBase element)
		{
			if (element == null)
			{
				Log("nullable GuiElement");
				return false;
			}
			if (element.StackName.IsNullOrEmpty())
			{
				Log("GuiElement StackName not setted");
				return false;
			}
			if (Stacks.ContainsKey(element.StackName) && Stacks[element.StackName].Contains(element))
			{
				Log("GuiElement allready exists in stack");
				return false;
			}
			if (!Stacks.ContainsKey(element.StackName))
			{
				Stacks.Add(element.StackName, new List<GuiElementBase>());
			}
			List<GuiElementBase> list = Stacks[element.StackName];
			list.Add(element);
			element.WhenPush();
			if (list.Count > 1)
			{
				GuiElementBase guiElementBase = list[list.Count - 2];
				if (guiElementBase != null)
				{
					guiElementBase.WhenPop();
				}
			}
			return true;
		}

		private static bool Pop(GuiElementBase element)
		{
			if (element == null)
			{
				Log("nullable GuiElement");
				return false;
			}
			if (element.StackName.IsNullOrEmpty())
			{
				Log("GuiElement StackName not setted");
				return false;
			}
			if (Stacks.ContainsKey(element.StackName) && !Stacks[element.StackName].Contains(element))
			{
				Log("GuiElement not exists in stack");
				return false;
			}
			List<GuiElementBase> list = Stacks[element.StackName];
			if (list.Count < 1)
			{
				return false;
			}
			if (element.GetHashCode() != list.Last().GetHashCode())
			{
				Log("GuiElement not is top in stack");
				return false;
			}
			list.Remove(list.Last());
			element.WhenPop();
			if (list.Any() && list.Last() != null)
			{
				list.Last().WhenPush();
			}
			return true;
		}

		private static bool Remove(GuiElementBase element)
		{
			if (!InStack(element))
			{
				return false;
			}
			Stacks[element.StackName].Remove(element);
			return true;
		}

		public static bool InStack(GuiElementBase element)
		{
			if (element == null)
			{
				return false;
			}
			if (element.StackName.IsNullOrEmpty())
			{
				return false;
			}
			if (Stacks.ContainsKey(element.StackName))
			{
				return Stacks[element.StackName].Contains(element);
			}
			return false;
		}

		public static bool InStack(string stackName, string elementName)
		{
			if (stackName.IsNullOrEmpty())
			{
				Log("stackName is empty");
				return false;
			}
			if (elementName.IsNullOrEmpty())
			{
				Log("elementName is empty");
				return false;
			}
			elementName = RiliExtensions.NameNoClone(elementName);
			if (!Stacks.ContainsKey(stackName))
			{
				return false;
			}
			return Stacks[stackName].Any((GuiElementBase e) => e.ElementName == elementName);
		}

		public static void Log(string format, params object[] args)
		{
		}

		public void PushRequest()
		{
			if (!InStack(this))
			{
				Push(this);
			}
			else if (!IsTopInStack && Remove(this))
			{
				Push(this);
			}
		}

		public void PopRequest()
		{
			if (IsTopInStack)
			{
				Pop(this);
			}
			else if (InStack(this))
			{
				Remove(this);
			}
			else
			{
				WhenPop();
			}
		}

		public void RemoveRequest()
		{
			if (Remove(this))
			{
				Dispose();
			}
		}

		protected virtual void WhenPush()
		{
		}

		protected virtual void WhenPop()
		{
		}

		public virtual void Dispose()
		{
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void OnDestroy()
		{
			Dispose();
		}

		public GuiElementBase()
		{
		}
	}
}
