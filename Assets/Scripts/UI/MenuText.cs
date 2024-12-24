using UnityEngine;
using TMPro;

public class AlternateLetterColorsWithOutline : MonoBehaviour
{
    public TMP_Text textComponent; // Reference to the TMP_Text component

    void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("Text Component not assigned!");
            return;
        }

        // Enable Outline and Set Properties
        EnableOutline();

        // Apply alternating colors
        ApplyColorAlternation();
    }

    void EnableOutline()
    {
        // Enable the outline and set its color and thickness
        textComponent.outlineColor = Color.black; // Set outline color
        textComponent.outlineWidth = 0.2f; // Adjust outline thickness
    }

    void ApplyColorAlternation()
    {
        // Get the original text
        string originalText = textComponent.text;

        // Start building the new formatted text
        string formattedText = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            char c = originalText[i];

            // Skip colorizing spaces and newlines
            if (char.IsWhiteSpace(c))
            {
                formattedText += c;
                continue;
            }

            // Use red for even-indexed characters and white for odd-indexed
            string colorHex = (i % 2 == 0) ? "#FF0000" : "#FFFFFF";
            formattedText += $"<color={colorHex}>{c}</color>";
        }

        // Apply the formatted text to the TextMeshPro component
        textComponent.text = formattedText;
    }
}
