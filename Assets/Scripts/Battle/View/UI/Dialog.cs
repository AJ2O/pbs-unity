using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI
{
    public class Dialog : MonoBehaviour
    {
        [Header("Components")]
        public Image dialogBox;
        public Text dialogBoxText;

        [Header("Settings")]
        public float charPerSec = 60f;
        public float scrollSpeed = 0.1f;
        public int defaultDialogLines = 2;
        public bool advancedDialogPressed = false;

        public void DrawBox(bool clearOnDraw = true)
        {
            dialogBox.gameObject.SetActive(true);
            if (clearOnDraw)
            {
                ClearBox();
            }
        }
        public void UndrawBox()
        {
            dialogBox.gameObject.SetActive(false);
        }
        public void ClearBox()
        {
            dialogBoxText.text = "";
        }

        public IEnumerator DrawText(string text, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text, 
                secPerChar: 1f / charPerSec,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawTextInstant(string text, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text, 
                secPerChar: 0,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawText(string text, float time, float lockedTime, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text, 
                secPerChar: 1f / charPerSec,
                time: time, 
                lockedTime: lockedTime,
                silent: true,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }

        public IEnumerator DrawText(
            string text,
            float secPerChar,
            float time = 2f,
            float lockedTime = 0,
            bool silent = true,
            bool hold = false,
            bool undrawOnFinish = false,
            Text textBox = null,
            int lines = -1
            )
        {
            yield return StartCoroutine(DrawRenderedText(
                text: text,
                secPerChar: secPerChar,
                time: time,
                lockedTime: lockedTime,
                silent: silent,
                hold: hold,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines,

                capitalize: false,
                bold: false,
                italic: false,
                color: null
                ));
        }

        private IEnumerator DrawRenderedText(
            string text,
            float secPerChar,
            float time = 2f,
            float lockedTime = 0,
            bool silent = true,
            bool hold = false,
            bool undrawOnFinish = false,
            Text textBox = null,
            int lines = -1,

            // draw options
            bool capitalize = false,
            bool bold = false,
            bool italic = false,
            string color = null)
        {
            advancedDialogPressed = false;
            if (textBox == null) textBox = dialogBoxText;
            if (textBox == dialogBoxText) dialogBox.gameObject.SetActive(text != null);

            int splitPoint = getOverflowPoint(GetFilteredText(text), textBox, lines);
            int newStartPoint = 0;
            string textToDraw = text.Substring(newStartPoint).TrimStart();

            // draw text in segments that fit the textbox
            while (splitPoint != -1)
            {
                textBox.text = "";
                float drawTime = Time.time;
                int realCharsDrawn = 0;
                int charsToDraw = splitPoint;
                int i;

                for (i = 0; i < textToDraw.Length; i++)
                {
                    int skippedChars = 0;

                    // check for operators
                    if (textToDraw[i] == '\\')
                    {
                        skippedChars += 1;
                        switch (textToDraw[i + skippedChars])
                        {
                            // bold
                            case 'b':
                                bold = !bold;
                                break;

                            // italics
                            case 'i':
                                italic = !italic;
                                break;

                            // uppercase
                            case 'u':
                                capitalize = !capitalize;
                                break;

                            // color
                            case 'c':
                                string colorChange = textToDraw.Substring(i + skippedChars + 1).Split('\\')[0];
                                skippedChars += colorChange.Length + 1;
                                color = (colorChange == ".") ? null : colorChange;
                                break;
                        }

                    }
                    // write regular character
                    else
                    {
                        textBox.text += GetDrawnCharacter(
                            textToDraw[i].ToString(),
                            capitalize: capitalize,
                            bold: bold,
                            italic: italic,
                            color: color
                            );

                        if (!advancedDialogPressed) while (Time.time < drawTime + (secPerChar * (realCharsDrawn + 1))) { yield return null; }
                        realCharsDrawn += 1;
                        if (charsToDraw > 0)
                        {
                            if (realCharsDrawn >= charsToDraw)
                            {
                                i += 1;
                                break;
                            }
                        }
                    }

                    i += skippedChars;
                }
                newStartPoint += i + 1;
                textToDraw = text.Substring(newStartPoint).TrimStart();
                splitPoint = getOverflowPoint(GetFilteredText(textToDraw), textBox, lines);

                advancedDialogPressed = false;
                float startTime = Time.time;
                if (time > 0)
                {
                    while (!advancedDialogPressed && Time.time < startTime + time) { yield return null; }
                }
                advancedDialogPressed = false;
            }

            // draw whole text 
            if (splitPoint == -1)
            {
                textBox.text = "";
                yield return StartCoroutine(DrawTextNew(
                    text: textToDraw,
                    secPerChar: secPerChar,
                    textBox: textBox,
                    capitalize: capitalize,
                    bold: bold,
                    italic: italic,
                    color: color
                    ));

                // wait time
                float startTime = Time.time;
                if (lockedTime > 0)
                {
                    while (Time.time < startTime + lockedTime) { yield return null; }
                }

                advancedDialogPressed = false;
                if (time > 0)
                {
                    while (!advancedDialogPressed && Time.time < startTime + time) 
                    { 
                        yield return null; 
                    }
                }
                else if (hold)
                {
                    while (!advancedDialogPressed) { yield return null; }
                }
            }
    
            if (undrawOnFinish && textBox == dialogBoxText)
            {
                UndrawBox();
            }
        }

        private IEnumerator DrawTextNew(
            string text,
            float secPerChar,
            Text textBox,
            int charsToDraw = -1,
            bool capitalize = false,
            bool bold = false,
            bool italic = false,
            string color = null
            )
        {
            float startTime = Time.time;
        
            int realCharsDrawn = 0;
            int i;
            for (i = 0; i < text.Length; i++)
            {
                int skippedChars = 0;

                // check for operators
                if (text[i] == '\\')
                {
                    skippedChars += 1;
                    switch (text[i+skippedChars])
                    {
                        // bold
                        case 'b':
                            bold = !bold;
                            break;

                        // italics
                        case 'i':
                            italic = !italic;
                            break;

                        // uppercase
                        case 'u':
                            capitalize = !capitalize;
                            break;

                        // color
                        case 'c':
                            string colorChange = text.Substring(i + skippedChars + 1).Split('\\')[0];
                            skippedChars += colorChange.Length + 1;
                            color = (colorChange == ".") ? null : colorChange;
                            break;
                    }

                }
                // write regular character
                else
                {
                    textBox.text += GetDrawnCharacter(
                        text[i].ToString(),
                        capitalize: capitalize,
                        bold: bold,
                        italic: italic,
                        color: color
                        );

                    if (!advancedDialogPressed) while (Time.time < startTime + (secPerChar * (realCharsDrawn + 1))) { yield return null; }
                    realCharsDrawn += 1;
                    if (charsToDraw > 0)
                    {
                        if (realCharsDrawn >= charsToDraw)
                        {
                            break;
                        }
                    }
                }

                i += skippedChars;
            }
        }

        private string GetRenderedText(
            string text,
            bool capitalize = false,
            bool bold = false,
            bool italic = false,
            string color = null
            )
        {
            string drawnText = "";
            int i;
            for (i = 0; i < text.Length; i++)
            {
                int skippedChars = 0;

                // check for operators
                if (text[i] == '\\')
                {
                    skippedChars += 1;
                    switch (text[i + skippedChars])
                    {
                        // bold
                        case 'b':
                            bold = !bold;
                            break;

                        // italics
                        case 'i':
                            italic = !italic;
                            break;

                        // uppercase
                        case 'u':
                            capitalize = !capitalize;
                            break;

                        // color
                        case 'c':
                            string colorChange = text.Substring(i + skippedChars + 1).Split('\\')[0];
                            skippedChars += colorChange.Length + 1;
                            color = (colorChange == ".") ? null : colorChange;
                            break;
                    }

                }
                // write regular character
                else
                {
                    drawnText += GetDrawnCharacter(
                        text[i].ToString(),
                        capitalize: capitalize,
                        bold: bold,
                        italic: italic,
                        color: color
                        );
                }

                i += skippedChars;
            }
            return drawnText;
        }

        public static string GetFilteredText(
            string text
            )
        {
            string drawnText = "";
            int i;
            for (i = 0; i < text.Length; i++)
            {
                int skippedChars = 0;

                // check for operators
                if (text[i] == '\\')
                {
                    skippedChars += 1;
                    switch (text[i + skippedChars])
                    {
                        // bold
                        case 'b':
                            break;

                        // italics
                        case 'i':
                            break;

                        // uppercase
                        case 'u':
                            break;

                        // color
                        case 'c':
                            string colorChange = text.Substring(i + skippedChars + 1).Split('\\')[0];
                            skippedChars += colorChange.Length + 1;
                            break;
                    }

                }
                // write regular character
                else
                {
                    drawnText += text[i].ToString();
                }

                i += skippedChars;
            }
            return drawnText;
        }

        public static string GetDrawnCharacter(
            string text,
            bool capitalize = false,
            bool bold = false,
            bool italic = false,
            string color = null
            )
        {
            if (text == " "
                || text == "\n")
            {
                return text;
            }

            string boldPre = bold ? "<b>" : "";
            string boldPost = bold ? "</b>" : "";

            string italicPre = italic ? "<i>" : "";
            string italicPost = italic ? "</i>" : "";

            string colorPre = !string.IsNullOrEmpty(color) ? "<color=" + color + ">" : "";
            string colorPost = !string.IsNullOrEmpty(color) ? "</color>" : "";

            string builtText = boldPre + italicPre + colorPre +
                text +
                colorPost + italicPost + boldPost;

            if (capitalize)
            {
                builtText = builtText.ToUpper();
            }

            return builtText;
        }

        public int getOverflowPoint(
            string message, 
            Text textBox = null,
            int lines = -1)
        {
            if (textBox == null) textBox = dialogBoxText;
            if (lines <= 0) lines = defaultDialogLines;

            TextGenerator textGen = new TextGenerator();
            TextGenerationSettings generationSettings = textBox.GetGenerationSettings(textBox.rectTransform.rect.size);

            for (int i = 0; i < message.Length; i++) // Got overflow point
            {
                float height = textGen.GetPreferredHeight(message.Substring(0, i), generationSettings);
                if (height > generationSettings.fontSize * (lines + 2))
                {
                    for (int i2 = i; i2 > 0; i2--)
                    {
                        if (message[i2] == ' ' || message[i2] == '\n') // Stop counter at first space or newline
                        {
                            return i2;
                        }
                    }
                    return i;
                }
            }
            return -1;
        }
    }
}

