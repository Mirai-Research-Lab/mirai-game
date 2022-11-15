using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //target is in Editor class and it's the currently selected gameobject
        Interactable interactable = (Interactable) target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.prompyMessage = EditorGUILayout.TextField("Prompy Message", interactable.prompyMessage);
            EditorGUILayout.HelpBox("Event Only Interacble that only use unity events", MessageType.Info);
            if(interactable.GetComponent<InteractionEvents>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvents>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvents)
            {
                if (interactable.gameObject.GetComponent<InteractionEvents>() == null)
                    interactable.gameObject.AddComponent<InteractionEvents>();
            }
            else
            {
                if (interactable.gameObject.GetComponent<InteractionEvents>() != null)
                    DestroyImmediate(interactable.GetComponent<InteractionEvents>());
            }
        }
    }
}
