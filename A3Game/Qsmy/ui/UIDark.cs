using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MuGame
{
	[AddComponentMenu("UI/Effects/DarkUI")]
	[ExecuteInEditMode]
	public class UIDark : MonoBehaviour
	{
		public bool IsDark { get; set; }
		void Start() {
			
		}

		[ContextMenu("重置操作")]
		public void UIReset() {
			REMO();
		}

		[ContextMenu("应用操作")]
		public void UIAppliy() {
			ADDO();
		}

		public void ADDO() {
			Material mat = U3DAPI.U3DResLoad<Material>("uifx/uiGray");
			if (mat == null) {
				Debug.LogError("找不到需求材质！ = uiGray.mat");
				return;
			}
			foreach (var v in transform.GetComponentsInChildren<Image>()) {
				v.material = mat;
				v.material.SetFloat("_grayScale",0.3f);
			}
			IsDark = true;
		}

		public void REMO() {
			foreach (var v in transform.GetComponentsInChildren<Image>()) {
				v.material = null;
			}
			IsDark = false;
		}

	}

}
