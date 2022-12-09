using KitchenData;
using KitchenLib;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Reflection;
using UnityEngine;

namespace BurgerSuit {

    public class BurgerSuitMod : BaseMod {

        public const string MOD_ID = "blargle.BurgerSuit";
        public const string MOD_NAME = "Burger Suit";
        public const string MOD_VERSION = "0.0.1";

        public BurgerSuitMod() : base(MOD_ID, MOD_NAME, "blargle", MOD_VERSION, "1.1.2", Assembly.GetExecutingAssembly()) {
            Debug.Log($"Mod loaded: {MOD_ID} {MOD_VERSION}");
        }

        protected override void OnInitialise() {
            AddGameDataObject<BurgerSuitOutfit>();
        }
    }

    public class BurgerSuitOutfit : CustomPlayerCosmetic {

        public override CosmeticType CosmeticType => CosmeticType.Outfit;
        public override GameObject Visual => ((Item)GDOUtils.GetExistingGDO(KitchenLib.References.ItemReferences.BurgerUnplated)).Prefab;

        public override void OnRegister(GameDataObject gameDataObject) {
            GameObject prefab = ((PlayerCosmetic)gameDataObject).Visual;
            rotateAndScaleBurgerToCoverBody(prefab);
            hideObject(prefab, "Plate");
            hideObject(prefab, "Tomato - Chopped");
            hideObject(prefab, "Onion - Chopped");
            makeCheeseVisibleOnSides(prefab);
            makePattyThicker(prefab);
            makeBunSitFlat(prefab);
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
}
