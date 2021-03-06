using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor;

/*
 This test checks that a maskableGraphic within a RectMask2D will be properly clipped.
 Test for case (1013182 - [RectMask2D] Child gameObject is masked if there are less than 2 corners of GO that matches RectMask's x position)
*/
namespace Graphics
{
    public class RectMask2DClipping : IPrebuildSetup
    {
        GameObject m_PrefabRoot;

        const string kPrefabPath = "Assets/Resources/Mask2DRectCullingPrefab.prefab";

        public void Setup()
        {
#if UNITY_EDITOR
            var rootGO = new GameObject("RootGO");
            var rootCanvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler));
            rootCanvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            rootCanvasGO.transform.SetParent(rootGO.transform);

            var maskGO = new GameObject("Mask", typeof(RectMask2D), typeof(RectTransform));
            var maskTransform = maskGO.GetComponent<RectTransform>();
            maskTransform.SetParent(rootCanvasGO.transform);
            maskTransform.localPosition = Vector3.zero;
            maskTransform.sizeDelta = new Vector2(200, 200);
            maskTransform.localScale = Vector3.one;

            var imageGO = new GameObject("Image", typeof(Image), typeof(RectTransform));
            var imageTransform = imageGO.GetComponent<RectTransform>();
            imageTransform.SetParent(maskTransform);
            imageTransform.localPosition = new Vector3(-125, 0, 0);
            imageTransform.sizeDelta = new Vector2(100, 100);
            imageTransform.localScale = Vector3.one;

            if (!Directory.Exists("Assets/Resources/"))
                Directory.CreateDirectory("Assets/Resources/");

            UnityEditor.PrefabUtility.SaveAsPrefabAsset(rootGO, kPrefabPath);
            GameObject.DestroyImmediate(rootGO);

#endif
        }

        [SetUp]
        public void TestSetup()
        {
            m_PrefabRoot = Object.Instantiate(Resources.Load("Mask2DRectCullingPrefab")) as GameObject;
            new GameObject("Camera", typeof(Camera));
        }

        [UnityTest]
        public IEnumerator Mask2DRect_CorrectClipping()
        {
            //root->canvas->mask->image
            RectTransform t = m_PrefabRoot.transform.GetChild(0).GetChild(0).GetChild(0) as RectTransform;
            CanvasRenderer cr = t.GetComponent<CanvasRenderer>();
            bool cull = false;
            for (int i = 0; i < 360; i += 45)
            {
                t.localEulerAngles = new Vector3(0, 0, i);
                cull |= cr.cull;
                yield return null;
            }

            Assert.IsFalse(cull);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_PrefabRoot);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
#if UNITY_EDITOR
            AssetDatabase.DeleteAsset(kPrefabPath);
#endif
        }
    }
}
