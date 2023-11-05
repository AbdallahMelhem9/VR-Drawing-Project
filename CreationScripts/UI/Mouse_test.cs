using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Mouse_test : MonoBehaviour
{
    public Image colorPalette; // 指向调色盘的Image组件
    public Color cubeColor;    // 保存所选颜色的Color变量

    private void Update()
    {
        Mouse currentMouse = Mouse.current;

        if (currentMouse != null && currentMouse.leftButton.wasPressedThisFrame)
        {
            // 获取鼠标点击位置
            Vector2 mousePosition = currentMouse.position.ReadValue();

            // 检查是否点击到调色盘
            if (RectTransformUtility.RectangleContainsScreenPoint(colorPalette.rectTransform, mousePosition))
            {
                // 获取点击在调色盘上的UV坐标
                Vector2 localPosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPalette.rectTransform, mousePosition, null, out localPosition))
                {
                    // 计算点击位置在调色盘纹理中的UV坐标
                    Vector2 normalizedPosition = new Vector2(
                        Mathf.InverseLerp(0f, colorPalette.rectTransform.rect.width, localPosition.x),
                        Mathf.InverseLerp(0f, colorPalette.rectTransform.rect.height, localPosition.y)
                    );

                    // 根据UV坐标获取对应像素的颜色
                    Texture2D paletteTexture = colorPalette.sprite.texture;
                    Color selectedColor = paletteTexture.GetPixel(
                        Mathf.FloorToInt(normalizedPosition.x * paletteTexture.width),
                        Mathf.FloorToInt(normalizedPosition.y * paletteTexture.height)
                    );

                    // 将所选颜色保存到cubeColor变量
                    cubeColor = selectedColor;

                    // 可以在这里将颜色用于你的应用程序逻辑，比如修改物体的颜色
                    // 例如，renderer.material.color = cubeColor;
                }
            }
        }
    }
}
