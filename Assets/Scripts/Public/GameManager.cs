using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Joshua.Model;
using Joshua.Presenter;
using Joshua.View;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private SlotMachinePresenter presenter;
    private SlotMachineView view;
    private DataHandle data;

    private void Awake()
    {
        ItemData itemData = Resources.Load<ItemData>("Data/SpritesData");
        GameObject wheelPrefab = Resources.Load<GameObject>("Prefabs/SpinWheel");

        GameObject bt_Spin = GameObject.Find("Bt_Spin");
        TextMeshProUGUI uGUI = bt_Spin.GetComponentInChildren<TextMeshProUGUI>();

        Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        for (int i = 0; i < itemData.sprites.Length; i++)
        {
            sprites.Add(itemData.sprites[i].name, itemData.sprites[i]);
        }

        GameObject go;
        Transform root = GameObject.Find("SlotMachine/Container").transform;
        SpinWheel[] spinWheels = new SpinWheel[itemData.NumberOfSpinWheels];

        for (int j = 0; j < itemData.NumberOfSpinWheels; j++)
        {
            go = Instantiate(wheelPrefab, root);
            spinWheels[j] = new SpinWheel(go.GetComponentsInChildren<Image>());
        }

        view = new SlotMachineView(uGUI, spinWheels, sprites, bt_Spin);
        data = new DataHandle();
        presenter = new SlotMachinePresenter(view, data);
    }

    private void OnDestroy()
    {
        presenter.Dispose();
        view = null;
        data = null;
    }
}
