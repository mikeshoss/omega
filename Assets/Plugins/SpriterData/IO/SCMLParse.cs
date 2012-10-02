// SCML Parse methods
// SCMLParse.cs
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
// Spriter is (c) by BrashMonkey.
//
using BrashMonkey.Spriter.Data.ObjectModel;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace BrashMonkey.Spriter.Data.IO
{
	public static class SCMLParse
	{	
		/// <summary>
		/// Loads spriter information from SCML.
		/// </summary>
		public static ISpriterData LoadSCML<TData, TCharacter, TAnimation, TFrame, TKeyframe, TSprite>(this ISpriterData data, string path)
			where TData : ISpriterData
			where TCharacter : ISpriterCharacter
			where TAnimation : ISpriterAnimation
			where TFrame : ISpriterFrame
			where TKeyframe : ISpriterKeyframe
			where TSprite : ISpriterSprite
		{	
			if (data == null)
				data = System.Activator.CreateInstance<TData>();
				
			using (XmlReader reader = XmlReader.Create(path))
			{
				
				Debug.Log (reader.ReadToFollowing("char"));
				// Create character
				ISpriterCharacter character = System.Activator.CreateInstance<TCharacter>();
				Debug.Log(character.boundingBox.ToString());
				Debug.Log (character.ToString());
				data.character = character;
				Debug.Log (data.character.name);
				// Character name
				reader.ReadToDescendant("name");
				character.name = reader.ReadElementContentAsString();
				
				// Animations
				character.animations = new List<ISpriterAnimation>();
				
				bool doneReadingAnimations = false;
				
				while (!doneReadingAnimations)
				{
					reader.Read();
					
					if (reader.Name.Equals("anim"))
					{
						// Create animation
						ISpriterAnimation anim = System.Activator.CreateInstance<TAnimation>();
						character.animations.Add(anim);
						
						// Animation name
						reader.ReadToDescendant("name");
						anim.name = reader.ReadElementContentAsString();
						
						// Keyframes
						anim.keyframes = new List<ISpriterKeyframe>();
						
						while(reader.ReadToNextSibling("frame"))
						{			
							// Create keyframe
							ISpriterKeyframe keyframe = System.Activator.CreateInstance<TKeyframe>();
							anim.keyframes.Add(keyframe);
							
							// Keyframe name
							reader.ReadToDescendant("name");
							keyframe.name = reader.ReadElementContentAsString();
							
							// Keyframe duration
							reader.ReadToNextSibling("duration");
							keyframe.duration = reader.ReadElementContentAsFloat();
							
							reader.ReadEndElement();
						}
					}
					else if (reader.Name.Equals("box"))
					{
						// Bounding box and pivot
						float bottom, top, right, left;
						
						reader.ReadToDescendant("bottom");
						bottom = reader.ReadElementContentAsFloat();
						
						reader.ReadToNextSibling("top");
						top = reader.ReadElementContentAsFloat();
						
						reader.ReadToNextSibling("right");
						right = reader.ReadElementContentAsFloat();
						
						reader.ReadToNextSibling("left");
						left = reader.ReadElementContentAsFloat();
						
						character.boundingBox = new Rect(left, top, Mathf.Abs(left) + Mathf.Abs(right), Mathf.Abs(top) + Mathf.Abs(bottom));
						
						doneReadingAnimations = true;
					}
					
					reader.ReadEndElement();
				}
				
				bool hasFrames = reader.ReadToFollowing("frame");
				
				data.frames = new List<ISpriterFrame>();
				
				if (!hasFrames)
					return data;
				
				do
				{
					// Create frame
					ISpriterFrame frame = System.Activator.CreateInstance<TFrame>();
					data.frames.Add(frame);
					
					// Frame name
					reader.ReadToDescendant("name");
					frame.name = reader.ReadElementContentAsString();
					
					// Frame sprites
					frame.sprites = new List<ISpriterSprite>();
					
					while (reader.ReadToNextSibling("sprite"))
					{
						// Create sprite
						ISpriterSprite sprite = System.Activator.CreateInstance<TSprite>();
						frame.sprites.Add(sprite);
						
						// Image
						reader.ReadToDescendant("image");
						sprite.imagePath = reader.ReadElementContentAsString();
						
						// Color and opacity
						reader.ReadToNextSibling("color");
						int rgbInt = reader.ReadElementContentAsInt();
						
						float r, g, b, a;
						r = (rgbInt & 255)/255f;
						g = ((rgbInt >> 8) & 255)/255f;
						b = ((rgbInt >> 16) & 255)/255f;
					
						reader.ReadToNextSibling("opacity");
						a = reader.ReadElementContentAsFloat() / 100f;
						
						sprite.color = new Color(r, g, b, a);
						
						// Angle
						reader.ReadToNextSibling("angle");
						sprite.angle = reader.ReadElementContentAsFloat();
						
						// X Flip
						reader.ReadToNextSibling("xflip");
						sprite.xFlip = System.Convert.ToBoolean(reader.ReadElementContentAsInt());
						
						// Y Flip
						reader.ReadToNextSibling("yflip");
						sprite.yFlip = System.Convert.ToBoolean(reader.ReadElementContentAsInt());
						
						// Sprite rect
						float width, height, x, y;
						
						reader.ReadToNextSibling("width");
						width = reader.ReadElementContentAsFloat();
						
						reader.ReadToNextSibling("height");
						height = reader.ReadElementContentAsFloat();
						
						reader.ReadToNextSibling("x");
						x = reader.ReadElementContentAsFloat();
					
						reader.ReadToNextSibling("y");
						y = reader.ReadElementContentAsFloat();
						
						sprite.spriteRect = new Rect(x, y, width, height);
						
						reader.ReadEndElement();
					}
					
					reader.ReadEndElement();
				}
				while (reader.ReadToNextSibling("frame"));
			}
			
			return data;
		}
	
		/// <summary>
		/// Saves spriter information to SCML.
		/// </summary>
		public static ISpriterData SaveSCML<TData, TCharacter, TAnimation, TFrame, TKeyframe, TSprite>(this ISpriterData data, string path)
		{
			throw new System.NotImplementedException();
		}
	}
}