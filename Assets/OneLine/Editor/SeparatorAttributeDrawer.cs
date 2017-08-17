using UnityEditor;
using UnityEngine;
using OneLine;

namespace OneLine {
    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorAttributeDrawer : UnityEditor.DecoratorDrawer {

        private new SeparatorAttribute attribute { get { return base.attribute as SeparatorAttribute; }}

        // Workaround, see http://answers.unity3d.com/questions/377207/drawing-a-texture-in-a-custom-propertydrawer.html
        private GUIStyle lineStyle;
        private GUIStyle textStyle;
        private const float spaceBefore = 10;

        public override float GetHeight(){
            return spaceBefore + Mathf.Max(attribute.Thickness, EditorGUIUtility.singleLineHeight + 5);
        }

        public override void OnGUI(Rect rect) {
            PrepareStyles();
            rect.y += spaceBefore;
            rect.height -= spaceBefore;

            string text = attribute.Text;
            int thickness = attribute.Thickness;

            if (string.IsNullOrEmpty(text)){
                DrawLine(rect, thickness);
            }
            else {
                var textSize = textStyle.CalcSize(new GUIContent(text));
                var rects = rect.Split(new float[]{1,0,1}, new float[]{0, textSize.x, 0});

                DrawLine(rects[0], thickness);
                DrawText(rects[1], text);
                DrawLine(rects[2], thickness);
            }
        }

        private void PrepareStyles(){
            if (lineStyle != null && textStyle != null){ return; }

            var color = new Color(0.3f, 0.3f, 0.3f, 1);

            var lineTexture = new Texture2D(1,1);
            lineTexture.SetPixel(0,0,color);
            lineTexture.Apply();
            lineStyle = new GUIStyle();
            lineStyle.normal.background = lineTexture;

            textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = color;
            textStyle.alignment = TextAnchor.MiddleCenter;
        }

        private void DrawText(Rect rect, string text){
            EditorGUI.LabelField(rect, text, textStyle); 
        }
        private void DrawLine(Rect rect, int thickness){
            rect.y += (rect.height - thickness) / 2;
            rect.height = thickness;
            EditorGUI.LabelField(rect, GUIContent.none, lineStyle);
        }
    }
}
