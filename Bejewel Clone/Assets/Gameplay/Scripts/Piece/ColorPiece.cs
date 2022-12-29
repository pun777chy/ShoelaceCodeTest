using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoelace.Piece
{
    public enum ColorType
    {
        Blue,
        Orange,
        Pink,
        Red,
        Yellow
    }
    public class ColorPiece : MonoBehaviour
    {
        [System.Serializable]
        public struct ColorSprite
        {
            public ColorType color;
            public Sprite sprite;
        }

        public ColorSprite[] colorSprites;
        private ColorType color;
        public ColorType Color
        {
            get 
            { 
                return color; 
            }
            set
            { 
               SetColor(value); 
            }
        }
        private SpriteRenderer sprite;
        private Dictionary<ColorType, Sprite> colorSpriteDict;
        private void Awake()
        {
            FillTheColorOPieceDictionary();
        }

        private void FillTheColorOPieceDictionary()
        {
            sprite = GetComponent<SpriteRenderer>();
            colorSpriteDict = new Dictionary<ColorType, Sprite>();
            for (int i = 0; i < colorSprites.Length; i++)
            {
                if (!colorSpriteDict.ContainsKey(colorSprites[i].color))
                {
                    colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);

                }
            }
        }
        public void SetColor(ColorType newColor)
        {
            color = newColor;
            if(colorSpriteDict.ContainsKey(newColor))
            {
                sprite.sprite = colorSpriteDict[newColor];
            }
        }
    }
}
