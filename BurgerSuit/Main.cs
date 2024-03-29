﻿using HarmonyLib;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Customs;
using KitchenLib.Utils;
using KitchenMods;
using System.Reflection;
using UnityEngine;

namespace BurgerSuit {

    public class BurgerSuitMod : BaseMod {

        public const string MOD_ID = "blargle.BurgerSuit";
        public const string MOD_NAME = "Burger Suit";
        public const string MOD_VERSION = "0.0.5";
        public const string MOD_AUTHOR = "blargle";

        public BurgerSuitMod() : base(MOD_ID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, ">=1.1.7", Assembly.GetExecutingAssembly()) { }

        protected override void OnPostActivate(Mod mod) {
            Debug.Log($"[{MOD_ID}] v{MOD_VERSION}: initialized");
            AddGameDataObject<BurgerSuitOutfit>();
        }
    }

    public class BurgerSuitOutfit : CustomPlayerCosmetic {

        public override string UniqueNameID => "BurgerSuit";

        public static bool isRegistered = false;

        public override CosmeticType CosmeticType => CosmeticType.Outfit;
        public override GameObject Visual { get {
                GameObject gameObject = new GameObject();
                gameObject.SetActive(false);
                var prefab = Object.Instantiate(((Item) GDOUtils.GetExistingGDO(KitchenLib.References.ItemReferences.BurgerUnplated)).Prefab);
                prefab.transform.ParentTo(gameObject);
                return prefab;
            }
        }

        public override void OnRegister(PlayerCosmetic gameDataObject) {
            if (isRegistered) {
                Debug.Log($"[{BurgerSuitMod.MOD_ID}] v{BurgerSuitMod.MOD_VERSION}: skipping re-registering custom cosmetic");
                return;
            }

            GameObject prefab = gameDataObject.Visual;
            rotateAndScaleBurgerToCoverBody(prefab);
            hideObject(prefab, "Plate");
            hideObject(prefab, "Tomato - Chopped");
            hideObject(prefab, "Onion - Chopped");
            makeCheeseVisibleOnSides(prefab);
            makePattyThicker(prefab);
            makeBunSitFlat(prefab);

            GameObject colourblind = GameObjectUtils.GetChildObject(prefab, "Colour Blind");
            if (colourblind != null) {
                colourblind.transform.localScale = new Vector3(1, 1, 1);
            }

            isRegistered = true;
            Debug.Log($"[{BurgerSuitMod.MOD_ID}] v{BurgerSuitMod.MOD_VERSION} registered custom cosmetic");
        }

        private void rotateAndScaleBurgerToCoverBody(GameObject prefab) {
            Vector3 objectScale = prefab.transform.localScale;
            prefab.transform.localScale = new Vector3(objectScale.x * 2, objectScale.y * 2, objectScale.z * 2);
            prefab.transform.Rotate(90f, 0f, 0f, Space.Self);
            prefab.transform.position += new Vector3(0, 0.6f, -0.3f);
        }

        private void hideObject(GameObject prefab, string path) {
            GameObject objectToHide = GameObjectUtils.GetChildObject(prefab, path);
            if (objectToHide != null) {
                objectToHide.SetActive(false);
            }
        }

        private void makeCheeseVisibleOnSides(GameObject prefab) {
            GameObject cheeseObject = GameObjectUtils.GetChildObject(prefab, "Cheese - Grated");
            if (cheeseObject != null) {
                cheeseObject.transform.position += new Vector3(0, 0, 0.1525f);
            }
            GameObject cheeseObject2 = GameObjectUtils.GetChildObject(prefab, "Cheese - Grated/Potato - Chopped (1)");
            if (cheeseObject2 != null) {
                cheeseObject2.transform.position += new Vector3(-0.2f, 0.0039f, 0.0422f);
            }
        }

        private void makePattyThicker(GameObject prefab) {
            GameObject burgerPatty = GameObjectUtils.GetChildObject(prefab, "Burger Patty Cooked");
            if (burgerPatty != null) {
                Vector3 pattyScale = burgerPatty.transform.localScale;
                burgerPatty.transform.localScale = new Vector3(pattyScale.x, pattyScale.y * 2.5f, pattyScale.z);
            }
        }

        private void makeBunSitFlat(GameObject prefab) {
            GameObject topBun = GameObjectUtils.GetChildObject(prefab, "Burger Bun/BurgerBun/Top");
            if (topBun != null) {
                topBun.transform.eulerAngles = new Vector3(0, 0, 0);
                topBun.transform.position += new Vector3(0, 0.1338f, 0.1f);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerCosmeticSubview), "UpdateData")]
    public class FixMirrorPositionPatch {

        public static void Postfix(PlayerCosmeticSubview __instance) {
            Transform transform = __instance.transform.Find("Burger - Plated(Clone)(Clone)");
            if (transform != null && __instance.transform.parent.name == "Container") {
                transform.localPosition = new Vector3(0, -1.4f, -0.3f);
            }
        }
    }

}
