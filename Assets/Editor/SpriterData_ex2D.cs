// ex2D import/export plugin
// SpriterData_ex2D.cs
// Spriter Data API - Unity
//  
// Authors:
//       Josh Montoute <josh@thinksquirrel.com>
//
// 
// Copyright (c) 2012 Thinksquirrel Software, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this 
// software and associated documentation files (the "Software"), to deal in the Software 
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit 
// persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// ex2D is (c) by exDev. Spriter is (c) by BrashMonkey.
//

// Some notes:
//
// The SCML file and all images must be imported and within the project's Assets folder.
// This limitation may be removed in a future version.
//
// The default implementation of ImportData works in two phases:
//
// Phase 1 creates a sprite atlas, with all textures.
// Phase 2 creates a character prefab and animation data.
//
// Overrides are planned to allow support for updating an existing atlas and character prefab.
// 
// Due to some strange behaviour in the Unity Editor, animations are not very editable once imported.

//
// ---------------------------------------------
// Pre-processor directives:
// ---------------------------------------------
//

// NO_CURVES: Disables animation curve interpolation between Spriter frames.
// Keep this define enabled until Spriter properly supports animation curves.
#define NO_CURVES

//
// ---------------------------------------------
//

using BrashMonkey.Spriter.Data;
using BrashMonkey.Spriter.Data.IO;
using BrashMonkey.Spriter.Data.ObjectModel;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace BrashMonkey.Spriter.DataPlugins.ex2D
{
	public class SpriterData_ex2D : ISpriterData
	{
		#region Menu items
		[MenuItem("Spriter/ex2D Plugin/Create new character")]
		static void CreateAtlas()
		{
			var data = new SpriterData_ex2D();
			
			string path = EditorUtility.OpenFilePanel("Open Spriter File", Application.dataPath, "scml");
			
			if (!string.IsNullOrEmpty(path))
			{
				data.LoadSCML(path);
				data.ImportData();
			}
		}
		#endregion
		
		#region Internal types
		[System.Flags]
		enum TangentMode
		{
			Editable = 0,
			Smooth = 1,
			Linear = 2,
			Stepped = 3,
		}
		#endregion
		
		#region Private fields
		private ISpriterCharacter mCharacter;
		private List<ISpriterFrame> mFrames;
		private string mPath;
		private Object[] mCachedSelectionObjects;
		const int kLeftTangentMask = 1 << 1 | 1 << 2;
		const int kRightTangentMask = 1 << 3 | 1 << 4;	
		#endregion
		
		#region Sprite atlas creation
		private void CreateSpriteAtlas()
		{
			// Create the atlas info
			exAtlasInfo atlasInfo = exAtlasInfoUtility.CreateAtlasInfo ( GetSelectionFolder(), mCharacter.name + " Atlas",
                                                             1024,
                                                             1024 );
			// Textures
			List<Object> texLoad = new List<Object>();
			List<string> paths = new List<string>();
			foreach(var frame in mFrames)
			{
				foreach(var sprite in frame.sprites)
				{
					string p = mPath + sprite.imagePath;
					if (!paths.Contains(p))
					{
						var tex = Resources.LoadAssetAtPath(p, typeof(Texture));
						texLoad.Add(tex);
						paths.Add(p);
					}
				}
			}
			
			atlasInfo.ImportObjects(texLoad.ToArray());
			
			// Layout elements
			atlasInfo.allowRotate = false; // I haven't support it
			atlasInfo.algorithm = exAtlasInfo.Algorithm.Tree; // choose tree layout algorithm
			atlasInfo.LayoutElements ();
			
			// Build the altas
			exAtlasInfoUtility.Build ( atlasInfo );	
		}
		private string GetSelectionFolder ()
		{
			if (Selection.activeObject != null)
			{
				string path = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
	
				if (!string.IsNullOrEmpty(path))
				{
					int dot = path.LastIndexOf('.');
					int slash = Mathf.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
					if (slash > 0) return (dot > slash) ? path.Substring(0, slash + 1) : path;
				}
			}
			return "Assets";
		}
		#endregion
		
		#region Character prefab creation
		
		float RecordFrame(Transform characterRoot, ISpriterKeyframe keyframe, ref ISpriterFrame lastFrame, ref ISpriterKeyframe lastKeyframe, ISpriterFrame endFrame, float currentTime, float frameRate, Dictionary<string, AnimationCurve>[] curves)
		{
			ISpriterFrame frame = FindFrame(keyframe.name);
					
			if (frame == null)
			{
				Debug.LogError(string.Format("Frame not found: {1}", keyframe.name)); 
				return currentTime;
			}
			
			// Go through each sprite, keeping track of what is processed
			List<string> processedSprites = new List<string>();
			
			int spriteDepth = 0;
			
			foreach(ISpriterSprite sprite in frame.sprites)
			{
				// Gets the last angle
				float lastAngle = 0;
				
				if (lastFrame != null)
				{
					foreach(var s in lastFrame.sprites)
					{
						if (s.imagePath.Equals(sprite.imagePath))
						{
							lastAngle = s.angle;
							break;
						}
					}
				}
				
				// Find the sprite object
				exSprite uiSprite = FindSpriteObject(characterRoot, sprite);
				
				Vector3 localPosition = new Vector3(sprite.spriteRect.x, -sprite.spriteRect.y, spriteDepth -= 1);
				
				var screenPos = uiSprite.GetComponent<exScreenPosition>();
				screenPos.x = localPosition.x;
				screenPos.y = localPosition.y;
				uiSprite.transform.localPosition = new Vector3(uiSprite.transform.localPosition.x, uiSprite.transform.localPosition.y, localPosition.z);
				
				float xFlip = sprite.xFlip ? -1 : 1;
				float yFlip = sprite.yFlip ? -1 : 1;
				
				exAtlas.Element atlasSprite = uiSprite.GetCurrentElement();
				float pW = (atlasSprite.originalWidth - atlasSprite.trimRect.width);
				float pH = (atlasSprite.originalHeight - atlasSprite.trimRect.height);
				float w = sprite.spriteRect.width - pW;
				float h = sprite.spriteRect.height - pH;
				uiSprite.customSize = true;
				
				Vector3 localScale = new Vector3(w * xFlip, h * yFlip);
				
				uiSprite.width = localScale.x;
				uiSprite.height = localScale.y;
				
				Quaternion oldRotation = uiSprite.transform.localRotation;
				Quaternion newRotation = Quaternion.Slerp(oldRotation, oldRotation * Quaternion.AngleAxis(sprite.angle - lastAngle, Vector3.forward), 1f);
				uiSprite.transform.localRotation = newRotation;
					
				uiSprite.color = sprite.color;
				
				AnimationCurve currentCurve_xPos = null;
				AnimationCurve currentCurve_yPos = null;
				AnimationCurve currentCurve_zPos = null;
				AnimationCurve currentCurve_xScale = null;
				AnimationCurve currentCurve_yScale = null;
				AnimationCurve currentCurve_zScale = null;
				AnimationCurve currentCurve_xRot = null;
				AnimationCurve currentCurve_yRot = null;
				AnimationCurve currentCurve_zRot = null;
				AnimationCurve currentCurve_wRot = null;
				AnimationCurve currentCurve_toggle = null;
				AnimationCurve currentCurve_colorR = null;
				AnimationCurve currentCurve_colorG = null;
				AnimationCurve currentCurve_colorB = null;
				
				if (!curves[0].ContainsKey(uiSprite.name))
				{
					// New object, create new animation curves
					currentCurve_xPos = new AnimationCurve();
					currentCurve_yPos = new AnimationCurve();
					currentCurve_zPos = new AnimationCurve();
					currentCurve_xScale = new AnimationCurve();
					currentCurve_yScale = new AnimationCurve();
					currentCurve_zScale = new AnimationCurve();
					currentCurve_xRot = new AnimationCurve();
					currentCurve_yRot = new AnimationCurve();
					currentCurve_zRot = new AnimationCurve();
					currentCurve_wRot = new AnimationCurve();
					currentCurve_colorR = new AnimationCurve();
					currentCurve_colorG = new AnimationCurve();
					currentCurve_colorB = new AnimationCurve();
					
					curves[0].Add(uiSprite.name, currentCurve_xPos);
					curves[1].Add(uiSprite.name, currentCurve_yPos);
					curves[2].Add(uiSprite.name, currentCurve_zPos);
					curves[3].Add(uiSprite.name, currentCurve_xScale);
					curves[4].Add(uiSprite.name, currentCurve_yScale);
					curves[5].Add(uiSprite.name, currentCurve_zScale);
					curves[6].Add(uiSprite.name, currentCurve_xRot);
					curves[7].Add(uiSprite.name, currentCurve_yRot);
					curves[8].Add(uiSprite.name, currentCurve_zRot);
					curves[9].Add(uiSprite.name, currentCurve_wRot);
					curves[11].Add(uiSprite.name, currentCurve_colorR);
					curves[12].Add(uiSprite.name, currentCurve_colorG);
					curves[13].Add(uiSprite.name, currentCurve_colorB);
					
				}
				else
				{
					// Get the current animation curves
					currentCurve_xPos = curves[0][uiSprite.name];
					currentCurve_yPos = curves[1][uiSprite.name];
					currentCurve_zPos = curves[2][uiSprite.name];
					currentCurve_xScale = curves[3][uiSprite.name];
					currentCurve_yScale = curves[4][uiSprite.name];
					currentCurve_zScale = curves[5][uiSprite.name];
					currentCurve_xRot = curves[6][uiSprite.name];
					currentCurve_yRot = curves[7][uiSprite.name];
					currentCurve_zRot = curves[8][uiSprite.name];
					currentCurve_wRot = curves[9][uiSprite.name];
					currentCurve_colorR = curves[11][uiSprite.name];
					currentCurve_colorG = curves[12][uiSprite.name];
					currentCurve_colorB = curves[13][uiSprite.name];
				}
				
				if (!curves[10].ContainsKey(uiSprite.name))
				{
					currentCurve_toggle = new AnimationCurve();
					curves[10].Add(uiSprite.name, currentCurve_toggle);
				}
				else
				{
					currentCurve_toggle = curves[10][uiSprite.name];
				}
				
				int keyPos = currentCurve_toggle.AddKey(SteppedKeyframe(currentTime, uiSprite.color.a));
				
				if (currentTime > 0)
				{	
					currentCurve_xPos.AddKey(LinearKeyframe(0, localPosition.x));
					currentCurve_yPos.AddKey(LinearKeyframe(0, localPosition.y));
					currentCurve_zPos.AddKey(LinearKeyframe(0, localPosition.z));
					
					currentCurve_xScale.AddKey(LinearKeyframe(0, localScale.x));
					currentCurve_yScale.AddKey(LinearKeyframe(0, localScale.y));
					currentCurve_zScale.AddKey(LinearKeyframe(0, localScale.z));
					
					Quaternion slerp = Quaternion.Slerp(oldRotation, uiSprite.transform.localRotation, 1f);
					
					currentCurve_xRot.AddKey(LinearKeyframe(0, slerp.x));
					currentCurve_yRot.AddKey(LinearKeyframe(0, slerp.y));
					currentCurve_zRot.AddKey(LinearKeyframe(0, slerp.z));
					currentCurve_wRot.AddKey(LinearKeyframe(0, slerp.w));
					
					currentCurve_colorR.AddKey(LinearKeyframe(0, uiSprite.color.r));
					currentCurve_colorG.AddKey(LinearKeyframe(0, uiSprite.color.g));
					currentCurve_colorB.AddKey(LinearKeyframe(0, uiSprite.color.b));
					
					if (keyPos > 0)
					{
						// Sprite was off
						if (currentCurve_toggle.keys[keyPos - 1].value < uiSprite.color.a)
						{
							currentCurve_toggle.AddKey(SteppedKeyframe(currentTime - (.001f / frameRate), 0));
						}
					}
					
					currentCurve_toggle.AddKey(SteppedKeyframe(0, 0));
				}

#if NO_CURVES
				float f = ((keyframe.duration / 100f)) - (.001f / frameRate);
				
				currentCurve_xPos.AddKey(SteppedKeyframe(currentTime, localPosition.x));
				currentCurve_yPos.AddKey(SteppedKeyframe(currentTime, localPosition.y));
				currentCurve_zPos.AddKey(SteppedKeyframe(currentTime, localPosition.z));
				
				currentCurve_xScale.AddKey(SteppedKeyframe(currentTime, localScale.x));
				currentCurve_yScale.AddKey(SteppedKeyframe(currentTime, localScale.y));
				currentCurve_zScale.AddKey(SteppedKeyframe(currentTime, localScale.z));
				
				currentCurve_xRot.AddKey(SteppedKeyframe(currentTime, uiSprite.transform.localRotation.x));
				currentCurve_yRot.AddKey(SteppedKeyframe(currentTime, uiSprite.transform.localRotation.y));
				currentCurve_zRot.AddKey(SteppedKeyframe(currentTime, uiSprite.transform.localRotation.z));
				currentCurve_wRot.AddKey(SteppedKeyframe(currentTime, uiSprite.transform.localRotation.w));
				
				currentCurve_colorR.AddKey(SteppedKeyframe(currentTime, uiSprite.color.r));
				currentCurve_colorG.AddKey(SteppedKeyframe(currentTime, uiSprite.color.g));
				currentCurve_colorB.AddKey(SteppedKeyframe(currentTime, uiSprite.color.b));
				
				currentCurve_xPos.AddKey(SteppedKeyframe(currentTime + f, localPosition.x));
				currentCurve_yPos.AddKey(SteppedKeyframe(currentTime + f, localPosition.y));
				currentCurve_zPos.AddKey(SteppedKeyframe(currentTime + f, localPosition.z));
				
				currentCurve_xScale.AddKey(SteppedKeyframe(currentTime + f, localScale.x));
				currentCurve_yScale.AddKey(SteppedKeyframe(currentTime + f, localScale.y));
				currentCurve_zScale.AddKey(SteppedKeyframe(currentTime + f, localScale.z));
				
				currentCurve_xRot.AddKey(SteppedKeyframe(currentTime + f, uiSprite.transform.localRotation.x));
				currentCurve_yRot.AddKey(SteppedKeyframe(currentTime + f, uiSprite.transform.localRotation.y));
				currentCurve_zRot.AddKey(SteppedKeyframe(currentTime + f, uiSprite.transform.localRotation.z));
				currentCurve_wRot.AddKey(SteppedKeyframe(currentTime + f, uiSprite.transform.localRotation.w));
				
				currentCurve_colorR.AddKey(SteppedKeyframe(currentTime + f, uiSprite.color.r));
				currentCurve_colorG.AddKey(SteppedKeyframe(currentTime + f, uiSprite.color.g));
				currentCurve_colorB.AddKey(SteppedKeyframe(currentTime + f, uiSprite.color.b));
#endif
				
				if (frame == endFrame)
					uiSprite.color = sprite.color;
				else
					uiSprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
				
				// Mark as processed
				processedSprites.Add(uiSprite.name);
				
			}	
			
			// Check through all existing sprite objects
			foreach(Transform transform in characterRoot)
			{
				// Is this needed?
				if (transform == characterRoot)
					continue;
				
				if (!processedSprites.Contains(transform.name))
				{	
					float t = currentTime;
					
					AnimationCurve val;
					if (curves[10].TryGetValue(transform.name, out val))
					{
						int pos = val.AddKey(SteppedKeyframe(t, 0));
						
						if (pos > 0)
						{
							if (val.keys[pos - 1].value > 0)
							{
								val.AddKey(SteppedKeyframe(t - (.001f / frameRate), val.keys[pos -1].value));
							}
						}
					}
					else
					{
						val = new AnimationCurve();
						curves[10].Add(transform.name, val);
					
						int pos = val.AddKey(SteppedKeyframe(t, 0));
						
						if (pos > 0)
						{
							if (val.keys[pos - 1].value > 0)
							{
								val.AddKey(SteppedKeyframe(t - (.001f / frameRate), val.keys[pos -1].value));
							}
						}
					}
				}
			}
			
			// Advance the time
			currentTime += (keyframe.duration / 100f);
			lastKeyframe = keyframe;
			lastFrame = frame;
			
			return currentTime;
		}
		float LerpUnclamped (float from, float to, float value)
		{
			return (1.0f - value)*from + value*to;
    	}				
		void SetKeyTangentMode (ref Keyframe key, int leftRight, TangentMode mode)
		{
			if (leftRight == 0)
			{
				key.tangentMode &= ~kLeftTangentMask;
				key.tangentMode |= (int)mode << 1;
			}
			else
			{
				key.tangentMode &= ~kRightTangentMask;
				key.tangentMode |= (int)mode << 3;
			}
			
			if (GetKeyTangentMode (key, leftRight) != mode)
				Debug.Log("bug");
		}
		TangentMode GetKeyTangentMode (Keyframe key, int leftRight)
		{
			if (leftRight == 0)
			{
				return (TangentMode)((key.tangentMode & kLeftTangentMask) >> 1);
			}
			else
			{
				return (TangentMode)((key.tangentMode & kRightTangentMask) >> 3);
			}
		}
		// TODO: Once curves are supported, use linear keyframes where possible (or even better, get tangents straight from SCML)
		Keyframe LinearKeyframe(float time, float value)
		{
#if NO_CURVES
			return SteppedKeyframe(time, value);
#else
			Keyframe result = new Keyframe();
			result.value = value;
			result.time = time;
			SetKeyTangentMode(ref result, 0, TangentMode.Editable | TangentMode.Linear);
			SetKeyTangentMode(ref result, 1, TangentMode.Editable | TangentMode.Linear);
			
			return result;
#endif
		}
		Keyframe SteppedKeyframe(float time, float value)
		{
			Keyframe result = new Keyframe();
			result.value = value;
			result.time = time;
			SetKeyTangentMode(ref result, 0, TangentMode.Editable | TangentMode.Stepped);
			SetKeyTangentMode(ref result, 1, TangentMode.Editable | TangentMode.Stepped);
			
			return result;
		}
		void SaveAssets(GameObject root)
		{	
#if UNITY_3_5
			Transform parent = root.transform.parent;
			var prefab = PrefabUtility.CreateEmptyPrefab(GetSelectionFolder() + "/" + root.name + ".prefab");
			if (prefab)
			{
				prefab = PrefabUtility.ReplacePrefab(root, prefab);
				UnityEngine.Object.DestroyImmediate(root);
				var newObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				newObj.transform.parent = parent;
				newObj.transform.localScale = Vector3.one;
			}
#else
			Transform parent = root.transform.parent;
			var prefab = EditorUtility.CreateEmptyPrefab(GetSelectionFolder() + "/" + root.name + ".prefab");
			if (prefab)
			{
				prefab = EditorUtility.ReplacePrefab(root, prefab);
				UnityEngine.Object.DestroyImmediate(root);
				var newObj = EditorUtility.InstantiatePrefab(prefab) as GameObject;
				newObj.transform.parent = parent;
				newObj.transform.localScale = Vector3.one;
			}
#endif		
			AssetDatabase.Refresh();
		}
		exSprite FindSpriteObject(Transform characterRoot, ISpriterSprite sprite)
		{
			// Find existing
			foreach(Transform transform in characterRoot)
			{
				// Is this needed?
				if (transform == characterRoot)
					continue;
				
				if (transform.name == GetSpriteName(sprite.imagePath))
				{
					return transform.GetComponent<exSprite>();
				}
			}
			
			// None exists
			GameObject go = new GameObject(GetSpriteName(sprite.imagePath));
			go.transform.parent = characterRoot;
			go.transform.localPosition = Vector3.zero;
			
			exSprite result = go.AddComponent<exSprite>();
			result.anchor = exPlane.Anchor.TopLeft;
			
			var screenPos = go.AddComponent<exScreenPosition>();
			screenPos.anchor = exPlane.Anchor.MidCenter;
				
			go.AddComponent<Spriterex2DHelper>();
			
			string p = mPath + sprite.imagePath.Replace("\\", "/");
			Texture2D tex = AssetDatabase.LoadAssetAtPath(p, typeof(Texture2D)) as Texture2D;
			
			if ( tex == null )
   				Debug.LogError("texture not found: " + p);
			
			result.textureGUID = exEditorHelper.AssetToGUID(tex);
			exAtlasDB.ElementInfo elInfo = exAtlasDB.GetElementInfo( result.textureGUID );
			exSpriteEditor.UpdateAtlas( result, elInfo );
			return result;
			
		}
		ISpriterFrame FindFrame(string name)
		{
			foreach(var frame in mFrames)
			{
				if (frame.name.Equals(name))
					return frame;
			}
			
			return null;
		}	
		string GetSpriteName(string imagePath)
		{
			int index = imagePath.LastIndexOf('\\') + 1;
			string file = index == -1 ? imagePath : imagePath.Substring(index);
			return file.Substring(0, file.LastIndexOf('.'));
		}
		#endregion
		
		#region ISpriterData implementation
		public void LoadSCML(string path)
		{
			LoadSCML<SpriterData_ex2D, SpriterCharacter, SpriterAnimation, SpriterFrame, SpriterKeyframe, SpriterSprite>(path);
			int len = Application.dataPath.Length + 1;
			string sub = path.Substring(len);
			int index = sub.LastIndexOf('/') + 1;
			mPath = "Assets/" + sub.Substring(0, index);
		}

		public void SaveSCML(string path)
		{
			throw new System.NotImplementedException();
		}
		
		public void ExportData()
		{
			throw new System.NotImplementedException();
		}
		
		public void ImportData()
		{
			if (string.IsNullOrEmpty(mPath))
				return;
			
			mCachedSelectionObjects = Selection.objects;
			
			CreateSpriteAtlas();
		}
		
		public ISpriterCharacter character
		{
			get
			{
				return mCharacter;
			}
			set
			{
				mCharacter = value;
			}
		}

		public List<ISpriterFrame> frames
		{
			get
			{
				return mFrames;
			}
			set
			{
				mFrames = value;
			}
		}
		#endregion
	
	}
}