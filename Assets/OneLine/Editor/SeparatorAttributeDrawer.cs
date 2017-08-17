using UnityEditor;
using UnityEngine;
using OneLine;

namespace OneLine {
    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorAttributeDrawer : UnityEditor.DecoratorDrawer {

        private new SeparatorAttribute attribute { get { return base.attribute as SeparatorAttribute; }}

        // Workaround, see http://answers.unity3d.com/questions/377207/drawing-a-texture-in-a-custom-propertydrawer.html
        private static GUIStyle lineStyle;
        private static Texture2D lineTexture;
        private static GUIStyle textStyle;

        static SeparatorAttributeDrawer(){
            var color = Color.gray;

            lineTexture = new Texture2D(1,1);
            lineTexture.SetPixel(0,0,color);
            lineTexture.Apply();
            lineStyle = new GUIStyle();
            lineStyle.normal.background = lineTexture;

            textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.normal.textColor = color;
        }

        public override void OnGUI(Rect rect) {
            string text = attribute.Text;
            if (string.IsNullOrEmpty(text)){
                DrawLine(rect);
            }
            else {
                var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
                var rects = rect.Split(new float[]{1,0,1}, new float[]{0, textSize.x, 0}); 

                DrawLine(rects[0]);
                DrawText(rects[1], text);
                DrawLine(rects[2]);
            }
        }

        private static void DrawText(Rect rect, string text){
            EditorGUI.LabelField(rect, text, textStyle); 
        }
        private static void DrawLine(Rect rect){
            var height = 2;
            rect.y += (rect.height - height) / 2;
            rect.height = height;
            EditorGUI.LabelField(rect, GUIContent.none, lineStyle);
        }
    }
}
