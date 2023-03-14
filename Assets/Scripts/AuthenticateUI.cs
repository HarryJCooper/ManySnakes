using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour {


    [SerializeField] private Button authenticateButton;
    [SerializeField] private Button[] colorButtons;
    Color playerColor;


    private void Awake() {
        authenticateButton.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(EditPlayerName.Instance.GetPlayerName(), playerColor);
            Hide();
        });

        foreach (Button button in colorButtons) {
            button.onClick.AddListener(() => {
                playerColor = button.GetComponent<Image>().color;
                Debug.Log(playerColor);
            });
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
