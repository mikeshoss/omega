// ex2D helper
// Spriterex2DHelper.cs
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
using UnityEngine;
using System.Collections;

/// <summary>
/// Updates ex2D sprite properties through animation.
/// </summary>
[RequireComponent(typeof(exSprite))]
[ExecuteInEditMode]
public class Spriterex2DHelper : MonoBehaviour
{
	public exSprite sprite;
	public exScreenPosition screenPos;
	private Transform cachedTransform;
	public Color color = Color.white;
	public Vector3 localPosition;
	public Vector3 localScale;
	
	void Awake()
	{
		cachedTransform = transform;
		sprite = GetComponent<exSprite>();
		screenPos = GetComponent<exScreenPosition>();
		color = sprite.color;
		localPosition = new Vector3(screenPos.x, screenPos.y, transform.localPosition.z);
		localScale = new Vector3(sprite.width, sprite.height, 1);
	}
	
	void LateUpdate()
	{
		sprite.color = color;
		sprite.width = localScale.x;
		sprite.height = localScale.y;
		screenPos.x = localPosition.x;
		screenPos.y = localPosition.y;
		cachedTransform.localPosition = new Vector3(cachedTransform.localPosition.x, cachedTransform.localPosition.y, localPosition.z);
	}
}
