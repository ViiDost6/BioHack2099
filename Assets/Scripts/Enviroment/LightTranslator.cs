using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;

public class LightTranslator : MonoBehaviour
{
    public Material material;       // Material with Shader Graph

    [Range(0f, 1f)]
    public float glowAmount = 0f;   // Glow intensity (0 = off, 1 = full glow)
    [Range(0f, 5f)]
    public float glowSpeed = 1f;    // Speed of glow effect
    public float glowMult = 5f;     // Maximum glow multiplier

    private bool isIncreasing = false; // Toggle for glow effect
    private Coroutine currentCoroutine = null; // Reference to the currently running coroutine

    private void Start()
    {
        if (material != null)
        {
            material.SetFloat("GlowMult", glowMult); // Set the glow multiplier in the shader
            material.SetFloat("_GlowAmount", glowAmount); // Set the initial glow amount in the shader
        }
        else
        {
            Debug.LogError("Material is not assigned!");
        }
    }
    // Public method to trigger the glow effect
    public void TriggerGlow()
    {
        isIncreasing = !isIncreasing; // Toggle the glow effect

        // Stop the currently running coroutine, if any
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Start the appropriate coroutine and store its reference
        if (isIncreasing)
        {
            currentCoroutine = StartCoroutine(IncreaseGlow());
        }
        else
        {
            currentCoroutine = StartCoroutine(DecreaseGlow());
        }
    }

    IEnumerator IncreaseGlow()
    {
        while (glowAmount < 1f)
        {
            glowAmount += Time.deltaTime * glowSpeed; // Increase glow amount over time

            // Snap to 1 if close enough to avoid floating-point inaccuracies
            if (1f - glowAmount < 0.01f)
            {
                glowAmount = 1f;
                break;
            }

            glowAmount = Mathf.Clamp(glowAmount, 0f, 1f); // Ensure glow amount stays within bounds

            // Update the shader
            if (material != null)
            {
                material.SetFloat("_GlowAmount", glowAmount);
            }

            yield return null; // Wait for the next frame
        }

        glowAmount = 1f; // Ensure glow amount is capped at 1
        currentCoroutine = null; // Mark the coroutine as finished
    }

    IEnumerator DecreaseGlow()
    {
        while (glowAmount > 0f)
        {
            glowAmount -= Time.deltaTime * glowSpeed; // Decrease glow amount over time

            // Snap to 0 if close enough to avoid floating-point inaccuracies
            if (glowAmount < 0.01f)
            {
                glowAmount = 0f;
                break;
            }

            glowAmount = Mathf.Clamp(glowAmount, 0f, 1f); // Ensure glow amount stays within bounds

            // Update the shader
            if (material != null)
            {
                material.SetFloat("_GlowAmount", glowAmount);
            }

            yield return null; // Wait for the next frame
        }

        glowAmount = 0f; // Ensure glow amount is capped at 0
        currentCoroutine = null; // Mark the coroutine as finished
    }
}
